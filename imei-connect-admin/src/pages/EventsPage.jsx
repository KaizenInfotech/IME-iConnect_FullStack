import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getEvents, deleteEvent } from '../api/eventService';

function formatDate(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  return d.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' });
}

export default function EventsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [pageName, setPageName] = useState('');
  const [allEvents, setAllEvents] = useState([]);
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [deleteTarget, setDeleteTarget] = useState(null);

  useEffect(() => { fetchData(); }, []);

  useEffect(() => {
    if (!searchTerm.trim()) { setEvents(allEvents); return; }
    const q = searchTerm.toLowerCase();
    setEvents(allEvents.filter(ev => (ev.eventTitle || '').toLowerCase().includes(q)));
  }, [searchTerm, allEvents]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const groupId = filterGroupId || '33359';
      if (filterGroupId) {
        try {
          const { getClubList } = await import('../api/groupService');
          const clubRes = await getClubList();
          const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
          const found = clubs.find(c => String(c.GroupId) === filterGroupId);
          if (found) setPageName(found.group_name);
        } catch {}
      }
      const res = await getEvents(groupId, '13010', '0');
      const records = res.data?.EventsListResult || res.data?.EventListDetailResult?.EventListResult || res.data?.Result || [];
      setAllEvents(Array.isArray(records) ? records : []);
    } catch { setError('Failed to load events'); }
    finally { setLoading(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteEvent(deleteTarget.eventID || deleteTarget.id);
      alert('Event deleted successfully');
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{pageName || 'Chapter'}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Event</span>
        </div>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
          <input
            type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
            style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none', width: '150px' }}
          />
          <button onClick={() => navigate(`/events/new${filterGroupId ? `?groupId=${filterGroupId}` : ''}`)} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>+ Add Event</button>
          <button onClick={() => filterGroupId ? navigate(`/groups/${filterGroupId}`) : navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
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
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal', width: '50px' }}>Sr.No.</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal', width: '120px' }}>Date</th>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>Title</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '100px' }}>Attendance</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '110px' }}>Attendance(%)</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '60px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '60px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {events.length === 0 ? (
              <tr><td colSpan={7} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No events found</td></tr>
            ) : (
              events.map((ev, idx) => (
                <tr key={ev.eventID || idx} style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '10px 12px', color: '#333' }}>{idx + 1}</td>
                  <td style={{ padding: '10px 12px', color: '#333' }}>{formatDate(ev.eventDateTime || ev.EventDate)}</td>
                  <td style={{ padding: '10px 16px', color: '#333' }}>{ev.eventTitle || ev.Title || '-'}</td>
                  <td style={{ padding: '10px 8px', textAlign: 'center', color: '#333' }}>{ev.Attendance || '-'}</td>
                  <td style={{ padding: '10px 8px', textAlign: 'center', color: '#333' }}>{ev.AttendancePercent ? `${ev.AttendancePercent}` : '-'}</td>
                  {/* Edit */}
                  <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                    <button onClick={() => navigate(`/events/${ev.eventID || ev.id}${filterGroupId ? `?groupId=${filterGroupId}` : ''}`)} title="Edit" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                    </button>
                  </td>
                  {/* Delete */}
                  <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                    <button onClick={() => setDeleteTarget(ev)} title="Delete" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#f44336', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Event" message={`Are you sure you want to delete "${deleteTarget?.eventTitle}"?`} />
    </div>
  );
}