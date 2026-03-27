import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Modal from '../components/shared/Modal';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getClubList, createGroup } from '../api/groupService';

const emptyForm = {
  GrpName: '', GrpType: '', GrpCategory: '', Address1: '',
  City: '', State: '', Country: '', Email: '', Mobile: '',
};

export default function GroupsPage() {
  const navigate = useNavigate();
  const [allGroups, setAllGroups] = useState([]);
  const [groups, setGroups] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => { fetchData(); }, []);

  useEffect(() => {
    if (!searchTerm.trim()) {
      setGroups(allGroups);
    } else {
      const q = searchTerm.toLowerCase();
      setGroups(allGroups.filter(g =>
        (g.GrpName || g.grpName || '').toLowerCase().includes(q)
      ));
    }
  }, [searchTerm, allGroups]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getClubList();
      const clubs = res.data?.TBGetClubResult?.ClubResult?.Table || [];
      const data = clubs.map(c => ({
        Id: c.GroupId,
        GrpName: c.group_name,
        City: c.city,
        State: c.State,
        Country: c.country_name,
        MeetingDay: c.Meeting_Day,
        PresidentName: c.member_name,
        SecretaryName: c.member_name0,
      }));
      setAllGroups(data);
      setGroups(data);
    } catch { setError('Failed to load groups'); }
    finally { setLoading(false); }
  };

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!form.GrpName.trim()) return;
    setSaving(true);
    try {
      await createGroup(form);
      setShowModal(false);
      setForm(emptyForm);
      fetchData();
    } catch (err) { setError(err.response?.data?.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  const handleDelete = async () => {
    try {
      await deleteGroup(deleteTarget.Id || deleteTarget.id);
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#666', fontSize: '14px' }}>Dashboard - </span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Branch & Chapter committee Dashboard</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button
            onClick={() => navigate('/groups/add')}
            style={{
              display: 'flex', alignItems: 'center', gap: '4px',
              backgroundColor: '#6b9300', color: '#fff', border: 'none',
              padding: '7px 16px', borderRadius: '4px', fontSize: '13px',
              cursor: 'pointer', fontWeight: '500',
            }}
          >
            + Add New Chapter/Branch
          </button>
          <button
            onClick={() => navigate('/dashboard')}
            style={{
              display: 'flex', alignItems: 'center', gap: '6px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '7px 16px', borderRadius: '4px', fontSize: '13px',
              cursor: 'pointer',
            }}
          >
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
              <path d="M19 12H5M12 19l-7-7 7-7" />
            </svg>
            Back
          </button>
        </div>
      </div>

      {error && <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '6px', marginBottom: '12px', fontSize: '13px' }}>{error}<button onClick={() => setError('')} style={{ float: 'right', background: 'none', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>&times;</button></div>}

      {/* Search Bar */}
      <div style={{ marginBottom: '15px' }}>
        <input
          type="text"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          placeholder="Search by Chapters/ Branches name"
          style={{
            width: '100%', height: '36px', border: '1px solid #d0d0d0',
            borderRadius: '4px', padding: '6px 12px', fontSize: '13px',
            outline: 'none',
          }}
        />
      </div>

      {/* Table */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
          <thead>
            <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>Chapters / Branches</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '80px' }}>Dashboard</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '60px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '60px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {groups.length === 0 ? (
              <tr><td colSpan={4} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No chapters / branches found.</td></tr>
            ) : (
              groups.map((g, idx) => {
                const name = g.GrpName || g.grpName || '';
                const gId = g.Id || g.id;
                return (
                  <tr
                    key={gId || idx}
                    style={{
                      backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8',
                      borderBottom: '1px solid #eee',
                    }}
                  >
                    <td style={{ padding: '10px 16px' }}>{name}</td>
                    {/* Dashboard icon - blue circle */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => navigate(`/groups/${gId}`)}
                        title="Dashboard"
                        style={{
                          width: '28px', height: '28px', borderRadius: '50%',
                          backgroundColor: '#2196F3', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="14" height="14" fill="currentColor" viewBox="0 0 24 24">
                          <path d="M3 3h8v8H3V3zm10 0h8v8h-8V3zm0 10h8v8h-8v-8zM3 13h8v8H3v-8z" />
                        </svg>
                      </button>
                    </td>
                    {/* Edit icon - green circle */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => navigate(`/groups/${gId}/edit`)}
                        title="Edit"
                        style={{
                          width: '28px', height: '28px', borderRadius: '50%',
                          backgroundColor: '#0ead9a', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                        </svg>
                      </button>
                    </td>
                    {/* Delete icon - red circle */}
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => setDeleteTarget(g)}
                        title="Delete"
                        style={{
                          width: '28px', height: '28px', borderRadius: '50%',
                          backgroundColor: '#f44336', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M6 18L18 6M6 6l12 12" />
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

      {/* Add New Chapter/Branch Modal */}
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title="Add New Chapter/Branch" size="lg">
        <form onSubmit={handleCreate}>
          <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>
            Chapter/Branch Name <span style={{ color: '#dd4b39' }}>*</span>
          </label>
          <input
            type="text"
            value={form.GrpName}
            onChange={(e) => setForm({ ...form, GrpName: e.target.value })}
            style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }}
          />

          <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>Type</label>
          <input
            type="text"
            value={form.GrpType}
            onChange={(e) => setForm({ ...form, GrpType: e.target.value })}
            style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }}
          />

          <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>Category</label>
          <input
            type="text"
            value={form.GrpCategory}
            onChange={(e) => setForm({ ...form, GrpCategory: e.target.value })}
            style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }}
          />

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px' }}>
            <div>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>City</label>
              <input type="text" value={form.City} onChange={(e) => setForm({ ...form, City: e.target.value })} style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
            <div>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>State</label>
              <input type="text" value={form.State} onChange={(e) => setForm({ ...form, State: e.target.value })} style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
            <div>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>Email</label>
              <input type="email" value={form.Email} onChange={(e) => setForm({ ...form, Email: e.target.value })} style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
            <div>
              <label style={{ display: 'block', fontWeight: '600', fontSize: '13px', padding: '10px 0 5px' }}>Mobile</label>
              <input type="tel" value={form.Mobile} onChange={(e) => setForm({ ...form, Mobile: e.target.value })} style={{ width: '100%', height: '35px', border: '1px solid #d0d0d0', borderRadius: '8px', padding: '6px 10px', fontSize: '13px', outline: 'none' }} />
            </div>
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px', paddingTop: '20px', paddingBottom: '8px' }}>
            <button type="button" onClick={() => setShowModal(false)} style={{ padding: '7px 20px', border: '1px solid #ccc', borderRadius: '8px', background: '#fff', fontSize: '13px', cursor: 'pointer', color: '#333' }}>Cancel</button>
            <button type="submit" disabled={saving} style={{ padding: '7px 20px', borderRadius: '8px', border: 'none', backgroundColor: '#6b9300', color: '#fff', fontSize: '13px', cursor: 'pointer', opacity: saving ? 0.6 : 1 }}>{saving ? 'Saving...' : 'Save'}</button>
          </div>
        </form>
      </Modal>

      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title="Delete"
        message={`Are you sure you want to delete "${deleteTarget?.GrpName || deleteTarget?.grpName}"?`}
      />
    </div>
  );
}
