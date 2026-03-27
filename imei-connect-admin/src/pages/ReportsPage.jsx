import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { getClubList } from '../api/groupService';
import { getMembers } from '../api/memberService';

export default function ReportsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');

  const [groups, setGroups] = useState([]);
  const [groupName, setGroupName] = useState('');
  const [membersListGroup, setMembersListGroup] = useState('');
  const [usersListGroup, setUsersListGroup] = useState('');
  const [membersError, setMembersError] = useState('');
  const [usersError, setUsersError] = useState('');

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

  const downloadCSV = (data, filename) => {
    if (!data || data.length === 0) { alert('No records found for download.'); return; }
    const headers = Object.keys(data[0]);
    const csv = [
      headers.join(','),
      ...data.map(row => headers.map(h => `"${(row[h] ?? '').toString().replace(/"/g, '""')}"`).join(','))
    ].join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = filename; a.click();
    URL.revokeObjectURL(url);
  };

  const handleMembersDownload = async () => {
    const gid = filterGroupId || membersListGroup;
    if (!gid) { setMembersError('mandatory'); return; }
    setMembersError('');
    try {
      const res = await getMembers(gid);
      const data = res.data?.MemberDetail?.NewMemberList || [];
      downloadCSV(data, 'MembersList.csv');
    } catch { alert('Failed to download Members List'); }
  };

  const handleUsersDownload = async () => {
    if (!usersListGroup) { setUsersError('mandatory'); return; }
    setUsersError('');
    try {
      const res = await getMembers(usersListGroup);
      const data = res.data?.MemberDetail?.NewMemberList || [];
      downloadCSV(data, 'UsersList.csv');
    } catch { alert('Failed to download Users List'); }
  };

  // Chapter-level Reports (when accessed from chapter dashboard)
  if (filterGroupId) {
    return (
      <div>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
          <div>
            <span style={{ color: '#1a297d', fontSize: '14px' }}>{groupName}</span>
            <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Reports</span>
          </div>
          <button onClick={() => navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>

        <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '15px 25px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '15px', padding: '12px 0' }}>
            <div style={{ fontSize: '13px', fontWeight: '500', color: '#333' }}>Members List</div>
            <div style={{ marginLeft: 'auto' }}>
              <button
                onClick={handleMembersDownload}
                style={{ background: 'none', border: 'none', color: '#1a297d', fontSize: '13px', cursor: 'pointer', textDecoration: 'underline' }}
              >
                Click here to Download
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // National Admin Reports
  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Reports</span>
        </div>
        <button onClick={() => navigate('/dashboard')} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
          <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
          Back
        </button>
      </div>

      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '15px 25px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        {/* Row 1: Members List */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '15px', padding: '12px 0', borderBottom: '1px solid #eee' }}>
          <div style={{ flex: '0 0 150px', fontSize: '13px', fontWeight: '500', color: '#333' }}>Members List</div>
          <div style={{ flex: '0 0 200px' }}>
            <select value={membersListGroup} onChange={(e) => { setMembersListGroup(e.target.value); setMembersError(''); }}
              style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none', backgroundColor: '#fff' }}>
              <option value="">-Select-</option>
              {groups.map(g => <option key={g.Id} value={g.Id}>{g.GrpName}</option>)}
            </select>
          </div>
          {membersError && <span style={{ color: '#dd4b39', fontSize: '12px', fontWeight: '500' }}>{membersError}</span>}
          <div style={{ marginLeft: 'auto' }}>
            <button onClick={handleMembersDownload} style={{ background: 'none', border: 'none', color: '#1a297d', fontSize: '13px', cursor: 'pointer', textDecoration: 'underline' }}>
              Click here to Download
            </button>
          </div>
        </div>

        {/* Row 2: Users List */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '15px', padding: '12px 0' }}>
          <div style={{ flex: '0 0 150px', fontSize: '13px', fontWeight: '500', color: '#333' }}>Users List</div>
          <div style={{ flex: '0 0 200px' }}>
            <select value={usersListGroup} onChange={(e) => { setUsersListGroup(e.target.value); setUsersError(''); }}
              style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none', backgroundColor: '#fff' }}>
              <option value="">-Select-</option>
              {groups.map(g => <option key={g.Id} value={g.Id}>{g.GrpName}</option>)}
            </select>
          </div>
          {usersError && <span style={{ color: '#dd4b39', fontSize: '12px', fontWeight: '500' }}>{usersError}</span>}
          <div style={{ marginLeft: 'auto' }}>
            <button onClick={handleUsersDownload} style={{ background: 'none', border: 'none', color: '#1a297d', fontSize: '13px', cursor: 'pointer', textDecoration: 'underline' }}>
              Click here to Download
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}