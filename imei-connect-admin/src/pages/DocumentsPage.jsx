import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getDocuments, createDocument, deleteDocument } from '../api/documentService';

const FILTER_OPTIONS = ['All', 'Published', 'Unpublished', 'Expired'];
const ACCEPTED_TYPES = '.pdf,.png,.jpeg,.jpg,.xls,.xlsx,.doc,.docx';
const MAX_FILE_SIZE = 30 * 1024 * 1024; // 30MB
const PAGE_SIZE = 10;

const emptyForm = {
  DocTitle: '',
  DocFile: null,
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

function getFilterType(item) {
  const now = new Date();
  const pub = item.PublishDate || item.publishDate;
  const exp = item.ExpiryDate || item.expiryDate;
  if (exp && new Date(exp) < now) return 'Expired';
  if (pub && new Date(pub) <= now) return 'Published';
  return 'Unpublished';
}

function getFileTypeLabel(item) {
  const url = item.DocURL || item.docURL || item.FileUrl || item.fileUrl || '';
  const title = item.DocTitle || item.docTitle || '';
  const combined = (url + title).toLowerCase();
  if (combined.includes('.pdf')) return 'PDF';
  if (combined.includes('.doc')) return 'Word';
  if (combined.includes('.xls')) return 'Excel';
  if (combined.includes('.png') || combined.includes('.jpg') || combined.includes('.jpeg')) return 'Image';
  return item.DocType || item.docType || 'File';
}

export default function DocumentsPage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const groupName = user?.GrpName || user?.groupName || user?.ClubName || 'Group';
  const fileRef = useRef(null);

  const [allItems, setAllItems] = useState([]);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('All');
  const [searchTerm, setSearchTerm] = useState('');
  const [pageNo, setPageNo] = useState(1);
  const [showModal, setShowModal] = useState(false);
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
        (item.DocTitle || item.docTitle || '').toLowerCase().includes(q)
      );
    }
    setItems(filtered);
    setPageNo(1);
  }, [filter, searchTerm, allItems]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getDocuments();
      const data = res.data?.data || res.data || [];
      setAllItems(data);
      setItems(data);
    } catch { setError('Failed to load documents'); }
    finally { setLoading(false); }
  };

  const validate = () => {
    const errs = {};
    if (!form.DocTitle.trim()) errs.DocTitle = 'mandatory';
    if (!form.PublishDate) errs.PublishDate = 'mandatory';
    if (!form.ExpiryDate) errs.ExpiryDate = 'mandatory';
    if (form.DocFile && form.DocFile.size > MAX_FILE_SIZE) {
      errs.DocFile = 'File size must be under 30MB';
    }
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;
    setSaving(true);
    try {
      const formData = new FormData();
      formData.append('DocTitle', form.DocTitle);
      formData.append('PublishDate', form.PublishDate);
      formData.append('ExpiryDate', form.ExpiryDate);
      if (form.DocFile) formData.append('DocFile', form.DocFile);
      await createDocument(formData);
      setShowModal(false);
      setForm(emptyForm);
      setErrors({});
      if (fileRef.current) fileRef.current.value = '';
      fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteDocument(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  const openAdd = () => {
    setForm(emptyForm);
    setErrors({});
    setShowModal(true);
  };

  const handleDownload = (item) => {
    const url = item.DocURL || item.docURL || item.FileUrl || item.fileUrl;
    if (url) window.open(url, '_blank');
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
        <h1 className="page-title">{groupName} Documents List</h1>
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
              <th className="px-4 py-3 text-left font-normal" style={{ width: '80%' }}>Documents</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Reorder</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Download</th>
              <th className="px-2 py-3 text-center font-normal" style={{ width: '5%' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {paginatedItems.length === 0 ? (
              <tr><td colSpan={4} className="px-4 py-8 text-center text-gray-500">No documents found.</td></tr>
            ) : (
              paginatedItems.map((item, idx) => {
                const title = item.DocTitle || item.docTitle || '';
                const fileType = getFileTypeLabel(item);
                const pubDate = formatDateDMY(item.PublishDate || item.publishDate);
                const filterType = getFilterType(item);
                return (
                  <tr key={item.id || item.Id || idx} className={`border-b border-gray-100 hover:bg-gray-50 ${idx % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`}>
                    <td className="px-4 py-3">
                      <div className="font-medium text-[#1a297d]">{title}</div>
                      <div className="text-xs text-gray-500 mt-1">
                        {fileType} | Publish Date: {pubDate} | {filterType}
                      </div>
                    </td>
                    <td className="px-2 py-3 text-center">
                      <button className="text-gray-500 hover:text-[#1a297d]" title="Reorder">
                        <svg className="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 16V4m0 0L3 8m4-4l4 4m6 0v12m0 0l4-4m-4 4l-4-4" /></svg>
                      </button>
                    </td>
                    <td className="px-2 py-3 text-center">
                      <button onClick={() => handleDownload(item)} className="text-gray-500 hover:text-[#1a297d]" title="Download">
                        <svg className="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" /></svg>
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

      {/* Create Document Modal */}
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title="Add Document" size="lg">
        <form onSubmit={handleSubmit}>
          {/* Title */}
          <label className="form-label">Title <span className="text-[#dd4b39]">*</span></label>
          <input
            type="text"
            value={form.DocTitle}
            onChange={(e) => setForm({ ...form, DocTitle: e.target.value })}
            className="form-input"
          />
          {errors.DocTitle && <span className="validation-error">{errors.DocTitle}</span>}

          {/* File Upload */}
          <label className="form-label">File Upload <span className="text-xs text-gray-500">(pdf, png, jpeg, jpg, xls, xlsx, doc, docx - max 30MB)</span></label>
          <input
            type="file"
            ref={fileRef}
            accept={ACCEPTED_TYPES}
            onChange={(e) => setForm({ ...form, DocFile: e.target.files[0] || null })}
            className="form-input"
            style={{ height: 'auto', padding: '6px 10px' }}
          />
          {errors.DocFile && <span className="validation-error">{errors.DocFile}</span>}

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
        title="Delete Document"
        message="Are you sure you want to delete?"
      />
    </div>
  );
}
