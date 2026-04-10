import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import * as XLSX from 'xlsx';
import { useAuth } from '../context/AuthContext';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getAttendanceRecords, getAttendanceMembers, createAttendance, updateAttendance, deleteAttendance } from '../api/attendanceService';
// memberService imported dynamically in fetchMembers

const currentYear = new Date().getFullYear();
const years = [currentYear];

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
    // Filter by year
    if (year) {
      filtered = filtered.filter(item => {
        const dateStr = item.AttendanceDate || item.attendanceDate || '';
        if (!dateStr) return false;
        const d = new Date(dateStr);
        if (!isNaN(d)) return d.getFullYear() === year;
        // Handle DD/MM/YYYY format
        const match = dateStr.match(/(\d{4})/);
        return match && parseInt(match[1]) === year;
      });
    }
    if (searchTerm.trim()) {
      const q = searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        (item.AttendanceName || item.attendanceName || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
  }, [searchTerm, allItems, year]);

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
      const groupId = filterGroupId || '33359';
      const payload = {
        AttendanceID: editing ? String(editing.Id || editing.id || '0') : '0',
        GroupId: groupId,
        AttendanceName: form.AttendanceName,
        AttendanceDesc: form.AttendanceDesc,
        AttendanceDate: form.AttendanceDate,
        Members: selectedMembers.map(m => ({
          Id: String(m.Id || m.id || m.MemberProfileId || ''),
          MemberProfileId: String(m.Id || m.id || m.MemberProfileId || ''),
          MemberName: m.MemberName || m.memberName || '',
          Type: m.Type || 'Present',
        })),
        Visitors: visitors.map(v => ({
          VisitorName: v.VisitorName || v.visitorName || '',
          Type: v.Type || 'Visitor',
        })),
      };
      if (editing) await updateAttendance(payload);
      else await createAttendance(payload);
      alert(editing ? 'Attendance updated successfully' : 'Attendance added successfully');
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
      await deleteAttendance(deleteTarget.Id || deleteTarget.id, '0');
      alert('Attendance deleted successfully');
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = async (item) => {
    // Clear previous state first
    setSelectedMembers([]);
    setVisitors([]);
    setErrors({});
    setEditing(item);
    setForm({
      AttendanceName: item.AttendanceName || item.attendanceName || '',
      AttendanceDesc: item.Description || item.AttendanceDesc || item.attendanceDesc || '',
      AttendanceDate: toDatetimeLocal(item.AttendanceDate || item.attendanceDate),
      GroupId: item.GroupId || item.groupId || 0,
      MemberCount: item.MemberCount || item.member_count || 0,
      VisitorCount: item.VisitorCount || item.visitor_count || 0,
    });

    // Fetch all data BEFORE showing the modal
    const attId = String(item.Id || item.AttendanceID);
    const [, memRes, visRes] = await Promise.all([
      fetchMembers(),
      import('../api/attendanceService').then(mod => mod.getAttendanceMembers(attId)).catch(() => ({ data: {} })),
      import('../api/attendanceService').then(mod => mod.getAttendanceVisitors(attId)).catch(() => ({ data: {} })),
    ]);

    const existingMembers = memRes.data?.TBAttendanceMemberDetailsResult?.AttendanceMemberResult || [];
    const selMembers = existingMembers.map(m => ({ Id: String(m.FK_MemberID), MemberName: m.MemberName, MemberProfileId: String(m.FK_MemberID) }));
    setSelectedMembers(selMembers);

    const existingVisitors = visRes.data?.TBAttendanceMemberDetailsResult?.AttendanceMemberResult || [];
    setVisitors(existingVisitors.map(v => ({ VisitorName: v.MemberName || v.VisitorName, Type: 'Visitor' })));

    // Update counts from actual fetched data
    setForm(prev => ({
      ...prev,
      MemberCount: selMembers.length || item.MemberCount || item.member_count || 0,
      VisitorCount: existingVisitors.length || item.VisitorCount || item.visitor_count || 0,
    }));

    // Show modal only after data is ready
    setShowModal(true);
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

  const getMemberId = (m) => String(m.id || m.Id || m.MemberProfileId || '');

  const toggleMember = (member) => {
    const id = getMemberId(member);
    const exists = selectedMembers.find(m => getMemberId(m) === id);
    if (exists) {
      setSelectedMembers(selectedMembers.filter(m => getMemberId(m) !== id));
    } else {
      setSelectedMembers([...selectedMembers, { ...member, Type: 'Present' }]);
    }
  };

  // ──────────────────────────────────────────────────────────────────────
  //  Reports
  //
  //  Format spec is taken directly from the user's reference files:
  //    /Users/ios2/Downloads/Meeting_wise.xlsx   – real Excel
  //    /Users/ios2/Downloads/Member_wise.xls     – HTML-as-Excel
  //  Both reports are scoped to the currently-selected chapter and the
  //  currently-displayed (year-filtered, search-filtered) list of events.
  // ──────────────────────────────────────────────────────────────────────

  const safeFile = (s) => String(s || 'Chapter').replace(/[^a-z0-9_\-]+/gi, '_');

  const num = (v) => {
    const n = Number(v);
    return isNaN(n) ? 0 : n;
  };

  const exportMeetingWise = () => {
    if (!items || items.length === 0) { alert('No records found for download.'); return; }
    const chapter = chapterName || 'Chapter';

    // Build the sheet as a 2-D array of cell values matching the reference.
    // Rows:
    //   1: chapter name (merged across all columns)
    //   2: column headers
    //   3..N: one row per event
    //   last: totals row
    const headers = [
      'Sr.No', 'Club Name', 'Date', 'Title', 'Description',
      'MembersCount', 'VisitorsCount',
    ];

    const rows = items.map((it, idx) => {
      const dateRaw = it.AttendanceDate || it.attendanceDate || '';
      const d = dateRaw ? new Date(dateRaw) : null;
      return [
        idx + 1,
        chapter,
        d && !isNaN(d) ? d : (dateRaw || ''),
        it.AttendanceName || it.attendanceName || '',
        it.Description || it.AttendanceDesc || '',
        num(it.MemberCount ?? it.member_count),
        num(it.VisitorCount ?? it.visitor_count),
      ];
    });

    // Totals across all data rows (only the count columns).
    const totals = ['', '', '', '', 'Total',
      rows.reduce((s, r) => s + num(r[5]), 0),
      rows.reduce((s, r) => s + num(r[6]), 0),
    ];

    const aoa = [
      [chapter, '', '', '', '', '', ''], // row 1 (will be merged)
      headers,                            // row 2
      ...rows,                            // data
      totals,                             // totals
    ];

    const ws = XLSX.utils.aoa_to_sheet(aoa);

    // Merge the chapter name across all columns on row 1 (A1:G1).
    ws['!merges'] = [{ s: { r: 0, c: 0 }, e: { r: 0, c: headers.length - 1 } }];

    // Format the Date column (column C, index 2) as a date for any cell that
    // holds a real Date object. SheetJS will write the underlying serial number.
    for (let r = 2; r < 2 + rows.length; r++) {   // data rows only
      const addr = XLSX.utils.encode_cell({ r, c: 2 });
      const cell = ws[addr];
      if (cell && cell.v instanceof Date) {
        cell.t = 'd';
        cell.z = 'dd-mmm-yyyy';
      }
    }

    // Reasonable column widths matching the reference.
    ws['!cols'] = [
      { wch: 7 }, { wch: 18 }, { wch: 12 }, { wch: 24 }, { wch: 22 },
      { wch: 14 }, { wch: 14 },
    ];

    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Meeting Wise');
    XLSX.writeFile(wb, `Meeting_wise_${safeFile(chapter)}.xlsx`);
  };

  const exportMemberWise = async () => {
    if (!items || items.length === 0) { alert('No records found for download.'); return; }
    const chapter = chapterName || 'Chapter';
    const groupId = filterGroupId || '33359';

    // Kick off the member-list fetch and the per-event attendee fetches in
    // parallel. We read the chapter member list directly from the API here
    // (instead of from `membersList` state) so this function works even when
    // the page first loads, before any modal has triggered fetchMembers().
    let chapterMembers, perEvent;
    try {
      const { getMembers } = await import('../api/memberService');
      const [memRes, ...attRes] = await Promise.all([
        getMembers(groupId),
        ...items.map(it => getAttendanceMembers(String(it.Id || it.AttendanceID))),
      ]);
      chapterMembers = (memRes.data?.MemberDetail?.NewMemberList || []).map(m => ({
        id: String(m.profileID),
        name: [m.memberName, m.lastName].filter(Boolean).join(' ').trim(),
      }));
      perEvent = items.map((it, idx) => {
        const mems = attRes[idx].data?.TBAttendanceMemberDetailsResult?.AttendanceMemberResult || [];
        return {
          event: it,
          attendingIds: new Set(mems.map(m => String(m.FK_MemberID))),
          attendingNames: mems.map(m => ({ id: String(m.FK_MemberID), name: m.MemberName || '' })),
        };
      });
    } catch {
      alert('Failed to fetch attendance details for the report.');
      return;
    }

    // Build the unique member list. Start with the full chapter directory
    // (so members who never attended still appear with all "No"s, matching
    // the reference Member_wise.xls). Then merge in any additional names
    // that showed up in attendance records but aren't in the directory.
    const memberMap = new Map(); // id -> name
    chapterMembers.forEach(({ id, name }) => { if (id) memberMap.set(id, name); });
    perEvent.forEach(({ attendingNames }) => {
      attendingNames.forEach(({ id, name }) => {
        if (id && !memberMap.has(id)) memberMap.set(id, name);
      });
    });
    if (memberMap.size === 0) {
      alert('No members found for this chapter — cannot build the member-wise report.');
      return;
    }

    // Sort members alphabetically by name (case-insensitive).
    const sortedMembers = Array.from(memberMap.entries())
      .map(([id, name]) => ({ id, name }))
      .sort((a, b) => a.name.localeCompare(b.name, undefined, { sensitivity: 'base' }));

    // Format event date headers like "Wed, 20 Aug 2025 11:20:00".
    const fmtEventDate = (raw) => {
      if (!raw) return '';
      const d = new Date(raw);
      if (isNaN(d)) return raw;
      const wd = ['Sun','Mon','Tue','Wed','Thu','Fri','Sat'][d.getDay()];
      const mo = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][d.getMonth()];
      const day = String(d.getDate()).padStart(2,'0');
      const yr  = d.getFullYear();
      const hh  = String(d.getHours()).padStart(2,'0');
      const mm  = String(d.getMinutes()).padStart(2,'0');
      const ss  = String(d.getSeconds()).padStart(2,'0');
      return `${wd}, ${day} ${mo} ${yr} ${hh}:${mm}:${ss}`;
    };

    // Build the HTML table — same shape as the reference Member_wise.xls.
    const blueHdr = "color:White;background-color:#2d7fc7;";
    const colCount = perEvent.length + 2; // Member name + N events + Total

    const eventTitleRow = `<tr><th>Event Name</th>` +
      perEvent.map(p => `<th>${escapeHtml(p.event.AttendanceName || '')}</th>`).join('') +
      `<th></th></tr>`;

    const eventDateRow = `<tr><th>Member name</th>` +
      perEvent.map(p => `<th>${escapeHtml(fmtEventDate(p.event.AttendanceDate))}</th>`).join('') +
      `<th>Total</th></tr>`;

    const memberRows = sortedMembers.map(({ id, name }) => {
      let total = 0;
      const cells = perEvent.map(p => {
        const present = p.attendingIds.has(id);
        if (present) total += 1;
        return `<td>${present ? 'Yes' : 'No'}</td>`;
      }).join('');
      return `<tr><td>${escapeHtml(name)}</td>${cells}<td>${total}</td></tr>`;
    }).join('');

    const html =
      `<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">` +
      `<font style='font-size:10.0pt; font-family:Calibri;'>` +
      `<BR><BR><BR>` +
      `<table border='1' style='width:100%'>` +
        `<tr><th colspan=${colCount} style='${blueHdr}'>${escapeHtml(chapter)}<br/>&nbsp;</th></tr>` +
        `<tr><th colspan=${colCount} style='${blueHdr}'>Attendance_Report_Member<br/>&nbsp;</th></tr>` +
        eventTitleRow +
        eventDateRow +
        memberRows +
      `</table>` +
      `</font>`;

    const blob = new Blob([html], { type: 'application/vnd.ms-excel' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Member_wise_${safeFile(chapter)}.xls`;
    a.click();
    URL.revokeObjectURL(url);
  };

  function escapeHtml(s) {
    return String(s ?? '')
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;');
  }



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

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>
          {error}<button onClick={() => setError('')} style={{ float: 'right', background: 'none', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>&times;</button>
        </div>
      )}

      {/* Controls Row */}
      <div style={{ display: 'flex', flexWrap: 'wrap', alignItems: 'center', gap: '8px', marginBottom: '15px' }}>
        <input
          type="text"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          placeholder="Search"
          style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none', width: '150px' }}
        />
        <select
          value={year}
          onChange={(e) => setYear(Number(e.target.value))}
          style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '13px', outline: 'none' }}
        >
          {years.map(y => <option key={y} value={y}>{y}</option>)}
        </select>
        <button onClick={exportMeetingWise} style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 12px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: '4px' }} title="Meeting Wise Report">
          <svg width="14" height="14" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
          Meeting Wise Report
        </button>
        <button onClick={exportMemberWise} style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 12px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: '4px' }} title="Member Wise Report">
          <svg width="14" height="14" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
          Member Wise Report
        </button>
        <div style={{ marginLeft: 'auto', display: 'flex', alignItems: 'center', gap: '8px' }}>
          <button onClick={() => navigate(`/attendance/new${filterGroupId ? `?groupId=${filterGroupId}` : ''}`)} style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>+ Add</button>
          <button onClick={() => filterGroupId ? navigate(`/groups/${filterGroupId}`) : navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {/* Table */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
          <thead>
            <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal', width: '5%' }}>Sr.No.</th>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal', width: '70%' }}>Attendance Name</th>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal', width: '10%' }}>Attendance Date</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '5%' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '5%' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr><td colSpan={5} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No Events Found!!!</td></tr>
            ) : (
              items.map((item, idx) => {
                const srNo = idx + 1;
                const name = item.AttendanceName || item.attendanceName || '';
                const date = formatDateDMY(item.AttendanceDate || item.attendanceDate);
                return (
                  <tr
                    key={item.id || item.Id || idx}
                    style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee', cursor: 'pointer' }}
                    onClick={() => navigate(`/attendance/${item.Id || item.id}${filterGroupId ? `?groupId=${filterGroupId}` : ''}`)}
                  >
                    <td style={{ padding: '10px 16px' }}>{srNo}</td>
                    <td style={{ padding: '10px 16px', color: '#1a297d', fontWeight: '500' }}>{name}</td>
                    <td style={{ padding: '10px 16px' }}>{date}</td>
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button onClick={(e) => { e.stopPropagation(); navigate(`/attendance/${item.Id || item.id}${filterGroupId ? `?groupId=${filterGroupId}` : ''}`); }} title="Edit" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </button>
                    </td>
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button onClick={(e) => { e.stopPropagation(); setDeleteTarget(item); }} title="Delete" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#f44336', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
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

          {/* Member Count & Visitor Count */}
          {editing && (
            <div className="flex items-center gap-4 mt-3">
              <div>
                <label className="form-label">Member Count</label>
                <input type="text" value={form.MemberCount || 0} readOnly className="form-input" style={{ backgroundColor: '#eee', width: '100px' }} />
              </div>
              <div>
                <label className="form-label">Visitor Count</label>
                <input type="text" value={form.VisitorCount || 0} readOnly className="form-input" style={{ backgroundColor: '#eee', width: '100px' }} />
              </div>
            </div>
          )}

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
                  const isSelected = selectedMembers.some(m => getMemberId(m) === String(memberId));
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
          style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '5px 12px', borderRadius: '4px', fontSize: '12px', cursor: 'pointer', marginBottom: '12px' }}
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
