import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getGroupMembers } from '../api/groupService';

const modules = [
  {
    key: 'member',
    label: 'Member',
    path: '/members',
    countField: 'memberCount',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="32" cy="20" r="10" fill="#FFC107" />
        <path d="M16 52c0-8.837 7.163-16 16-16s16 7.163 16 16" fill="#FF9800" />
      </svg>
    ),
  },
  {
    key: 'pastEvents',
    label: 'Past Events',
    path: '/events',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <rect x="10" y="10" width="28" height="8" rx="2" fill="#FFC107" />
        <rect x="10" y="18" width="28" height="28" rx="2" fill="#2196F3" />
        <rect x="16" y="24" width="6" height="6" rx="1" fill="#fff" />
        <rect x="26" y="24" width="6" height="6" rx="1" fill="#fff" />
        <rect x="16" y="34" width="6" height="6" rx="1" fill="#fff" />
        <rect x="26" y="34" width="6" height="6" rx="1" fill="#1565C0" />
        <circle cx="48" cy="40" r="12" fill="#fff" stroke="#2196F3" strokeWidth="2" />
        <path d="M48 34v6l4 3" stroke="#2196F3" strokeWidth="2" strokeLinecap="round" />
      </svg>
    ),
  },
  {
    key: 'announcements',
    label: 'Announcements',
    path: '/announcements',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <path d="M12 28l28-10v28L12 36v-8z" fill="#2196F3" />
        <rect x="8" y="26" width="6" height="12" rx="2" fill="#1976D2" />
        <path d="M40 18v28" stroke="#1565C0" strokeWidth="3" />
        <circle cx="46" cy="18" r="4" fill="#FFC107" />
        <circle cx="52" cy="24" r="3" fill="#FFC107" />
        <path d="M14 38l-2 10h6l-2-10" fill="#1976D2" />
      </svg>
    ),
  },
  {
    key: 'pastChairman',
    label: 'Past Chairman',
    path: '/past-presidents',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="32" cy="22" r="10" fill="#00BCD4" />
        <path d="M18 52c0-8 6-14 14-14s14 6 14 14" fill="#0097A7" />
      </svg>
    ),
  },
  {
    key: 'executiveCommittee',
    label: 'Executive Committee',
    path: '/executive-committee',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="22" cy="20" r="8" fill="#9C27B0" />
        <circle cx="42" cy="20" r="8" fill="#4CAF50" />
        <circle cx="32" cy="28" r="6" fill="#E91E63" />
        <path d="M10 48c0-6 5-10 12-10h4c7 0 12 4 12 10" fill="#7B1FA2" />
        <path d="M30 48c0-6 5-10 12-10h4c7 0 12 4 12 10" fill="#388E3C" />
      </svg>
    ),
  },
  {
    key: 'eventAttendance',
    label: 'Event Attendance',
    path: '/attendance',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <rect x="14" y="8" width="36" height="48" rx="4" fill="#7E57C2" />
        <rect x="20" y="16" width="24" height="3" rx="1" fill="#fff" />
        <rect x="20" y="22" width="18" height="3" rx="1" fill="#D1C4E9" />
        <rect x="20" y="28" width="24" height="3" rx="1" fill="#fff" />
        <rect x="20" y="34" width="14" height="3" rx="1" fill="#D1C4E9" />
        <rect x="20" y="40" width="20" height="3" rx="1" fill="#fff" />
      </svg>
    ),
  },
  {
    key: 'reports',
    label: 'Reports',
    path: '/reports',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <rect x="14" y="8" width="36" height="48" rx="4" fill="#607D8B" />
        <rect x="22" y="24" width="8" height="24" rx="2" fill="#4CAF50" />
        <rect x="34" y="32" width="8" height="16" rx="2" fill="#2196F3" />
        <rect x="20" y="14" width="24" height="3" rx="1" fill="#fff" />
      </svg>
    ),
  },
];

export default function GroupDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [group, setGroup] = useState(null);
  const [memberCount, setMemberCount] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    (async () => {
      try {
        const mRes = await getGroupMembers(id);
        const members = mRes.data?.MemberDetail?.NewMemberList || [];
        setMemberCount(members.length);
        // Use first member's GrpName as group name
        if (members.length > 0) {
          setGroup({ GrpName: members[0].GrpName || 'Chapter' });
        } else {
          setGroup({ GrpName: 'Chapter' });
        }
      } catch { setGroup({ GrpName: 'Chapter' }); }
      finally { setLoading(false); }
    })();
  }, [id]);

  if (loading) return <LoadingSpinner className="h-screen" />;

  const groupName = group?.GrpName || group?.grpName || 'Chapter';

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{groupName}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Dashboard</span>
        </div>
        <button
          onClick={() => navigate('/groups')}
          style={{
            display: 'flex', alignItems: 'center', gap: '6px',
            backgroundColor: '#1a297d', color: '#fff', border: 'none',
            padding: '6px 16px', borderRadius: '4px', fontSize: '13px',
            cursor: 'pointer',
          }}
        >
          <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <path d="M19 12H5M12 19l-7-7 7-7" />
          </svg>
          Back
        </button>
      </div>

      {/* Module Grid */}
      <div style={{
        backgroundColor: '#fff',
        borderRadius: '15px',
        padding: '30px 20px',
        boxShadow: '0 3px 5px 0 rgba(0,0,0,0.1)',
      }}>
        <div style={{
          display: 'flex',
          flexWrap: 'wrap',
          justifyContent: 'center',
          gap: '20px',
        }}>
          {modules.map((mod, idx) => (
            <div
              key={idx}
              onClick={() => navigate(mod.path + (mod.path === '/members' ? `?groupId=${id}` : `?groupId=${id}`))}

              style={{
                width: '130px',
                padding: '20px 10px 15px',
                textAlign: 'center',
                cursor: 'pointer',
                backgroundColor: '#f2f2f2',
                borderRadius: '16px',
                border: '1px solid #efefef',
                boxShadow: '0px 4px 8px 0 rgba(0,0,0,0.05)',
                transition: 'box-shadow 0.3s',
                position: 'relative',
              }}
              onMouseEnter={(e) => { e.currentTarget.style.boxShadow = '0px 10px 20px 0 rgba(0,0,0,0.1)'; }}
              onMouseLeave={(e) => { e.currentTarget.style.boxShadow = '0px 4px 8px 0 rgba(0,0,0,0.05)'; }}
            >
              {/* Red badge for Member count */}
              {mod.key === 'member' && memberCount > 0 && (
                <span style={{
                  position: 'absolute', top: '8px', right: '8px',
                  backgroundColor: '#f44336', color: '#fff',
                  fontSize: '10px', fontWeight: 'bold',
                  padding: '2px 6px', borderRadius: '10px',
                  minWidth: '20px', textAlign: 'center',
                  lineHeight: '14px',
                }}>
                  {memberCount}
                </span>
              )}
              <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '10px' }}>
                {mod.icon}
              </div>
              <div style={{
                fontSize: '12px',
                fontWeight: '500',
                color: '#333',
                lineHeight: '1.3',
              }}>
                {mod.label}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
