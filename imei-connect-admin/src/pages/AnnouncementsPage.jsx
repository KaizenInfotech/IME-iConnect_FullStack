import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getAnnouncements, deleteAnnouncement } from '../api/announcementService';

const FILTER_OPTIONS = ['All', 'Published', 'Unpublished', 'Expired'];

function formatDateDMY(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  const dd = String(d.getDate()).padStart(2, '0');
  const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  const mmm = months[d.getMonth()];
  const yyyy = d.getFullYear();
  const min = String(d.getMinutes()).padStart(2, '0');
  const ampm = d.getHours() >= 12 ? 'PM' : 'AM';
  const hh12 = d.getHours() % 12 || 12;
  return `${dd} ${mmm} ${yyyy} ${String(hh12).padStart(2, '0')}:${min} ${ampm}`;
}

function getFilterType(item) {
  const now = new Date();
  const pub = item.PublishDate || item.publishDate;
  const exp = item.ExpiryDate || item.expiryDate;
  if (exp && new Date(exp) < now) return 'Expired';
  if (pub && new Date(pub) <= now) return 'Published';
  return 'Unpublished';
}

export default function AnnouncementsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [allItems, setAllItems] = useState([]);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('All');
  const [searchTerm, setSearchTerm] = useState('');
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');
  const [pageName, setPageName] = useState('National Admin');

  useEffect(() => { fetchData(); }, []);

  useEffect(() => {
    let filtered = [...allItems];
    if (filter !== 'All') {
      filtered = filtered.filter(item => getFilterType(item) === filter);
    }
    if (searchTerm.trim()) {
      const q = searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        (item.AnnounTitle || item.announTitle || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
  }, [filter, searchTerm, allItems]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const groupId = filterGroupId || '31185';
      const profileId = '13010';
      if (filterGroupId) {
        try {
          const { getClubList } = await import('../api/groupService');
          const clubRes = await getClubList();
          const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
          const found = clubs.find(c => String(c.GroupId) === filterGroupId);
          if (found) setPageName(found.group_name);
        } catch {}
      }
      const res = await getAnnouncements(groupId, profileId);
      const list = res.data?.TBAnnounceListResult?.AnnounListResult || [];
      const data = list.map(item => {
        const a = item.AnnounceList || item;
        return { ...a, Id: a.announID, AnnounTitle: a.announTitle, PublishDate: a.publishDateTime, ExpiryDate: a.expiryDateTime };
      });
      setAllItems(data);
      setItems(data);
    } catch { setError('Failed to load announcements'); }
    finally { setLoading(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteAnnouncement(deleteTarget.Id || deleteTarget.id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{pageName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Announcements</span>
        </div>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
          {/* Search */}
          <input
            type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
            style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none', width: '150px' }}
          />
          {/* Search icon */}
          <button style={{ width: '32px', height: '32px', borderRadius: '50%', backgroundColor: '#2196F3', color: '#fff', border: 'none', cursor: 'pointer', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><circle cx="11" cy="11" r="8" /><path d="M21 21l-4.35-4.35" /></svg>
          </button>
          {/* Filter */}
          <select value={filter} onChange={(e) => setFilter(e.target.value)} style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '13px', outline: 'none' }}>
            {FILTER_OPTIONS.map(opt => <option key={opt} value={opt}>{opt}</option>)}
          </select>
          {/* Add */}
          <button onClick={() => navigate('/announcements/add')} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            + Add
          </button>
          {/* Back */}
          <button onClick={() => navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>
          {error}<button onClick={() => setError('')} style={{ float: 'right', background: 'none', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>&times;</button>
        </div>
      )}

      {/* Table */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
          <thead>
            <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>Announcements</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr><td colSpan={3} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No announcements found.</td></tr>
            ) : (
              items.map((item, idx) => {
                const title = item.AnnounTitle || item.announTitle || '';
                const pubDate = formatDateDMY(item.PublishDate || item.publishDate);
                const filterType = getFilterType(item);
                const filterColor = filterType === 'Published' ? '#4CAF50' : filterType === 'Expired' ? '#f44336' : '#FF9800';
                return (
                  <tr key={item.Id || item.id || idx} style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee' }}>
                    <td style={{ padding: '10px 16px' }}>
                      <div style={{ fontWeight: '600', color: '#333', fontSize: '13px' }}>{title}</div>
                      <div style={{ fontSize: '12px', marginTop: '2px' }}>
                        <span style={{ color: '#666' }}>{pubDate}</span>
                        <span style={{ color: '#666' }}> | </span>
                        <span style={{ color: filterColor, fontWeight: '500' }}>{filterType}</span>
                      </div>
                    </td>
                    {/* Edit - green square */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button onClick={() => navigate(`/announcements/${item.Id || item.id}/edit`)} title="Edit" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </button>
                    </td>
                    {/* Delete - red square */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button onClick={() => setDeleteTarget(item)} title="Delete" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#f44336', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
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

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Announcement" message={`Are you sure you want to delete "${deleteTarget?.AnnounTitle || deleteTarget?.announTitle}"?`} />
    </div>
  );
}
