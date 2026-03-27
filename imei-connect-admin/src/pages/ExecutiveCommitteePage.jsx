import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getBodList } from '../api/memberService';

export default function ExecutiveCommitteePage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const groupId = searchParams.get('groupId') || '';
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [chapterName, setChapterName] = useState('');

  useEffect(() => { fetchData(); }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getBodList(groupId);
      const members = res.data?.TBGetBODResult?.BODResult || [];
      setItems(members);
      // Get chapter name from club list
      try {
        const { getClubList } = await import('../api/groupService');
        const clubRes = await getClubList();
        const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
        const found = clubs.find(c => String(c.GroupId) === groupId);
        if (found) setChapterName(found.group_name);
      } catch {}
    } catch {}
    finally { setLoading(false); }
  };

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{chapterName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Executive Committee</span>
        </div>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
          <button
            style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#217346', color: '#fff', border: 'none', padding: '6px 12px', borderRadius: '4px', fontSize: '12px', cursor: 'pointer' }}
            onClick={() => {
              if (items.length === 0) { alert('No records found'); return; }
              const headers = ['Name', 'Designation', 'Mobile Number', 'Email'];
              const csv = [headers.join(','), ...items.map(m => [
                `"${m.memberName || ''}"`, `"${m.MemberDesignation || ''}"`, `"${m.membermobile || ''}"`, `"${m.Email || ''}"`
              ].join(','))].join('\n');
              const blob = new Blob([csv], { type: 'text/csv' });
              const url = URL.createObjectURL(blob);
              const a = document.createElement('a'); a.href = url; a.download = 'ExecutiveCommittee.csv'; a.click();
            }}
          >
            <svg width="14" height="14" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
            Export To Excel
          </button>
          <button onClick={() => navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {/* Table */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '12px' }}>
          <thead>
            <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Name</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Designation</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Mobile Number</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Email</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '60px' }}>Receive</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr><td colSpan={5} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No Record Found!</td></tr>
            ) : (
              items.map((m, idx) => (
                <tr key={m.profileID || idx} style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '8px 12px', color: '#333' }}>{m.memberName}</td>
                  <td style={{ padding: '8px 12px', color: '#555' }}>{m.MemberDesignation}</td>
                  <td style={{ padding: '8px 12px', color: '#555' }}>{m.membermobile}</td>
                  <td style={{ padding: '8px 12px', color: '#555', fontSize: '11px' }}>{m.Email}</td>
                  <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                    <input type="checkbox" defaultChecked style={{ cursor: 'pointer' }} />
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}