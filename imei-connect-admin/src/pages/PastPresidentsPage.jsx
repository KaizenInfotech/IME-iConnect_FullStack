import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getPastPresidents, createPastPresident, updatePastPresident, deletePastPresident } from '../api/pastPresidentService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px',
};

export default function PastPresidentsPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');
  const [pageName, setPageName] = useState('National Admin');

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [saving, setSaving] = useState(false);
  const [photoPreview, setPhotoPreview] = useState(null);
  const [form, setForm] = useState({ MemberName: '', Designation: '', PhotoFile: null });

  useEffect(() => { fetchData(); }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const groupId = filterGroupId || '31185';
      if (filterGroupId) {
        try {
          const { getClubList } = await import('../api/groupService');
          const clubRes = await getClubList();
          const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
          const found = clubs.find(c => String(c.GroupId) === filterGroupId);
          if (found) setPageName(found.group_name);
        } catch {}
      }
      const res = await getPastPresidents(groupId);
      const list = res.data?.TBPastPresidentListResult?.TBPastPresidentList?.newRecords || [];
      const data = list.map(p => ({ ...p, Id: p.PastPresidentId, MemberName: p.MemberName, PhotoPath: p.PhotoPath, TenureYear: p.TenureYear }));
      setItems(data);
    } catch { setError('Failed to load past presidents'); }
    finally { setLoading(false); }
  };

  const handleDelete = async () => {
    try {
      await deletePastPresident(deleteTarget.Id || deleteTarget.id);
      setDeleteTarget(null); fetchData();
    } catch { setError('Delete failed'); }
  };

  const openEdit = (item) => {
    const name = item.MemberName || item.memberName || '';
    const tenure = item.TenureYear || item.tenureYear || '';
    setEditing(item);
    setForm({
      MemberName: tenure ? `${name} (${tenure})` : name,
      Designation: item.Designation || item.designation || 'Past President',
      PhotoFile: null,
    });
    setPhotoPreview(item.PhotoPath || item.photoPath || null);
    setShowModal(true);
  };

  const openAdd = () => {
    setEditing(null);
    setForm({ MemberName: '', Designation: '', PhotoFile: null });
    setPhotoPreview(null);
    setShowModal(true);
  };

  const handlePhotoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setForm({ ...form, PhotoFile: file });
      const reader = new FileReader();
      reader.onload = (ev) => setPhotoPreview(ev.target.result);
      reader.readAsDataURL(file);
    }
  };

  const handleRemovePhoto = () => {
    setPhotoPreview(null);
    setForm({ ...form, PhotoFile: null });
  };

  const handleSave = async () => {
    if (!form.MemberName.trim()) { alert('Please Enter Name'); return; }
    if (!form.Designation.trim()) { alert('Please Enter Designation'); return; }
    setSaving(true);
    try {
      if (editing) {
        await updatePastPresident(editing.Id || editing.id, {
          MemberName: form.MemberName,
          Designation: form.Designation,
        });
      } else {
        await createPastPresident({
          MemberName: form.MemberName,
          Designation: form.Designation,
        });
      }
      setShowModal(false);
      setEditing(null);
      fetchData();
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{pageName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>{filterGroupId ? ' - Past Chairman' : ' - Past Presidents'}</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={openAdd} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            + Add
          </button>
          <button onClick={() => navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
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
              <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>Name</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr><td colSpan={3} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No Record Found!</td></tr>
            ) : (
              items.map((item, idx) => {
                const name = item.MemberName || item.memberName || '';
                const tenure = item.TenureYear || item.tenureYear || '';
                return (
                  <tr key={item.Id || item.id || idx} style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee' }}>
                    <td style={{ padding: '10px 16px', color: '#333' }}>
                      {name}{tenure ? ` (${tenure})` : ''}
                    </td>
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button onClick={() => openEdit(item)} title="Edit" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </button>
                    </td>
                    <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                      <button onClick={() => setDeleteTarget(item)} title="Delete" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#f44336', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                        <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {/* Past President Details Modal */}
      {showModal && (
        <div style={{ position: 'fixed', inset: 0, zIndex: 1050, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
          <div style={{ position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={() => setShowModal(false)} />
          <div style={{ position: 'relative', backgroundColor: '#fff', borderRadius: '6px', width: '100%', maxWidth: '480px', margin: '0 15px', zIndex: 1051 }}>
            {/* Header */}
            <div style={{ padding: '15px 20px', borderBottom: '1px solid #e5e5e5', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <h4 style={{ margin: 0, fontSize: '16px', fontWeight: 'bold', color: '#333' }}>Past President Details</h4>
              <button onClick={() => setShowModal(false)} style={{ background: 'none', border: 'none', fontSize: '22px', cursor: 'pointer', color: '#000', lineHeight: 1 }}>&times;</button>
            </div>
            {/* Body */}
            <div style={{ padding: '20px 25px' }}>
              <div style={{ display: 'flex', gap: '20px' }}>
                {/* Left - Profile Photo */}
                <div style={{ flex: '0 0 120px', textAlign: 'center' }}>
                  <label style={labelStyle}>Profile Photo</label>
                  <div style={{
                    width: '100px', height: '100px', border: '1px solid #ccc',
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                    margin: '0 auto 8px', backgroundColor: '#f5f5f5', borderRadius: '2px',
                    overflow: 'hidden',
                  }}>
                    {photoPreview ? (
                      <img src={photoPreview} alt="Profile" style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }} />
                    ) : (
                      <svg width="40" height="40" fill="#ccc" viewBox="0 0 20 20">
                        <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
                      </svg>
                    )}
                  </div>
                  <div style={{ display: 'flex', gap: '6px', justifyContent: 'center' }}>
                    <label style={{
                      display: 'inline-flex', alignItems: 'center',
                      backgroundColor: '#6c757d', color: '#fff', border: 'none',
                      padding: '4px 10px', borderRadius: '4px', fontSize: '11px', cursor: 'pointer',
                    }}>
                      Add
                      <input type="file" accept="image/*" onChange={handlePhotoChange} style={{ display: 'none' }} />
                    </label>
                    <button onClick={handleRemovePhoto} style={{
                      display: 'inline-flex', alignItems: 'center',
                      backgroundColor: '#28a745', color: '#fff', border: 'none',
                      padding: '4px 10px', borderRadius: '4px', fontSize: '11px', cursor: 'pointer',
                    }}>
                      Remove
                    </button>
                  </div>
                </div>

                {/* Right - Name + Designation */}
                <div style={{ flex: 1 }}>
                  <div style={{ marginBottom: '10px' }}>
                    <label style={labelStyle}>Name</label>
                    <input type="text" value={form.MemberName} onChange={(e) => setForm({ ...form, MemberName: e.target.value })} style={inputStyle} />
                  </div>
                  <div style={{ marginBottom: '10px' }}>
                    <label style={labelStyle}>Designation</label>
                    <input type="text" value={form.Designation} onChange={(e) => setForm({ ...form, Designation: e.target.value })} style={inputStyle} />
                  </div>
                </div>
              </div>

              {/* Save/Update button - centered */}
              <div style={{ textAlign: 'center', paddingTop: '15px' }}>
                <button
                  onClick={handleSave}
                  disabled={saving}
                  style={{
                    display: 'inline-flex', alignItems: 'center', gap: '4px',
                    backgroundColor: '#1a297d', color: '#fff', border: 'none',
                    padding: '7px 20px', borderRadius: '4px', fontSize: '13px',
                    cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1,
                  }}
                >
                  <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
                  {editing ? 'Update' : 'Save'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete" message={`Are you sure you want to delete "${deleteTarget?.MemberName || deleteTarget?.memberName}"?`} />
    </div>
  );
}
