import { useState, useEffect } from 'react';
import PageHeader from '../components/shared/PageHeader';
import DataTable from '../components/shared/DataTable';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getEntries, createEntry, updateEntry, deleteEntry, getCategories } from '../api/serviceDirectoryService';

const emptyForm = { MemberName: '', Description: '', ContactNo: '', Email: '', Address: '', City: '', State: '', Keywords: '', ServiceCategoryId: '' };

export default function ServiceDirectoryPage() {
  const [items, setItems] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [filterCat, setFilterCat] = useState('');
  const [error, setError] = useState('');

  useEffect(() => { fetchData(); fetchCategories(); }, []);

  const fetchData = async (categoryId) => {
    setLoading(true);
    try {
      const params = categoryId ? { categoryId } : {};
      const res = await getEntries(params);
      setItems(res.data?.data || res.data || []);
    } catch { setError('Failed to load entries'); }
    finally { setLoading(false); }
  };

  const fetchCategories = async () => {
    try {
      const res = await getCategories();
      setCategories(res.data?.data || res.data || []);
    } catch {}
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      if (editing) await updateEntry(editing.id || editing.Id, form);
      else await createEntry(form);
      setShowModal(false); setEditing(null); setForm(emptyForm); fetchData(filterCat);
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteEntry(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null); fetchData(filterCat);
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({ MemberName: item.MemberName || item.memberName || '', Description: item.Description || item.description || '', ContactNo: item.ContactNo || item.contactNo || '', Email: item.Email || item.email || '', Address: item.Address || item.address || '', City: item.City || item.city || '', State: item.State || item.state || '', Keywords: item.Keywords || item.keywords || '', ServiceCategoryId: item.ServiceCategoryId || item.serviceCategoryId || '' });
    setShowModal(true);
  };

  const handleFilterChange = (catId) => {
    setFilterCat(catId);
    fetchData(catId);
  };

  const columns = [
    { key: 'MemberName', label: 'Name' },
    { key: 'Description', label: 'Service', render: (r) => <span className="line-clamp-1">{r.Description || r.description || '-'}</span> },
    { key: 'ContactNo', label: 'Contact' },
    { key: 'Email', label: 'Email' },
    { key: 'City', label: 'City' },
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
      <PageHeader title="Service Directory" action={{ label: 'Add Entry', onClick: () => { setEditing(null); setForm(emptyForm); setShowModal(true); } }} />
      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}</div>}
      <div className="bg-white rounded-xl shadow-sm p-6">
        <div className="flex gap-3 mb-4 items-center">
          <label className="text-sm font-medium text-gray-700">Category:</label>
          <select value={filterCat} onChange={(e) => handleFilterChange(e.target.value)} className="border border-gray-300 rounded-lg px-3 py-1.5 text-sm focus:ring-2 focus:ring-[#1a297d] outline-none">
            <option value="">All Categories</option>
            {categories.map(c => <option key={c.id || c.Id} value={c.id || c.Id}>{c.CategoryName || c.categoryName}</option>)}
          </select>
        </div>
        <DataTable columns={columns} data={items} emptyMessage="No service directory entries found" />
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editing ? 'Edit Entry' : 'Add Entry'} size="lg">
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div><label className="block text-sm font-medium text-gray-700 mb-1">Member Name</label><input required value={form.MemberName} onChange={e => setForm({...form, MemberName: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
            <div><label className="block text-sm font-medium text-gray-700 mb-1">Category</label><select value={form.ServiceCategoryId} onChange={e => setForm({...form, ServiceCategoryId: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none"><option value="">Select</option>{categories.map(c => <option key={c.id || c.Id} value={c.id || c.Id}>{c.CategoryName || c.categoryName}</option>)}</select></div>
            <div><label className="block text-sm font-medium text-gray-700 mb-1">Contact No</label><input value={form.ContactNo} onChange={e => setForm({...form, ContactNo: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
            <div><label className="block text-sm font-medium text-gray-700 mb-1">Email</label><input type="email" value={form.Email} onChange={e => setForm({...form, Email: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
            <div><label className="block text-sm font-medium text-gray-700 mb-1">City</label><input value={form.City} onChange={e => setForm({...form, City: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
            <div><label className="block text-sm font-medium text-gray-700 mb-1">State</label><input value={form.State} onChange={e => setForm({...form, State: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          </div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Description</label><textarea rows={2} value={form.Description} onChange={e => setForm({...form, Description: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Address</label><input value={form.Address} onChange={e => setForm({...form, Address: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Keywords</label><input value={form.Keywords} onChange={e => setForm({...form, Keywords: e.target.value})} placeholder="Comma-separated keywords" className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div className="flex justify-end gap-3 pt-2">
            <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50">Cancel</button>
            <button type="submit" disabled={saving} className="px-4 py-2 bg-[#1a297d] text-white rounded-lg hover:bg-[#15226a] disabled:opacity-50">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>
      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Entry" message={`Are you sure you want to delete "${deleteTarget?.MemberName || deleteTarget?.memberName}"?`} />
    </div>
  );
}
