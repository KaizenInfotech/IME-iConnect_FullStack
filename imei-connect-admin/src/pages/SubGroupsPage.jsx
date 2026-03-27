import { useState, useEffect } from 'react';
import PageHeader from '../components/shared/PageHeader';
import DataTable from '../components/shared/DataTable';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getSubGroups, createSubGroup, deleteSubGroup } from '../api/subGroupService';

const emptyForm = { SubgrpTitle: '', ParentId: '', GroupId: 0 };

export default function SubGroupsPage() {
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
      const res = await getSubGroups();
      setItems(res.data?.data || res.data || []);
    } catch { setError('Failed to load sub-groups'); }
    finally { setLoading(false); }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      await createSubGroup(form);
      setShowModal(false); setForm(emptyForm); fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteSubGroup(deleteTarget.id || deleteTarget.Id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  const columns = [
    { key: 'SubgrpTitle', label: 'Sub-Group Name', render: (r) => r.SubgrpTitle || r.subgrpTitle || '-' },
    { key: 'ParentId', label: 'Parent ID', render: (r) => r.ParentId || r.parentId || '-' },
    { key: 'actions', label: 'Actions', render: (row) => (
      <button onClick={() => setDeleteTarget(row)} className="text-red-600 hover:text-red-800 text-sm font-medium">Delete</button>
    )}
  ];

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <PageHeader title="Sub-Groups" action={{ label: 'Add Sub-Group', onClick: () => { setForm(emptyForm); setShowModal(true); } }} />
      {error && <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg mb-4">{error}</div>}
      <div className="bg-white rounded-xl shadow-sm p-6">
        <DataTable columns={columns} data={items} emptyMessage="No sub-groups found" />
      </div>

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title="Add Sub-Group">
        <form onSubmit={handleSubmit} className="space-y-4">
          <div><label className="block text-sm font-medium text-gray-700 mb-1">Sub-Group Title</label><input required value={form.SubgrpTitle} onChange={e => setForm({...form, SubgrpTitle: e.target.value})} className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-[#1a297d] outline-none" /></div>
          <div className="flex justify-end gap-3 pt-2">
            <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50">Cancel</button>
            <button type="submit" disabled={saving} className="px-4 py-2 bg-[#1a297d] text-white rounded-lg hover:bg-[#15226a] disabled:opacity-50">{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>
      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete Sub-Group" message={`Are you sure you want to delete "${deleteTarget?.SubgrpTitle || deleteTarget?.subgrpTitle}"?`} />
    </div>
  );
}
