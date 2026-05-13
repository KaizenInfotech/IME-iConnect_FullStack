import 'dart:io';
import 'dart:math';

import 'package:device_info_plus/device_info_plus.dart';

import '../storage/secure_storage.dart';

/// Returns a stable per-install device identifier used as `imeiNo` in every
/// API request. Must stay identical between login and subsequent session
/// checks, otherwise the server treats it as another device and forces
/// session-expired. Must also differ between two physical devices so that
/// logging in on device B expires device A.
///
/// iOS: identifierForVendor (stable per vendor per device).
/// Android: a random 32-hex-char ID generated once on first use and persisted
/// in SecureStorage. Survives app restarts; resets only on reinstall — same
/// behaviour as iOS identifierForVendor.
class DeviceIdHelper {
  DeviceIdHelper._();

  static const String _androidKey = 'android_device_id';

  static Future<String> getDeviceId() async {
    try {
      if (Platform.isIOS) {
        final info = await DeviceInfoPlugin().iosInfo;
        return info.identifierForVendor ?? '';
      }
      if (Platform.isAndroid) {
        final stored = await SecureStorage.instance.read(_androidKey);
        if (stored != null && stored.isNotEmpty) return stored;
        final generated = _generateId();
        await SecureStorage.instance.write(_androidKey, generated);
        return generated;
      }
    } catch (_) {}
    return '';
  }

  static String _generateId() {
    final rnd = Random.secure();
    final bytes = List<int>.generate(16, (_) => rnd.nextInt(256));
    return bytes
        .map((b) => b.toRadixString(16).padLeft(2, '0'))
        .join();
  }
}
