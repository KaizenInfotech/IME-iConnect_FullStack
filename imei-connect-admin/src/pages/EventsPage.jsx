import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getEvents, deleteEvent } from '../api/eventService';

export default function EventsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [pageName, setPageName] = useState('National Admin');
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalRecords, setTotalRecords] = useState(0);

  // Filters
  const [year, setYear] = useState(new Date().getFullYear().toString());
  const [search, setSearch] = useState('');
  const [filter, setFilter] = useState('All');

  useEffect(() => { fetchData(); }, [page, year, filter]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const groupId = filterGroupId || '33359';
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
      const res = await getEvents(groupId, profileId, '0');
      const records = res.data?.EventListDetailResult?.EventListResult || res.data?.Result || [];
      setEvents(Array.isArray(records) ? records : []);
      setTotalPages(1);
      setTotalRecords(Array.isArray(records) ? records.length : 0);
    } catch {
      setError('Failed to load events');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = () => {
    setPage(1);
    fetchData();
  };

  const handleDelete = async (ev) => {
    if (!window.confirm('Are you sure you want to delete?')) return;
    try {
      await deleteEvent(ev.id || ev._id);
      fetchData();
    } catch {
      setError('Delete failed');
    }
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return '-';
    try {
      const d = new Date(dateStr);
      const day = String(d.getDate()).padStart(2, '0');
      const month = String(d.getMonth() + 1).padStart(2, '0');
      const yr = d.getFullYear();
      return `${day}/${month}/${yr}`;
    } catch {
      return dateStr.substring(0, 10);
    }
  };

  // Year options
  const currentYear = new Date().getFullYear();
  const yearOptions = [];
  for (let y = currentYear; y >= currentYear - 5; y--) {
    yearOptions.push(y.toString());
  }

  // Pagination
  const renderPagination = () => {
    if (totalPages <= 1) return null;
    const pages = [];
    const maxVisible = 5;
    let start = Math.max(1, page - Math.floor(maxVisible / 2));
    let end = Math.min(totalPages, start + maxVisible - 1);
    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    pages.push(
      <button
        key="prev"
        onClick={() => setPage((p) => Math.max(1, p - 1))}
        disabled={page === 1}
        className="px-[8px] py-[4px] border border-[#ddd] bg-white text-[12px] disabled:opacity-50 disabled:cursor-not-allowed hover:bg-[#f5f5f5]"
      >
        &laquo;
      </button>
    );
    for (let i = start; i <= end; i++) {
      pages.push(
        <button
          key={i}
          onClick={() => setPage(i)}
          className={`px-[10px] py-[4px] border border-[#ddd] text-[12px] ${
            i === page ? 'bg-[#1a297d] text-white' : 'bg-white text-[#333] hover:bg-[#f5f5f5]'
          }`}
        >
          {i}
        </button>
      );
    }
    pages.push(
      <button
        key="next"
        onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
        disabled={page === totalPages}
        className="px-[8px] py-[4px] border border-[#ddd] bg-white text-[12px] disabled:opacity-50 disabled:cursor-not-allowed hover:bg-[#f5f5f5]"
      >
        &raquo;
      </button>
    );
    return <div className="flex items-center justify-center gap-[2px] mt-[10px]">{pages}</div>;
  };

  if (loading && events.length === 0) return <LoadingSpinner />;

  return (
    <div>
      {/* Page Title */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{pageName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>{filterGroupId ? ' - Past Events' : ' - Events'}</span>
        </div>
      </div>

      {/* Top Controls */}
      <div className="flex items-center gap-[8px] mb-[15px] flex-wrap">
        {/* Filter dropdown - left */}
        <select
          value={filter}
          onChange={(e) => { setFilter(e.target.value); setPage(1); }}
          className="h-[35px] rounded-[8px] border border-[#d0d0d0] px-[10px] text-[13px] outline-none"
        >
          <option value="All">All</option>
          <option value="Published">Published</option>
          <option value="Unpublished">Unpublished</option>
          <option value="Expired">Expired</option>
        </select>
        <select
          value={year}
          onChange={(e) => { setYear(e.target.value); setPage(1); }}
          className="h-[35px] rounded-[8px] border border-[#d0d0d0] px-[10px] text-[13px] outline-none"
        >
          {yearOptions.map((y) => (
            <option key={y} value={y}>{y}</option>
          ))}
        </select>
        <input
          type="text"
          placeholder="Search..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
          className="h-[35px] rounded-[8px] border border-[#d0d0d0] px-[10px] text-[13px] outline-none w-[200px]"
        />
        {/* Add Album & Back - right */}
        <div className="ml-auto flex items-center gap-[8px]">
          <button
            onClick={() => navigate('/events/new')}
            className="px-[12px] py-[6px] text-[13px] text-white rounded-[4px] border-0 cursor-pointer"
            style={{ backgroundColor: '#e3a712' }}
          >
            Add Album
          </button>
          <button
            onClick={() => navigate(-1)}
            className="px-[16px] py-[6px] text-[13px] text-white rounded-[4px] border-0 cursor-pointer bg-[#1a297d]"
          >
            ← Back
          </button>
        </div>
      </div>

      {error && (
        <div className="bg-[#fdf2f2] text-[#dd4b39] px-[15px] py-[10px] rounded-[4px] mb-[15px] text-[13px]">
          {error}
        </div>
      )}

      {/* Table */}
      <div
        className="bg-white border border-[#eee] rounded-[8px]"
        style={{ padding: '25px 28px 10px 28px' }}
      >
        <table className="w-full border-collapse text-[13px]">
          <thead>
            <tr className="bg-[#1a297d] text-white">
              <th className="p-[8px] text-left font-normal" style={{ width: '5%' }}>Sr.No.</th>
              <th className="p-[8px] text-left font-normal" style={{ width: '10%' }}>Date</th>
              <th className="p-[8px] text-left font-normal" style={{ width: '65%' }}>Title</th>
              <th className="p-[8px] text-center font-normal" style={{ width: '5%' }}>Attendance</th>
              <th className="p-[8px] text-center font-normal" style={{ width: '5%' }}>Attendance(%)</th>
              <th className="p-[8px] text-center font-normal" style={{ width: '5%' }}>Edit</th>
              <th className="p-[8px] text-center font-normal" style={{ width: '5%' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {events.length === 0 ? (
              <tr>
                <td colSpan={7} className="p-[15px] text-center text-[#999]">
                  No events found
                </td>
              </tr>
            ) : (
              events.map((ev, idx) => (
                <tr
                  key={ev.id || ev._id || idx}
                  className={`border-b border-[#eee] hover:bg-[#f9f9f9] ${
                    idx % 2 === 0 ? 'bg-white' : 'bg-[#f9f9f9]'
                  }`}
                >
                  <td className="p-[8px]">{(page - 1) * 20 + idx + 1}</td>
                  <td className="p-[8px]">{formatDate(ev.EventDate)}</td>
                  <td className="p-[8px]">{ev.Title || '-'}</td>
                  <td className="p-[8px] text-center">{ev.Attendance ?? ev.AttendanceCount ?? '-'}</td>
                  <td className="p-[8px] text-center">
                    {ev.AttendancePercent != null ? `${ev.AttendancePercent}%` : '-'}
                  </td>
                  <td className="p-[8px] text-center">
                    <button
                      onClick={() => navigate(`/events/${ev.id || ev._id}`)}
                      className="text-[#1a297d] hover:text-[#092c5e] border-0 bg-transparent cursor-pointer"
                      title="Edit"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                    </button>
                  </td>
                  <td className="p-[8px] text-center">
                    <button
                      onClick={() => handleDelete(ev)}
                      className="text-[#e87e04] hover:text-[#c56a00] border-0 bg-transparent cursor-pointer"
                      title="Delete"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><line x1="10" y1="11" x2="10" y2="17"/><line x1="14" y1="11" x2="14" y2="17"/></svg>
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>

        {/* Pagination */}
        {renderPagination()}
        {totalRecords > 0 && (
          <div className="text-[11px] text-[#999] text-center mt-[6px] mb-[10px]">
            Showing {(page - 1) * 20 + 1} - {Math.min(page * 20, totalRecords)} of {totalRecords} records
          </div>
        )}
      </div>
    </div>
  );
}
