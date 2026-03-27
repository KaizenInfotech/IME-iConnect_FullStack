import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import ConfirmDialog from '../components/shared/ConfirmDialog';
import { getMembers, deleteMember } from '../api/memberService';

const PAGE_SIZE = 15;

export default function MembersPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const [allMembers, setAllMembers] = useState([]);
  const [members, setMembers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [pageNo, setPageNo] = useState(1);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [error, setError] = useState('');
  const [chapterName, setChapterName] = useState('National Admin');
  const [changeTarget, setChangeTarget] = useState(null);
  const [changeForm, setChangeForm] = useState({ newMobile: '', newEmail: '' });
  const [changeSaving, setChangeSaving] = useState(false);

  useEffect(() => { fetchData(); }, []);

  useEffect(() => {
    if (!searchTerm.trim()) {
      setMembers(allMembers);
    } else {
      const q = searchTerm.toLowerCase();
      setMembers(allMembers.filter(m =>
        (m.MemberName || m.memberName || '').toLowerCase().includes(q) ||
        (m.MemberEmail || m.memberEmail || '').toLowerCase().includes(q) ||
        (m.MemberMobile || m.memberMobile || '').includes(q)
      ));
    }
    setPageNo(1);
  }, [searchTerm, allMembers]);

  const fetchData = async () => {
    setLoading(true);
    try {
      if (filterGroupId) {
        // Fetch members for specific chapter only
        const memRes = await getMembers(filterGroupId);
        const memberList = memRes.data?.MemberDetail?.NewMemberList || [];
        const grpName = memberList[0]?.GrpName || '';
        if (grpName) setChapterName(grpName);
        memberList.forEach(m => { m.GroupName = grpName; m.GrpName = grpName; m.GroupId = filterGroupId; });
        setAllMembers(memberList);
        setMembers(memberList);
      } else {
        // Get all chapters/branches via GetClubList
        const { getClubList } = await import('../api/groupService');
        const clubRes = await getClubList();
        const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];

        // Fetch members for each chapter
        let allMems = [];
        for (const c of clubs) {
          const gid = c.GroupId;
          const grpName = c.group_name || '';
          try {
            const memRes = await getMembers(String(gid));
            const memberList = memRes.data?.MemberDetail?.NewMemberList || [];
            memberList.forEach(m => { m.GroupName = grpName; m.GrpName = grpName; m.GroupId = gid; });
            allMems = allMems.concat(memberList);
          } catch {}
        }
        setAllMembers(allMems);
        setMembers(allMems);
      }
    } catch { setError('Failed to load members'); }
    finally { setLoading(false); }
  };

  const handleDelete = async () => {
    try {
      const pid = deleteTarget.profileID || deleteTarget.masterID || deleteTarget.Id || deleteTarget.id;
      await deleteMember(pid);
      setDeleteTarget(null);
      fetchData();
    } catch { setError('Delete failed'); }
  };

  // Pagination
  const totalPages = Math.ceil(members.length / PAGE_SIZE);
  const paginatedMembers = members.slice((pageNo - 1) * PAGE_SIZE, pageNo * PAGE_SIZE);

  const renderPagination = () => {
    if (totalPages <= 1) return null;
    const maxVisible = 8;
    const pages = [];
    for (let i = 1; i <= Math.min(totalPages, maxVisible); i++) pages.push(i);

    return (
      <div style={{ display: 'flex', alignItems: 'center', gap: '2px', padding: '8px 0', fontSize: '12px' }}>
        {pages.map(p => (
          p === pageNo ? (
            <span key={p} style={{
              backgroundColor: '#1a297d', color: '#fff',
              padding: '3px 8px', borderRadius: '3px', fontWeight: 'bold',
            }}>{p}</span>
          ) : (
            <a key={p} href="#" onClick={(e) => { e.preventDefault(); setPageNo(p); }} style={{
              padding: '3px 8px', borderRadius: '3px', color: '#1a297d',
              textDecoration: 'none', border: '1px solid #ddd',
            }}>{p}</a>
          )
        ))}
        {totalPages > maxVisible && (
          <>
            <span style={{ padding: '3px 4px', color: '#999' }}>...</span>
            <a href="#" onClick={(e) => { e.preventDefault(); if (pageNo < totalPages) setPageNo(pageNo + 1); }} style={{
              padding: '3px 8px', borderRadius: '3px', color: '#1a297d',
              textDecoration: 'none', border: '1px solid #ddd',
            }}>&gt;</a>
            <a href="#" onClick={(e) => { e.preventDefault(); setPageNo(totalPages); }} style={{
              padding: '3px 8px', borderRadius: '3px', color: '#1a297d',
              textDecoration: 'none', border: '1px solid #ddd',
            }}>Last</a>
          </>
        )}
      </div>
    );
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{chapterName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Member</span>
        </div>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
          {/* Member Count */}
          <span style={{ fontSize: '13px', color: '#1a297d', fontWeight: 'bold' }}>
            Total: {members.length}
          </span>
          {/* Search */}
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
            style={{
              height: '32px', border: '1px solid #ccc', borderRadius: '4px',
              padding: '4px 10px', fontSize: '13px', outline: 'none', width: '180px',
            }}
          />
          {/* Add New - only for admin (not chapter view) */}
          {!filterGroupId && (
            <button
              onClick={() => navigate('/members/add')}
              style={{
                display: 'flex', alignItems: 'center', gap: '4px',
                backgroundColor: '#6b9300', color: '#fff', border: 'none',
                padding: '6px 14px', borderRadius: '4px', fontSize: '13px',
                cursor: 'pointer', whiteSpace: 'nowrap',
              }}
            >
              + Add New
            </button>
          )}
          {/* Back */}
          <button
            onClick={() => navigate(-1)}
            style={{
              display: 'flex', alignItems: 'center', gap: '6px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '6px 14px', borderRadius: '4px', fontSize: '13px',
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

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>
          {error}
          <button onClick={() => setError('')} style={{ float: 'right', background: 'none', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>&times;</button>
        </div>
      )}

      {/* Pagination - top */}
      {renderPagination()}

      {/* Table */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)' }}>
        <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '12px' }}>
          <thead>
            <tr style={{ backgroundColor: '#1a297d', color: '#fff' }}>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal', width: '50px' }}>Photo</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Name</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Chapter / Branch Name</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Mobile No</th>
              <th style={{ padding: '10px 12px', textAlign: 'left', fontWeight: 'normal' }}>Email</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Edit</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Delete</th>
              <th style={{ padding: '10px 8px', textAlign: 'center', fontWeight: 'normal', width: '55px' }}>Change</th>
            </tr>
          </thead>
          <tbody>
            {paginatedMembers.length === 0 ? (
              <tr><td colSpan={8} style={{ padding: '30px', textAlign: 'center', color: '#999' }}>No members found.</td></tr>
            ) : (
              paginatedMembers.map((m, idx) => {
                const mId = m.masterID || m.profileID || m.Id || m.id;
                const name = [m.memberName, m.middleName, m.lastName].filter(Boolean).join(' ').trim() || m.MemberName || '';
                const chapter = m.GrpName || m.GroupName || '';
                const mobile = m.memberMobile || m.MemberMobile || '';
                const email = m.memberEmail || m.MemberEmail || '';
                const photo = m.profilePic || m.ProfilePic || '';

                return (
                  <tr
                    key={mId || idx}
                    style={{
                      backgroundColor: idx % 2 === 0 ? '#fff' : '#f8f8f8',
                      borderBottom: '1px solid #eee',
                    }}
                  >
                    {/* Photo */}
                    <td style={{ padding: '6px 12px' }}>
                      <div style={{
                        width: '30px', height: '30px', borderRadius: '50%',
                        backgroundColor: '#e0e0e0', overflow: 'hidden',
                        display: 'flex', alignItems: 'center', justifyContent: 'center',
                      }}>
                        {photo ? (
                          <img src={photo} alt="" style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
                        ) : (
                          <svg width="16" height="16" fill="#999" viewBox="0 0 20 20">
                            <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
                          </svg>
                        )}
                      </div>
                    </td>
                    {/* Name */}
                    <td style={{ padding: '8px 12px', color: '#333' }}>{name}</td>
                    {/* Chapter / Branch Name */}
                    <td style={{ padding: '8px 12px', color: '#555' }}>{chapter}</td>
                    {/* Mobile No */}
                    <td style={{ padding: '8px 12px', color: '#555' }}>{mobile}</td>
                    {/* Email */}
                    <td style={{ padding: '8px 12px', color: '#555', fontSize: '11px' }}>{email}</td>
                    {/* Edit - green circle */}
                    <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => navigate(`/members/${mId}?groupId=${m.grpID || m.GroupId || filterGroupId || ''}`)}
                        title="Edit"
                        style={{
                          width: '26px', height: '26px', borderRadius: '50%',
                          backgroundColor: '#0ead9a', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="11" height="11" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                        </svg>
                      </button>
                    </td>
                    {/* Delete - red circle */}
                    <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => setDeleteTarget(m)}
                        title="Delete"
                        style={{
                          width: '26px', height: '26px', borderRadius: '50%',
                          backgroundColor: '#f44336', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="11" height="11" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M6 18L18 6M6 6l12 12" />
                        </svg>
                      </button>
                    </td>
                    {/* Change - blue circle */}
                    <td style={{ padding: '8px 8px', textAlign: 'center' }}>
                      <button
                        onClick={() => { setChangeTarget(m); setChangeForm({ newMobile: '', newEmail: '' }); }}
                        title="Change"
                        style={{
                          width: '26px', height: '26px', borderRadius: '50%',
                          backgroundColor: '#2196F3', color: '#fff',
                          border: 'none', cursor: 'pointer',
                          display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                        }}
                      >
                        <svg width="11" height="11" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
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

      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title="Delete Member"
        message={`Are you sure you want to delete "${deleteTarget?.MemberName || deleteTarget?.memberName}"?`}
      />

      {/* Change Contact Modal */}
      {changeTarget && (
        <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000 }}>
          <div style={{ backgroundColor: '#fff', borderRadius: '8px', width: '450px', maxWidth: '90%', boxShadow: '0 10px 30px rgba(0,0,0,0.3)' }}>
            {/* Header */}
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '15px 20px', borderBottom: '1px solid #eee' }}>
              <h3 style={{ margin: 0, fontSize: '16px', fontWeight: '600', color: '#333' }}>Update Required</h3>
              <button onClick={() => setChangeTarget(null)} style={{ background: 'none', border: 'none', fontSize: '20px', cursor: 'pointer', color: '#999' }}>&times;</button>
            </div>
            {/* Body */}
            <div style={{ padding: '20px' }}>
              {/* Existing Details */}
              <div style={{ marginBottom: '20px' }}>
                <div style={{ fontSize: '13px', fontWeight: '600', color: '#333', marginBottom: '10px' }}>Existing Contact Details</div>
                <div style={{ marginBottom: '8px' }}>
                  <label style={{ fontSize: '12px', color: '#666', display: 'block', marginBottom: '3px' }}>Mobile No</label>
                  <div style={{ display: 'flex', gap: '8px' }}>
                    <input value="India (+91)" disabled style={{ width: '120px', height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', backgroundColor: '#f5f5f5' }} />
                    <input value={changeTarget?.memberMobile || changeTarget?.MemberMobile || ''} disabled style={{ flex: 1, height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', backgroundColor: '#f5f5f5' }} />
                  </div>
                </div>
                <div>
                  <label style={{ fontSize: '12px', color: '#666', display: 'block', marginBottom: '3px' }}>Email Id</label>
                  <input value={changeTarget?.memberEmail || changeTarget?.MemberEmail || ''} disabled style={{ width: '100%', height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', backgroundColor: '#f5f5f5' }} />
                </div>
              </div>
              {/* New Details */}
              <div>
                <div style={{ fontSize: '13px', fontWeight: '600', color: '#333', marginBottom: '10px' }}>New Contact Details</div>
                <div style={{ marginBottom: '8px' }}>
                  <label style={{ fontSize: '12px', color: '#666', display: 'block', marginBottom: '3px' }}>Mobile Number</label>
                  <div style={{ display: 'flex', gap: '8px' }}>
                    <input value="India (+91)" disabled style={{ width: '120px', height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', backgroundColor: '#f5f5f5' }} />
                    <input value={changeForm.newMobile} onChange={(e) => setChangeForm({ ...changeForm, newMobile: e.target.value })} placeholder="Enter Mobile Number" style={{ flex: 1, height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', outline: 'none' }} />
                  </div>
                </div>
                <div>
                  <label style={{ fontSize: '12px', color: '#666', display: 'block', marginBottom: '3px' }}>Email Id</label>
                  <input value={changeForm.newEmail} onChange={(e) => setChangeForm({ ...changeForm, newEmail: e.target.value })} placeholder="Enter Email Id" style={{ width: '100%', height: '32px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', outline: 'none' }} />
                </div>
              </div>
            </div>
            {/* Footer */}
            <div style={{ padding: '15px 20px', borderTop: '1px solid #eee', textAlign: 'center' }}>
              <button
                disabled={changeSaving}
                onClick={async () => {
                  if (!changeForm.newMobile && !changeForm.newEmail) { alert('Please enter new mobile or email'); return; }
                  setChangeSaving(true);
                  try {
                    const { updateProfile } = await import('../api/memberService');
                    const profileId = String(changeTarget?.profileID || changeTarget?.masterID || changeTarget?.Id || '');
                    await updateProfile({
                      ProfileId: profileId,
                      memberName: [changeTarget?.memberName, changeTarget?.middleName, changeTarget?.lastName].filter(Boolean).join(' ').trim(),
                      memberMobile: changeForm.newMobile || changeTarget?.memberMobile || '',
                      memberEmailid: changeForm.newEmail || changeTarget?.memberEmail || '',
                    });
                    alert('Contact updated successfully');
                    setChangeTarget(null);
                    fetchData();
                  } catch { alert('Update failed'); }
                  finally { setChangeSaving(false); }
                }}
                style={{ backgroundColor: '#f0a500', color: '#fff', border: 'none', padding: '8px 30px', borderRadius: '4px', fontSize: '13px', fontWeight: '600', cursor: 'pointer' }}
              >
                {changeSaving ? 'Saving...' : 'Save'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
