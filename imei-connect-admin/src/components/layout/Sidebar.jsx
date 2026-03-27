import { NavLink } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const menuItems = [
  { label: 'Dashboard', path: '/dashboard', icon: (
    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1a297d" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
      <path d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
    </svg>
  )},
  { label: 'Branch & Chapter', path: '/groups', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <rect x="8" y="28" width="20" height="24" rx="3" fill="#FF9800" />
      <rect x="36" y="20" width="20" height="32" rx="3" fill="#F44336" />
      <rect x="22" y="12" width="20" height="40" rx="3" fill="#FFC107" />
      <rect x="14" y="34" width="8" height="6" rx="1" fill="#fff" />
      <rect x="28" y="20" width="8" height="6" rx="1" fill="#fff" />
      <rect x="42" y="28" width="8" height="6" rx="1" fill="#fff" />
    </svg>
  )},
  { label: 'Member', path: '/members', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <circle cx="32" cy="20" r="10" fill="#FFC107" />
      <path d="M16 52c0-8.837 7.163-16 16-16s16 7.163 16 16" fill="#FF9800" />
    </svg>
  )},
  { label: 'MER (I)', path: '/mer', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <circle cx="32" cy="18" r="10" fill="#8BC34A" />
      <path d="M18 50c0-7.732 6.268-14 14-14s14 6.268 14 14" fill="#4CAF50" />
      <rect x="38" y="30" width="16" height="20" rx="2" fill="#FFC107" />
      <path d="M42 36h8M42 40h8M42 44h6" stroke="#fff" strokeWidth="1.5" />
    </svg>
  )},
  { label: 'iMelange', path: '/imelange', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <circle cx="32" cy="24" r="12" fill="#9C27B0" opacity="0.2" />
      <circle cx="32" cy="20" r="8" fill="#9C27B0" />
      <path d="M32 32c-6 0-10 4-10 4v4h20v-4s-4-4-10-4z" fill="#7B1FA2" />
      <path d="M26 14l6-6 6 6" stroke="#FFC107" strokeWidth="2" fill="none" />
    </svg>
  )},
  { label: 'Governing Council', path: '/governing-council', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <circle cx="20" cy="22" r="8" fill="#2196F3" />
      <circle cx="44" cy="22" r="8" fill="#4CAF50" />
      <circle cx="32" cy="18" r="8" fill="#FF9800" />
      <path d="M10 48c0-6 4-10 10-10h4c6 0 10 4 10 10" fill="#1976D2" />
      <path d="M30 48c0-6 4-10 10-10h4c6 0 10 4 10 10" fill="#388E3C" />
      <path d="M20 46c0-6 4-10 10-10h4c6 0 10 4 10 10" fill="#F57C00" />
    </svg>
  )},
  { label: 'Announcements', path: '/announcements', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <path d="M12 28l28-10v28L12 36v-8z" fill="#2196F3" />
      <rect x="8" y="26" width="6" height="12" rx="2" fill="#1976D2" />
      <path d="M40 18v28" stroke="#1565C0" strokeWidth="3" />
      <circle cx="46" cy="18" r="4" fill="#FFC107" />
      <circle cx="52" cy="24" r="3" fill="#FFC107" />
      <path d="M14 38l-2 10h6l-2-10" fill="#1976D2" />
    </svg>
  )},
  { label: 'Upcoming Events', path: '/upcoming-events', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <rect x="10" y="14" width="32" height="36" rx="4" fill="#2196F3" />
      <rect x="10" y="14" width="32" height="10" rx="4" fill="#1565C0" />
      <rect x="16" y="30" width="8" height="6" rx="1" fill="#fff" />
      <rect x="28" y="30" width="8" height="6" rx="1" fill="#fff" />
      <rect x="16" y="40" width="8" height="6" rx="1" fill="#fff" />
      <circle cx="48" cy="40" r="12" fill="#4CAF50" />
      <path d="M48 34v6l4 2" stroke="#fff" strokeWidth="2" strokeLinecap="round" />
    </svg>
  )},
  { label: 'Past Presidents', path: '/past-presidents', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <circle cx="28" cy="22" r="10" fill="#00BCD4" />
      <path d="M14 52c0-8 6-14 14-14s14 6 14 14" fill="#0097A7" />
      <path d="M44 16l4 8 8 1-6 5 2 8-8-4-8 4 2-8-6-5 8-1z" fill="#FFC107" />
    </svg>
  )},
  { label: 'Reports', path: '/reports', icon: (
    <svg width="20" height="20" viewBox="0 0 64 64" fill="none">
      <rect x="14" y="8" width="36" height="48" rx="4" fill="#607D8B" />
      <rect x="20" y="16" width="24" height="3" rx="1" fill="#fff" />
      <rect x="20" y="22" width="16" height="3" rx="1" fill="#B0BEC5" />
      <rect x="20" y="30" width="10" height="18" rx="2" fill="#4CAF50" />
      <rect x="34" y="36" width="10" height="12" rx="2" fill="#2196F3" />
    </svg>
  )},
];

export default function Sidebar({ isOpen, onClose }) {
  const { user } = useAuth();

  return (
    <>
      {/* Mobile overlay */}
      {isOpen && (
        <div className="fixed inset-0 bg-black/30 z-20 lg:hidden" onClick={onClose} />
      )}
      <aside
        className={`fixed top-0 left-0 z-30 h-full w-64 shadow-sm transition-transform duration-300 lg:translate-x-0 lg:static lg:z-auto ${
          isOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
        style={{ backgroundColor: '#fff', borderRight: '1px solid #e7e7e7' }}
      >
        {/* Header - Logo area */}
        <div style={{ backgroundColor: '#6b6b6b', borderBottom: '1px solid #e7e7e7', padding: '18px 20px', textAlign: 'center' }}>
          <p style={{ color: '#fff', fontSize: '15px', fontWeight: 'bold', letterSpacing: '0.5px', margin: 0 }}>Admin Panel</p>
        </div>

        {/* Navigation Menu */}
        <nav className="mt-1 px-2 pb-4 overflow-y-auto h-[calc(100%-72px)]">
          {menuItems.map((item) => (
            <NavLink
              key={item.path}
              to={item.path}
              onClick={onClose}
              className={({ isActive }) =>
                `flex items-center gap-3 px-3 py-3.5 text-sm transition-colors mb-0.5 ${
                  isActive
                    ? ''
                    : 'hover:bg-gray-50'
                }`
              }
              style={({ isActive }) => ({
                backgroundColor: isActive ? '#eee' : 'transparent',
                color: isActive ? '#1a297d' : '#333',
                fontWeight: 'bold',
                borderBottom: '1px solid #e7e7e7',
                borderRadius: '0',
              })}
            >
              <span className="flex-shrink-0">{item.icon}</span>
              <span>{item.label}</span>
            </NavLink>
          ))}
        </nav>
      </aside>
    </>
  );
}