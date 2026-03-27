import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function LoginPage() {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [country, setCountry] = useState('India');
  const [countryCode, setCountryCode] = useState('+91');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);
  const [showForgot, setShowForgot] = useState(false);
  const [forgotMobile, setForgotMobile] = useState('');
  const [countries, setCountries] = useState([]);

  // Load saved credentials on mount
  useEffect(() => {
    const saved = localStorage.getItem('rememberLogin');
    if (saved) {
      try {
        const data = JSON.parse(saved);
        setUsername(data.username || '');
        setPassword(data.password || '');
        setCountry(data.country || 'India');
        setCountryCode(data.countryCode || '+91');
        setRememberMe(true);
      } catch {}
    }
    // Fetch countries
    (async () => {
      try {
        const { getCountries } = await import('../api/utilityService');
        const res = await getCountries();
        const list = res.data?.CountryLists || [];
        if (list.length > 0) setCountries(list);
      } catch {}
    })();
  }, []);

  const handleCountryChange = (e) => {
    const selected = e.target.value;
    setCountry(selected);
    const found = countries.find(c => (c.countryName || c.CountryName) === selected);
    setCountryCode(found ? '+' + (found.countryCode || found.CountryCode) : '+91');
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    if (!username) { setError('Please enter mobile number'); return; }
    if (!password) { setError('Please enter password'); return; }
    setError('');
    setLoading(true);
    try {
      await login({ username, password });
      // Save or clear remember me
      if (rememberMe) {
        localStorage.setItem('rememberLogin', JSON.stringify({ username, password, country, countryCode }));
      } else {
        localStorage.removeItem('rememberLogin');
      }
      navigate('/dashboard');
    } catch (err) {
      setError(err.message || 'Invalid mobile number or password');
    } finally {
      setLoading(false);
    }
  };

  const handleForgotPassword = async () => {
    if (!forgotMobile.trim()) { alert('Please enter mobile number'); return; }
    try {
      const api = (await import('../api/axiosInstance')).default;
      const res = await api.post('/Login/ForgotPassword', { mobileNo: forgotMobile });
      const data = res.data;
      if (data.status === '0') {
        alert('Password sent successfully on your registered email address');
        setShowForgot(false);
        setForgotMobile('');
      } else {
        alert(data.message || 'No such mobile number exists');
      }
    } catch {
      alert('Please call to IME I Connect support for regenerate admin password.');
    }
  };

  return (
    <div style={{ backgroundColor: '#092c5e', minHeight: '100vh', display: 'flex', flexDirection: 'column', alignItems: 'center', paddingTop: '30px' }}>

      {/* Logo + Title */}
      <div style={{ textAlign: 'center', marginBottom: '25px' }}>
        <img src="/ime_logo.png" alt="IME Logo" style={{ maxWidth: '450px', width: '90%' }} />
      </div>

      {/* Login Panel */}
      <div style={{ width: '100%', maxWidth: '460px', padding: '0 15px' }}>

        {/* Log in Header */}
        <div style={{
          backgroundColor: '#d9d9d9',
          padding: '10px 0',
          textAlign: 'center',
          borderTop: '1px solid #bbb',
          borderLeft: '1px solid #bbb',
          borderRight: '1px solid #bbb',
          borderTopLeftRadius: '4px',
          borderTopRightRadius: '4px',
        }}>
          <h2 style={{ margin: 0, fontSize: '16px', fontWeight: 'bold', color: '#000' }}>Log in</h2>
        </div>

        {/* Form Body */}
        <div style={{
          backgroundColor: '#fff',
          padding: '20px 25px',
          border: '1px solid #bbb',
          borderTop: 'none',
          borderBottomLeftRadius: '4px',
          borderBottomRightRadius: '4px',
        }}>
          <form onSubmit={handleLogin}>

            {/* Select Country */}
            <div style={{ marginBottom: '10px' }}>
              <label style={{ fontWeight: 'bold', fontSize: '12px', display: 'block', marginBottom: '4px', color: '#333' }}>
                Select Country
              </label>
              <select
                value={country}
                onChange={handleCountryChange}
                style={{
                  width: '100%', height: '34px', border: '1px solid #ccc',
                  borderRadius: '2px', padding: '4px 8px', fontSize: '13px',
                  backgroundColor: '#fff', boxSizing: 'border-box', color: '#333',
                }}
              >
                {countries.length > 0
                  ? countries.map(c => <option key={c.countryId || c.Id} value={c.countryName || c.CountryName}>{c.countryName || c.CountryName}</option>)
                  : <option>India</option>
                }
              </select>
            </div>

            {/* Mobile Number */}
            <div style={{ marginBottom: '10px' }}>
              <label style={{ fontWeight: 'bold', fontSize: '12px', display: 'block', marginBottom: '4px', color: '#333' }}>
                Mobile Number
              </label>
              <div style={{ display: 'flex' }}>
                <div style={{
                  width: '50px', height: '34px', border: '1px solid #ccc', borderRight: 'none',
                  borderRadius: '2px 0 0 2px', display: 'flex', alignItems: 'center', justifyContent: 'center',
                  backgroundColor: '#eee', fontSize: '13px', color: '#333', flexShrink: 0,
                }}>
                  {countryCode}
                </div>
                <input
                  type="text"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  autoComplete="off"
                  style={{
                    flex: 1, height: '34px', border: '1px solid #ccc',
                    borderRadius: '0 2px 2px 0', padding: '4px 8px', fontSize: '13px',
                    backgroundColor: '#fff', boxSizing: 'border-box', outline: 'none',
                  }}
                />
              </div>
            </div>

            {/* Password */}
            <div style={{ marginBottom: '12px' }}>
              <label style={{ fontWeight: 'bold', fontSize: '12px', display: 'block', marginBottom: '4px', color: '#333' }}>
                Password
              </label>
              <div style={{ display: 'flex', border: '1px solid #ccc', borderRadius: '2px', overflow: 'hidden' }}>
                <span style={{
                  display: 'flex', alignItems: 'center', justifyContent: 'center',
                  width: '38px', backgroundColor: '#eee', borderRight: '1px solid #ccc',
                  color: '#555', flexShrink: 0,
                }}>
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0110 0v4" />
                  </svg>
                </span>
                <input
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  autoComplete="off"
                  style={{
                    flex: 1, height: '34px', border: 'none', padding: '4px 8px',
                    fontSize: '13px', outline: 'none',
                  }}
                />
              </div>
            </div>

            {/* Login Button */}
            <button
              type="submit"
              disabled={loading}
              style={{
                width: '100%', height: '38px', fontSize: '14px', fontWeight: 'bold',
                backgroundColor: '#1a297d', color: '#fff', border: '1px solid #15226a',
                borderRadius: '2px', cursor: loading ? 'not-allowed' : 'pointer',
                opacity: loading ? 0.65 : 1, marginBottom: '10px',
              }}
            >
              {loading ? 'Please wait...' : 'Login'}
            </button>

            {/* Error message */}
            {error && (
              <div style={{ textAlign: 'center', marginBottom: '8px' }}>
                <span style={{ color: 'red', fontSize: '13px' }}>{error}</span>
              </div>
            )}

            {/* Remember Password + Forgot Password */}
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '12px', color: '#333', cursor: 'pointer' }}>
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={(e) => {
                    setRememberMe(e.target.checked);
                    if (!e.target.checked) localStorage.removeItem('rememberLogin');
                  }}
                  style={{ cursor: 'pointer' }}
                />
                Remember Password
              </label>
              <a href="#" onClick={(e) => { e.preventDefault(); setShowForgot(true); }} style={{ fontSize: '12px', color: '#337ab7', textDecoration: 'none' }}>
                Forgot Password?
              </a>
            </div>

          </form>
        </div>
      </div>

      {/* Forgot Password Modal */}
      {showForgot && (
        <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000 }}>
          <div style={{ backgroundColor: '#fff', borderRadius: '4px', width: '520px', maxWidth: '90%', boxShadow: '0 10px 30px rgba(0,0,0,0.3)' }}>
            {/* Header */}
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '12px 20px', borderBottom: '1px solid #eee' }}>
              <h3 style={{ margin: 0, fontSize: '16px', fontWeight: '600', color: '#333' }}>Password Assistance</h3>
              <button onClick={() => setShowForgot(false)} style={{ background: 'none', border: 'none', fontSize: '24px', cursor: 'pointer', color: '#333', lineHeight: 1 }}>&times;</button>
            </div>
            {/* Body */}
            <div style={{ padding: '25px 30px' }}>
              <p style={{ fontSize: '13px', color: '#333', textAlign: 'center', marginBottom: '25px' }}>Enter Mobile Number associated with your IME I Connect.</p>
              <div style={{ display: 'flex', gap: '20px', marginBottom: '20px' }}>
                {/* Select Country */}
                <div style={{ flex: 1 }}>
                  <label style={{ fontWeight: 'bold', fontSize: '12px', display: 'block', marginBottom: '4px', color: '#333' }}>Select Country</label>
                  <select style={{ width: '100%', height: '34px', border: '1px solid #ccc', borderRadius: '2px', padding: '4px 8px', fontSize: '13px', backgroundColor: '#fff', boxSizing: 'border-box', color: '#333' }}>
                    {countries.length > 0
                      ? countries.map(c => <option key={c.countryId || c.Id} value={c.countryName || c.CountryName}>{c.countryName || c.CountryName}</option>)
                      : <option>India</option>
                    }
                  </select>
                </div>
                {/* Mobile Number */}
                <div style={{ flex: 1 }}>
                  <label style={{ fontWeight: 'bold', fontSize: '12px', display: 'block', marginBottom: '4px', color: '#333' }}>Mobile Number</label>
                  <div style={{ display: 'flex' }}>
                    <div style={{ width: '45px', height: '34px', border: '1px solid #ccc', borderRight: 'none', borderRadius: '2px 0 0 2px', display: 'flex', alignItems: 'center', justifyContent: 'center', backgroundColor: '#eee', fontSize: '12px', color: '#333', flexShrink: 0 }}>
                      {countryCode}
                    </div>
                    <input
                      type="text"
                      value={forgotMobile}
                      onChange={(e) => setForgotMobile(e.target.value)}
                      placeholder="Your Mobile number"
                      style={{ flex: 1, height: '34px', border: '1px solid #ccc', borderRadius: '0 2px 2px 0', padding: '4px 8px', fontSize: '13px', boxSizing: 'border-box', outline: 'none' }}
                    />
                  </div>
                </div>
              </div>
              {/* Send Button */}
              <div style={{ textAlign: 'center' }}>
                <button onClick={handleForgotPassword} style={{ padding: '8px 30px', fontSize: '14px', fontWeight: 'bold', backgroundColor: '#1a297d', color: '#fff', border: 'none', borderRadius: '2px', cursor: 'pointer' }}>
                  Send
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}