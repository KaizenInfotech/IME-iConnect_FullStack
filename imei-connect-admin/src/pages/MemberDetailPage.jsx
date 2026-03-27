import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getMember, updateProfile, updateAddress } from '../api/memberService';

const bloodGroupOptions = ['- Select -', 'A +ve', 'A -ve', 'B +ve', 'B -ve', 'AB +ve', 'AB -ve', 'O +ve', 'O -ve'];

const stateOptions = [
  '- Select -', 'Andhra Pradesh', 'Arunachal Pradesh', 'Assam', 'Bihar', 'Chhattisgarh',
  'Goa', 'Gujarat', 'Haryana', 'Himachal Pradesh', 'Jharkhand', 'Karnataka', 'Kerala',
  'Madhya Pradesh', 'Maharashtra', 'Manipur', 'Meghalaya', 'Mizoram', 'Nagaland', 'Odisha',
  'Punjab', 'Rajasthan', 'Sikkim', 'Tamil Nadu', 'Telangana', 'Tripura', 'Uttar Pradesh',
  'Uttarakhand', 'West Bengal', 'Delhi', 'Jammu & Kashmir', 'Ladakh',
];

const countryOptions = [
  '- Select -', 'India', 'United States', 'United Kingdom', 'Canada', 'Australia',
  'Nepal', 'Bangladesh', 'Sri Lanka', 'Singapore', 'UAE',
];

export default function MemberDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const groupId = searchParams.get('groupId') || '33359';
  const fileInputRef = useRef(null);
  const [member, setMember] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [errors, setErrors] = useState({});
  const [profilePhoto, setProfilePhoto] = useState(null);
  const [profilePhotoPreview, setProfilePhotoPreview] = useState('');

  const [form, setForm] = useState({
    FirstName: '',
    MiddleName: '',
    LastName: '',
    Country: '',
    Mobile: '',
    Email: '',
    BirthDate: '',
    BirthMonth: '',
    AnniversaryDate: '',
    AnniversaryMonth: '',
    BloodGroup: '- Select -',
    SecondaryMobile: '',
    MembershipID: '',
    MembershipGrade: '- Select -',
    Category: '- Select -',
    CompanyName: '',
    ChapterBranchName: '- Select -',
    Address: '',
    City: '',
    State: '- Select -',
    PinCode: '',
    ResCountry: '- Select -',
  });

  useEffect(() => { fetchData(); }, [id]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getMember(id, groupId);
      const tbl = res.data?.TBGetSponsorReferredResult?.Result?.Table;
      const m = Array.isArray(tbl) && tbl.length > 0 ? tbl[0] : null;
      setMember(m);
      if (m) {
        const dob = m.DOB || '';
        const doa = m.DOA || '';
        const dobParts = dob.includes('/') ? dob.split('/') : [];
        const doaParts = doa.includes('/') ? doa.split('/') : [];
        setForm({
          FirstName: m.First_Name || '',
          MiddleName: m.Middle_Name || '',
          LastName: m.Last_Name || '',
          Country: m.Country || '- Select -',
          Mobile: m.Whatsapp_num || '',
          Email: m.member_email_id || '',
          BirthDate: dobParts.length >= 1 ? dobParts[0] : '',
          BirthMonth: dobParts.length >= 2 ? dobParts[1] : '',
          AnniversaryDate: doaParts.length >= 1 ? doaParts[0] : '',
          AnniversaryMonth: doaParts.length >= 2 ? doaParts[1] : '',
          BloodGroup: m.blood_Group || '- Select -',
          SecondaryMobile: m.Secondry_num || '',
          MembershipID: m.IMEI_Membership_Id || '',
          MembershipGrade: m.Membership_Grade || '- Select -',
          Category: m.CategoryName || '- Select -',
          CompanyName: m.Company_name || '',
          ChapterBranchName: m.Chaptr_Brnch_Name || '- Select -',
          Address: m.Address || '',
          City: m.City || '',
          State: m.State || '- Select -',
          PinCode: m.pincode || '',
          ResCountry: m.Country || '- Select -',
        });
        setProfilePhotoPreview(m.member_profile_photo_path || '');
      }
    } catch {
      setError('Failed to load member details');
    } finally {
      setLoading(false);
    }
  };

  const setField = (key, value) => {
    setForm((prev) => ({ ...prev, [key]: value }));
    if (errors[key]) setErrors((prev) => ({ ...prev, [key]: '' }));
  };

  const validate = () => {
    const newErrors = {};
    const firstName = form.FirstName?.trim();
    const country = form.Country?.trim();
    if (!firstName) newErrors.FirstName = 'mandatory';
    if (!country || country === '- Select -' || country === '') newErrors.Country = 'mandatory';
    return newErrors;
  };

  const handleSave = async () => {
    setErrors({});
    const newErrors = validate();
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }
    setSaving(true);
    try {
      const profileId = String(member?.MemProfileId || id);
      // Update profile (name, mobile, email)
      await updateProfile({
        ProfileId: profileId,
        memberName: [form.FirstName, form.MiddleName, form.LastName].filter(Boolean).join(' ').trim(),
        memberMobile: form.Mobile,
        memberEmailid: form.Email,
      });
      // Update address
      if (form.Address || form.City || form.State || form.PinCode) {
        await updateAddress({
          profileID: profileId,
          addressType: 'Residence',
          address: form.Address,
          city: form.City,
          state: form.State === '- Select -' ? '' : form.State,
          country: form.ResCountry === '- Select -' ? '' : form.ResCountry,
          pincode: form.PinCode,
        });
      }
      setError('');
      alert('Member updated successfully');
    } catch (err) {
      setError(err.response?.data?.message || 'Save failed');
    } finally {
      setSaving(false);
    }
  };

  const handlePhotoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setProfilePhoto(file);
      setProfilePhotoPreview(URL.createObjectURL(file));
    }
  };

  const handleRemovePhoto = () => {
    setProfilePhoto(null);
    setProfilePhotoPreview('');
    if (fileInputRef.current) fileInputRef.current.value = '';
  };

  if (loading) return <LoadingSpinner />;
  if (!member) return <div className="text-[#999] text-center py-[40px]">Member not found</div>;

  const memberFullName = `${member.First_Name || member.FirstName || ''} ${member.Middle_Name || member.MiddleName || ''} ${member.Last_Name || member.LastName || ''}`.trim();

  // Shared input style
  const inputCls = 'w-full h-[35px] rounded-[8px] border border-[#d0d0d0] px-[10px] text-[13px] outline-none';
  const inputDisabledCls = `${inputCls} bg-[#eee] cursor-not-allowed`;
  const labelCls = 'block text-[12px] text-[#555] mb-[4px]';
  const mandatoryCls = 'text-[#dd4b39] text-[11px] ml-[4px]';

  const dayOptions = Array.from({ length: 31 }, (_, i) => i + 1);
  const monthOptions = [
    { value: '', label: '- Select -' },
    { value: '01', label: 'January' }, { value: '02', label: 'February' },
    { value: '03', label: 'March' }, { value: '04', label: 'April' },
    { value: '05', label: 'May' }, { value: '06', label: 'June' },
    { value: '07', label: 'July' }, { value: '08', label: 'August' },
    { value: '09', label: 'September' }, { value: '10', label: 'October' },
    { value: '11', label: 'November' }, { value: '12', label: 'December' },
  ];

  return (
    <div>
      {/* Page Title */}
      <div className="mb-[15px]">
        <h3 className="text-[18px] font-normal text-[#333]">
          {memberFullName} - <span className="text-[#555]">Profile</span>
        </h3>
      </div>

      {/* Top Buttons */}
      <div className="flex items-center gap-[8px] mb-[15px]">
        <button
          onClick={handleSave}
          disabled={saving}
          className="px-[12px] py-[6px] text-[13px] text-white rounded-[4px] border-0 cursor-pointer disabled:opacity-50"
          style={{ backgroundColor: '#6b9300' }}
        >
          {saving ? 'Saving...' : 'Save'}
        </button>
        <button
          onClick={() => navigate('/members')}
          className="px-[12px] py-[6px] text-[13px] text-white rounded-[4px] border-0 cursor-pointer bg-[#1a297d]"
        >
          Back
        </button>
      </div>

      {error && (
        <div className="bg-[#fdf2f2] text-[#dd4b39] px-[15px] py-[10px] rounded-[4px] mb-[15px] text-[13px]">
          {error}
        </div>
      )}

      {/* Well Container */}
      <div
        className="bg-white border border-[#eee] rounded-[8px] mb-[15px]"
        style={{ padding: '25px 28px 10px 28px' }}
      >
        <div className="flex gap-[20px]">
          {/* Personal Details (col-md-10) */}
          <div className="flex-1">
            <h4 className="text-[14px] font-semibold text-[#333] mb-[15px] border-b border-[#eee] pb-[8px]">
              Personal Details
            </h4>
            <div className="grid grid-cols-3 gap-[12px] mb-[15px]">
              <div>
                <label className={labelCls}>First Name<span className={mandatoryCls}>mandatory</span></label>
                <input type="text" value={form.FirstName} onChange={(e) => setField('FirstName', e.target.value)} className={inputCls} />
                {errors.FirstName && <span className="text-[#dd4b39] text-[11px]">{errors.FirstName}</span>}
              </div>
              <div>
                <label className={labelCls}>Middle Name</label>
                <input type="text" value={form.MiddleName} onChange={(e) => setField('MiddleName', e.target.value)} className={inputCls} />
              </div>
              <div>
                <label className={labelCls}>Last Name</label>
                <input type="text" value={form.LastName} onChange={(e) => setField('LastName', e.target.value)} className={inputCls} />
              </div>
            </div>
            <div className="grid grid-cols-3 gap-[12px] mb-[15px]">
              <div>
                <label className={labelCls}>
                  Country
                  <span className={mandatoryCls}>mandatory</span>
                </label>
                <select
                  value={form.Country}
                  onChange={(e) => setField('Country', e.target.value)}
                  className={inputCls}
                >
                  {countryOptions.map((c) => (
                    <option key={c} value={c}>{c}</option>
                  ))}
                </select>
                {errors.Country && <span className="text-[#dd4b39] text-[11px]">{errors.Country}</span>}
              </div>
              <div>
                <label className={labelCls}>
                  Mobile Number <span className="text-[#999] text-[11px]">(Active on WhatsApp)</span>
                </label>
                <input
                  type="text"
                  value={form.Mobile}
                  onChange={(e) => setField('Mobile', e.target.value)}
                  className={inputCls}
                />
              </div>
              <div>
                <label className={labelCls}>Email ID</label>
                <input type="email" value={form.Email} onChange={(e) => setField('Email', e.target.value)} className={inputCls} />
              </div>
            </div>
          </div>

          {/* Profile Photo (col-md-2) */}
          <div className="w-[150px] flex-shrink-0">
            <h4 className="text-[14px] font-semibold text-[#333] mb-[15px] border-b border-[#eee] pb-[8px]">
              Profile Photo
            </h4>
            <div className="flex flex-col items-center">
              <div className="w-[80px] h-[80px] border border-[#d0d0d0] rounded-[4px] mb-[8px] overflow-hidden bg-[#f5f5f5] flex items-center justify-center">
                {profilePhotoPreview ? (
                  <img src={profilePhotoPreview} alt="Profile" className="w-full h-full object-cover" />
                ) : (
                  <span className="text-[#ccc] text-[24px]">
                    <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1" strokeLinecap="round" strokeLinejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
                  </span>
                )}
              </div>
              <input
                ref={fileInputRef}
                type="file"
                accept="image/*"
                onChange={handlePhotoChange}
                className="hidden"
              />
              <button
                onClick={() => fileInputRef.current?.click()}
                className="px-[10px] py-[4px] text-[11px] text-white rounded-[4px] border-0 cursor-pointer mb-[4px]"
                style={{ backgroundColor: '#6b9300' }}
              >
                ATTACH FILE
              </button>
              {profilePhotoPreview && (
                <button
                  onClick={handleRemovePhoto}
                  className="text-[#dd4b39] text-[11px] bg-transparent border-0 cursor-pointer underline"
                >
                  Remove Photo
                </button>
              )}
            </div>
          </div>
        </div>
      </div>

      {/* Additional Details */}
      <div
        className="bg-white border border-[#eee] rounded-[8px] mb-[15px]"
        style={{ padding: '25px 28px 10px 28px' }}
      >
        <h4 className="text-[14px] font-semibold text-[#333] mb-[15px] border-b border-[#eee] pb-[8px]">
          Additional Details
        </h4>
        <div className="grid grid-cols-4 gap-[12px] mb-[15px]">
          <div>
            <label className={labelCls}>Birth Date (d/m)</label>
            <div className="flex gap-[6px]">
              <select
                value={form.BirthDate}
                onChange={(e) => setField('BirthDate', e.target.value)}
                className={`${inputCls} w-[60%]`}
              >
                <option value="">Day</option>
                {dayOptions.map((d) => (
                  <option key={d} value={String(d).padStart(2, '0')}>{d}</option>
                ))}
              </select>
              <select
                value={form.BirthMonth}
                onChange={(e) => setField('BirthMonth', e.target.value)}
                className={`${inputCls} w-[40%]`}
              >
                {monthOptions.map((m) => (
                  <option key={m.value} value={m.value}>{m.label}</option>
                ))}
              </select>
            </div>
          </div>
          <div>
            <label className={labelCls}>Anniversary Date (d/m)</label>
            <div className="flex gap-[6px]">
              <select
                value={form.AnniversaryDate}
                onChange={(e) => setField('AnniversaryDate', e.target.value)}
                className={`${inputCls} w-[60%]`}
              >
                <option value="">Day</option>
                {dayOptions.map((d) => (
                  <option key={d} value={String(d).padStart(2, '0')}>{d}</option>
                ))}
              </select>
              <select
                value={form.AnniversaryMonth}
                onChange={(e) => setField('AnniversaryMonth', e.target.value)}
                className={`${inputCls} w-[40%]`}
              >
                {monthOptions.map((m) => (
                  <option key={m.value} value={m.value}>{m.label}</option>
                ))}
              </select>
            </div>
          </div>
          <div>
            <label className={labelCls}>Blood Group</label>
            <select
              value={form.BloodGroup}
              onChange={(e) => setField('BloodGroup', e.target.value)}
              className={inputCls}
            >
              {bloodGroupOptions.map((b) => (
                <option key={b} value={b}>{b}</option>
              ))}
            </select>
          </div>
          <div>
            <label className={labelCls}>Secondary Mobile No.</label>
            <input
              type="text"
              value={form.SecondaryMobile}
              onChange={(e) => setField('SecondaryMobile', e.target.value)}
              className={inputCls}
            />
          </div>
        </div>
      </div>

      {/* IME I Details */}
      <div
        className="bg-white border border-[#eee] rounded-[8px] mb-[15px]"
        style={{ padding: '25px 28px 10px 28px' }}
      >
        <h4 className="text-[14px] font-semibold text-[#333] mb-[15px] border-b border-[#eee] pb-[8px]">
          IME I Details
        </h4>
        <div className="grid grid-cols-3 gap-[12px] mb-[15px]">
          <div>
            <label className={labelCls}>Membership ID</label>
            <input
              type="text"
              value={form.MembershipID}
              onChange={(e) => setField('MembershipID', e.target.value)}
              className={inputCls}
            />
          </div>
          <div>
            <label className={labelCls}>Membership Grade</label>
            <select
              value={form.MembershipGrade}
              onChange={(e) => setField('MembershipGrade', e.target.value)}
              className={inputCls}
            >
              <option>- Select -</option>
            </select>
          </div>
          <div>
            <label className={labelCls}>Category</label>
            <select
              value={form.Category}
              onChange={(e) => setField('Category', e.target.value)}
              className={inputCls}
            >
              <option>- Select -</option>
            </select>
          </div>
        </div>
        <div className="grid grid-cols-3 gap-[12px] mb-[15px]">
          <div>
            <label className={labelCls}>Company Name</label>
            <input
              type="text"
              value={form.CompanyName}
              onChange={(e) => setField('CompanyName', e.target.value)}
              className={inputCls}
            />
          </div>
          <div>
            <label className={labelCls}>Chapter/Branch Name</label>
            <select
              value={form.ChapterBranchName}
              onChange={(e) => setField('ChapterBranchName', e.target.value)}
              className={inputCls}
            >
              <option>- Select -</option>
            </select>
          </div>
        </div>
      </div>

      {/* Residential Details */}
      <div
        className="bg-white border border-[#eee] rounded-[8px] mb-[15px]"
        style={{ padding: '25px 28px 10px 28px' }}
      >
        <h4 className="text-[14px] font-semibold text-[#333] mb-[15px] border-b border-[#eee] pb-[8px]">
          Residential Details
        </h4>
        <div className="grid grid-cols-3 gap-[12px] mb-[15px]">
          <div className="col-span-3">
            <label className={labelCls}>Address</label>
            <textarea
              value={form.Address}
              onChange={(e) => setField('Address', e.target.value)}
              rows={3}
              className="w-full rounded-[8px] border border-[#d0d0d0] px-[10px] py-[8px] text-[13px] outline-none resize-vertical"
            />
          </div>
        </div>
        <div className="grid grid-cols-4 gap-[12px] mb-[15px]">
          <div>
            <label className={labelCls}>City</label>
            <input
              type="text"
              value={form.City}
              onChange={(e) => setField('City', e.target.value)}
              className={inputCls}
            />
          </div>
          <div>
            <label className={labelCls}>State</label>
            <input
              type="text"
              value={form.State === '- Select -' ? '' : form.State}
              onChange={(e) => setField('State', e.target.value)}
              placeholder="Enter State"
              className={inputCls}
            />
          </div>
          <div>
            <label className={labelCls}>Pin/Zip Code</label>
            <input
              type="text"
              value={form.PinCode}
              onChange={(e) => setField('PinCode', e.target.value)}
              className={inputCls}
            />
          </div>
          <div>
            <label className={labelCls}>Country</label>
            <select
              value={form.ResCountry}
              onChange={(e) => setField('ResCountry', e.target.value)}
              className={inputCls}
            >
              {countryOptions.map((c) => (
                <option key={c} value={c}>{c}</option>
              ))}
            </select>
          </div>
        </div>
      </div>
    </div>
  );
}
