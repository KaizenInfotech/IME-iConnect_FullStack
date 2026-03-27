import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getMerItems, getMerYears, createMerItem, updateMerItem, deleteMerItem } from '../api/merService';

const FILTER_OPTIONS = ['All', 'Published', 'Unpublished', 'Expired'];
const currentYear = new Date().getFullYear();
const years = Array.from({ length: 10 }, (_, i) => currentYear - i);

const emptyForm = {
  Title: '', Link: '', FilePath: '', PublishDate: '', ExpiryDate: '', FinanceYear: '', TransType: '1',
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
  return `${dd}/${mm}/${yyyy} ${hh}:${min}:00`;
}

function toDatetimeLocal(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`;
}

function getFilterType(item) {
  const now = new Date();
  const pub = item.PublishDate || item.publishDate;
  const exp = item.ExpiryDate || item.expiryDate;
  if (exp && new Date(exp) < now) return 'Expired';
  if (pub && new Date(pub) <= now) return 'Published';
  return 'Unpublished';
}

function getItemType(item) {
  const filePath = item.FilePath || item.filePath;
  const link = item.Link || item.link;
  if (filePath) return 'File';
  if (link) return 'Link';
  return 'File';
}

export default function MerPage() {
  const navigate = useNavigate();
  const [allItems, setAllItems] = useState([]);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('All');
  const [year, setYear] = useState(currentYear);
  const [searchTerm, setSearchTerm] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => { fetchData(); }, [year]);

  useEffect(() => {
    let filtered = [...allItems];
    if (filter !== 'All') {
      filtered = filtered.filter(item => getFilterType(item) === filter);
    }
    if (searchTerm.trim()) {
      const q = searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        (item.Title || item.title || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
  }, [filter, searchTerm, allItems]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getMerItems(String(year), '1');
      const list = res.data?.TBMERListResult?.MERListResult || [];
      const data = list.map(m => ({
        Id: m.MER_ID, Title: m.Title, Link: m.Link, FilePath: m.File_Path,
        PublishDate: m.publish_date, ExpiryDate: m.expiry_date, FinanceYear: m.FinanceYear,
      }));
      setAllItems(data);
      setItems(data);
    } catch { setError('Failed to load MER items'); }
    finally { setLoading(false); }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!form.Title.trim()) { alert('Please Enter Title'); return; }
    setSaving(true);
    try {
      if (editing) await updateMerItem(editing.Id || editing.id, form);
      else await createMerItem(form);
      setShowModal(false); setEditing(null); setForm(emptyForm); fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteMerItem(deleteTarget.Id || deleteTarget.id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({
      Title: item.Title || item.title || '',
      Link: item.Link || item.link || '',
      FilePath: item.FilePath || item.filePath || '',
      PublishDate: toDatetimeLocal(item.PublishDate || item.publishDate),
      ExpiryDate: toDatetimeLocal(item.ExpiryDate || item.expiryDate),
      FinanceYear: item.FinanceYear || item.financeYear || '',
      TransType: item.TransType || item.transType || '1',
    });
    setShowModal(true);
  };

  const openAdd = () => {
    setEditing(null);
    setForm(emptyForm);
    setShowModal(true);
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - MER (I)</span>
        </div>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
          {/* Search */}
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
            style={{
              height: '32px', border: '1px solid #ccc', borderRadius: '4px',
              padding: '4px 10px', fontSize: '13px', outline: 'none', width: '150px',
            }}
          />
          {/* Search icon button */}
          <button
            style={{
              width: '32px', height: '32px', borderRadius: '50%',
              backgroundColor: '#2196F3', color: '#fff', border: 'none',
              cursor: 'pointer', display: 'flex', alignItems: 'center', justifyContent: 'center',
            }}
          >
            <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
              <circle cx="11" cy="11" r="8" /><path d="M21 21l-4.35-4.35" />
            </svg>
          </button>
          {/* Filter */}
          <select
            value={filter}
            onChange={(e) => setFilter(e.target.value)}
            style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '13px', outline: 'none' }}
          >
            {FILTER_OPTIONS.map(opt => <option key={opt} value={opt}>{opt}</option>)}
          </select>
          {/* Year */}
          <select
            value={year}
            onChange={(e) => setYear(Number(e.target.value))}
            style={{ height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '13px', outline: 'none' }}
          >
            {years.map(y => <option key={y} value={y}>{y}</option>)}
          </select>
          {/* Add */}
          <button
            onClick={() => navigate('/mer/add')}
            style={{
              display: 'flex', alignItems: 'center', gap: '4px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer',
            }}
          >
            + Add
          </button>
          {/* Back */}
          <button
            onClick={() => navigate('/dashboard')}
            style={{
              display: 'flex', alignItems: 'center', gap: '6px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer',
            }}
          >
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
              <path d="M19 12H5M12 19l-7-7 7-7" />
            </svg>
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
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>MER</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr><td colSpan={3} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No MER items found.</td></tr>
            ) : (
              items.map((item, idx) => {
                const title = item.Title || item.title || '';
                const itemType = getItemType(item);
                const pubDate = formatDateDMY(item.PublishDate || item.publishDate);
                const filterType = getFilterType(item);
                const filterColor = filterType === 'Published' ? '#4CAF50' : filterType === 'Expired' ? '#f44336' : '#FF9800';

                return (
                  <tr
                    key={item.Id || item.id || idx}
                    style={{
                      backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8',
                      borderBottom: '1px solid #eee',
                    }}
                  >
                    {/* MER info */}
                    <td style={{ padding: '10px 16px' }}>
                      <div style={{ fontWeight: '600', color: '#333', fontSize: '13px' }}>
                        {title} | {itemType}
                      </div>
                      <div style={{ fontSize: '12px', marginTop: '2px' }}>
                        <span style={{ color: '#666' }}>{pubDate}</span>
                        <span style={{ color: '#666' }}> | </span>
                        <span style={{ color: filterColor, fontWeight: '500' }}>{filterType}</span>
                      </div>
                    </td>
                    {/* Edit - blue square */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => navigate(`/mer/${item.Id || item.id}/edit`)}
                        title="Edit"
                        style={{
                          width: '28px', height: '28px', borderRadius: '4px',
                          backgroundColor: '#2196F3', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                        </svg>
                      </button>
                    </td>
                    {/* Delete - red square */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => setDeleteTarget(item)}
                        title="Delete"
                        style={{
                          width: '28px', height: '28px', borderRadius: '4px',
                          backgroundColor: '#f44336', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                        </svg>
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
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editing ? 'Edit MER' : 'Add MER'} size="lg">
        <form onSubmit={handleSubmit}>
          <label style={{ display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px', marginTop: '10px' }}>
            Title <span style={{ color: '#dd4b39' }}>*</span>
          </label>
          <input type="text" value={form.Title} onChange={(e) => setForm({ ...form, Title: e.target.value })}
            style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none' }} />

          <label style={{ display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px', marginTop: '10px' }}>Link URL</label>
          <input type="text" value={form.Link} onChange={(e) => setForm({ ...form, Link: e.target.value })} placeholder="https://..."
            style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none' }} />

          <div style={{ display: 'flex', gap: '15px' }}>
            <div style={{ flex: 1 }}>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px', marginTop: '10px' }}>Publish Date & Time</label>
              <input type="datetime-local" value={form.PublishDate} onChange={(e) => setForm({ ...form, PublishDate: e.target.value })}
                style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
            <div style={{ flex: 1 }}>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px', marginTop: '10px' }}>Expiry Date & Time</label>
              <input type="datetime-local" value={form.ExpiryDate} onChange={(e) => setForm({ ...form, ExpiryDate: e.target.value })}
                style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
          </div>

          <div style={{ display: 'flex', gap: '15px' }}>
            <div style={{ flex: 1 }}>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px', marginTop: '10px' }}>Finance Year</label>
              <input type="text" value={form.FinanceYear} onChange={(e) => setForm({ ...form, FinanceYear: e.target.value })} placeholder="e.g. 2026"
                style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
            <div style={{ flex: 1 }}>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px', marginTop: '10px' }}>Type</label>
              <select value={form.TransType} onChange={(e) => setForm({ ...form, TransType: e.target.value })}
                style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none', backgroundColor: '#fff' }}>
                <option value="1">MER</option>
                <option value="2">iMélange</option>
              </select>
            </div>
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px', paddingTop: '20px', paddingBottom: '8px' }}>
            <button type="button" onClick={() => setShowModal(false)} style={{ padding: '7px 20px', border: '1px solid #ccc', borderRadius: '4px', background: '#fff', fontSize: '13px', cursor: 'pointer', color: '#333' }}>Cancel</button>
            <button type="submit" disabled={saving} style={{ padding: '7px 20px', borderRadius: '4px', border: 'none', backgroundColor: '#6b9300', color: '#fff', fontSize: '13px', cursor: 'pointer', opacity: saving ? 0.6 : 1 }}>{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>

      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title="Delete MER"
        message={`Are you sure you want to delete "${deleteTarget?.Title || deleteTarget?.title}"?`}
      />
    </div>
  );
}
