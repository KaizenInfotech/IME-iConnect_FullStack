import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { createMember } from '../api/memberService';
import { getCountries } from '../api/utilityService';
import { getClubList } from '../api/groupService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px', marginTop: '10px',
};

const sectionHeaderStyle = {
  backgroundColor: '#f0a500',
  color: '#fff',
  padding: '8px 16px',
  borderRadius: '4px',
  fontSize: '14px',
  fontWeight: 'bold',
  marginBottom: '15px',
  marginTop: '20px',
  display: 'flex',
  alignItems: 'center',
  gap: '8px',
};

const bloodGroups = ['- Select -', 'A +ve', 'A -ve', 'B +ve', 'B -ve', 'AB +ve', 'AB -ve', 'O +ve', 'O -ve'];
const membershipGrades = ['-Select-', 'Fellow', 'Member', 'Associate Member', 'Student', 'Technician'];
const categoryOptions = ['-Select-', 'Marine', 'Shore', 'Student', 'Other'];

export default function AddMemberPage() {
  const navigate = useNavigate();
  const [countries, setCountries] = useState([]);
  const [groups, setGroups] = useState([]);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [photoPreview, setPhotoPreview] = useState(null);

  const [form, setForm] = useState({
    FirstName: '',
    MiddleName: '',
    LastName: '',
    MobileCountry: 'India (+91)',
    MobileCountryCode: '91',
    MemberMobile: '',
    MemberEmail: '',
    BirthDate: '',
    AnniversaryDate: '',
    BloodGroup: '',
    SecondaryCountry: '',
    SecondaryMobileNo: '',
    MembershipId: '',
    MembershipGrade: '',
    Category: '',
    CompanyName: '',
    ChapterBranchName: '',
    Address: '',
    City: '',
    State: '',
    PinZipCode: '',
    ResCountry: '',
    ProfilePic: null,
  });

  useEffect(() => {
    (async () => {
      try {
        const [cRes, gRes] = await Promise.all([getCountries(), getClubList()]);
        const countryData = cRes.data?.CountryLists || cRes.data?.CountryCategoryResult?.countries || cRes.data?.data || [];
        setCountries(countryData);
        const clubs = gRes.data?.TBGetClubResult?.ClubResult?.Table || [];
        setGroups(clubs.map(c => ({ Id: c.GroupId, GrpName: c.group_name })));
      } catch {}
    })();
  }, []);

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setForm({ ...form, ProfilePic: file });
      const reader = new FileReader();
      reader.onload = (ev) => setPhotoPreview(ev.target.result);
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async () => {
    if (!form.FirstName.trim()) { alert('Please Enter First Name'); return; }
    if (!form.LastName.trim()) { alert('Please Enter Last Name'); return; }
    if (!form.MemberMobile.trim()) { alert('Please Enter Mobile Number'); return; }
    if (!form.MembershipId?.trim()) { alert('Please Enter Membership ID'); return; }
    setSaving(true);
    setError('');
    try {
      await createMember({
        MemberName: [form.FirstName, form.MiddleName, form.LastName].filter(Boolean).join(' '),
        MemberMobile: form.MemberMobile,
        MemberEmail: form.MemberEmail,
        CountryCode: form.MobileCountryCode,
        Dob: form.BirthDate,
        Doa: form.AnniversaryDate,
        BloodGroup: form.BloodGroup,
        SecondaryMobileNo: form.SecondaryMobileNo,
        MembershipGrade: form.MembershipGrade,
        Classification: form.Category,
        CompanyName: form.CompanyName,
        Designation: form.MembershipId,
      });
      navigate('/members');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin - Member</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Add Member</span>
        </div>
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
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
            Save
          </button>
          <button
            onClick={() => navigate('/members')}
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
        </div>
      )}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '20px 25px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>

        {/* ===== Personal Details (no header) ===== */}
        <div style={{ display: 'flex', gap: '15px' }}>
          {/* Left side fields */}
          <div style={{ flex: '1 1 75%' }}>
            {/* Row 1: First, Middle, Last Name */}
            <div style={{ display: 'flex', gap: '15px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>First Name</label>
                <input type="text" value={form.FirstName} onChange={(e) => setForm({ ...form, FirstName: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Middle Name</label>
                <input type="text" value={form.MiddleName} onChange={(e) => setForm({ ...form, MiddleName: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Last Name</label>
                <input type="text" value={form.LastName} onChange={(e) => setForm({ ...form, LastName: e.target.value })} style={inputStyle} />
              </div>
            </div>

            {/* Row 2: Country, Mobile, Email */}
            <div style={{ display: 'flex', gap: '15px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Country</label>
                <select
                  value={form.MobileCountry}
                  onChange={(e) => {
                    const val = e.target.value;
                    setForm({ ...form, MobileCountry: val });
                    const c = countries.find(c => `${c.CountryName || c.countryName} (${c.CountryCode || c.countryCode})` === val);
                    if (c) setForm(prev => ({ ...prev, MobileCountry: val, MobileCountryCode: (c.CountryCode || c.countryCode || '').replace('+', '') }));
                  }}
                  style={{ ...inputStyle, backgroundColor: '#fff' }}
                >
                  <option value="India (+91)">India (+91)</option>
                  {countries.map(c => {
                    const label = `${c.CountryName || c.countryName} (${c.CountryCode || c.countryCode})`;
                    return <option key={c.Id || c.id} value={label}>{label}</option>;
                  })}
                </select>
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Mobile Number (Active on WhatsApp)</label>
                <input type="tel" value={form.MemberMobile} onChange={(e) => setForm({ ...form, MemberMobile: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Email ID</label>
                <input type="email" value={form.MemberEmail} onChange={(e) => setForm({ ...form, MemberEmail: e.target.value })} style={inputStyle} />
              </div>
            </div>

            {/* Row 3: Birth Date, Anniversary Date, Blood group */}
            <div style={{ display: 'flex', gap: '15px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Birth Date</label>
                <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                  <input type="date" value={form.BirthDate} onChange={(e) => setForm({ ...form, BirthDate: e.target.value })} style={{ ...inputStyle, flex: 1 }} />
                  <button type="button" onClick={() => setForm({ ...form, BirthDate: '' })} style={{ background: 'none', border: 'none', color: '#1a297d', fontSize: '12px', cursor: 'pointer', whiteSpace: 'nowrap', textDecoration: 'underline' }}>Remove Birth date</button>
                </div>
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Anniversary Date</label>
                <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                  <input type="date" value={form.AnniversaryDate} onChange={(e) => setForm({ ...form, AnniversaryDate: e.target.value })} style={{ ...inputStyle, flex: 1 }} />
                  <button type="button" onClick={() => setForm({ ...form, AnniversaryDate: '' })} style={{ background: 'none', border: 'none', color: '#1a297d', fontSize: '12px', cursor: 'pointer', whiteSpace: 'nowrap', textDecoration: 'underline' }}>Remove Anniversary date</button>
                </div>
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Blood group</label>
                <select value={form.BloodGroup} onChange={(e) => setForm({ ...form, BloodGroup: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                  {bloodGroups.map(bg => <option key={bg} value={bg === '- Select -' ? '' : bg}>{bg}</option>)}
                </select>
              </div>
            </div>

            {/* Row 4: Country (secondary), Secondary Mobile No */}
            <div style={{ display: 'flex', gap: '15px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Country</label>
                <select value={form.SecondaryCountry} onChange={(e) => setForm({ ...form, SecondaryCountry: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
                  <option value="">--Select--</option>
                  {countries.map(c => <option key={c.Id || c.id} value={c.CountryName || c.countryName}>{c.CountryName || c.countryName}</option>)}
                </select>
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Secondary Mobile No.</label>
                <input type="tel" value={form.SecondaryMobileNo} onChange={(e) => setForm({ ...form, SecondaryMobileNo: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }} />
            </div>
          </div>

          {/* Right side: Profile Photo */}
          <div style={{ flex: '0 0 140px', textAlign: 'center', paddingTop: '10px' }}>
            <div style={{
              width: '100px', height: '100px', borderRadius: '50%',
              border: '2px solid #ccc', overflow: 'hidden',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              margin: '0 auto 8px', backgroundColor: '#f5f5f5',
            }}>
              {photoPreview ? (
                <img src={photoPreview} alt="Profile" style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
              ) : (
                <svg width="40" height="40" fill="#ccc" viewBox="0 0 20 20">
                  <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
                </svg>
              )}
            </div>
            <div style={{ fontSize: '11px', fontWeight: 'bold', color: '#333', marginBottom: '4px' }}>ATTACH FILE</div>
            <input type="file" accept="image/*" onChange={handleFileChange} style={{ fontSize: '11px', width: '130px' }} />
          </div>
        </div>

        {/* ===== IME I Details ===== */}
        <div style={sectionHeaderStyle}>
          <span style={{
            display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
            width: '22px', height: '22px', borderRadius: '50%', backgroundColor: '#fff', color: '#f0a500', fontSize: '12px', fontWeight: 'bold',
          }}>2</span>
          IME I Details
        </div>

        <div style={{ display: 'flex', gap: '15px', marginBottom: '10px' }}>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Membership ID</label>
            <input type="text" value={form.MembershipId} onChange={(e) => setForm({ ...form, MembershipId: e.target.value })} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Membership Grade</label>
            <select value={form.MembershipGrade} onChange={(e) => setForm({ ...form, MembershipGrade: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
              {membershipGrades.map(g => <option key={g} value={g === '-Select-' ? '' : g}>{g}</option>)}
            </select>
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Category</label>
            <select value={form.Category} onChange={(e) => setForm({ ...form, Category: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
              {categoryOptions.map(c => <option key={c} value={c === '-Select-' ? '' : c}>{c}</option>)}
            </select>
          </div>
        </div>

        <div style={{ display: 'flex', gap: '15px', marginBottom: '10px' }}>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Company Name</label>
            <input type="text" value={form.CompanyName} onChange={(e) => setForm({ ...form, CompanyName: e.target.value })} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Chapter/Branch Name</label>
            <select value={form.ChapterBranchName} onChange={(e) => setForm({ ...form, ChapterBranchName: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
              <option value="">-Select-</option>
              {groups.map(g => <option key={g.Id || g.id} value={g.GrpName || g.grpName}>{g.GrpName || g.grpName}</option>)}
            </select>
          </div>
          <div style={{ flex: 1 }} />
        </div>

        {/* ===== Residential Details ===== */}
        <div style={sectionHeaderStyle}>
          <span style={{
            display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
            width: '22px', height: '22px', borderRadius: '50%', backgroundColor: '#fff', color: '#f0a500', fontSize: '12px', fontWeight: 'bold',
          }}>3</span>
          Residential Details
        </div>

        <div style={{ marginBottom: '10px' }}>
          <label style={labelStyle}>Address</label>
          <textarea value={form.Address} onChange={(e) => setForm({ ...form, Address: e.target.value })} rows={3} style={{ ...inputStyle, height: 'auto', resize: 'vertical' }} />
        </div>

        <div style={{ display: 'flex', gap: '15px', marginBottom: '10px' }}>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>City</label>
            <input type="text" value={form.City} onChange={(e) => setForm({ ...form, City: e.target.value })} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>State</label>
            <input type="text" value={form.State} onChange={(e) => setForm({ ...form, State: e.target.value })} placeholder="Enter State" style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Pin/Zip Code</label>
            <input type="text" value={form.PinZipCode} onChange={(e) => setForm({ ...form, PinZipCode: e.target.value })} style={inputStyle} />
          </div>
        </div>

        <div style={{ display: 'flex', gap: '15px', marginBottom: '20px' }}>
          <div style={{ flex: '0 0 33%' }}>
            <label style={labelStyle}>Country</label>
            <select value={form.ResCountry} onChange={(e) => setForm({ ...form, ResCountry: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
              <option value="">--Select--</option>
              {countries.map(c => <option key={c.Id || c.id} value={c.CountryName || c.countryName}>{c.CountryName || c.countryName}</option>)}
            </select>
          </div>
        </div>
      </div>
    </div>
  );
}
