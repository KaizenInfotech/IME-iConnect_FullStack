import { useState, useEffect } from 'react';
import PageHeader from '../components/shared/PageHeader';
import DataTable from '../components/shared/DataTable';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getWebLinks, createWebLink, updateWebLink, deleteWebLink } from '../api/webLinkService';

const emptyForm = { Title: '', LinkUrl: '', FullDesc: '' };

export default function WebLinksPage() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => { fetchData(); }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getWebLinks();
      setItems(res.data?.data || res.data || []);
    } catch { setError('Failed to load web links'); }
    finally { setLoading(false); }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      if (editing) await updateWebLink(editing.id || editing.Id, form);
      else await createWebLink(form);
      setShowModal(false); setEditing(null); setForm(emptyForm); fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteWebLink(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({ Title: item.Title || item.title || '', LinkUrl: item.LinkUrl || item.linkUrl || '', FullDesc: item.FullDesc || item.fullDesc || '' });
    setShowModal(true);
  };

  const columns = [
    { key: 'Title', label: 'Title' },
    { key: 'LinkUrl', label: 'URL', render: (r) => (r.LinkUrl || r.linkUrl) ? <a href={r.LinkUrl || r.linkUrl} target="_blank" rel="noreferrer" className="text-blue-600 hover:underline text-sm truncate max-w-[200px] inline-block">{r.LinkUrl || r.linkUrl}</a> : '-' },
    { key: 'FullDesc', label: 'Description', render: (r) => <span className="line-clamp-1">{r.FullDesc || r.fullDesc || '-'}</span> },
    { key: 'actions', label: 'Actions', render: (row) => (
      <div className="flex gap-2">
        <button onClick={() => openEdit(row)} className="text-blue-600 hover:text-blue-800 text-sm font-medium">Edit</button>
        <button onClick={() => setDeleteTarget(row)} className="text-red-600 hover:text-red-800 text-sm font-medium">Delete</button>
      </div>
    )}
  ];

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <PageHeader title="Web Links" action={{ label: 'Add Link', onClick: () => { setEditing(null); setForm(emptyForm); setShowModal(true); } }} />
      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}</div>}
      <div className="bg-white rounded-xl shadow-sm p-6">
        <DataTable columns={columns} data={items} emptyMessage="No web links found" />
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editing ? 'Edit Web Link' : 'Add Web Link'}>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Title</label><input required value={form.Title} onChange={e => setForm({...form, Title: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">URL</label><input required value={form.LinkUrl} onChange={e => setForm({...form, LinkUrl: e.target.value})} placeholder="https://..." className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Description</label><textarea rows={3} value={form.FullDesc} onChange={e => setForm({...form, FullDesc: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div className="flex justify-end gap-3 pt-2">
            <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50">Cancel</button>
            <button type="submit" disabled={saving} className="px-4 py-2 bg-[#1a297d] text-white rounded-lg hover:bg-[#15226a] disabled:opacity-50">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>
      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Web Link" message={`Are you sure you want to delete "${deleteTarget?.Title || deleteTarget?.title}"?`} />
    </div>
  );
}
