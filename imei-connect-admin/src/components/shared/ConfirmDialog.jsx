export default function ConfirmDialog({ isOpen, onClose, onConfirm, title, message }) {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div className="fixed inset-0 bg-black/50" onClick={onClose} />
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-sm p-6">
        <h3 className="text-lg font-semibold text-gray-800 mb-2">{title || 'Confirm'}</h3>
        <p className="text-gray-600 mb-6">{message || 'Are you sure you want to proceed?'}</p>
        <div className="flex justify-end gap-3">
          <button onClick={onClose} className="btn-outline text-sm">
            Cancel
          </button>
          <button
            onClick={() => { onConfirm(); onClose(); }}
            className="btn-danger text-sm"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}
