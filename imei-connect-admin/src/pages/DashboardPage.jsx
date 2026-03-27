import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const modules = [
  {
    label: 'Branch & Chapter\ncommittees',
    path: '/groups',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <rect x="8" y="28" width="20" height="24" rx="3" fill="#FF9800" />
        <rect x="36" y="20" width="20" height="32" rx="3" fill="#F44336" />
        <rect x="22" y="12" width="20" height="40" rx="3" fill="#FFC107" />
        <rect x="14" y="34" width="8" height="6" rx="1" fill="#fff" />
        <rect x="28" y="20" width="8" height="6" rx="1" fill="#fff" />
        <rect x="42" y="28" width="8" height="6" rx="1" fill="#fff" />
      </svg>
    ),
  },
  {
    label: 'Member',
    path: '/members',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="32" cy="20" r="10" fill="#FFC107" />
        <path d="M16 52c0-8.837 7.163-16 16-16s16 7.163 16 16" fill="#FF9800" />
      </svg>
    ),
  },
  {
    label: 'MER (I)',
    path: '/mer',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="32" cy="18" r="10" fill="#8BC34A" />
        <path d="M18 50c0-7.732 6.268-14 14-14s14 6.268 14 14" fill="#4CAF50" />
        <rect x="38" y="30" width="16" height="20" rx="2" fill="#FFC107" />
        <path d="M42 36h8M42 40h8M42 44h6" stroke="#fff" strokeWidth="1.5" />
      </svg>
    ),
  },
  {
    label: 'iMelange',
    path: '/imelange',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="32" cy="24" r="12" fill="#9C27B0" opacity="0.2" />
        <circle cx="32" cy="20" r="8" fill="#9C27B0" />
        <path d="M32 32c-6 0-10 4-10 4v4h20v-4s-4-4-10-4z" fill="#7B1FA2" />
        <path d="M26 14l6-6 6 6" stroke="#FFC107" strokeWidth="2" fill="none" />
      </svg>
    ),
  },
  {
    label: 'Governing council\nMember',
    path: '/governing-council',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="20" cy="22" r="8" fill="#2196F3" />
        <circle cx="44" cy="22" r="8" fill="#4CAF50" />
        <circle cx="32" cy="18" r="8" fill="#FF9800" />
        <path d="M10 48c0-6 4-10 10-10h4c6 0 10 4 10 10" fill="#1976D2" />
        <path d="M30 48c0-6 4-10 10-10h4c6 0 10 4 10 10" fill="#388E3C" />
        <path d="M20 46c0-6 4-10 10-10h4c6 0 10 4 10 10" fill="#F57C00" />
      </svg>
    ),
  },
  {
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
    label: 'Upcoming Events',
    path: '/upcoming-events',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <rect x="10" y="14" width="32" height="36" rx="4" fill="#2196F3" />
        <rect x="10" y="14" width="32" height="10" rx="4" fill="#1565C0" />
        <rect x="16" y="30" width="8" height="6" rx="1" fill="#fff" />
        <rect x="28" y="30" width="8" height="6" rx="1" fill="#fff" />
        <rect x="16" y="40" width="8" height="6" rx="1" fill="#fff" />
        <circle cx="48" cy="40" r="12" fill="#4CAF50" />
        <path d="M48 34v6l4 2" stroke="#fff" strokeWidth="2" strokeLinecap="round" />
      </svg>
    ),
  },
  {
    label: 'Past Presidents',
    path: '/past-presidents',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <circle cx="28" cy="22" r="10" fill="#00BCD4" />
        <path d="M14 52c0-8 6-14 14-14s14 6 14 14" fill="#0097A7" />
        <path d="M44 16l4 8 8 1-6 5 2 8-8-4-8 4 2-8-6-5 8-1z" fill="#FFC107" />
      </svg>
    ),
  },
  {
    label: 'Reports',
    path: '/reports',
    icon: (
      <svg width="40" height="40" viewBox="0 0 64 64" fill="none">
        <rect x="14" y="8" width="36" height="48" rx="4" fill="#607D8B" />
        <rect x="20" y="16" width="24" height="3" rx="1" fill="#fff" />
        <rect x="20" y="22" width="16" height="3" rx="1" fill="#B0BEC5" />
        <rect x="20" y="30" width="10" height="18" rx="2" fill="#4CAF50" />
        <rect x="34" y="36" width="10" height="12" rx="2" fill="#2196F3" />
      </svg>
    ),
  },
];

export default function DashboardPage() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const roleName = (user?.role === 'Admin' || user?.role === 'SuperAdmin') ? 'National Admin' : 'Member';

  return (
    <div>
      {/* Page Title */}
      <div style={{ marginBottom: '20px' }}>
        <span style={{ color: '#1a297d', fontSize: '14px' }}>{roleName}</span>
        <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Dashboard</span>
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
              onClick={() => navigate(mod.path)}
              style={{
                width: '140px',
                padding: '25px 10px 18px',
                textAlign: 'center',
                cursor: 'pointer',
                backgroundColor: '#f2f2f2',
                borderRadius: '16px',
                border: '1px solid #efefef',
                boxShadow: '0px 4px 8px 0 rgba(0,0,0,0.05)',
                transition: 'box-shadow 0.3s',
              }}
              onMouseEnter={(e) => { e.currentTarget.style.boxShadow = '0px 10px 20px 0 rgba(0,0,0,0.1)'; }}
              onMouseLeave={(e) => { e.currentTarget.style.boxShadow = '0px 4px 8px 0 rgba(0,0,0,0.05)'; }}
            >
              <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '10px' }}>
                {mod.icon}
              </div>
              <div style={{
                fontSize: '12px',
                fontWeight: '500',
                color: '#333',
                lineHeight: '1.3',
                whiteSpace: 'pre-line',
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
