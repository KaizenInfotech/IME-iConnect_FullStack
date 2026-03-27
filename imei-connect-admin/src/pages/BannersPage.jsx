import { useState, useEffect } from 'react';
import PageHeader from '../components/shared/PageHeader';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getBanners, createBanner, updateBanner, deleteBanner } from '../api/bannerService';

const emptyForm = { BannerTitle: '', BannerImage: '', BannerUrl: '', BannerType: 'Main', IsActive: true };
const bannerTypes = ['Main', 'Sidebar', 'Footer', 'Popup'];

export default function BannersPage() {
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
      const res = await getBanners();
      setItems(res.data?.data || res.data || []);
    } catch { setError('Failed to load banners'); }
    finally { setLoading(false); }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      if (editing) await updateBanner(editing.id || editing.Id, form);
      else await createBanner(form);
      setShowModal(false); setEditing(null); setForm(emptyForm); fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteBanner(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({ BannerTitle: item.BannerTitle || item.bannerTitle || '', BannerImage: item.BannerImage || item.bannerImage || '', BannerUrl: item.BannerUrl || item.bannerUrl || '', BannerType: item.BannerType || item.bannerType || 'Main', IsActive: item.IsActive ?? item.isActive ?? true });
    setShowModal(true);
  };

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <PageHeader title="Banners" action={{ label: 'Add Banner', onClick: () => { setEditing(null); setForm(emptyForm); setShowModal(true); } }} />
      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}</div>}

      {items.length === 0 ? (
        <div className="bg-white rounded-xl shadow-sm p-12 text-center text-gray-500">No banners found</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {items.map((banner) => (
            <div key={banner.id || banner.Id} className="bg-white rounded-xl shadow-sm overflow-hidden">
              <div className="h-40 bg-gray-100 flex items-center justify-center">
                {(banner.BannerImage || banner.bannerImage) ? (
                  <img src={banner.BannerImage || banner.bannerImage} alt={banner.BannerTitle || banner.bannerTitle} className="w-full h-full object-cover" />
                ) : (
                  <svg className="w-12 h-12 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M3 21v-4m0 0V5a2 2 0 012-2h6.5l1 1H21l-3 6 3 6h-8.5l-1-1H5a2 2 0 00-2 2zm9-13.5V9" /></svg>
                )}
              </div>
              <div className="p-4">
                <h3 className="font-medium text-gray-800">{banner.BannerTitle || banner.bannerTitle || 'Untitled'}</h3>
                <div className="mt-2 flex items-center gap-2">
                  <span className="text-xs px-2 py-1 rounded-full bg-purple-100 text-purple-700">{banner.BannerType || banner.bannerType}</span>
                  <span className={`text-xs px-2 py-1 rounded-full ${(banner.IsActive ?? banner.isActive) ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>{(banner.IsActive ?? banner.isActive) ? 'Active' : 'Inactive'}</span>
                </div>
                <div className="mt-3 flex gap-2">
                  <button onClick={() => openEdit(banner)} className="text-blue-600 hover:text-blue-800 text-sm font-medium">Edit</button>
                  <button onClick={() => setDeleteTarget(banner)} className="text-red-600 hover:text-red-800 text-sm font-medium">Delete</button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editing ? 'Edit Banner' : 'Add Banner'}>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Title</label><input required value={form.BannerTitle} onChange={e => setForm({...form, BannerTitle: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Image URL</label><input value={form.BannerImage} onChange={e => setForm({...form, BannerImage: e.target.value})} placeholder="https://..." className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Link URL</label><input value={form.BannerUrl} onChange={e => setForm({...form, BannerUrl: e.target.value})} placeholder="https://..." className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Type</label><select value={form.BannerType} onChange={e => setForm({...form, BannerType: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none">{bannerTypes.map(t => <option key={t}>{t}</option>)}</select></div>
          <label className="flex items-center gap-2 text-sm"><input type="checkbox" checked={form.IsActive} onChange={e => setForm({...form, IsActive: e.target.checked})} className="rounded" /> Active</label>
          <div className="flex justify-end gap-3 pt-2">
            <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50">Cancel</button>
            <button type="submit" disabled={saving} className="px-4 py-2 bg-[#1a297d] text-white rounded-lg hover:bg-[#15226a] disabled:opacity-50">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>
      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Banner" message={`Are you sure you want to delete "${deleteTarget?.BannerTitle || deleteTarget?.bannerTitle}"?`} />
    </div>
  );
}
