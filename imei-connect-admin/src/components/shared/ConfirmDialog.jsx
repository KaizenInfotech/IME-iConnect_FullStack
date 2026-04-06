export default function ConfirmDialog({ isOpen, onClose, onConfirm, title, message }) {
  if (!isOpen) return null;

  return (
    <div style={{ position: 'fixed', inset: 0, zIndex: 9999, display: 'flex', alignItems: 'center', justifyContent: 'center', padding: '16px' }}>
      <div style={{ position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={onClose} />
      <div style={{ position: 'relative', backgroundColor: '#fff', borderRadius: '12px', boxShadow: '0 10px 40px rgba(0,0,0,0.2)', width: '100%', maxWidth: '380px', padding: '24px', zIndex: 10000 }}>
        <h3 style={{ fontSize: '18px', fontWeight: '600', color: '#333', marginBottom: '8px' }}>{title || 'Confirm'}</h3>
        <p style={{ fontSize: '14px', color: '#666', marginBottom: '24px' }}>{message || 'Are you sure you want to proceed?'}</p>
        <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '12px' }}>
          <button onClick={onClose} style={{ padding: '8px 18px', fontSize: '13px', border: '1px solid #ccc', borderRadius: '6px', backgroundColor: '#fff', color: '#333', cursor: 'pointer' }}>
            Cancel
          </button>
          <button
            onClick={() => { onConfirm(); }}
            style={{ padding: '8px 18px', fontSize: '13px', border: 'none', borderRadius: '6px', backgroundColor: '#f44336', color: '#fff', cursor: 'pointer' }}
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}