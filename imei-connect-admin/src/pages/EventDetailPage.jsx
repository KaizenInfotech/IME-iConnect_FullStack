import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getEvent, createEvent, updateEvent, getEventExtras, saveEventExtras } from '../api/eventService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px', marginTop: '10px',
};

const mandatoryStyle = { color: '#dd4b39', fontSize: '11px', marginLeft: '4px' };

const btnStyle = {
  display: 'inline-flex', alignItems: 'center', gap: '4px',
  color: '#fff', border: 'none', padding: '5px 14px', borderRadius: '4px',
  fontSize: '12px', cursor: 'pointer',
};

function toDatetimeLocal(dateStr) {
  if (!dateStr) return '';
  // Normalize unicode spaces and try DD/MM/YYYY format
  const clean = dateStr.replace(/[\u202f\u00a0]/g, ' ').trim();
  const dmyMatch = clean.match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})\s+(\d{1,2}):(\d{2}):?(\d{2})?\s*(AM|PM)?$/i);
  if (dmyMatch) {
    let [, dd, mm, yyyy, hh, min, , ampm] = dmyMatch;
    let h = parseInt(hh);
    if (ampm) {
      if (ampm.toUpperCase() === 'PM' && h < 12) h += 12;
      if (ampm.toUpperCase() === 'AM' && h === 12) h = 0;
    }
    return `${yyyy}-${mm.padStart(2, '0')}-${dd.padStart(2, '0')}T${String(h).padStart(2, '0')}:${min}`;
  }
  const d = new Date(clean);
  if (isNaN(d)) return dateStr;
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`;
}

export default function EventDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const filterGroupId = searchParams.get('groupId');
  const isNew = !id || id === 'new';
  const [loading, setLoading] = useState(!isNew);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [errors, setErrors] = useState({});
  const [pageName, setPageName] = useState('');
  const totalMembersRef = useRef(0);

  const [form, setForm] = useState({
    Title: '',
    Description: '',
    Attendance: '0',
    AttendancePercent: '0',
    EventDate: '',
  });

  const [agendas, setAgendas] = useState([{ file: null, name: '' }]);
  const [minutes, setMinutes] = useState([{ file: null, name: '' }]);
  const [photos, setPhotos] = useState([
    { file: null, preview: null, description: '' },
    { file: null, preview: null, description: '' },
    { file: null, preview: null, description: '' },
    { file: null, preview: null, description: '' },
    { file: null, preview: null, description: '' },
  ]);

  useEffect(() => {
    (async () => {
      if (filterGroupId) {
        try {
          const { getClubList } = await import('../api/groupService');
          const clubRes = await getClubList();
          const clubs = clubRes.data?.TBGetClubResult?.ClubResult?.Table || [];
          const found = clubs.find(c => String(c.GroupId) === filterGroupId);
          if (found) setPageName(found.group_name);
        } catch {}
        try {
          const { getMembers } = await import('../api/memberService');
          const memRes = await getMembers(filterGroupId);
          const members = memRes.data?.MemberDetail?.NewMemberList || [];
          totalMembersRef.current = members.length;
        } catch {}
      }
      // If no filterGroupId, fetch from default group
      if (!filterGroupId) {
        try {
          const { getMembers } = await import('../api/memberService');
          const memRes = await getMembers('33359');
          const members = memRes.data?.MemberDetail?.NewMemberList || [];
          totalMembersRef.current = members.length;
        } catch {}
      }
      if (!isNew) fetchData();
    })();
  }, [id]);

  const fetchData = async () => {
    setLoading(true);
    // Fetch event details
    try {
      const res = await getEvent(id);
      const evtList = res.data?.EventsDetailResult || [];
      const ev = evtList.length > 0 ? (evtList[0].EventsDetail || evtList[0]) : (res.data?.data || res.data);
      if (ev) {
        setForm({
          Title: ev.Title || ev.EventTitle || ev.eventTitle || '',
          Description: ev.Description || ev.EventDesc || ev.eventDesc || '',
          Attendance: ev.Attendance || ev.AttendanceCount || '0',
          AttendancePercent: ev.AttendancePercent || '0',
          EventDate: toDatetimeLocal(ev.EventDate || ev.eventDate || ev.eventDateTime || ''),
        });
      }
    } catch { setError('Failed to load event details'); }

    // Fetch extras separately
    try {
      const extrasRes = await getEventExtras(id);
      console.log('[EventDetailPage] extras response:', JSON.stringify(extrasRes.data));
      if (extrasRes.data?.status === '0') {
        if (extrasRes.data.agendas?.length) setAgendas(extrasRes.data.agendas.map(a => ({ file: null, name: a.fileName || '' })));
        if (extrasRes.data.minutes?.length) setMinutes(extrasRes.data.minutes.map(m => ({ file: null, name: m.fileName || '' })));
        if (extrasRes.data.photos?.length) {
          const fetched = extrasRes.data.photos.map(p => ({ file: null, preview: p.photoPath || null, description: p.description || '' }));
          while (fetched.length < 5) fetched.push({ file: null, preview: null, description: '' });
          setPhotos(fetched);
        }
      }
    } catch {}

    setLoading(false);
  };

  const setField = (key, value) => {
    setForm(prev => {
      const updated = { ...prev, [key]: value };
      if (key === 'Attendance') {
        const total = totalMembersRef.current;
        const att = parseInt(value, 10) || 0;
        if (total > 0) {
          updated.AttendancePercent = Math.round((att / total) * 100).toString();
        } else {
          updated.AttendancePercent = att > 0 ? '100' : '0';
        }
      }
      return updated;
    });
    if (errors[key]) setErrors(prev => ({ ...prev, [key]: '' }));
  };

  // Agenda handler (single file)
  const handleAgendaFile = (idx, e) => {
    const file = e.target.files[0];
    if (file) { const a = [...agendas]; a[idx] = { file, name: file.name }; setAgendas(a); }
  };

  // Minutes handler (single file)
  const handleMinuteFile = (idx, e) => {
    const file = e.target.files[0];
    if (file) { const m = [...minutes]; m[idx] = { file, name: file.name }; setMinutes(m); }
  };

  // Photo handlers
  const handlePhotoChange = (idx, e) => {
    const file = e.target.files[0];
    if (file) {
      const p = [...photos];
      p[idx] = { ...p[idx], file };
      const reader = new FileReader();
      reader.onload = (ev) => { p[idx].preview = ev.target.result; setPhotos([...p]); };
      reader.readAsDataURL(file);
      setPhotos(p);
    }
  };

  const updatePhotoDesc = (idx, val) => {
    const p = [...photos];
    p[idx] = { ...p[idx], description: val };
    setPhotos(p);
  };

  const handleSave = async () => {
    const newErrors = {};
    if (!form.Title.trim()) newErrors.Title = 'mandatory';
    if (!form.EventDate) newErrors.EventDate = 'mandatory';
    if (Object.keys(newErrors).length > 0) { setErrors(newErrors); return; }

    setSaving(true);
    try {
      const groupId = filterGroupId || '33359';
      let savedEventID = isNew ? '0' : String(id).replace(/^E/, '');
      if (isNew) {
        const createRes = await createEvent({
          grpID: groupId,
          userID: '13010',
          evntTitle: form.Title,
          evntDesc: form.Description,
          evntDate: form.EventDate,
          publishDate: form.EventDate,
          expiryDate: form.EventDate,
          eventVenue: '',
          rsvpEnable: '0',
          questionEnable: '0',
          attendance: form.Attendance,
          attendancePercent: form.AttendancePercent,
        });
        savedEventID = createRes.data?.eventID || '0';
        alert('Event created successfully');
      } else {
        await updateEvent({
          eventID: savedEventID,
          grpID: groupId,
          userID: '13010',
          evntTitle: form.Title,
          evntDesc: form.Description,
          evntDate: form.EventDate,
          publishDate: form.EventDate,
          expiryDate: form.EventDate,
          eventVenue: '',
          rsvpEnable: '0',
          attendance: form.Attendance,
          attendancePercent: form.AttendancePercent,
          questionEnable: '0',
        });
        alert('Event updated successfully');
      }
      // Save extras (photos with descriptions)
      const extrasPayload = {
        eventID: savedEventID,
        photos: photos.filter(p => p.description || p.preview).map(p => ({ photoPath: p.preview || '', description: p.description || '' })),
        agendaFileNames: agendas.filter(a => a.name).map(a => a.name),
        minutesFileNames: minutes.filter(m => m.name).map(m => m.name),
      };
      console.log('[EventDetailPage] saving extras:', JSON.stringify(extrasPayload));
      if (savedEventID && savedEventID !== '0') {
        try {
          const extrasResult = await saveEventExtras(extrasPayload);
          console.log('[EventDetailPage] extras save result:', JSON.stringify(extrasResult.data));
        } catch (ex) { console.error('[EventDetailPage] extras save error:', ex); }
      }
      navigate(-1);
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>{pageName || 'Chapter'}</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Event</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleSave} disabled={saving} style={{ ...btnStyle, backgroundColor: '#1a297d', cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1 }}>
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
            {isNew ? 'Save' : 'Update'}
          </button>
          <button onClick={() => navigate(-1)} style={{ ...btnStyle, backgroundColor: '#1a297d' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {error && <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>{error}</div>}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>

        {/* Title */}
        <div style={{ marginBottom: '12px' }}>
          <label style={labelStyle}>
            Title
            {errors.Title && <span style={mandatoryStyle}>mandatory</span>}
          </label>
          <input type="text" value={form.Title} onChange={(e) => setField('Title', e.target.value)} style={inputStyle} />
        </div>

        {/* Description */}
        <div style={{ marginBottom: '12px' }}>
          <label style={labelStyle}>Description <span style={{ fontWeight: 'normal', fontSize: '11px' }}>(Content should be only in English)</span></label>
          <textarea value={form.Description} onChange={(e) => setField('Description', e.target.value)} rows={3} style={{ ...inputStyle, height: 'auto', resize: 'vertical' }} />
        </div>

        {/* Attendance | Attendance(%) | Date */}
        <div style={{ display: 'flex', gap: '20px', marginBottom: '12px' }}>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Attendance</label>
            <input type="text" value={form.Attendance} onChange={(e) => setField('Attendance', e.target.value)} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Attendance(%)</label>
            <input type="text" value={form.AttendancePercent} readOnly style={{ ...inputStyle, backgroundColor: '#f0f0f0' }} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>
              Date
              {errors.EventDate && <span style={mandatoryStyle}>mandatory</span>}
            </label>
            <input type="datetime-local" value={form.EventDate} onChange={(e) => setField('EventDate', e.target.value)} style={inputStyle} />
          </div>
        </div>

        {/* Agenda + Minutes of Meeting */}
        <div style={{ display: 'flex', gap: '30px', marginBottom: '15px' }}>
          {/* Agenda */}
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>
              Agenda
              <svg width="12" height="12" fill="none" stroke="#1a297d" viewBox="0 0 24 24" strokeWidth="2" style={{ marginLeft: '4px', verticalAlign: 'middle' }}><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
            </label>
            <div style={{ display: 'flex', alignItems: 'center', gap: '8px', border: '1px solid #eee', borderRadius: '4px', padding: '8px 10px' }}>
              <label style={{ ...btnStyle, backgroundColor: '#1a297d', padding: '4px 10px', cursor: 'pointer', fontSize: '11px' }}>
                Choose File
                <input type="file" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.jpg,.png" onChange={(e) => handleAgendaFile(0, e)} style={{ display: 'none' }} />
              </label>
              <span style={{ fontSize: '12px', color: agendas[0]?.name ? '#333' : '#999' }}>{agendas[0]?.name || 'No file chosen'}</span>
            </div>
          </div>

          {/* Minutes Of Meeting */}
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>
              Minutes Of Meeting
              <svg width="12" height="12" fill="none" stroke="#1a297d" viewBox="0 0 24 24" strokeWidth="2" style={{ marginLeft: '4px', verticalAlign: 'middle' }}><path d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
            </label>
            <div style={{ display: 'flex', alignItems: 'center', gap: '8px', border: '1px solid #eee', borderRadius: '4px', padding: '8px 10px' }}>
              <label style={{ ...btnStyle, backgroundColor: '#1a297d', padding: '4px 10px', cursor: 'pointer', fontSize: '11px' }}>
                Choose File
                <input type="file" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.jpg,.png" onChange={(e) => handleMinuteFile(0, e)} style={{ display: 'none' }} />
              </label>
              <span style={{ fontSize: '12px', color: minutes[0]?.name ? '#333' : '#999' }}>{minutes[0]?.name || 'No file chosen'}</span>
            </div>
          </div>
        </div>

        {/* Photo (Maximum 5 Photos) */}
        <div style={{ marginBottom: '10px' }}>
          <label style={{ ...labelStyle, marginBottom: '10px' }}>Photo (Maximum 5 Photos)</label>
          {photos.map((photo, idx) => (
            <div key={idx} style={{ display: 'flex', gap: '15px', marginBottom: '12px', border: '1px solid #eee', borderRadius: '4px', padding: '12px' }}>
              {/* Image */}
              <div style={{ flex: '0 0 100px' }}>
                <label style={{ cursor: 'pointer', display: 'block' }}>
                  <div style={{
                    width: '100px', height: '80px', border: '1px solid #ccc', borderRadius: '4px',
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                    backgroundColor: '#f5f5f5', overflow: 'hidden',
                  }}>
                    {photo.preview ? (
                      <img src={photo.preview} alt={`Photo ${idx + 1}`} style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }} />
                    ) : (
                      <div style={{ textAlign: 'center', color: '#999', fontSize: '10px' }}>
                        <svg width="30" height="30" fill="#ccc" viewBox="0 0 24 24" style={{ display: 'block', margin: '0 auto 2px' }}>
                          <path d="M21 19V5c0-1.1-.9-2-2-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2zM8.5 13.5l2.5 3.01L14.5 12l4.5 6H5l3.5-4.5z" />
                        </svg>
                        Click to add image
                      </div>
                    )}
                  </div>
                  <input type="file" accept="image/*" onChange={(e) => handlePhotoChange(idx, e)} style={{ display: 'none' }} />
                </label>
              </div>
              {/* Description */}
              <div style={{ flex: 1 }}>
                <textarea
                  value={photo.description}
                  onChange={(e) => updatePhotoDesc(idx, e.target.value)}
                  placeholder="Enter Description"
                  rows={3}
                  style={{ ...inputStyle, height: 'auto', resize: 'vertical' }}
                />
              </div>
            </div>
          ))}
        </div>

      </div>
    </div>
  );
}