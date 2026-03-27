import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import PageHeader from '../components/shared/PageHeader';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getAlbums, createAlbum, deleteAlbum } from '../api/albumService';

const emptyForm = { Title: '', Description: '', Type: 'Photo', Image: '' };

export default function AlbumsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => { fetchData(); }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const groupId = filterGroupId || '33359';
      const year = new Date().getFullYear().toString();
      const res = await getAlbums(groupId, year);
      const albums = res.data?.TBAlbumsListResult?.Result?.newAlbums || [];
      setItems(albums.map(a => ({
        Id: a.albumId, id: a.albumId, Title: a.title, Description: a.description,
        Image: a.image, ProjectDate: a.project_date, GroupId: a.groupId,
      })));
    } catch { setError('Failed to load albums'); }
    finally { setLoading(false); }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      await createAlbum(form);
      setShowModal(false); setForm(emptyForm); fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteAlbum(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <PageHeader title="Gallery / Albums" action={{ label: 'Add Album', onClick: () => { setForm(emptyForm); setShowModal(true); } }} />
      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}</div>}

      {items.length === 0 ? (
        <div className="bg-white rounded-xl shadow-sm p-12 text-center text-gray-500">No albums found</div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {items.map((album) => (
            <div key={album.id || album.Id} onClick={() => navigate(`/albums/${album.id || album.Id}`)} className="bg-white rounded-xl shadow-sm overflow-hidden cursor-pointer hover:shadow-md transition-shadow group">
              <div className="h-40 bg-gray-100 flex items-center justify-center">
                {album.Image || album.image ? (
                  <img src={album.Image || album.image} alt={album.Title || album.title} className="w-full h-full object-cover" />
                ) : (
                  <svg className="w-12 h-12 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
                )}
              </div>
              <div className="p-4">
                <h3 className="font-medium text-gray-800 group-hover:text-[#1a297d] transition-colors">{album.Title || album.title}</h3>
                <p className="text-sm text-gray-500 mt-1 line-clamp-2">{album.Description || album.description || 'No description'}</p>
                <div className="mt-3 flex justify-between items-center">
                  <span className="text-xs px-2 py-1 rounded-full bg-blue-100 text-blue-700">{album.Type || album.type || 'Photo'}</span>
                  <button onClick={(e) => { e.stopPropagation(); setDeleteTarget(album); }} className="text-red-500 hover:text-red-700 text-sm">Delete</button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title="Add Album">
        <form onSubmit={handleSubmit} className="space-y-4">
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Title</label><input required value={form.Title} onChange={e => setForm({...form, Title: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Description</label><textarea rows={3} value={form.Description} onChange={e => setForm({...form, Description: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Type</label><select value={form.Type} onChange={e => setForm({...form, Type: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none"><option>Photo</option><option>Video</option><option>Mixed</option></select></div>
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Cover Image URL</label><input value={form.Image} onChange={e => setForm({...form, Image: e.target.value})} placeholder="https://..." className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div className="flex justify-end gap-3 pt-2">
            <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50">Cancel</button>
            <button type="submit" disabled={saving} className="px-4 py-2 bg-[#1a297d] text-white rounded-lg hover:bg-[#15226a] disabled:opacity-50">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>
      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Album" message={`Are you sure you want to delete "${deleteTarget?.Title || deleteTarget?.title}"?`} />
    </div>
  );
}
