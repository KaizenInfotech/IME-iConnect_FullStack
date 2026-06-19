import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { getClubList } from '../api/groupService';
import { getMembers } from '../api/memberService';

const ALL = 'ALL';

export default function ReportsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');

  const [groups, setGroups] = useState([]);
  const [groupName, setGroupName] = useState('');
  // When opened from a chapter, default both selectors to that chapter.
  const [membersListGroup, setMembersListGroup] = useState(filterGroupId || '');
  const [usersListGroup, setUsersListGroup] = useState(filterGroupId || '');
  const [membersError, setMembersError] = useState('');
  const [usersError, setUsersError] = useState('');
  const [busy, setBusy] = useState('');

  useEffect(() => {
    (async () => {
      try {
        const res = await getClubList();
        const clubs = res.data?.TBGetClubResult?.ClubResult?.Table || [];
        const mapped = clubs.map(c => ({ Id: c.GroupId, GrpName: c.group_name }));
        setGroups(mapped);
        if (filterGroupId) {
          const found = mapped.find(g => String(g.Id) === filterGroupId);
          setGroupName(found?.GrpName || 'Chapter');
        }
      } catch {}
    })();
  }, []);

  const downloadExcel = (data, headers, filename) => {
    if (!data || data.length === 0) { alert('No records found for download.'); return; }
    const headerRow = headers.map(h => `<th style="background-color:#1a297d;color:#fff;font-weight:bold;padding:5px 10px;border:1px solid #000;font-size:12px;">${h}</th>`).join('');
    const bodyRows = data.map(row =>
      headers.map(h => `<td style="padding:4px 8px;border:1px solid #ccc;font-size:12px;">${row[h] ?? ''}</td>`).join('')
    ).map(r => `<tr>${r}</tr>`).join('');
    const html = `<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40">
      <head><meta charset="utf-8"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Sheet1</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head>
      <body><table border="1" cellpadding="0" cellspacing="0"><thead><tr>${headerRow}</tr></thead><tbody>${bodyRows}</tbody></table></body></html>`;
    const blob = new Blob([html], { type: 'application/vnd.ms-excel' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = filename; a.click();
    URL.revokeObjectURL(url);
  };

  const formatDevicePlatform = (p) => {
    if (!p) return '';
    const v = String(p).toLowerCase();
    if (v === 'ios') return 'iOS';
    if (v === 'android') return 'Android';
    return p;
  };

  const formatDOB = (dateStr) => {
    if (!dateStr) return '';
    // Handle dd/MM/yyyy format
    if (dateStr.includes('/')) return dateStr;
    // Handle yyyy-MM-dd or ISO format
    const d = new Date(dateStr);
    if (isNaN(d)) return dateStr;
    return `${String(d.getDate()).padStart(2, '0')}/${String(d.getMonth() + 1).padStart(2, '0')}/${d.getFullYear()}`;
  };

  // Run fn over items with limited concurrency so "All" doesn't fire hundreds
  // of requests at once.
  const mapLimit = async (items, limit, fn) => {
    const out = [];
    for (let i = 0; i < items.length; i += limit) {
      const batch = items.slice(i, i + limit);
      out.push(...await Promise.all(batch.map(fn)));
    }
    return out;
  };

  // Returns a flat list of { m, branchName } for one chapter or every chapter.
  const collectMembers = async (gid) => {
    const targetIds = gid === ALL ? groups.map(g => String(g.Id)) : [String(gid)];
    const perGroup = await mapLimit(targetIds, 6, async (id) => {
      try {
        const res = await getMembers(id);
        const raw = res.data?.MemberDetail?.NewMemberList || [];
        const branchName = groups.find(g => String(g.Id) === id)?.GrpName || '';
        return raw.map(m => ({ m, branchName }));
      } catch { return []; }
    });
    return perGroup.flat();
  };

  const handleMembersDownload = async () => {
    const gid = membersListGroup;
    if (!gid) { setMembersError('mandatory'); return; }
    setMembersError('');
    setBusy('members');
    try {
      const rows = await collectMembers(gid);
      if (!rows.length) { alert('No records found for download.'); return; }
      const memberHeaders = ['SrNo', 'Branch & Chapter', 'MemberName', 'MemberShipId', 'Email', 'MobileNumber', 'DOB', 'Device name'];
      const data = rows.map(({ m, branchName }, idx) => ({
        SrNo: idx + 1,
        'Branch & Chapter': m.GrpName || branchName,
        MemberName: [m.memberName, m.lastName].filter(Boolean).join(' '),
        MemberShipId: m.member_IMEI_id || '',
        Email: m.memberEmail || '',
        MobileNumber: m.memberMobile || '',
        DOB: formatDOB(m.member_date_of_birth),
        'Device name': formatDevicePlatform(m.device_platform),
      }));
      downloadExcel(data, memberHeaders, gid === ALL ? 'AllMembersList.xls' : 'MembersList.xls');
    } catch { alert('Failed to download Members List'); }
    finally { setBusy(''); }
  };

  const handleUsersDownload = async () => {
    const gid = usersListGroup;
    if (!gid) { setUsersError('mandatory'); return; }
    setUsersError('');
    setBusy('users');
    try {
      const rows = await collectMembers(gid);
      if (!rows.length) { alert('No records found for download.'); return; }
      const userHeaders = ['SrNo', 'Branch & Chapter', 'MemberShipId', 'MemberName', 'MobileNo', 'EmailID', 'Device name'];
      const data = rows.map(({ m, branchName }, idx) => ({
        SrNo: idx + 1,
        'Branch & Chapter': m.GrpName || branchName,
        MemberShipId: m.member_IMEI_id || '',
        MemberName: [m.memberName, m.lastName].filter(Boolean).join(' '),
        MobileNo: m.memberMobile || '',
        EmailID: m.memberEmail || '',
        'Device name': formatDevicePlatform(m.device_platform),
      }));
      downloadExcel(data, userHeaders, gid === ALL ? 'AllUsersList.xls' : 'UsersList.xls');
    } catch { alert('Failed to download Users List'); }
    finally { setBusy(''); }
  };

  // When opened inside a specific branch/chapter, the report is locked to that
  // chapter: no selector, no other branches, and only the Members List.
  const isChapter = !!filterGroupId;
  const headerLabel = filterGroupId ? groupName : 'National Admin';
  const handleBack = () => filterGroupId ? navigate(`/groups/${filterGroupId}`) : navigate('/dashboard');

  const selectStyle = { width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none', backgroundColor: '#fff' };
  const linkStyle = { background: 'none', border: 'none', color: '#1a297d', fontSize: '13px', cursor: 'pointer', textDecoration: 'underline' };

  const groupOptions = (
    <>
      <option value="">-Select-</option>
      <option value={ALL}>All</option>
      {groups.map(g => <option key={g.Id} value={g.Id}>{g.GrpName}</option>)}
    </>
  );

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{headerLabel}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Reports</span>
        </div>
        <button onClick={handleBack} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
          <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
          Back
        </button>
      </div>

      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '15px 25px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        {/* Row 1: Members List */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '15px', padding: '12px 0', borderBottom: isChapter ? 'none' : '1px solid #eee' }}>
          <div style={{ flex: '0 0 150px', fontSize: '13px', fontWeight: '500', color: '#333' }}>Members List</div>
          {!isChapter && (
            <>
              <div style={{ flex: '0 0 200px' }}>
                <select value={membersListGroup} onChange={(e) => { setMembersListGroup(e.target.value); setMembersError(''); }} style={selectStyle}>
                  {groupOptions}
                </select>
              </div>
              {membersError && <span style={{ color: '#dd4b39', fontSize: '12px', fontWeight: '500' }}>{membersError}</span>}
            </>
          )}
          <div style={{ marginLeft: 'auto' }}>
            <button onClick={handleMembersDownload} disabled={busy === 'members'} style={{ ...linkStyle, cursor: busy === 'members' ? 'wait' : 'pointer' }}>
              {busy === 'members' ? 'Downloading…' : 'Click here to Download'}
            </button>
          </div>
        </div>

        {/* Row 2: Users List — global report only; a chapter report shows the Members List alone */}
        {!isChapter && (
          <div style={{ display: 'flex', alignItems: 'center', gap: '15px', padding: '12px 0' }}>
            <div style={{ flex: '0 0 150px', fontSize: '13px', fontWeight: '500', color: '#333' }}>Users List</div>
            <div style={{ flex: '0 0 200px' }}>
              <select value={usersListGroup} onChange={(e) => { setUsersListGroup(e.target.value); setUsersError(''); }} style={selectStyle}>
                {groupOptions}
              </select>
            </div>
            {usersError && <span style={{ color: '#dd4b39', fontSize: '12px', fontWeight: '500' }}>{usersError}</span>}
            <div style={{ marginLeft: 'auto' }}>
              <button onClick={handleUsersDownload} disabled={busy === 'users'} style={{ ...linkStyle, cursor: busy === 'users' ? 'wait' : 'pointer' }}>
                {busy === 'users' ? 'Downloading…' : 'Click here to Download'}
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
