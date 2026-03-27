import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { createGroup } from '../api/groupService';
import { getCountries } from '../api/utilityService';

const meetingDays = ['-Select-', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px',
};

export default function AddGroupPage() {
  const navigate = useNavigate();
  const [countries, setCountries] = useState([]);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [logoPreview, setLogoPreview] = useState(null);

  const [form, setForm] = useState({
    GrpName: '',
    GrpCategory: '',
    GrpImg: null,
    MeetingDay: '',
    FromTime: '',
    ToTime: '',
    Address1: '',
    Country: 'India',
    City: '',
    State: '',
    Pincode: '',
  });

  useEffect(() => {
    (async () => {
      try {
        const res = await getCountries();
        const data = res.data?.CountryLists || res.data?.CountryCategoryResult?.countries || res.data?.data || [];
        setCountries(data);
      } catch {}
    })();
  }, []);

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setForm({ ...form, GrpImg: file });
      const reader = new FileReader();
      reader.onload = (ev) => setLogoPreview(ev.target.result);
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async () => {
    if (!form.GrpName.trim()) { alert('Please Enter Chapter / Branch Name'); return; }
    if (!form.GrpCategory.trim()) { alert('Please Enter Chapter / Branch ID'); return; }
    if (!form.MeetingDay) { alert('Please Select Meeting Day'); return; }
    if (!form.FromTime.trim()) { alert('Please Enter From Time'); return; }
    if (!form.ToTime.trim()) { alert('Please Enter To Time'); return; }
    if (!form.Address1.trim()) { alert('Please Enter Meeting Address'); return; }
    if (!form.City.trim()) { alert('Please Enter City'); return; }
    if (!form.State.trim()) { alert('Please Enter State'); return; }
    if (!form.Pincode.trim()) { alert('Please Enter Pincode'); return; }
    setSaving(true);
    setError('');
    try {
      await createGroup(form);
      navigate('/groups');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h1 style={{ fontSize: '16px', fontWeight: 'bold', color: '#000', margin: 0 }}>Add New</h1>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button
            onClick={handleSubmit}
            disabled={saving}
            style={{
              display: 'flex', alignItems: 'center', gap: '4px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '7px 16px', borderRadius: '4px', fontSize: '13px',
              cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1,
            }}
          >
            + Add
          </button>
          <button
            onClick={() => navigate('/groups')}
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

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>
          {error}
          <button onClick={() => setError('')} style={{ float: 'right', background: 'none', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>&times;</button>
        </div>
      )}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>

        {/* Row 1: Name, ID, Logo */}
        <div style={{ display: 'flex', gap: '20px', marginBottom: '15px' }}>
          {/* Chapter / Branch Name */}
          <div style={{ flex: '1 1 35%' }}>
            <label style={labelStyle}>Chapter / Branch Name</label>
            <input
              type="text"
              value={form.GrpName}
              onChange={(e) => setForm({ ...form, GrpName: e.target.value })}
              style={inputStyle}
            />
          </div>
          {/* Chapter / Branch ID */}
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>Chapter / Branch ID</label>
            <input
              type="text"
              value={form.GrpCategory}
              onChange={(e) => setForm({ ...form, GrpCategory: e.target.value })}
              style={inputStyle}
            />
          </div>
          {/* Chapter / Branch Logo */}
          <div style={{ flex: '1 1 30%' }}>
            <label style={labelStyle}>Chapter / Branch Logo</label>
            <div style={{
              width: '80px', height: '80px', border: '1px solid #ccc',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              marginBottom: '6px', backgroundColor: '#fafafa',
            }}>
              {logoPreview ? (
                <img src={logoPreview} alt="Logo" style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }} />
              ) : (
                <svg width="24" height="24" fill="#ccc" viewBox="0 0 24 24">
                  <path d="M21 19V5c0-1.1-.9-2-2-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2zM8.5 13.5l2.5 3.01L14.5 12l4.5 6H5l3.5-4.5z" />
                </svg>
              )}
            </div>
            <input
              type="file"
              accept="image/*"
              onChange={handleFileChange}
              style={{ fontSize: '12px' }}
            />
          </div>
        </div>

        {/* Row 2: Meeting Day, From Time, To Time */}
        <div style={{ display: 'flex', gap: '20px', marginBottom: '15px' }}>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>Meeting Day</label>
            <select
              value={form.MeetingDay}
              onChange={(e) => setForm({ ...form, MeetingDay: e.target.value })}
              style={{ ...inputStyle, backgroundColor: '#fff' }}
            >
              {meetingDays.map(d => <option key={d} value={d === '-Select-' ? '' : d}>{d}</option>)}
            </select>
          </div>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>From Time</label>
            <input
              type="text"
              value={form.FromTime}
              onChange={(e) => setForm({ ...form, FromTime: e.target.value })}
              style={inputStyle}
            />
          </div>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>To Time</label>
            <input
              type="text"
              value={form.ToTime}
              onChange={(e) => setForm({ ...form, ToTime: e.target.value })}
              style={inputStyle}
            />
          </div>
        </div>

        {/* Row 3: Meeting Address */}
        <div style={{ marginBottom: '15px' }}>
          <label style={labelStyle}>Meeting Address</label>
          <textarea
            value={form.Address1}
            onChange={(e) => setForm({ ...form, Address1: e.target.value })}
            rows={3}
            style={{ ...inputStyle, height: 'auto', resize: 'vertical' }}
          />
        </div>

        {/* Row 4: Country, City, State, Pincode */}
        <div style={{ display: 'flex', gap: '20px' }}>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>Country</label>
            <select
              value={form.Country}
              onChange={(e) => setForm({ ...form, Country: e.target.value })}
              style={{ ...inputStyle, backgroundColor: '#fff' }}
            >
              {countries.length === 0 && <option value="India">India</option>}
              {countries.map(c => (
                <option key={c.Id || c.id} value={c.CountryName || c.countryName}>
                  {c.CountryName || c.countryName}
                </option>
              ))}
            </select>
          </div>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>City</label>
            <input
              type="text"
              value={form.City}
              onChange={(e) => setForm({ ...form, City: e.target.value })}
              style={inputStyle}
            />
          </div>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>State</label>
            <input
              type="text"
              value={form.State}
              onChange={(e) => setForm({ ...form, State: e.target.value })}
              style={inputStyle}
            />
          </div>
          <div style={{ flex: '1 1 25%' }}>
            <label style={labelStyle}>Pincode</label>
            <input
              type="text"
              value={form.Pincode}
              onChange={(e) => setForm({ ...form, Pincode: e.target.value })}
              style={inputStyle}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
