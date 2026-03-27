import { createContext, useContext, useState, useEffect } from 'react';
import { webLogin } from '../api/authService';

const AuthContext = createContext(null);

// Temporary mock login until backend is re-deployed
const MOCK_USER = {
  id: 13010,
  masterUID: '13010',
  profileId: '13010',
  name: 'Mukesh dhole',
  role: 'SuperAdmin',
  mobile: '9763128181',
  email: 'mukesh.dhole@kaizeninfotech.com',
  groupId: '2765',
  groupName: 'Thane view city',
  groups: [{ grpId: 2765, grpName: 'Thane view city' }],
};

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    const storedToken = localStorage.getItem('token');
    if (storedUser && storedToken) {
      setUser(JSON.parse(storedUser));
      setToken(storedToken);
    }
    setLoading(false);
  }, []);

  const login = async ({ username, password }) => {
    // Try real API first
    try {
      const res = await webLogin(username, password);
      const data = res.data;
      if (data.status === '0') {
        const userData = {
          id: data.masterUID, masterUID: data.masterUID, profileId: data.profileId,
          name: data.name, role: data.role || 'Admin', mobile: data.mobile,
          email: data.email, groupId: data.groupId, groupName: data.groupName,
          profilePic: data.profilePic, groups: data.groups || [],
        };
        localStorage.setItem('token', data.token);
        localStorage.setItem('user', JSON.stringify(userData));
        setToken(data.token);
        setUser(userData);
        return { success: true };
      }
    } catch {}

    // Fallback to mock login if API not available
    if (username === '9763128181' && password === 'pass@1234') {
      const fakeToken = 'dev-token-' + Date.now();
      localStorage.setItem('token', fakeToken);
      localStorage.setItem('user', JSON.stringify(MOCK_USER));
      setToken(fakeToken);
      setUser(MOCK_USER);
      return { success: true };
    }

    throw new Error('Invalid mobile number or password');
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  };

  const isAuthenticated = !!token;

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isAuthenticated, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}

export default AuthContext;