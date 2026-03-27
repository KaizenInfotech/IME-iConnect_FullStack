import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getAttendanceRecords, createAttendance, updateAttendance, deleteAttendance } from '../api/attendanceService';
// memberService imported dynamically in fetchMembers

const currentYear = new Date().getFullYear();
const years = Array.from({ length: 10 }, (_, i) => currentYear - i);
const PAGE_SIZE = 10;

const emptyForm = {
  AttendanceName: '',
  AttendanceDesc: '',
  AttendanceDate: '',
  GroupId: 0,
};

function formatDateDMY(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const yyyy = d.getFullYear();
  const hh = String(d.getHours()).padStart(2, '0');
  const min = String(d.getMinutes()).padStart(2, '0');
  return `${dd}/${mm}/${yyyy} ${hh}:${min}`;
}

function toDatetimeLocal(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  const yyyy = d.getFullYear();
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const dd = String(d.getDate()).padStart(2, '0');
  const hh = String(d.getHours()).padStart(2, '0');
  const min = String(d.getMinutes()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}T${hh}:${min}`;
}

export default function AttendancePage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [chapterName, setChapterName] = useState('');

  const [allItems, setAllItems] = useState([]);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [year, setYear] = useState(currentYear);
  const [searchTerm, setSearchTerm] = useState('');
  const [pageNo, setPageNo] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');
  const [errors, setErrors] = useState({});

  // Members modal
  const [showMembersModal, setShowMembersModal] = useState(false);
  const [membersList, setMembersList] = useState([]);
  const [selectedMembers, setSelectedMembers] = useState([]);
  const [visitors, setVisitors] = useState([]);
  const [showVisitorsModal, setShowVisitorsModal] = useState(false);

  useEffect(() => { fetchData(); }, [year]);

  useEffect(() => {
    let filtered = [...allItems];
    if (searchTerm.trim()) {
      const q = searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        (item.AttendanceName || item.attendanceName || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
    setPageNo(1);
  }, [searchTerm, allItems]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const groupId = filterGroupId || '33359';
      // Get chapter name
      if (filterGroupId) {
        try {
          const { getClubList } = await import('../api/groupService');
          const clubRes = await getClubList();
          const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
          const found = clubs.find(c => String(c.GroupId) === filterGroupId);
          if (found) setChapterName(found.group_name);
        } catch {}
      }
      const res = await getAttendanceRecords(groupId);
      const records = res.data?.TBAttendanceListResult?.Result?.Table || [];
      const data = records.map(r => ({
        ...r, Id: r.AttendanceID, AttendanceName: r.AttendanceName,
        AttendanceDate: r.AttendanceDate, AttendanceTime: r.Attendancetime,
        MemberCount: r.member_count, VisitorCount: r.visitor_count,
        Description: r.Description,
      }));
      setAllItems(data);
      setItems(data);
    } catch { setError('Failed to load attendance records'); }
    finally { setLoading(false); }
  };

  const fetchMembers = async () => {
    try {
      const groupId = filterGroupId || '33359';
      const { getMembers: getMems } = await import('../api/memberService');
      const res = await getMems(groupId);
      const members = res.data?.MemberDetail?.NewMemberList || [];
      setMembersList(members.map(m => ({ Id: m.profileID, MemberName: [m.memberName, m.lastName].filter(Boolean).join(' '), profilePic: m.profilePic })));
    } catch { setMembersList([]); }
  };

  const validate = () => {
    const errs = {};
    if (!form.AttendanceName.trim()) errs.AttendanceName = 'mandatory';
    if (form.AttendanceName.length > 100) errs.AttendanceName = 'Max 100 characters';
    if (!form.AttendanceDesc.trim()) errs.AttendanceDesc = 'mandatory';
    if (form.AttendanceDesc.length > 100) errs.AttendanceDesc = 'Max 100 characters';
    if (!form.AttendanceDate) errs.AttendanceDate = 'mandatory';
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;
    setSaving(true);
    try {
      const payload = {
        ...form,
        Members: selectedMembers,
        Visitors: visitors,
      };
      if (editing) await updateAttendance(editing.id || editing.Id, payload);
      else await createAttendance(payload);
      setShowModal(false);
      setEditing(null);
      setForm(emptyForm);
      setSelectedMembers([]);
      setVisitors([]);
      setErrors({});
      fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteAttendance(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({
      AttendanceName: item.AttendanceName || item.attendanceName || '',
      AttendanceDesc: item.AttendanceDesc || item.attendanceDesc || '',
      AttendanceDate: toDatetimeLocal(item.AttendanceDate || item.attendanceDate),
      GroupId: item.GroupId || item.groupId || 0,
    });
    setSelectedMembers(item.Members || item.members || []);
    setVisitors(item.Visitors || item.visitors || []);
    setErrors({});
    setShowModal(true);
    fetchMembers();
  };

  const openAdd = () => {
    setEditing(null);
    setForm(emptyForm);
    setSelectedMembers([]);
    setVisitors([]);
    setErrors({});
    setShowModal(true);
    fetchMembers();
  };

  const toggleMember = (member) => {
    const id = member.id || member.Id || member.MemberProfileId;
    const exists = selectedMembers.find(m => (m.id || m.Id || m.MemberProfileId) === id);
    if (exists) {
      setSelectedMembers(selectedMembers.filter(m => (m.id || m.Id || m.MemberProfileId) !== id));
    } else {
      setSelectedMembers([...selectedMembers, { ...member, Type: 'Present' }]);
    }
  };

  const downloadCSV = (data, filename) => {
    if (!data || data.length === 0) { alert('No records found for download.'); return; }
    const headers = Object.keys(data[0]);
    const csv = [headers.join(','), ...data.map(row => headers.map(h => `"${(row[h] ?? '').toString().replace(/"/g, '""')}"`).join(','))].join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a'); a.href = url; a.download = filename; a.click();
    URL.revokeObjectURL(url);
  };

  const exportMeetingWise = () => {
    // Downloads attendance meetings list for this chapter
    const data = items.map(i => ({ AttendanceName: i.AttendanceName, AttendanceDate: i.AttendanceDate, AttendanceTime: i.AttendanceTime, MemberCount: i.MemberCount, VisitorCount: i.VisitorCount, Description: i.Description }));
    downloadCSV(data, 'MeetingWiseReport.csv');
  };

  const exportMemberWise = () => {
    // Downloads member-wise attendance summary (who attended which meetings)
    // This requires attendance records to exist — if none, show alert
    if (!items || items.length === 0) { alert('No records found for download.'); return; }
    const data = items.map(i => ({ AttendanceName: i.AttendanceName, AttendanceDate: i.AttendanceDate, MemberCount: i.MemberCount, VisitorCount: i.VisitorCount }));
    downloadCSV(data, 'MemberWiseReport.csv');
  };

  // Pagination
  const totalPages = Math.ceil(items.length / PAGE_SIZE);
  const paginatedItems = items.slice((pageNo - 1) * PAGE_SIZE, pageNo * PAGE_SIZE);

  const renderPagination = () => {
    if (totalPages <= 1) return null;
    const pages = [];
    for (let i = 1; i <= totalPages; i++) pages.push(i);
    return (
      <div className="gridPager flex items-center justify-center gap-0 py-3">
        <a href="#" onClick={(e) => { e.preventDefault(); setPageNo(1); }}>First</a>
        <a href="#" onClick={(e) => { e.preventDefault(); if (pageNo > 1) setPageNo(pageNo - 1); }}>&laquo;</a>
        {pages.map(p => (
          p === pageNo
            ? <span key={p}>{p}</span>
            : <a key={p} href="#" onClick={(e) => { e.preventDefault(); setPageNo(p); }}>{p}</a>
        ))}
        <a href="#" onClick={(e) => { e.preventDefault(); if (pageNo < totalPages) setPageNo(pageNo + 1); }}>&raquo;</a>
        <a href="#" onClick={(e) => { e.preventDefault(); setPageNo(totalPages); }}>Last</a>
      </div>
    );
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Page Title */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{chapterName || 'National Admin'}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Event Attendance</span>
        </div>
      </div>

      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}<button onClick={() => setError('')} className="float-right font-bold">&times;</button></div>}

      {/* Controls Row */}
      <div className="card mb-4">
        <div className="flex flex-wrap items-center gap-3">
          {/* Search */}
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search..."
            className="form-input"
            style={{ width: 'auto', maxWidth: 220 }}
          />
          <button className="btn-primary text-sm flex items-center gap-1">
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
            Search
          </button>

          {/* Year */}
          <select
            value={year}
            onChange={(e) => setYear(Number(e.target.value))}
            className="form-select"
            style={{ width: '10%', minWidth: 80 }}
          >
            {years.map(y => <option key={y} value={y}>{y}</option>)}
          </select>

          {/* Reports */}
          <button onClick={exportMeetingWise} className="btn-excel text-sm flex items-center gap-1" title="Meeting Wise Report">
            <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
            Meeting Wise Report
          </button>
          <button onClick={exportMemberWise} className="btn-excel text-sm flex items-center gap-1" title="Member Wise Report">
            <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
            Member Wise Report
          </button>

          <div className="ml-auto flex items-center gap-2">
            <button onClick={openAdd} className="btn-warning text-sm">Add</button>
            <button onClick={() => navigate(-1)} style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>← Back</button>
          </div>
        </div>
      </div>

      {/* Table */}
      <div className="card p-0 overflow-hidden">
        <table className="w-full text-sm">
          <thead>
            <tr className="table-header">
              <th className="px-4 py-3 text-left font-normal" style={{ width: '5%' }}>Sr.No.</th>
              <th className="px-4 py-3 text-left font-normal" style={{ width: '70%' }}>Attendance Name</th>
              <th className="px-4 py-3 text-left font-normal" style={{ width: '10%' }}>Attendance Date</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Edit</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {paginatedItems.length === 0 ? (
              <tr><td colSpan={5} className="px-4 py-8 text-center text-gray-500">No Events Found!!!</td></tr>
            ) : (
              paginatedItems.map((item, idx) => {
                const srNo = (pageNo - 1) * PAGE_SIZE + idx + 1;
                const name = item.AttendanceName || item.attendanceName || '';
                const date = formatDateDMY(item.AttendanceDate || item.attendanceDate);
                return (
                  <tr
                    key={item.id || item.Id || idx}
                    className={`border-b border-gray-100 hover:bg-gray-50 cursor-pointer ${idx % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`}
                    onClick={() => navigate(`/attendance/${item.id || item.Id}`)}
                  >
                    <td className="px-4 py-3">{srNo}</td>
                    <td className="px-4 py-3 text-[#1a297d] font-medium">{name}</td>
                    <td className="px-4 py-3">{date}</td>
                    <td className="px-2 py-3 text-center">
                      <button onClick={(e) => { e.stopPropagation(); openEdit(item); }} className="text-gray-500 hover:text-[#1a297d]" title="Edit">
                        <svg className="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </button>
                    </td>
                    <td className="px-2 py-3 text-center">
                      <button onClick={(e) => { e.stopPropagation(); setDeleteTarget(item); }} className="text-gray-500 hover:text-red-600" title="Delete">
                        <svg className="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
        {renderPagination()}
      </div>

      {/* Create/Edit Modal */}
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editing ? 'Edit Attendance' : 'Add Attendance'} size="lg">
        <form onSubmit={handleSubmit}>
          {/* Event Name */}
          <label className="form-label">Event Name <span className="text-[#dd4b39]">*</span></label>
          <input
            type="text"
            maxLength={100}
            value={form.AttendanceName}
            onChange={(e) => setForm({ ...form, AttendanceName: e.target.value })}
            className="form-input"
          />
          {errors.AttendanceName && <span className="validation-error">{errors.AttendanceName}</span>}

          {/* Description */}
          <label className="form-label">Description <span className="text-[#dd4b39]">*</span></label>
          <textarea
            rows={3}
            maxLength={100}
            value={form.AttendanceDesc}
            onChange={(e) => setForm({ ...form, AttendanceDesc: e.target.value })}
            className="form-input"
            style={{ height: 'auto' }}
          />
          {errors.AttendanceDesc && <span className="validation-error">{errors.AttendanceDesc}</span>}

          {/* Event Date & Time */}
          <label className="form-label">Event Date &amp; Time <span className="text-[#dd4b39]">*</span></label>
          <input
            type="datetime-local"
            value={form.AttendanceDate}
            onChange={(e) => setForm({ ...form, AttendanceDate: e.target.value })}
            className="form-input"
          />
          {errors.AttendanceDate && <span className="validation-error">{errors.AttendanceDate}</span>}

          {/* Members & Visitors buttons */}
          <div className="flex items-center gap-4 mt-4">
            <button
              type="button"
              onClick={() => setShowMembersModal(true)}
              className="btn-primary text-sm flex items-center gap-2"
            >
              Members
              <span className="bg-white text-[#1a297d] rounded-full px-2 py-0.5 text-xs font-bold">{selectedMembers.length}</span>
            </button>
            <button
              type="button"
              onClick={() => setShowVisitorsModal(true)}
              className="btn-primary text-sm flex items-center gap-2"
            >
              Visitors
              <span className="bg-white text-[#1a297d] rounded-full px-2 py-0.5 text-xs font-bold">{visitors.length}</span>
            </button>
          </div>

          {/* Buttons */}
          <div className="flex justify-end gap-3 pt-5 pb-2">
            <button type="button" onClick={() => setShowModal(false)} className="btn-outline text-sm">Cancel</button>
            <button type="submit" disabled={saving} className="btn-success text-sm">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>

      {/* Members Selection Modal */}
      <Modal isOpen={showMembersModal} onClose={() => setShowMembersModal(false)} title="Select Members" size="lg">
        <div className="max-h-96 overflow-y-auto">
          {membersList.length === 0 ? (
            <p className="text-gray-500 text-center py-4">No members available.</p>
          ) : (
            <table className="w-full text-sm">
              <thead>
                <tr className="table-header">
                  <th className="px-3 py-2 text-left font-normal" style={{ width: '5%' }}>
                    <input
                      type="checkbox"
                      checked={selectedMembers.length === membersList.length && membersList.length > 0}
                      onChange={(e) => {
                        if (e.target.checked) {
                          setSelectedMembers(membersList.map(m => ({ ...m, Type: 'Present' })));
                        } else {
                          setSelectedMembers([]);
                        }
                      }}
                    />
                  </th>
                  <th className="px-3 py-2 text-left font-normal">Member Name</th>
                </tr>
              </thead>
              <tbody>
                {membersList.map((member, i) => {
                  const memberId = member.id || member.Id || member.MemberProfileId;
                  const isSelected = selectedMembers.some(m => (m.id || m.Id || m.MemberProfileId) === memberId);
                  return (
                    <tr key={memberId || i} className={`border-b border-gray-100 hover:bg-gray-50 cursor-pointer ${i % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`} onClick={() => toggleMember(member)}>
                      <td className="px-3 py-2">
                        <input type="checkbox" checked={isSelected} onChange={() => toggleMember(member)} />
                      </td>
                      <td className="px-3 py-2">{member.MemberName || member.memberName || member.Name || member.name || '-'}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          )}
        </div>
        <div className="flex justify-end gap-3 pt-4">
          <button onClick={() => setShowMembersModal(false)} className="btn-primary text-sm">Done</button>
        </div>
      </Modal>

      {/* Visitors Modal */}
      <Modal isOpen={showVisitorsModal} onClose={() => setShowVisitorsModal(false)} title="Visitors" size="md">
        <div className="mb-3">
          {visitors.map((v, i) => (
            <div key={i} className="flex items-center gap-2 mb-2">
              <input
                type="text"
                value={v.VisitorName || v.visitorName || ''}
                onChange={(e) => {
                  const updated = [...visitors];
                  updated[i] = { ...updated[i], VisitorName: e.target.value };
                  setVisitors(updated);
                }}
                className="form-input"
                placeholder="Visitor name"
              />
              <button type="button" onClick={() => setVisitors(visitors.filter((_, idx) => idx !== i))} className="text-red-500 hover:text-red-700">
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>
          ))}
        </div>
        <button
          type="button"
          onClick={() => setVisitors([...visitors, { VisitorName: '', Type: 'Visitor' }])}
          className="btn-warning text-sm mb-3"
        >
          + Add Visitor
        </button>
        <div className="flex justify-end gap-3 pt-2">
          <button onClick={() => setShowVisitorsModal(false)} className="btn-primary text-sm">Done</button>
        </div>
      </Modal>

      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title="Delete Attendance"
        message="Are you sure you want to delete?"
      />
    </div>
  );
}
