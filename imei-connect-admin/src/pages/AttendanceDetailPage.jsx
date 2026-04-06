import { useState, useEffect } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import Modal from '../components/shared/Modal';
import { getAttendanceRecord, getAttendanceMembers, getAttendanceVisitors, updateAttendance, createAttendance } from '../api/attendanceService';
import { getMembers } from '../api/memberService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px', marginTop: '14px',
};

function toDatetimeLocal(dateStr) {
  if (!dateStr) return '';
  // Handle "DD/MM/YYYY H:MM:SS AM/PM" format from production DB
  const dmyMatch = dateStr.match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})\s+(\d{1,2}):(\d{2}):?(\d{2})?\s*(AM|PM)?$/i);
  if (dmyMatch) {
    let [, dd, mm, yyyy, hh, min, , ampm] = dmyMatch;
    let h = parseInt(hh);
    if (ampm) {
      if (ampm.toUpperCase() === 'PM' && h < 12) h += 12;
      if (ampm.toUpperCase() === 'AM' && h === 12) h = 0;
    }
    return `${yyyy}-${mm.padStart(2, '0')}-${dd.padStart(2, '0')}T${String(h).padStart(2, '0')}:${min}`;
  }
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`;
}

const getMemberId = (m) => String(m.id || m.Id || m.MemberProfileId || '');

export default function AttendanceDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId') || '';
  const isNew = !id || id === 'new';

  const [loading, setLoading] = useState(!isNew);
  const [saving, setSaving] = useState(false);
  const [chapterName, setChapterName] = useState('');
  const [error, setError] = useState('');

  const [form, setForm] = useState({ AttendanceName: '', AttendanceDesc: '', AttendanceDate: '' });
  const [selectedMembers, setSelectedMembers] = useState([]);
  const [visitors, setVisitors] = useState([]);
  const [membersList, setMembersList] = useState([]);
  const [showMembersModal, setShowMembersModal] = useState(false);
  const [showVisitorsModal, setShowVisitorsModal] = useState(false);

  useEffect(() => {
    (async () => {
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

      // Fetch chapter members
      try {
        const groupId = filterGroupId || '33359';
        const res = await getMembers(groupId);
        const members = res.data?.MemberDetail?.NewMemberList || [];
        setMembersList(members.map(m => ({ Id: String(m.profileID), MemberName: [m.memberName, m.lastName].filter(Boolean).join(' '), profilePic: m.profilePic })));
      } catch {}

      // If editing, fetch existing data separately so one failure doesn't block others
      if (!isNew) {
        // Fetch attendance details
        try {
          const detailRes = await getAttendanceRecord(id);
          const records = detailRes.data?.AttendanceDetailsResult || [];
          const record = records[0] || {};
          setForm({
            AttendanceName: record.AttendanceName || '',
            AttendanceDesc: record.AttendanceDesc || record.Description || '',
            AttendanceDate: toDatetimeLocal(record.AttendanceDate || ''),
          });
        } catch {}

        // Fetch selected members
        try {
          const memRes = await getAttendanceMembers(id);
          const existingMembers = memRes.data?.TBAttendanceMemberDetailsResult?.AttendanceMemberResult || [];
          setSelectedMembers(existingMembers.map(m => ({ Id: String(m.FK_MemberID), MemberName: m.MemberName, MemberProfileId: String(m.FK_MemberID) })));
        } catch {}

        // Fetch visitors
        try {
          const visRes = await getAttendanceVisitors(id);
          const existingVisitors = visRes.data?.TBAttendanceVisitorsDetailsResult?.AttendanceVisitorsResult || [];
          setVisitors(existingVisitors.map(v => ({ VisitorName: v.VisitorsName || v.VisitorName || '', InvitedBy: v.Member_whohas_Brought || '', Type: 'Visitor' })));
        } catch {}
      }
      setLoading(false);
    })();
  }, [id]);

  const toggleMember = (member) => {
    const mid = getMemberId(member);
    const exists = selectedMembers.find(m => getMemberId(m) === mid);
    if (exists) {
      setSelectedMembers(selectedMembers.filter(m => getMemberId(m) !== mid));
    } else {
      setSelectedMembers([...selectedMembers, { ...member, Type: 'Present' }]);
    }
  };

  const handleSave = async () => {
    if (!form.AttendanceName.trim()) { alert('Please Enter Event Name'); return; }
    if (!form.AttendanceDesc.trim()) { alert('Please Enter Description'); return; }
    if (!form.AttendanceDate) { alert('Please Enter Event Date & Time'); return; }
    setSaving(true);
    try {
      const groupId = filterGroupId || '33359';
      const payload = {
        AttendanceID: isNew ? '0' : String(id),
        GroupId: groupId,
        AttendanceName: form.AttendanceName,
        AttendanceDesc: form.AttendanceDesc,
        AttendanceDate: form.AttendanceDate,
        Members: selectedMembers.map(m => ({
          Id: getMemberId(m),
          MemberProfileId: getMemberId(m),
          MemberName: m.MemberName || '',
          Type: m.Type || 'Present',
        })),
        Visitors: visitors.map(v => ({
          VisitorName: v.VisitorName || '',
          InvitedBy: v.InvitedBy || '',
          Type: v.Type || 'Visitor',
        })),
      };
      if (isNew) await createAttendance(payload);
      else await updateAttendance(payload);
      alert(isNew ? 'Attendance added successfully' : 'Attendance updated successfully');
      navigate(-1);
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{chapterName || 'Chapter'}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - {isNew ? 'Add' : 'Edit'} Event Attendance</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleSave} disabled={saving} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1 }}>
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
            {isNew ? 'Save' : 'Update'}
          </button>
          <button onClick={() => navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {error && <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>{error}</div>}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        <div>
          <label style={labelStyle}>Event Name</label>
          <input type="text" value={form.AttendanceName} onChange={(e) => setForm({ ...form, AttendanceName: e.target.value })} style={inputStyle} />
        </div>
        <div>
          <label style={labelStyle}>Description</label>
          <textarea value={form.AttendanceDesc} onChange={(e) => setForm({ ...form, AttendanceDesc: e.target.value })} rows={3} style={{ ...inputStyle, height: 'auto', resize: 'vertical' }} />
        </div>
        <div>
          <label style={labelStyle}>Event Date & Time</label>
          <input type="datetime-local" value={form.AttendanceDate} onChange={(e) => setForm({ ...form, AttendanceDate: e.target.value })} style={{ ...inputStyle, maxWidth: '250px' }} />
        </div>

        {/* Members + Visitors buttons */}
        <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
          <button onClick={() => setShowMembersModal(true)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            {selectedMembers.length} Members +
          </button>
          <button onClick={() => setShowVisitorsModal(true)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            {visitors.length} Visitors +
          </button>
        </div>
      </div>

      {/* Members Selection Modal */}
      <Modal isOpen={showMembersModal} onClose={() => setShowMembersModal(false)} title="Select Members" size="lg">
        <div style={{ maxHeight: '400px', overflowY: 'auto' }}>
          {membersList.length === 0 ? (
            <p style={{ textAlign: 'center', color: '#999', padding: '20px' }}>No members available.</p>
          ) : (
            <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
              <thead>
                <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
                  <th style={{ padding: '8px 12px', textAlign: 'left', fontWeight: 'normal', width: '40px' }}>
                    <input type="checkbox"
                      checked={selectedMembers.length === membersList.length && membersList.length > 0}
                      onChange={(e) => {
                        if (e.target.checked) setSelectedMembers(membersList.map(m => ({ ...m, Type: 'Present' })));
                        else setSelectedMembers([]);
                      }}
                    />
                  </th>
                  <th style={{ padding: '8px 12px', textAlign: 'left', fontWeight: 'normal' }}>Member Name</th>
                </tr>
              </thead>
              <tbody>
                {membersList.map((member, i) => {
                  const mid = getMemberId(member);
                  const isSelected = selectedMembers.some(m => getMemberId(m) === mid);
                  return (
                    <tr key={mid || i} style={{ backgroundColor: i % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee', cursor: 'pointer' }} onClick={() => toggleMember(member)}>
                      <td style={{ padding: '8px 12px' }}>
                        <input type="checkbox" checked={isSelected} onChange={() => toggleMember(member)} />
                      </td>
                      <td style={{ padding: '8px 12px' }}>{member.MemberName || '-'}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          )}
        </div>
        <div style={{ textAlign: 'right', paddingTop: '12px' }}>
          <button onClick={() => setShowMembersModal(false)} style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>Done</button>
        </div>
      </Modal>

      {/* Visitors Modal */}
      <Modal isOpen={showVisitorsModal} onClose={() => setShowVisitorsModal(false)} title="Visitors" size="md">
        {/* Total + Add */}
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '12px' }}>
          <span style={{ fontSize: '13px', fontWeight: '600' }}>Total Visitors {visitors.length}</span>
          <button onClick={() => setVisitors([...visitors, { VisitorName: '', InvitedBy: '', Type: 'Visitor' }])} style={{ backgroundColor: '#2196F3', color: '#fff', border: 'none', padding: '5px 16px', borderRadius: '4px', fontSize: '12px', cursor: 'pointer', fontWeight: 'bold' }}>ADD</button>
        </div>
        {/* Table */}
        {visitors.length > 0 && (
          <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
            <thead>
              <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
                <th style={{ padding: '8px 12px', textAlign: 'left', fontWeight: 'normal' }}>Visitors Name</th>
                <th style={{ padding: '8px 12px', textAlign: 'left', fontWeight: 'normal' }}>Invited By</th>
                <th style={{ padding: '8px 8px', textAlign: 'center', fontWeight: 'normal', width: '40px' }}></th>
              </tr>
            </thead>
            <tbody>
              {visitors.map((v, i) => (
                <tr key={i} style={{ borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '6px 8px' }}>
                    <input type="text" value={v.VisitorName || ''} onChange={(e) => { const u = [...visitors]; u[i] = { ...u[i], VisitorName: e.target.value }; setVisitors(u); }} placeholder="Visitor name" style={{ ...inputStyle, border: '1px solid #ddd' }} />
                  </td>
                  <td style={{ padding: '6px 8px' }}>
                    <input type="text" value={v.InvitedBy || ''} onChange={(e) => { const u = [...visitors]; u[i] = { ...u[i], InvitedBy: e.target.value }; setVisitors(u); }} placeholder="Invited by" style={{ ...inputStyle, border: '1px solid #ddd' }} />
                  </td>
                  <td style={{ padding: '6px 4px', textAlign: 'center' }}>
                    <button onClick={() => setVisitors(visitors.filter((_, idx) => idx !== i))} style={{ background: 'none', border: '2px solid #f44336', borderRadius: '50%', color: '#f44336', cursor: 'pointer', width: '22px', height: '22px', display: 'inline-flex', alignItems: 'center', justifyContent: 'center', fontSize: '12px', lineHeight: 1 }}>&times;</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
        <div style={{ textAlign: 'center', paddingTop: '16px' }}>
          <button onClick={() => setShowVisitorsModal(false)} style={{ display: 'inline-flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 20px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M20 6L9 17l-5-5" /></svg>
            Done
          </button>
        </div>
      </Modal>
    </div>
  );
}