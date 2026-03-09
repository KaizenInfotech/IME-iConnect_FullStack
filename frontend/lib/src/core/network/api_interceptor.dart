import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;

import '../constants/app_constants.dart';

/// API request helper matching iOS ServiceManager headers and response handling.
/// Provides static methods for headers, response parsing, and error handling.
class ApiRequestHelper {
  ApiRequestHelper._();

  /// In-memory JWT token cache. Set after login, cleared on logout.
  /// Loaded from SecureStorage on app startup.
  static String? _jwtToken;

  /// Set the JWT token (call after successful OTP verification).
  static void setJwtToken(String? token) {
    _jwtToken = token;
  }

  /// Get the current JWT token.
  static String? get jwtToken => _jwtToken;

  /// Build the Authorization header value.
  /// Uses JWT Bearer token if available, falls back to Basic auth for login endpoints.
  static String _authHeaderValue() {
    if (_jwtToken != null && _jwtToken!.isNotEmpty) {
      return 'Bearer $_jwtToken';
    }
    return AppConstants.authorizationHeader;
  }

  /// Default headers for JSON requests.
  /// Uses JWT Bearer token when available.
  static Map<String, String> getDefaultHeaders() {
    return {
      'Authorization': _authHeaderValue(),
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    };
  }

  /// Headers for URL-encoded requests (login).
  static Map<String, String> getUrlEncodedHeaders() {
    return {
      'Authorization': _authHeaderValue(),
      'Accept': 'application/json',
      'Content-Type': 'application/x-www-form-urlencoded',
    };
  }

  /// Parse JSON response, handle status codes, return null on failure.
  /// Matches iOS response handling pattern where response.result.value
  /// is cast to NSDictionary.
  static Map<String, dynamic>? handleResponse(http.Response response) {
    try {
      if (response.statusCode >= 200 && response.statusCode < 300) {
        if (response.body.isEmpty) return null;
        final decoded = json.decode(response.body);
        if (decoded is Map<String, dynamic>) {
          return decoded;
        }
        return null;
      }

      // Handle 401 - session expiry
      if (response.statusCode == 401) {
        debugPrint('Session expired - 401 Unauthorized');
        return null;
      }

      debugPrint(
        'API Error: Status ${response.statusCode} - ${response.body}',
      );
      return null;
    } catch (e) {
      debugPrint('Response parsing error: $e');
      return null;
    }
  }

  /// Return user-friendly error message.
  /// Matches iOS error handling where ServiceManager returns
  /// ["serverError":"Error"] on failure.
  static String handleError(dynamic error) {
    if (error is http.ClientException) {
      return 'Could not connect to server, please try again.';
    }
    if (error is FormatException) {
      return 'Invalid response from server.';
    }
    if (error.toString().contains('SocketException') ||
        error.toString().contains('Connection refused')) {
      return 'No internet connection. Please check your Internet Connection and try again.';
    }
    if (error.toString().contains('TimeoutException') ||
        error.toString().contains('timed out')) {
      return 'Request timed out. Please try again.';
    }
    return 'Something went wrong, Please try again later';
  }

  /// Log request details in debug mode.
  static void logRequest(String method, String url, {dynamic body}) {
    if (kDebugMode) {
      debugPrint('─── REQUEST ───');
      debugPrint('$method $url');
      if (body != null) {
        debugPrint('Body: $body');
      }
    }
  }

  /// Log response details in debug mode.
  static void logResponse(http.Response response) {
    if (kDebugMode) {
      debugPrint('─── RESPONSE ───');
      debugPrint('Status: ${response.statusCode}');
      debugPrint(
        'Body: ${response.body.length > 500 ? '${response.body.substring(0, 500)}...' : response.body}',
      );
    }
  }
}
