// Shared mobile-number and email validation for member contact forms.

// Per-country mobile-number rules, keyed by dial code (with leading +).
// `len` = exact allowed national-number digit counts; `start` = required
// leading-digit pattern. Countries not listed fall back to DEFAULT_PHONE_RULE.
export const PHONE_RULES = {
  '+91': { len: [10], start: /^[6-9]/ }, // India
  '+1': { len: [10] },                   // US / Canada
  '+44': { len: [10] },                  // UK
  '+971': { len: [9] },                  // UAE
  '+65': { len: [8] },                   // Singapore
  '+61': { len: [9] },                   // Australia
  '+60': { len: [9, 10] },               // Malaysia
  '+977': { len: [10] },                 // Nepal
  '+880': { len: [10] },                 // Bangladesh
  '+94': { len: [9] },                   // Sri Lanka
  '+974': { len: [8] },                  // Qatar
  '+966': { len: [9] },                  // Saudi Arabia
  '+968': { len: [8] },                  // Oman
  '+973': { len: [8] },                  // Bahrain
  '+965': { len: [8] },                  // Kuwait
};
export const DEFAULT_PHONE_RULE = { min: 6, max: 14 };

// Normalise a dial code to "+NN" form (accepts "91", "+91", " 91 ").
export function normalizeDialCode(code) {
  const raw = String(code || '').trim();
  if (!raw) return '+91';
  return raw.startsWith('+') ? raw : `+${raw.replace(/\D/g, '')}`;
}

// Returns an error string, or null when valid.
export function validateMobile(countryCode, raw) {
  const digits = String(raw || '').replace(/\D/g, '');
  if (!digits) return 'Please enter mobile number';
  const dial = normalizeDialCode(countryCode);
  const rule = PHONE_RULES[dial] || DEFAULT_PHONE_RULE;
  if (rule.start && !rule.start.test(digits)) {
    return 'Enter a valid mobile number for the selected country';
  }
  if (rule.len && !rule.len.includes(digits.length)) {
    return `Mobile number must be ${rule.len.join(' or ')} digits for ${dial}`;
  }
  if (rule.min && (digits.length < rule.min || digits.length > rule.max)) {
    return `Mobile number must be ${rule.min}-${rule.max} digits`;
  }
  return null;
}

// Returns an error string, or null when valid. When `required` is false an
// empty value is allowed (returns null).
export function validateEmail(raw, required = true) {
  const email = String(raw || '').trim();
  if (!email) return required ? 'Please enter email id' : null;
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
  return re.test(email) ? null : 'Please enter a valid email id';
}
