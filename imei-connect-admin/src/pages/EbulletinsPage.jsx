import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getEbulletins, createEbulletin, updateEbulletin, deleteEbulletin } from '../api/ebulletinService';

const FILTER_OPTIONS = ['All', 'Published', 'Unpublished', 'Expired'];
const TYPES = ['Newsletter', 'Bulletin', 'Report', 'Magazine'];
const PAGE_SIZE = 10;

const emptyForm = {
  EbulletinTitle: '',
  EbulletinLink: '',
  EbulletinType: 'Newsletter',
  PublishDate: '',
  ExpiryDate: '',
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

function getFilterType(item) {
  const now = new Date();
  const pub = item.PublishDate || item.publishDate;
  const exp = item.ExpiryDate || item.expiryDate;
  if (exp && new Date(exp) < now) return 'Expired';
  if (pub && new Date(pub) <= now) return 'Published';
  return 'Unpublished';
}

export default function EbulletinsPage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const groupName = user?.GrpName || user?.groupName || user?.ClubName || 'Group';

  const [allItems, setAllItems] = useState([]);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('All');
  const [searchTerm, setSearchTerm] = useState('');
  const [pageNo, setPageNo] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');
  const [errors, setErrors] = useState({});

  useEffect(() => { fetchData(); }, []);

  useEffect(() => {
    let filtered = [...allItems];
    if (filter !== 'All') {
      filtered = filtered.filter(item => getFilterType(item) === filter);
    }
    if (searchTerm.trim()) {
      const q = searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        (item.EbulletinTitle || item.ebulletinTitle || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
    setPageNo(1);
  }, [filter, searchTerm, allItems]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getEbulletins();
      const data = res.data?.data || res.data || [];
      setAllItems(data);
      setItems(data);
    } catch { setError('Failed to load e-bulletins'); }
    finally { setLoading(false); }
  };

  const validate = () => {
    const errs = {};
    if (!form.EbulletinTitle.trim()) errs.EbulletinTitle = 'mandatory';
    if (!form.PublishDate) errs.PublishDate = 'mandatory';
    if (!form.ExpiryDate) errs.ExpiryDate = 'mandatory';
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;
    setSaving(true);
    try {
      if (editing) await updateEbulletin(editing.id || editing.Id, form);
      else await createEbulletin(form);
      setShowModal(false);
      setEditing(null);
      setForm(emptyForm);
      setErrors({});
      fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteEbulletin(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({
      EbulletinTitle: item.EbulletinTitle || item.ebulletinTitle || '',
      EbulletinLink: item.EbulletinLink || item.ebulletinLink || '',
      EbulletinType: item.EbulletinType || item.ebulletinType || 'Newsletter',
      PublishDate: toDatetimeLocal(item.PublishDate || item.publishDate),
      ExpiryDate: toDatetimeLocal(item.ExpiryDate || item.expiryDate),
    });
    setErrors({});
    setShowModal(true);
  };

  const openAdd = () => {
    setEditing(null);
    setForm(emptyForm);
    setErrors({});
    setShowModal(true);
  };

  // Pagination
  const totalPages = Math.ceil(items.length / PAGE_SIZE);
  const paginatedItems = items.slice((pageNo - 1) * PAGE_SIZE, pageNo * PAGE_SIZE);

  const renderPagination = () => {
    if (totalPages <= 1) return null;
    const pages = [];
    for (let i = 1; i <= totalPages; i++) pages.push(i);
    return (
      <div className="gridPager flex items-center justify-center gap-0 py-3">
        <a href="#" onClick={(e) => { e.preventDefault(); setPageNo(1); }}>First</a>
        <a href="#" onClick={(e) => { e.preventDefault(); if (pageNo > 1) setPageNo(pageNo - 1); }}>&laquo;</a>
        {pages.map(p => (
          p === pageNo
            ? <span key={p}>{p}</span>
            : <a key={p} href="#" onClick={(e) => { e.preventDefault(); setPageNo(p); }}>{p}</a>
        ))}
        <a href="#" onClick={(e) => { e.preventDefault(); if (pageNo < totalPages) setPageNo(pageNo + 1); }}>&raquo;</a>
        <a href="#" onClick={(e) => { e.preventDefault(); setPageNo(totalPages); }}>Last</a>
      </div>
    );
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Page Title */}
      <div className="flex items-center justify-between mb-4">
        <h1 className="page-title">{groupName} E-Bulletins List</h1>
      </div>

      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}<button onClick={() => setError('')} className="float-right font-bold">&times;</button></div>}

      {/* Controls Row */}
      <div className="card mb-4">
        <div className="flex flex-wrap items-center gap-3">
          {/* Filter */}
          <select
            value={filter}
            onChange={(e) => setFilter(e.target.value)}
            className="form-select"
            style={{ width: 'auto', minWidth: 140 }}
          >
            {FILTER_OPTIONS.map(opt => <option key={opt} value={opt}>{opt}</option>)}
          </select>

          {/* Search */}
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search..."
            className="form-input"
            style={{ width: 'auto', maxWidth: 220 }}
          />
          <button className="btn-primary text-sm flex items-center gap-1">
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
            Search
          </button>

          <div className="ml-auto flex items-center gap-2">
            <button onClick={openAdd} className="btn-warning text-sm">Add</button>
            <button onClick={() => navigate(-1)} className="btn-outline text-sm">Back</button>
          </div>
        </div>
      </div>

      {/* Table */}
      <div className="card p-0 overflow-hidden">
        <table className="w-full text-sm">
          <thead>
            <tr className="table-header">
              <th className="px-4 py-3 text-left font-normal">Title</th>
              <th className="px-4 py-3 text-left font-normal">Type</th>
              <th className="px-4 py-3 text-left font-normal">Publish Date</th>
              <th className="px-4 py-3 text-left font-normal">Expiry Date</th>
              <th className="px-4 py-3 text-center font-normal">Link</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Edit</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {paginatedItems.length === 0 ? (
              <tr><td colSpan={7} className="px-4 py-8 text-center text-gray-500">No e-bulletins found.</td></tr>
            ) : (
              paginatedItems.map((item, idx) => {
                const title = item.EbulletinTitle || item.ebulletinTitle || '';
                const type = item.EbulletinType || item.ebulletinType || '-';
                const pubDate = formatDateDMY(item.PublishDate || item.publishDate);
                const expDate = formatDateDMY(item.ExpiryDate || item.expiryDate);
                const link = item.EbulletinLink || item.ebulletinLink || '';
                return (
                  <tr key={item.id || item.Id || idx} className={`border-b border-gray-100 hover:bg-gray-50 ${idx % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`}>
                    <td className="px-4 py-3 text-[#1a297d] font-medium">{title}</td>
                    <td className="px-4 py-3">
                      <span className="px-2 py-1 rounded text-xs font-medium bg-purple-100 text-purple-700">{type}</span>
                    </td>
                    <td className="px-4 py-3">{pubDate}</td>
                    <td className="px-4 py-3">{expDate}</td>
                    <td className="px-4 py-3 text-center">
                      {link ? (
                        <a href={link} target="_blank" rel="noreferrer" className="text-[#1a297d] hover:underline text-sm">View</a>
                      ) : '-'}
                    </td>
                    <td className="px-2 py-3 text-center">
                      <button onClick={() => openEdit(item)} className="text-gray-500 hover:text-[#1a297d]" title="Edit">
                        <svg className="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </button>
                    </td>
                    <td className="px-2 py-3 text-center">
                      <button onClick={() => setDeleteTarget(item)} className="text-gray-500 hover:text-red-600" title="Delete">
                        <svg className="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
        {renderPagination()}
      </div>

      {/* Create/Edit E-Bulletin Modal */}
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editing ? 'Edit E-Bulletin' : 'Add E-Bulletin'} size="lg">
        <form onSubmit={handleSubmit}>
          {/* Title */}
          <label className="form-label">Title <span className="text-[#dd4b39]">*</span></label>
          <input
            type="text"
            value={form.EbulletinTitle}
            onChange={(e) => setForm({ ...form, EbulletinTitle: e.target.value })}
            className="form-input"
          />
          {errors.EbulletinTitle && <span className="validation-error">{errors.EbulletinTitle}</span>}

          {/* Link URL */}
          <label className="form-label">Link URL</label>
          <input
            type="text"
            value={form.EbulletinLink}
            onChange={(e) => setForm({ ...form, EbulletinLink: e.target.value })}
            placeholder="https://..."
            className="form-input"
          />

          {/* Type */}
          <label className="form-label">Type</label>
          <select
            value={form.EbulletinType}
            onChange={(e) => setForm({ ...form, EbulletinType: e.target.value })}
            className="form-select"
          >
            {TYPES.map(t => <option key={t} value={t}>{t}</option>)}
          </select>

          {/* Publish & Expiry Dates */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="form-label">Publish Date &amp; Time <span className="text-[#dd4b39]">*</span></label>
              <input
                type="datetime-local"
                value={form.PublishDate}
                onChange={(e) => setForm({ ...form, PublishDate: e.target.value })}
                className="form-input"
              />
              {errors.PublishDate && <span className="validation-error">{errors.PublishDate}</span>}
            </div>
            <div>
              <label className="form-label">Expiry Date &amp; Time <span className="text-[#dd4b39]">*</span></label>
              <input
                type="datetime-local"
                value={form.ExpiryDate}
                onChange={(e) => setForm({ ...form, ExpiryDate: e.target.value })}
                className="form-input"
              />
              {errors.ExpiryDate && <span className="validation-error">{errors.ExpiryDate}</span>}
            </div>
          </div>

          {/* Buttons */}
          <div className="flex justify-end gap-3 pt-5 pb-2">
            <button type="button" onClick={() => setShowModal(false)} className="btn-outline text-sm">Cancel</button>
            <button type="submit" disabled={saving} className="btn-success text-sm">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>

      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title="Delete E-Bulletin"
        message="Are you sure you want to delete?"
      />
    </div>
  );
}
