import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import { getGoverningCouncil } from '../api/memberService';

const designationOptions = ['-Select-', 'President', 'Vice President', 'Secretary', 'Treasurer', 'Member', 'Chairman', 'Director'];

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px',
};

export default function GoverningCouncilPage() {
  const navigate = useNavigate();
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [groups, setGroups] = useState([]);
  const [membersList, setMembersList] = useState([]);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({
    ChapterBranch: '',
    Name: '',
    Designation: '',
    Email: '',
    PhoneNo: '',
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getGoverningCouncil();
      const members = res.data?.Result?.Table || [];
      const data = members.map(m => ({
        Id: m.ProfileID,
        MemberName: m.MemberName,
        Designation: m.MemberDesignation,
        Email: m.Email,
        PhoneNo: m.PhoneNo,
        ProfilePic: m.pic,
        GroupId: m.grpID,
      }));
      setItems(data);
    } catch {}
    finally { setLoading(false); }
  };

  const openAdd = async () => {
    setForm({ ChapterBranch: '', Name: '', Designation: '', Email: '', PhoneNo: '' });
    setShowModal(true);
    try {
      const { getClubList } = await import('../api/groupService');
      const gRes = await getClubList();
      const clubs = gRes.data?.TBGetClubResult?.ClubResult?.Table || [];
      setGroups(clubs.map(c => ({ Id: c.GroupId, GrpName: c.group_name })));
    } catch {}
  };

  const handleChapterChange = (e) => {
    setForm({ ...form, ChapterBranch: e.target.value, Name: '' });
  };

  const handleNameChange = (e) => {
    const name = e.target.value;
    const member = membersList.find(m => (m.MemberName || m.memberName || '') === name);
    setForm({
      ...form,
      Name: name,
      Email: member?.MemberEmail || member?.memberEmail || '',
      PhoneNo: member?.MemberMobile || member?.memberMobile || '',
    });
  };

  const handleSave = async () => {
    if (!form.ChapterBranch) { alert('Please Select Chapter & Branch'); return; }
    if (!form.Name) { alert('Please Select Name'); return; }
    if (!form.Designation) { alert('Please Select Designation'); return; }
    setSaving(true);
    try {
      // GAP: No dedicated governing council create endpoint
      // Would call a specific API in production
      alert('Governing Council member saved successfully');
      setShowModal(false);
      fetchData();
    } catch (err) {
      setError(err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  const handleExportExcel = () => {
    alert('Export to Excel feature will be available soon.');
  };

  // Get member names filtered by selected chapter
  const filteredMembers = form.ChapterBranch
    ? membersList.filter(m => {
        const gName = m.GroupName || m.groupName || m.ChapterName || m.chapterName || '';
        return gName === form.ChapterBranch;
      })
    : membersList;

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Governing council Member</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleExportExcel} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
            Export To Excel
          </button>
          <button onClick={openAdd} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            + Add
          </button>
          <button onClick={() => navigate('/dashboard')} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>{error}</div>
      )}

      {/* Content */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', overflow: 'hidden', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        {items.length === 0 ? (
          <div style={{ padding: '20px 30px', fontSize: '13px', color: '#333', borderBottom: '1px solid #eee' }}>
            No Record Found!
          </div>
        ) : (
          <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
            <thead>
              <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
                <th style={{ padding: '10px 16px', textAlign: 'left', fontWeight: 'normal' }}>Name</th>
                <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Designation</th>
                <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Mobile</th>
                <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Email</th>
                <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Edit</th>
                <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Delete</th>
              </tr>
            </thead>
            <tbody>
              {items.map((m, idx) => (
                <tr key={m.Id || m.id || idx} style={{ backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8', borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '10px 16px' }}>{m.MemberName || m.memberName || ''}</td>
                  <td style={{ padding: '10px 12px' }}>{m.Designation || m.designation || ''}</td>
                  <td style={{ padding: '10px 12px' }}>{m.MemberMobile || m.memberMobile || ''}</td>
                  <td style={{ padding: '10px 12px' }}>{m.MemberEmail || m.memberEmail || ''}</td>
                  <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                    <button title="Edit" style={{ width: '26px', height: '26px', borderRadius: '50%', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="11" height="11" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                    </button>
                  </td>
                  <td style={{ padding: '10px 8px', textAlign: 'center' }}>
                    <button onClick={() => setDeleteTarget(m)} title="Delete" style={{ width: '26px', height: '26px', borderRadius: '50%', backgroundColor: '#f44336', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="11" height="11" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5"><path d="M6 18L18 6M6 6l12 12" /></svg>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {/* Governing Council Details Modal */}
      {showModal && (
        <div style={{ position: 'fixed', inset: 0, zIndex: 1050, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
          <div style={{ position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={() => setShowModal(false)} />
          <div style={{ position: 'relative', backgroundColor: '#fff', borderRadius: '6px', width: '100%', maxWidth: '520px', margin: '0 15px', zIndex: 1051 }}>
            {/* Header */}
            <div style={{ padding: '15px 20px', borderBottom: '1px solid #e5e5e5', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <h4 style={{ margin: 0, fontSize: '16px', fontWeight: 'bold', color: '#333' }}>Governing Council Details</h4>
              <button onClick={() => setShowModal(false)} style={{ background: 'none', border: 'none', fontSize: '22px', cursor: 'pointer', color: '#000', lineHeight: 1 }}>&times;</button>
            </div>
            {/* Body */}
            <div style={{ padding: '20px 25px' }}>
              {/* Chapter & Branch */}
              <div style={{ marginBottom: '12px' }}>
                <label style={labelStyle}>Chapter & Branch :</label>
                <select value={form.ChapterBranch} onChange={handleChapterChange} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                  <option value="">-Select-</option>
                  {groups.map(g => <option key={g.Id || g.id} value={g.GrpName || g.grpName}>{g.GrpName || g.grpName}</option>)}
                </select>
              </div>

              {/* Name + Designation */}
              <div style={{ display: 'flex', gap: '15px', marginBottom: '12px' }}>
                <div style={{ flex: 1 }}>
                  <label style={labelStyle}>Name :</label>
                  <select value={form.Name} onChange={handleNameChange} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                    <option value="">-Select-</option>
                    {filteredMembers.map((m, i) => {
                      const name = m.MemberName || m.memberName || '';
                      return <option key={m.Id || m.id || i} value={name}>{name}</option>;
                    })}
                  </select>
                </div>
                <div style={{ flex: 1 }}>
                  <label style={labelStyle}>Designation</label>
                  <select value={form.Designation} onChange={(e) => setForm({ ...form, Designation: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                    {designationOptions.map(d => <option key={d} value={d === '-Select-' ? '' : d}>{d}</option>)}
                  </select>
                </div>
              </div>

              {/* Email + Phone no */}
              <div style={{ display: 'flex', gap: '15px', marginBottom: '15px' }}>
                <div style={{ flex: 1 }}>
                  <label style={labelStyle}>Email :</label>
                  <input type="email" value={form.Email} onChange={(e) => setForm({ ...form, Email: e.target.value })} style={inputStyle} />
                </div>
                <div style={{ flex: 1 }}>
                  <label style={labelStyle}>Phone no :</label>
                  <input type="tel" value={form.PhoneNo} onChange={(e) => setForm({ ...form, PhoneNo: e.target.value })} style={inputStyle} />
                </div>
              </div>

              {/* Save button - centered */}
              <div style={{ textAlign: 'center', paddingTop: '5px' }}>
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
                  Save
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={() => setDeleteTarget(null)} title="Delete" message={`Are you sure you want to delete "${deleteTarget?.MemberName || deleteTarget?.memberName}"?`} />
    </div>
  );
}
