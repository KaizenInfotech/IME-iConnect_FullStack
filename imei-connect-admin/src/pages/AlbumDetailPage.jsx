import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import PageHeader from '../components/shared/PageHeader';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getAlbum, addPhoto, deletePhoto } from '../api/albumService';

export default function AlbumDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [album, setAlbum] = useState(null);
  const [photos, setPhotos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [photoUrl, setPhotoUrl] = useState('');
  const [photoDesc, setPhotoDesc] = useState('');
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  const fetchData = async () => {
    try {
      const res = await getAlbum(id);
      const data = res.data?.data || res.data;
      setAlbum(data);
      setPhotos(data?.photos || data?.Photos || []);
    } catch { setError('Failed to load album'); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, [id]);

  const handleAddPhoto = async (e) => {
    e.preventDefault();
    if (!photoUrl) return;
    setSaving(true);
    try {
      await addPhoto(id, { Url: photoUrl, Description: photoDesc });
      setPhotoUrl(''); setPhotoDesc(''); fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Failed to add photo'); }
    finally { setSaving(false); }
  };

  const handleDeletePhoto = async () => {
    try {
      await deletePhoto(id, deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  if (loading) return <LoadingSpinner />;
  if (!album) return <div className="text-gray-500 text-center py-8">Album not found</div>;

  return (
    <div>
      <PageHeader title={album.Title || album.title || 'Album'} action={{ label: 'Back to Albums', onClick: () => navigate('/albums') }} />
      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}</div>}

      <div className="bg-white rounded-xl shadow-sm p-6 mb-6">
        <p className="text-gray-600">{album.Description || album.description || 'No description'}</p>
        <span className="inline-block mt-2 text-xs px-2 py-1 rounded-full bg-blue-100 text-blue-700">{album.Type || album.type || 'Photo'}</span>
      </div>

      <div className="bg-white rounded-xl shadow-sm p-6 mb-6">
        <h3 className="text-lg font-semibold mb-4">Add Photo</h3>
        <form onSubmit={handleAddPhoto} className="flex flex-col sm:flex-row gap-3">
          <input value={photoUrl} onChange={e => setPhotoUrl(e.target.value)} placeholder="Photo URL" required className="flex-1 border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" />
          <input value={photoDesc} onChange={e => setPhotoDesc(e.target.value)} placeholder="Description (optional)" className="flex-1 border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" />
          <button type="submit" disabled={saving} className="px-4 py-2 bg-[#1a297d] text-white rounded-lg hover:bg-[#15226a] disabled:opacity-50 whitespace-nowrap">{saving ? 'Adding...' : 'Add Photo'}</button>
        </form>
      </div>

      <h3 className="text-lg font-semibold mb-4">Photos ({photos.length})</h3>
      {photos.length === 0 ? (
        <div className="bg-white rounded-xl shadow-sm p-12 text-center text-gray-500">No photos in this album</div>
      ) : (
        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
          {photos.map((photo) => (
            <div key={photo.id || photo.Id} className="relative group bg-white rounded-lg overflow-hidden shadow-sm">
              <img src={photo.Url || photo.url} alt={photo.Description || photo.description || ''} className="w-full h-36 object-cover" />
              <div className="p-2">
                <p className="text-xs text-gray-500 line-clamp-1">{photo.Description || photo.description || ''}</p>
              </div>
              <button onClick={() => setDeleteTarget(photo)} className="absolute top-2 right-2 bg-red-600 text-white rounded-full w-6 h-6 flex items-center justify-center text-xs opacity-0 group-hover:opacity-100 transition-opacity">X</button>
            </div>
          ))}
        </div>
      )}

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDeletePhoto} title="Delete Photo" message="Are you sure you want to delete this photo?" />
    </div>
  );
}
