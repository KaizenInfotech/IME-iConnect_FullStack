# TouchBase — Flutter App (IMEI-iConnect)

The cross-platform **member-facing mobile & web app** for the IMEI Connect
(TouchBase) platform — a Rotary International organization-management system.
It was migrated from the original native iOS app (Swift / Objective-C, preserved
in [`IMEI-iConnect/`](IMEI-iConnect)) to Flutter.

It is one of three clients in this repository, all backed by the same API:

- **This app** — members, on iOS / Android / web / desktop
- [React admin panel](../imei-connect-admin) — administrators
- [Backend API](../backend) — shared REST API + database

---

## What it does

Gives Rotary members and office bearers access to their club and district on the
go. The hierarchy it navigates:

```
Organization → Districts → Clubs (Groups) → Members → Family
```

Feature modules (under `lib/src/features/`):

| Area | Features |
|------|----------|
| **Access** | OTP login, group/welcome selection, dashboard |
| **People** | Member directory, profile & family, find rotarian, find club |
| **Club life** | Events, attendance, celebrations (birthdays/anniversaries) |
| **Content** | Announcements, gallery & albums, documents, e-bulletins, web links |
| **Structure** | Sub-groups / committees, district features, branch & chapter, leaderboard |
| **System** | Notifications (FCM), settings, monthly report & maps |

24 feature modules in total.

---

## Tech stack

| Concern | Choice |
|---------|--------|
| SDK | Dart `^3.10.1` / Flutter |
| Platforms | iOS 14+, Android, Web, macOS, Windows, Linux |
| State management | **Provider** (`ChangeNotifier`) |
| Navigation | **GoRouter** (~50 routes, auth guard) |
| Networking | `package:http` (singleton `ApiClient`) — **all requests are POST** |
| Local storage | SharedPreferences, `flutter_secure_storage`, SQLite (`sqflite`) |
| Models | `json_serializable` + `build_runner` (all fields nullable) |
| Push | Firebase Cloud Messaging |

---

## Prerequisites

- [Flutter SDK](https://docs.flutter.dev/get-started/install) with Dart `^3.10.1`
- Xcode (for iOS) / Android Studio + SDK (for Android)
- CocoaPods (`sudo gem install cocoapods`) for iOS builds

---

## Getting started

```bash
# Install dependencies
flutter pub get

# iOS only: install pods
cd ios && pod install && cd ..

# Run on a connected device / simulator
flutter run

# Generate model code after editing a @JsonSerializable model
dart run build_runner build --delete-conflicting-outputs
```

### Build

```bash
flutter build apk     # Android
flutter build ios     # iOS
flutter build web     # Web
```

### Quality

```bash
flutter analyze       # Lint (package:flutter_lints)
flutter test          # All tests
```

---

## Architecture: feature-first + Provider

```
lib/
├── main.dart    # Firebase init + singleton bootstrap (ApiClient, storage, DB)
├── app.dart     # MultiProvider (25 providers) wrapping MaterialApp.router
└── src/
    ├── core/        # Shared infrastructure
    │   ├── constants/   # api_constants (endpoints), app_constants (pref keys)
    │   ├── network/     # api_client, api_interceptor, connectivity_service
    │   ├── navigation/  # app_router (GoRouter)
    │   ├── storage/     # local_storage, secure_storage, database_helper (SQLite)
    │   ├── theme/       # colors (#673AB7 primary), text styles, theme
    │   ├── extensions/  # string / date / context / widget extensions
    │   ├── utils/       # validators, date utils, downloaders, image loader
    │   └── widgets/     # reusable widgets (CommonAppBar, CommonButton, …)
    └── features/    # 24 feature modules
```

Each feature follows the same shape:

```
features/<name>/
├── models/      # JSON-serializable result models (fields nullable)
├── providers/   # ChangeNotifier: isLoading / error / data + async API calls
├── screens/     # UI pages consuming state via Consumer<Provider>
└── widgets/     # feature-specific UI
```

### Networking notes

- Singleton `ApiClient.instance` over `package:http`.
- **All requests are POST**, mirroring the original iOS API contract.
- User identity (`masterUID`, `grpID`, `profileId`) travels in the request body, not headers.
- `401` triggers session expiry → re-login.

---

## Where to look

| You want to… | Read |
|--------------|------|
| Understand conventions & architecture in depth | [`CLAUDE.md`](CLAUDE.md) |
| Map an old iOS ViewController to its Flutter screen | [`folder_details.md`](folder_details.md) |
| Look up an API endpoint | [`api_details.md`](api_details.md) |
| See how the app was rebuilt phase by phase | [`migration_phases/00_MASTER_INDEX.md`](migration_phases/00_MASTER_INDEX.md) |

---

## Conventions for contributors

- **Nullable everything** — no force-unwrapping; access via `?.`, `??`, `Consumer`, `Selector`.
- **Singletons** for infrastructure: `ApiClient`, `LocalStorage`, `SecureStorage`, `DatabaseHelper`.
- New screens go in a feature module and route through `app_router.dart`.
- After changing any `@JsonSerializable` model, re-run `build_runner`.
- SharedPreferences keys mirror the original iOS `UserDefaults` keys — keep them in `app_constants.dart`.
