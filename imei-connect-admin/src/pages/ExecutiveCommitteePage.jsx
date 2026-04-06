import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import { getBodList, deleteBOD, updateBOD, reorderBOD } from '../api/memberService';

const designationOptions = ['-Select-', 'President', 'Vice President', 'Vice Chairman', 'Hon. Secretary', 'Hon. Treasurer', 'Hon. General Secretary', 'Chairman', 'Governing Council Member', 'Exe. Comm. Member', 'Others'];

const inputStyle = { width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none' };
const labelStyle = { display: 'block', fontWeight: '600', fontSize: '12px', color: '#333', marginBottom: '4px' };

export default function ExecutiveCommitteePage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const groupId = searchParams.get('groupId') || '';
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [chapterName, setChapterName] = useState('');
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');

  // Edit modal
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [saving, setSaving] = useState(false);
  const [membersList, setMembersList] = useState([]);
  const [form, setForm] = useState({ Name: '', Designation: '', OtherDesignation: '', Email: '', PhoneNo: '' });

  useEffect(() => { fetchData(); }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      if (groupId) {
        try {
          const { getClubList } = await import('../api/groupService');
          const clubRes = await getClubList();
          const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
          const found = clubs.find(c => String(c.GroupId) === groupId);
          if (found) setChapterName(found.group_name);
        } catch {}
      }
      const res = await getBodList(groupId);
      const members = res.data?.TBGetBODResult?.BODResult || [];
      setItems(members);
    } catch {}
    finally { setLoading(false); }
  };

  // --- Drag and drop reorder ---
  const [dragIdx, setDragIdx] = useState(null);

  const handleDragStart = (idx) => { setDragIdx(idx); };

  const handleDragOver = (e) => { e.preventDefault(); };

  const handleDrop = async (dropIdx) => {
    if (dragIdx === null || dragIdx === dropIdx) { setDragIdx(null); return; }
    const newItems = [...items];
    const [removed] = newItems.splice(dragIdx, 1);
    newItems.splice(dropIdx, 0, removed);
    setItems(newItems);
    setDragIdx(null);
    try {
      const reorderData = newItems.map((m, i) => ({ MemberId: parseInt(m.BOD_pkID), DisplayOrder: i + 1 }));
      await reorderBOD(reorderData);
      alert('Reorder saved successfully');
    } catch { alert('Reorder failed'); }
  };

  // --- Edit ---
  const openEdit = (item) => {
    setEditing(item);
    const standardDesigs = ['President', 'Vice President', 'Vice Chairman', 'Hon. Secretary', 'Hon. Treasurer', 'Hon. General Secretary', 'Chairman', 'Governing Council Member', 'Exe. Comm. Member'];
    const isStandard = standardDesigs.includes(item.MemberDesignation);
    setForm({
      Name: item.memberName || '',
      Designation: isStandard ? item.MemberDesignation : 'Others',
      OtherDesignation: isStandard ? '' : (item.MemberDesignation || ''),
      Email: item.Email || '',
      PhoneNo: item.membermobile || '',
    });
    setShowModal(true);
  };

  const openAdd = async () => {
    setEditing(null);
    setForm({ Name: '', Designation: '', OtherDesignation: '', Email: '', PhoneNo: '' });
    setMembersList([]);
    setShowModal(true);
    if (groupId) {
      try {
        const { getMembers } = await import('../api/memberService');
        const res = await getMembers(groupId);
        const members = res.data?.MemberDetail?.NewMemberList || [];
        setMembersList(members.map(m => ({
          Id: m.profileID || m.masterID,
          MemberName: [m.memberName, m.middleName, m.lastName].filter(Boolean).join(' ').trim(),
          MemberEmail: m.memberEmail,
          MemberMobile: m.memberMobile,
        })));
      } catch {}
    }
  };

  const handleNameChange = (e) => {
    const name = e.target.value;
    const member = membersList.find(m => m.MemberName === name);
    setForm({ ...form, Name: name, Email: member?.MemberEmail || '', PhoneNo: member?.MemberMobile || '' });
  };

  const handleSave = async () => {
    if (!form.Name) { alert('Please Select Name'); return; }
    if (!form.Designation || form.Designation === '-Select-') { alert('Please Select Designation'); return; }
    if (form.Designation === 'Others' && !form.OtherDesignation.trim()) { alert('Please Enter Designation'); return; }
    setSaving(true);
    try {
      const desigMap = { 'Chairman': '1', 'Vice Chairman': '2', 'Hon. Secretary': '3', 'Hon. Treasurer': '4', 'Governing Council Member': '5', 'Exe. Comm. Member': '6', 'President': '7', 'Vice President': '8', 'Hon. General Secretary': '9', 'Others': '10' };
      const desigName = form.Designation;
      const res = await updateBOD({
        BOD_PkID: editing ? (editing.BOD_pkID || '0') : '0',
        memberProfileID: editing ? (editing.profileID || '0') : (membersList.find(m => m.MemberName === form.Name)?.Id || '0'),
        groupId: groupId,
        designation: desigName === 'Others' ? '10' : (desigMap[desigName] || '10'),
        otherDesignation: desigName === 'Others' ? form.OtherDesignation : '',
        name: form.Name,
        emailID: form.Email,
        phoneNo: form.PhoneNo,
        yearFilter: `${new Date().getFullYear()}-${new Date().getFullYear()}`,
        chapterId: groupId,
      });
      if (res.data?.status === '1') { alert(res.data.message); setSaving(false); return; }
      alert(editing ? 'Updated Successfully' : 'Added Successfully');
      setShowModal(false);
      setEditing(null);
      fetchData();
    } catch (err) { setError(err.message || 'Save failed'); }
    finally { setSaving(false); }
  };

  // --- Delete ---
  const handleDelete = async () => {
    try {
      const res = await deleteBOD(deleteTarget.BOD_pkID, '');
      if (res.data?.status === '0') alert('Member Deleted Successfully');
      else alert(res.data?.message || 'Delete failed');
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  // --- Export ---
  const handleExport = () => {
    if (items.length === 0) { alert('No records found'); return; }
    const headers = ['Name', 'Designation', 'Mobile Number', 'Email'];
    const csv = [headers.join(','), ...items.map(m => [
      `"${m.memberName || ''}"`, `"${m.MemberDesignation || ''}"`, `"${m.membermobile || ''}"`, `"${m.Email || ''}"`
    ].join(','))].join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a'); a.href = url; a.download = 'ExecutiveCommittee.csv'; a.click();
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{chapterName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Executive Committee</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleExport} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4z" /></svg>
            Export To Excel
          </button>
          <button onClick={openAdd} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>+ Add</button>
          <button onClick={() => groupId ? navigate(`/groups/${groupId}`) : navigate(-1)} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {error && <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>{error}</div>}

      {/* Table */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '12px' }}>
          <thead>
            <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Name</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Designation</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Mobile Number</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Email</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '70px' }}>Reorder</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr><td colSpan={7} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No Record Found!</td></tr>
            ) : (
              items.map((m, idx) => (
                <tr key={m.BOD_pkID || m.profileID || idx}
                  draggable
                  onDragStart={() => handleDragStart(idx)}
                  onDragOver={handleDragOver}
                  onDrop={() => handleDrop(idx)}
                  style={{ backgroundColor: dragIdx === idx ? '#e3f2fd' : (idx % 2 === 0 ? '#fff' : '#f8f8f8'), borderBottom: '1px solid #eee', cursor: 'grab' }}
                >
                  <td style={{ padding: '8px 12px', color: '#333' }}>{m.memberName}</td>
                  <td style={{ padding: '8px 12px', color: '#555' }}>{m.MemberDesignation}</td>
                  <td style={{ padding: '8px 12px', color: '#555' }}>{m.membermobile}</td>
                  <td style={{ padding: '8px 12px', color: '#555', fontSize: '11px' }}>{m.Email}</td>
                  {/* Reorder - drag handle */}
                  <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                    <div title="Drag to reorder" style={{ cursor: 'grab', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="20" height="20" viewBox="0 0 24 24" fill="#2196F3"><path d="M3 15h18v-2H3v2zm0 4h18v-2H3v2zm0-8h18V9H3v2zm0-6v2h18V5H3z" /></svg>
                    </div>
                  </td>
                  {/* Edit */}
                  <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                    <button onClick={() => openEdit(m)} title="Edit" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#0ead9a', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                    </button>
                  </td>
                  {/* Delete */}
                  <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                    <button onClick={() => setDeleteTarget(m)} title="Delete" style={{ width: '28px', height: '28px', borderRadius: '4px', backgroundColor: '#f44336', color: '#fff', border: 'none', cursor: 'pointer', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}>
                      <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Edit/Add Modal */}
      {showModal && (
        <div style={{ position: 'fixed', inset: 0, zIndex: 1050, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
          <div style={{ position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={() => setShowModal(false)} />
          <div style={{ position: 'relative', backgroundColor: '#fff', borderRadius: '6px', width: '100%', maxWidth: '480px', margin: '0 15px', zIndex: 1051 }}>
            <div style={{ padding: '15px 20px', borderBottom: '1px solid #e5e5e5', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <h4 style={{ margin: 0, fontSize: '16px', fontWeight: 'bold' }}>{editing ? 'Edit Member' : 'Add Member'}</h4>
              <button onClick={() => setShowModal(false)} style={{ background: 'none', border: 'none', fontSize: '22px', cursor: 'pointer' }}>&times;</button>
            </div>
            <div style={{ padding: '20px 25px' }}>
              <div style={{ marginBottom: '12px' }}>
                <label style={labelStyle}>Name :</label>
                {editing ? (
                  <input type="text" value={form.Name} disabled style={{ ...inputStyle, backgroundColor: '#eee' }} />
                ) : (
                  <select value={form.Name} onChange={handleNameChange} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                    <option value="">-Select-</option>
                    {membersList.map((m, i) => <option key={m.Id || i} value={m.MemberName}>{m.MemberName}</option>)}
                  </select>
                )}
              </div>
              <div style={{ marginBottom: '12px' }}>
                <label style={labelStyle}>Designation :</label>
                <select value={form.Designation} onChange={(e) => setForm({ ...form, Designation: e.target.value, OtherDesignation: '' })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                  {designationOptions.map(d => <option key={d} value={d === '-Select-' ? '' : d}>{d}</option>)}
                </select>
              </div>
              {form.Designation === 'Others' && (
                <div style={{ marginBottom: '12px' }}>
                  <label style={labelStyle}>Other :</label>
                  <input type="text" value={form.OtherDesignation} onChange={(e) => setForm({ ...form, OtherDesignation: e.target.value })} placeholder="Enter Designation" style={inputStyle} />
                </div>
              )}
              <div style={{ display: 'flex', gap: '15px', marginBottom: '15px' }}>
                <div style={{ flex: 1 }}>
                  <label style={labelStyle}>Email :</label>
                  <input type="email" value={form.Email} disabled style={{ ...inputStyle, backgroundColor: '#eee' }} />
                </div>
                <div style={{ flex: 1 }}>
                  <label style={labelStyle}>Phone no :</label>
                  <input type="tel" value={form.PhoneNo} disabled style={{ ...inputStyle, backgroundColor: '#eee' }} />
                </div>
              </div>
              <div style={{ textAlign: 'center' }}>
                <button onClick={handleSave} disabled={saving} style={{ display: 'inline-flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 20px', borderRadius: '4px', fontSize: '13px', cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1 }}>
                  {editing ? 'Update' : 'Save'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      <ConfirmDialog isOpen={!!deleteTarget} onClose={() => setDeleteTarget(null)} onConfirm={handleDelete} title="Delete" message={`Are you sure you want to delete "${deleteTarget?.memberName}"?`} />
    </div>
  );
}