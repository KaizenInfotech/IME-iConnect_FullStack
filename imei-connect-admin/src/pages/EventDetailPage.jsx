import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getEvent, createEvent, updateEvent } from '../api/eventService';

export default function EventDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isNew = !id || id === 'new';
  const [loading, setLoading] = useState(!isNew);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [errors, setErrors] = useState({});

  const [form, setForm] = useState({
    Title: '',
    Description: '',
    Location: '',
    EventDate: '',
    PublishDate: '',
    ExpiryDate: '',
    Venue: '',
    RSVPEnable: false,
  });

  useEffect(() => {
    if (!isNew) fetchData();
  }, [id]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getEvent(id);
      const ev = res.data?.data || res.data;
      if (ev) {
        setForm({
          Title: ev.Title || '',
          Description: ev.Description || '',
          Location: ev.Location || ev.Venue || '',
          EventDate: formatDateTimeForInput(ev.EventDate),
          PublishDate: formatDateTimeForInput(ev.PublishDate),
          ExpiryDate: formatDateTimeForInput(ev.ExpiryDate),
          Venue: ev.Venue || '',
          RSVPEnable: ev.RSVPEnable ?? ev.IsRSVP ?? false,
        });
      }
    } catch {
      setError('Failed to load event details');
    } finally {
      setLoading(false);
    }
  };

  const formatDateTimeForInput = (dateStr) => {
    if (!dateStr) return '';
    try {
      const d = new Date(dateStr);
      if (isNaN(d.getTime())) return dateStr;
      const day = String(d.getDate()).padStart(2, '0');
      const month = String(d.getMonth() + 1).padStart(2, '0');
      const yr = d.getFullYear();
      const hr = String(d.getHours()).padStart(2, '0');
      const min = String(d.getMinutes()).padStart(2, '0');
      return `${day}/${month}/${yr} ${hr}:${min}`;
    } catch {
      return dateStr;
    }
  };

  const setField = (key, value) => {
    setForm((prev) => ({ ...prev, [key]: value }));
    if (errors[key]) setErrors((prev) => ({ ...prev, [key]: '' }));
  };

  const validate = () => {
    const newErrors = {};
    if (!form.Title.trim()) newErrors.Title = 'mandatory';
    if (!form.Location.trim()) newErrors.Location = 'mandatory';
    if (!form.EventDate.trim()) newErrors.EventDate = 'mandatory';
    if (!form.PublishDate.trim()) newErrors.PublishDate = 'mandatory';
    if (!form.ExpiryDate.trim()) newErrors.ExpiryDate = 'mandatory';
    return newErrors;
  };

  const handleSave = async () => {
    const newErrors = validate();
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }
    setSaving(true);
    try {
      if (isNew) {
        await createEvent(form);
        alert('Event created successfully');
      } else {
        await updateEvent(id, form);
        alert('Event updated successfully');
      }
      navigate('/events');
    } catch (err) {
      setError(err.response?.data?.message || 'Save failed');
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <LoadingSpinner />;

  const inputCls = 'w-full h-[35px] rounded-[8px] border border-[#d0d0d0] px-[10px] text-[13px] outline-none';
  const labelCls = 'block text-[12px] text-[#555] mb-[4px]';
  const mandatoryCls = 'text-[#dd4b39] text-[11px] ml-[4px]';

  return (
    <div>
      {/* Page Title */}
      <div className="mb-[15px]">
        <h3 className="text-[18px] font-normal text-[#333]">
          Events - <span className="text-[#555]">{isNew ? 'Create Event' : 'Edit Event'}</span>
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
          onClick={() => navigate('/events')}
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

      {/* Form */}
      <div
        className="bg-white border border-[#eee] rounded-[8px]"
        style={{ padding: '25px 28px 10px 28px' }}
      >
        {/* Title */}
        <div className="mb-[15px]">
          <label className={labelCls}>
            Title
            <span className={mandatoryCls}>mandatory</span>
          </label>
          <input
            type="text"
            value={form.Title}
            onChange={(e) => setField('Title', e.target.value)}
            className={inputCls}
          />
          {errors.Title && <span className="text-[#dd4b39] text-[11px]">{errors.Title}</span>}
        </div>

        {/* Description */}
        <div className="mb-[15px]">
          <label className={labelCls}>Description</label>
          <textarea
            value={form.Description}
            onChange={(e) => setField('Description', e.target.value)}
            rows={5}
            className="w-full rounded-[8px] border border-[#d0d0d0] px-[10px] py-[8px] text-[13px] outline-none resize-vertical"
          />
        </div>

        {/* Location of the Project */}
        <div className="mb-[15px]">
          <label className={labelCls}>
            Location of the Project
            <span className={mandatoryCls}>mandatory</span>
          </label>
          <input
            type="text"
            value={form.Location}
            onChange={(e) => setField('Location', e.target.value)}
            className={inputCls}
          />
          {errors.Location && <span className="text-[#dd4b39] text-[11px]">{errors.Location}</span>}
        </div>

        {/* Event Date */}
        <div className="mb-[15px]">
          <label className={labelCls}>
            Event Date
            <span className={mandatoryCls}>mandatory</span>
          </label>
          <input
            type="text"
            placeholder="d/m/Y H:i"
            value={form.EventDate}
            onChange={(e) => setField('EventDate', e.target.value)}
            className={inputCls}
          />
          {errors.EventDate && <span className="text-[#dd4b39] text-[11px]">{errors.EventDate}</span>}
        </div>

        {/* Publish Date */}
        <div className="mb-[15px]">
          <label className={labelCls}>
            Publish Date
            <span className={mandatoryCls}>mandatory</span>
          </label>
          <input
            type="text"
            placeholder="d/m/Y H:i"
            value={form.PublishDate}
            onChange={(e) => setField('PublishDate', e.target.value)}
            className={inputCls}
          />
          {errors.PublishDate && <span className="text-[#dd4b39] text-[11px]">{errors.PublishDate}</span>}
        </div>

        {/* Expiry Date */}
        <div className="mb-[15px]">
          <label className={labelCls}>
            Expiry Date
            <span className={mandatoryCls}>mandatory</span>
          </label>
          <input
            type="text"
            placeholder="d/m/Y H:i"
            value={form.ExpiryDate}
            onChange={(e) => setField('ExpiryDate', e.target.value)}
            className={inputCls}
          />
          {errors.ExpiryDate && <span className="text-[#dd4b39] text-[11px]">{errors.ExpiryDate}</span>}
        </div>

        {/* Event Venue */}
        <div className="mb-[15px]">
          <label className={labelCls}>Event Venue</label>
          <input
            type="text"
            value={form.Venue}
            onChange={(e) => setField('Venue', e.target.value)}
            className={inputCls}
          />
        </div>

        {/* RSVP Enable */}
        <div className="mb-[15px]">
          <label className="flex items-center gap-[8px] text-[13px] text-[#555] cursor-pointer">
            <input
              type="checkbox"
              checked={form.RSVPEnable}
              onChange={(e) => setField('RSVPEnable', e.target.checked)}
              className="w-[16px] h-[16px]"
            />
            RSVP Enable
          </label>
        </div>
      </div>
    </div>
  );
}
