# IMEI Connect — Admin Panel (React)

The **administrator-facing web panel** for the IMEI Connect (TouchBase)
platform — a Rotary International organization-management system. Office bearers
and national admins use it to manage clubs, members, content, and reports.

It is one of three clients in this repository, all backed by the same API:

- **This panel** — administrators, in the browser
- [Flutter app](../frontend) — members, on mobile / web / desktop
- [Backend API](../backend) — shared REST API + database

It is the modern replacement for the admin side of the legacy ASP.NET Web Forms
site in [`../production-project`](../production-project).

---

## What it does

Administrative control over the Rotary hierarchy:

```
Organization → Districts → Clubs (Groups) → Members → Family
```

| Area | Capabilities |
|------|--------------|
| **Branch & Chapter** | Region-wise branches/chapters (Groups), drill into a single chapter |
| **Members** | Add/edit members, member detail, directory |
| **Club content** | Events, upcoming events, announcements, albums, documents, e-bulletins, web links, banners |
| **Structure** | Sub-groups, executive committee, governing council, past presidents |
| **Engagement** | Attendance, service directory, notifications, MER / iMélange |
| **Reports** | Members & Users list exports (Excel) — global *and* per-branch/chapter |
| **System** | Login (JWT), change password, settings |

Routes are defined in [`src/routes/AppRouter.jsx`](src/routes/AppRouter.jsx);
each page lives in [`src/pages/`](src/pages) and its API calls in
[`src/api/`](src/api) (one service file per domain).

---

## Tech stack

| Concern | Choice |
|---------|--------|
| Framework | React 18 |
| Build tool | Vite 5 |
| Routing | React Router |
| HTTP | Axios (shared instance in `src/api/axiosInstance.js`) |
| Auth | JWT, held in `AuthContext` |
| Styling | Tailwind CSS (+ inline styles on some legacy-styled pages) |
| Excel export | `xlsx` / HTML-table export |

---

## Prerequisites

- [Node.js](https://nodejs.org) 18+ and npm
- A running [backend API](../backend) to point at

---

## Getting started

```bash
npm install
npm run dev        # dev server at http://localhost:5173
```

### Other scripts

```bash
npm run build      # production build
npm run preview    # preview the production build
npm run lint       # ESLint
```

### Configuration

Set the API base URL in `.env.local`:

```
VITE_API_URL=http://localhost:5000/api
```

---

## Project structure

```
src/
├── api/           # API service files — one per domain (memberService, groupService, …)
│   └── axiosInstance.js   # shared Axios client (base URL + JWT header)
├── components/
│   ├── layout/    # Sidebar, Navbar, Layout wrapper
│   └── shared/    # DataTable, Modal, ConfirmDialog, LoadingSpinner, …
├── context/       # AuthContext (JWT auth state)
├── pages/         # one component per route
├── routes/        # AppRouter — all route definitions + ProtectedRoute guard
├── main.jsx       # entry point
└── index.css      # Tailwind directives + global styles
```

---

## How the Reports page works (a worked example)

`ReportsPage` is reused in two contexts, keyed off a `?groupId` query param:

- **Global** (`/reports`, from the sidebar) — "National Admin" view with branch
  selectors (`-Select- / All / every branch`) for both a Members List and a
  Users List.
- **Per branch/chapter** (`/reports?groupId=<id>`, opened from inside a chapter)
  — locked to that one branch: no selector, no other branches, and only the
  Members List download.

This pattern — one page, behavior switched by a route param — recurs across the
panel, so check for a `groupId`/`:id` param before assuming a page is global.

---

## Conventions for contributors

- One API service file per domain in `src/api/`; components call services, not Axios directly.
- New pages get a route in `AppRouter.jsx` behind `ProtectedRoute`.
- Auth state and the JWT live in `AuthContext`; the token is attached by `axiosInstance`.
- Reuse `components/shared/` (DataTable, Modal, ConfirmDialog) rather than rebuilding tables/dialogs.
