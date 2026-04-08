import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getUpcomingEvents, deleteEvent } from '../api/eventService';

const FILTER_OPTIONS = ['All', 'Published', 'Unpublished', 'Expired'];

function parseDMY(dateStr) {
  if (!dateStr) return null;
  const m = dateStr.match(/^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})\s+(\d{1,2}):(\d{2}):?(\d{2})?/);
  if (m) return new Date(+m[3], +m[2] - 1, +m[1], +m[4], +m[5], +(m[6] || 0));
  const d = new Date(dateStr);
  return isNaN(d) ? null : d;
}

function getFilterType(item) {
  const now = new Date();
  const pub = parseDMY(item.PublishDate || item.publishDate);
  const exp = parseDMY(item.ExpiryDate || item.expiryDate);
  if (exp && exp < now) return 'Expired';
  if (pub && pub <= now) return 'Published';
  return 'Unpublished';
}

function parseDate(dateStr) {
  return parseDMY(dateStr);
}

function formatTime(dateStr) {
  const d = parseDate(dateStr);
  if (!d) return '';
  const hh12 = d.getHours() % 12 || 12;
  const min = String(d.getMinutes()).padStart(2, '0');
  const ampm = d.getHours() >= 12 ? 'PM' : 'AM';
  return `${String(hh12).padStart(2, '0')}:${min} ${ampm}`;
}

function getDayNumber(dateStr) {
  const d = parseDate(dateStr);
  return d ? d.getDate() : '';
}

function getYear(dateStr) {
  const d = parseDate(dateStr);
  return d ? d.getFullYear() : '';
}

export default function UpcomingEventsPage() {
  const navigate = useNavigate();
  const [allItems, setAllItems] = useState([]);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('All');
  const [searchTerm, setSearchTerm] = useState('');
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => { fetchData(); }, []);

  useEffect(() => {
    let filtered = [...allItems];
    if (filter !== 'All') {
      filtered = filtered.filter(item => getFilterType(item) === filter);
    }
    if (searchTerm.trim()) {
      const q = searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        (item.EventTitle || item.eventTitle || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
  }, [filter, searchTerm, allItems]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const { getEvents } = await import('../api/eventService');
      const res = await getEvents('31185', '13010', '0');
      const events = res.data?.EventsListResult || [];
      const data = events.map(e => ({
        Id: e.eventID, EventTitle: e.eventTitle, EventDate: e.eventDateTime,
        PublishDate: e.publishDate || e.pubDate || '',
        ExpiryDate: e.expiryDate || '',
        EventVenue: e.venue, EventDesc: '',
        EventImage: e.eventImg, GroupId: e.grpID, RegLink: '',
      }));
      setAllItems(data);
      setItems(data);
    } catch { setError('Failed to load events'); }
    finally { setLoading(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteEvent(String(deleteTarget.Id || deleteTarget.id).replace(/^E/, ''));
      alert('Event deleted successfully');
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  // Build rows without year separators
  const buildRows = () => {
    return items.map(item => ({ type: 'item', item }));
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  const rows = buildRows();

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Upcoming Events</span>
        </div>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
          <input
            type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
            style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none', width: '150px' }}
          />
          <select value={filter} onChange={(e) => setFilter(e.target.value)} style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '13px', outline: 'none' }}>
            {FILTER_OPTIONS.map(opt => <option key={opt} value={opt}>{opt}</option>)}
          </select>
          <button onClick={() => navigate('/events/add')} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>+ Add</button>
          <button onClick={() => navigate('/dashboard')} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
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
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal', width: '50px' }}>Date</th>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>Event</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {rows.length === 0 ? (
              <tr><td colSpan={4} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No upcoming events found.</td></tr>
            ) : (
              rows.map((row, idx) => {
                if (row.type === 'year') {
                  return (
                    <tr key={`year-${row.year}`} style={{ backgroundColor: '#f0f0f0' }}>
                      <td colSpan={4} style={{ padding: '6px 16px', fontWeight: 'bold', fontSize: '12px', color: '#666' }}>{row.year}</td>
                    </tr>
                  );
                }

                const item = row.item;
                const eventDate = item.EventDate || item.eventDate || '';
                const dayNum = getDayNumber(eventDate);
                const time = formatTime(eventDate);
                const title = item.EventTitle || item.eventTitle || '';
                const filterType = getFilterType(item);
                const filterColor = filterType === 'Published' ? '#4CAF50' : filterType === 'Expired' ? '#f44336' : '#FF9800';
                const rsvp = item.RsvpEnable || item.rsvpEnable;

                return (
                  <tr key={item.Id || item.id || idx} style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee' }}>
                    {/* Date */}
                    <td style={{ padding: '10px 12px', fontWeight: 'bold', fontSize: '12px', color: '#333', verticalAlign: 'top' }}>
                      {eventDate ? new Date(eventDate).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) : ''}
                    </td>
                    {/* Event details */}
                    <td style={{ padding: '10px 16px' }}>
                      <div style={{ fontWeight: '600', color: '#333', fontSize: '13px' }}>{title}</div>
                      <div style={{ fontSize: '12px', marginTop: '2px', color: '#666' }}>{time}</div>
                      <div style={{ fontSize: '11px', marginTop: '2px' }}>
                        <span style={{ color: '#666' }}>{item.EventVenue || item.eventVenue || item.EventDesc || ''}</span>
                        <span style={{ color: '#666' }}> | </span>
                        <span style={{ color: filterColor, fontWeight: '500' }}>{filterType}</span>
                        {(rsvp === '1' || rsvp === 'true' || rsvp === true) && (
                          <>
                            <span style={{ color: '#666' }}> | </span>
                            <span style={{ color: '#1a297d', cursor: 'pointer', textDecoration: 'underline' }}>RSVP Report</span>
                          </>
                        )}
                      </div>
                    </td>
                    {/* Edit */}
                    <td style={{ padding: '10px 8px', textAlign: 'center', verticalAlign: 'top' }}>
                      <button onClick={() => navigate(`/events/${item.Id || item.id}/edit`)} title="Edit" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </button>
                    </td>
                    {/* Delete */}
                    <td style={{ padding: '10px 8px', textAlign: 'center', verticalAlign: 'top' }}>
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

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Event" message={`Are you sure you want to delete "${deleteTarget?.EventTitle || deleteTarget?.eventTitle}"?`} />
    </div>
  );
}
