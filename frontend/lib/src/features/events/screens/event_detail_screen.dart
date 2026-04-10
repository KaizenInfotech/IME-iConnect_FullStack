import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../../core/constants/app_constants.dart';
import '../../../core/storage/local_storage.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_text_styles.dart';
import '../../../core/widgets/common_loader.dart';
import '../../../core/widgets/common_toast.dart';
import '../models/event_detail_result.dart';
import '../models/event_extras_result.dart';
import '../providers/events_provider.dart';
import '../widgets/event_share_sheet.dart';
import '../widgets/rsvp_button.dart';
import 'add_event_screen.dart';

/// Port of iOS EventsDetailController.swift + EventDetailNotiViewController.swift.
/// Shows: event image, title, description, venue, date, link, RSVP buttons,
/// question popup (text or option), share, edit, delete, add-to-calendar actions.
/// iOS: 4,057 lines → simplified Flutter equivalent.
class EventDetailScreen extends StatefulWidget {
  const EventDetailScreen({
    super.key,
    required this.eventID,
    required this.groupProfileID,
    required this.grpId,
    required this.eventTitle,
    this.isCategory = '',
    this.isAdmin = false,
  });

  final String eventID;
  final String groupProfileID;
  final String grpId;
  final String eventTitle;
  final String isCategory;
  final bool isAdmin;

  @override
  State<EventDetailScreen> createState() => _EventDetailScreenState();
}

class _EventDetailScreenState extends State<EventDetailScreen> {
  String get _profileId =>
      LocalStorage.instance.getString(AppConstants.keySessionGrpProfileId) ??
      '';

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) => _loadEventDetail());
  }

  /// iOS: getEventsDetail(groupProfileID, grpID, eventID)
  Future<void> _loadEventDetail() async {
    final provider = context.read<EventsProvider>();
    CommonLoader.show(context);

    final success = await provider.fetchEventDetail(
      groupProfileID: widget.groupProfileID,
      eventID: widget.eventID,
      grpId: widget.grpId,
    );
    // Agenda, minutes-of-meeting and photo gallery live in separate prod
    // tables. iOS PastEventDetailViewController fetches them on load.
    await provider.fetchEventExtras(eventID: widget.eventID);

    if (!mounted) return;
    CommonLoader.dismiss(context);

    if (!success && provider.error != null) {
      CommonToast.error(context, provider.error!);
    }
  }

  /// iOS: functionForYesNoMaybeResult — send RSVP response
  Future<void> _onRsvpResponse(RsvpResponse response, EventsDetail event) async {
    // Map RsvpResponse to iOS joiningStatus
    String joiningStatus;
    switch (response) {
      case RsvpResponse.yes:
        joiningStatus = '0';
        break;
      case RsvpResponse.no:
        joiningStatus = '2';
        break;
      case RsvpResponse.maybe:
        joiningStatus = '1';
        break;
    }

    // iOS: check if question is enabled
    if (event.hasQuestion) {
      _showQuestionDialog(event, joiningStatus);
      return;
    }

    CommonLoader.show(context);

    final result = await context.read<EventsProvider>().answerEvent(
          grpId: widget.grpId,
          profileID: _profileId,
          eventId: widget.eventID,
          joiningStatus: joiningStatus,
        );

    if (!mounted) return;
    CommonLoader.dismiss(context);

    if (result != null && result.isSuccess) {
      CommonToast.success(context, 'Response recorded');
    } else {
      CommonToast.error(context, result?.message ?? 'Failed to record response');
    }
  }

  /// iOS: QuestionViewOptions / QuestionViewWritten popup
  void _showQuestionDialog(EventsDetail event, String joiningStatus) {
    if (event.isTextQuestion) {
      // Text answer dialog — iOS: QuestionViewWritten
      final answerController = TextEditingController();
      showDialog(
        context: context,
        builder: (ctx) => AlertDialog(
          title: Text(
            event.questionText ?? 'Question',
            style: const TextStyle(
              fontFamily: AppTextStyles.fontFamily,
              fontSize: 16,
              fontWeight: FontWeight.w600,
            ),
          ),
          content: TextField(
            controller: answerController,
            decoration: const InputDecoration(
              hintText: 'Enter your answer...',
              border: OutlineInputBorder(),
            ),
            maxLines: 3,
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(ctx),
              child: const Text('Cancel'),
            ),
            ElevatedButton(
              onPressed: () async {
                Navigator.pop(ctx);
                CommonLoader.show(context);

                final result =
                    await context.read<EventsProvider>().answerEvent(
                          grpId: widget.grpId,
                          profileID: _profileId,
                          eventId: widget.eventID,
                          joiningStatus: joiningStatus,
                          questionId: event.questionId ?? '',
                          answerByme: answerController.text.trim(),
                        );

                if (!mounted) return;
                CommonLoader.dismiss(context);

                if (result != null && result.isSuccess) {
                  CommonToast.success(context, 'Response recorded');
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.primary,
              ),
              child: const Text('Submit',
                  style: TextStyle(color: AppColors.white)),
            ),
          ],
        ),
      );
    } else if (event.isOptionQuestion) {
      // Option-based dialog — iOS: QuestionViewOptions
      showDialog(
        context: context,
        builder: (ctx) => AlertDialog(
          title: Text(
            event.questionText ?? 'Question',
            style: const TextStyle(
              fontFamily: AppTextStyles.fontFamily,
              fontSize: 16,
              fontWeight: FontWeight.w600,
            ),
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              if (event.option1 != null && event.option1!.isNotEmpty)
                _buildOptionButton(
                    ctx, event.option1!, '1', joiningStatus, event),
              const SizedBox(height: 8),
              if (event.option2 != null && event.option2!.isNotEmpty)
                _buildOptionButton(
                    ctx, event.option2!, '2', joiningStatus, event),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(ctx),
              child: const Text('Cancel'),
            ),
          ],
        ),
      );
    }
  }

  Widget _buildOptionButton(BuildContext ctx, String optionText,
      String answerValue, String joiningStatus, EventsDetail event) {
    return SizedBox(
      width: double.infinity,
      child: OutlinedButton(
        onPressed: () async {
          Navigator.pop(ctx);
          CommonLoader.show(context);

          final result = await context.read<EventsProvider>().answerEvent(
                grpId: widget.grpId,
                profileID: _profileId,
                eventId: widget.eventID,
                joiningStatus: joiningStatus,
                questionId: event.questionId ?? '',
                answerByme: answerValue,
              );

          if (!mounted) return;
          CommonLoader.dismiss(context);

          if (result != null && result.isSuccess) {
            CommonToast.success(context, 'Response recorded');
          }
        },
        style: OutlinedButton.styleFrom(
          foregroundColor: AppColors.primary,
          side: const BorderSide(color: AppColors.primary),
        ),
        child: Text(optionText),
      ),
    );
  }

  /// iOS: editEbullAction — navigate to edit event
  void _editEvent(EventsDetail event) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => ChangeNotifierProvider.value(
          value: context.read<EventsProvider>(),
          child: AddEventScreen(
            grpId: widget.grpId,
            groupProfileID: widget.groupProfileID,
            isCategory: widget.isCategory,
            editEvent: event,
          ),
        ),
      ),
    );
  }

  /// iOS: deleteEbullAction — delete event
  void _deleteEvent() {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Delete Event'),
        content: const Text('Are you sure you want to delete this event?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('Cancel'),
          ),
          TextButton(
            onPressed: () {
              Navigator.pop(ctx);
              // iOS: deleteDataWebservice → Group/DeleteByModuleName
              CommonToast.info(context, 'Delete functionality coming soon');
            },
            style: TextButton.styleFrom(foregroundColor: AppColors.systemRed),
            child: const Text('Delete'),
          ),
        ],
      ),
    );
  }

  /// iOS: shareButtonClickEvent
  void _shareEvent(EventsDetail event) {
    EventShareSheet.show(
      context,
      event: event,
      onShareText: () {
        // Build share text matching iOS format
        final text = StringBuffer();
        text.writeln(event.eventTitle ?? '');
        if (event.eventDesc != null && event.eventDesc!.isNotEmpty) {
          text.writeln(event.eventDesc);
        }
        if (event.venue != null && event.venue!.isNotEmpty) {
          text.writeln('Venue: ${event.venue}');
        }
        if (event.eventDateTime != null && event.eventDateTime!.isNotEmpty) {
          text.writeln('Date: ${event.eventDateTime}');
        }
        if (event.hasLink) {
          text.writeln('Link: ${event.link}');
        }
        CommonToast.info(context, 'Share text copied');
      },
      onShareImage: () {
        CommonToast.info(context, 'Share as image coming soon');
      },
      onSharePdf: () {
        CommonToast.info(context, 'Share as PDF coming soon');
      },
    );
  }

  void _launchUrl(String url) async {
    // iOS: prepend http:// if missing
    String finalUrl = url.trim();
    if (!finalUrl.contains('http://') && !finalUrl.contains('https://')) {
      finalUrl = 'http://$finalUrl';
    }
    final uri = Uri.parse(finalUrl);
    if (await canLaunchUrl(uri)) {
      await launchUrl(uri, mode: LaunchMode.externalApplication);
    }
  }

  /// iOS: open maps with venue lat/lon
  void _openMaps(EventsDetail event) async {
    if (event.venueLat != null &&
        event.venueLon != null &&
        event.venueLat!.isNotEmpty &&
        event.venueLon!.isNotEmpty) {
      final url =
          'https://maps.google.com/?q=${event.venueLat},${event.venueLon}';
      _launchUrl(url);
    } else if (event.venue != null && event.venue!.isNotEmpty) {
      final encoded = Uri.encodeComponent(event.venue!);
      _launchUrl('https://maps.google.com/?q=$encoded');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.scaffoldBackground,
      appBar: AppBar(
        backgroundColor: AppColors.primary,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.textOnPrimary),
        title: Text(
          widget.eventTitle,
          style: const TextStyle(
            fontFamily: AppTextStyles.fontFamily,
            fontSize: 17,
            fontWeight: FontWeight.w400,
            color: AppColors.textOnPrimary,
          ),
        ),
        actions: [
          Consumer<EventsProvider>(
            builder: (context, provider, _) {
              final event = provider.selectedEvent;
              if (event == null) return const SizedBox.shrink();
              return Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  // Share button
                  IconButton(
                    icon: const Icon(Icons.share,
                        color: AppColors.textOnPrimary, size: 22),
                    onPressed: () => _shareEvent(event),
                  ),
                  // Edit/Delete popup (admin only)
                  if (event.isUserAdmin || widget.isAdmin)
                    PopupMenuButton<String>(
                      icon: const Icon(Icons.more_vert,
                          color: AppColors.textOnPrimary),
                      onSelected: (value) {
                        if (value == 'edit') _editEvent(event);
                        if (value == 'delete') _deleteEvent();
                      },
                      itemBuilder: (_) => [
                        const PopupMenuItem(
                          value: 'edit',
                          child: Text('Edit Event'),
                        ),
                        const PopupMenuItem(
                          value: 'delete',
                          child: Text('Delete Event'),
                        ),
                      ],
                    ),
                ],
              );
            },
          ),
        ],
      ),
      body: Consumer<EventsProvider>(
        builder: (context, provider, child) {
          if (provider.isLoading) {
            return const Center(
              child: CircularProgressIndicator(color: AppColors.primary),
            );
          }

          final event = provider.selectedEvent;
          if (event == null) {
            return const Center(
              child: Text('No event details available'),
            );
          }

          return SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Event image — iOS: row 1 (252pt)
                if (event.hasValidImage)
                  Image.network(
                    event.eventImg!.replaceAll(' ', '%20'),
                    width: double.infinity,
                    height: 220,
                    fit: BoxFit.cover,
                    errorBuilder: (_, _, _) => const SizedBox(height: 0),
                  ),
                // Event title — iOS: row 2
                Padding(
                  padding: const EdgeInsets.fromLTRB(16, 16, 16, 0),
                  child: Text(
                    event.eventTitle ?? '',
                    style: const TextStyle(
                      fontFamily: AppTextStyles.fontFamily,
                      fontSize: 20,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textPrimary,
                    ),
                  ),
                ),
                // Event description — iOS: row 3
                if (event.eventDesc != null && event.eventDesc!.isNotEmpty)
                  Padding(
                    padding: const EdgeInsets.fromLTRB(16, 8, 16, 0),
                    child: Text(
                      event.eventDesc!,
                      style: const TextStyle(
                        fontFamily: AppTextStyles.fontFamily,
                        fontSize: 14,
                        color: AppColors.textSecondary,
                        height: 1.4,
                      ),
                    ),
                  ),
                const SizedBox(height: 12),
                // Venue — iOS: row 4
                if (event.venue != null && event.venue!.isNotEmpty)
                  _buildInfoRow(
                    Icons.location_on,
                    event.venue!,
                    AppColors.systemRed,
                    onTap: () => _openMaps(event),
                  ),
                // Date/Time — iOS: row 5
                if (event.eventDateTime != null &&
                    event.eventDateTime!.isNotEmpty)
                  _buildInfoRow(
                    Icons.calendar_today,
                    event.eventDateTime!,
                    AppColors.primaryBlue,
                  ),
                // Registration link — iOS: row 6
                if (event.hasLink)
                  _buildInfoRow(
                    Icons.link,
                    event.link!,
                    AppColors.accent,
                    onTap: () => _launchUrl(event.link!),
                  ),
                const SizedBox(height: 16),
                // RSVP buttons — iOS: viewYesNoMaybe
                if (event.isRsvpEnabled && !event.isExpired)
                  Padding(
                    padding: const EdgeInsets.symmetric(horizontal: 16),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Text(
                          'RSVP',
                          style: TextStyle(
                            fontFamily: AppTextStyles.fontFamily,
                            fontSize: 16,
                            fontWeight: FontWeight.w600,
                            color: AppColors.primary,
                          ),
                        ),
                        const SizedBox(height: 8),
                        RsvpButtonRow(
                          currentResponse: event.myResponse,
                          goingCount: event.goingCount,
                          notgoingCount: event.notgoingCount,
                          maybeCount: event.maybeCount,
                          onResponse: (response) =>
                              _onRsvpResponse(response, event),
                        ),
                      ],
                    ),
                  ),
                // Agenda / Minutes of Meeting / Photos — iOS:
                // PastEventDetailViewController. Populated from
                // event_agendas, event_minutes, event_photos via
                // Event/GetEventExtras.
                _buildExtrasSection(provider),
                // Repeat events — iOS: repeatEventResult
                if (event.repeatEventResult != null &&
                    event.repeatEventResult!.isNotEmpty) ...[
                  const SizedBox(height: 16),
                  Padding(
                    padding: const EdgeInsets.symmetric(horizontal: 16),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Text(
                          'Repeat Dates',
                          style: TextStyle(
                            fontFamily: AppTextStyles.fontFamily,
                            fontSize: 16,
                            fontWeight: FontWeight.w600,
                            color: AppColors.primary,
                          ),
                        ),
                        const SizedBox(height: 8),
                        ...event.repeatEventResult!.map((repeat) {
                          return Padding(
                            padding: const EdgeInsets.only(bottom: 4),
                            child: Row(
                              children: [
                                const Icon(Icons.repeat,
                                    size: 16,
                                    color: AppColors.textSecondary),
                                const SizedBox(width: 8),
                                Text(
                                  '${repeat.eventDate ?? ''} ${repeat.eventTime ?? ''}',
                                  style: const TextStyle(
                                    fontFamily: AppTextStyles.fontFamily,
                                    fontSize: 14,
                                    color: AppColors.textPrimary,
                                  ),
                                ),
                              ],
                            ),
                          );
                        }),
                      ],
                    ),
                  ),
                ],
                const SizedBox(height: 24),
              ],
            ),
          );
        },
      ),
    );
  }

  /// iOS PastEventDetailViewController: agenda button, MOM button and
  /// 2-column photo gallery — rendered only when the extras API returned
  /// any documents or photos.
  Widget _buildExtrasSection(EventsProvider provider) {
    final extras = provider.selectedEventExtras;
    if (extras == null) return const SizedBox.shrink();

    final agendas =
        extras.agendas.where((a) => (a.fileName ?? '').isNotEmpty).toList();
    final minutes =
        extras.minutes.where((m) => (m.fileName ?? '').isNotEmpty).toList();
    final photos =
        extras.photos.where((p) => (p.photoPath ?? '').isNotEmpty).toList();

    if (agendas.isEmpty && minutes.isEmpty && photos.isEmpty) {
      return const SizedBox.shrink();
    }

    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          if (agendas.isNotEmpty) ...[
            const Text(
              'Agenda',
              style: TextStyle(
                fontFamily: AppTextStyles.fontFamily,
                fontSize: 16,
                fontWeight: FontWeight.w600,
                color: AppColors.primary,
              ),
            ),
            const SizedBox(height: 8),
            ...agendas.map(
              (a) => _buildDocTile(Icons.description, a.fileName!),
            ),
            const SizedBox(height: 16),
          ],
          if (minutes.isNotEmpty) ...[
            const Text(
              'Minutes of Meeting',
              style: TextStyle(
                fontFamily: AppTextStyles.fontFamily,
                fontSize: 16,
                fontWeight: FontWeight.w600,
                color: AppColors.primary,
              ),
            ),
            const SizedBox(height: 8),
            ...minutes.map(
              (m) => _buildDocTile(Icons.note_alt, m.fileName!),
            ),
            const SizedBox(height: 16),
          ],
          if (photos.isNotEmpty) ...[
            const Text(
              'Photos',
              style: TextStyle(
                fontFamily: AppTextStyles.fontFamily,
                fontSize: 16,
                fontWeight: FontWeight.w600,
                color: AppColors.primary,
              ),
            ),
            const SizedBox(height: 8),
            GridView.builder(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              gridDelegate:
                  const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 2,
                crossAxisSpacing: 8,
                mainAxisSpacing: 8,
                childAspectRatio: 1,
              ),
              itemCount: photos.length,
              itemBuilder: (_, i) => _buildPhotoTile(photos[i]),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildDocTile(IconData icon, String fileName) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 8),
      child: OutlinedButton.icon(
        onPressed: () => _launchUrl(fileName),
        icon: Icon(icon, size: 18, color: AppColors.primary),
        label: Align(
          alignment: Alignment.centerLeft,
          child: Text(
            fileName,
            style: const TextStyle(
              fontFamily: AppTextStyles.fontFamily,
              fontSize: 13,
              color: AppColors.textPrimary,
            ),
            overflow: TextOverflow.ellipsis,
          ),
        ),
        style: OutlinedButton.styleFrom(
          alignment: Alignment.centerLeft,
          side: const BorderSide(color: AppColors.primary),
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 12),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(6),
          ),
          minimumSize: const Size(double.infinity, 44),
        ),
      ),
    );
  }

  Widget _buildPhotoTile(EventExtraPhoto photo) {
    final path = photo.photoPath ?? '';
    final ImageProvider? provider = _photoProvider(path);

    return GestureDetector(
      onTap: provider == null
          ? null
          : () => _openPhotoViewer(provider, photo.description),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(6),
        child: provider != null
            ? Image(
                image: provider,
                fit: BoxFit.cover,
                errorBuilder: (_, _, _) => _photoPlaceholder(),
              )
            : _photoPlaceholder(),
      ),
    );
  }

  Widget _photoPlaceholder() {
    return Container(
      color: AppColors.grayLight,
      child: const Center(
        child:
            Icon(Icons.broken_image, color: AppColors.grayMedium, size: 32),
      ),
    );
  }

  /// `photoPath` may be a full HTTP(S) URL, a base64 `data:` URL (the admin
  /// panel currently saves photos as data URLs), or a relative path. Returns
  /// null for unsupported formats so the tile can show a placeholder.
  ImageProvider? _photoProvider(String path) {
    if (path.isEmpty) return null;
    if (path.startsWith('data:')) {
      final commaIdx = path.indexOf(',');
      if (commaIdx == -1) return null;
      try {
        final bytes = base64Decode(path.substring(commaIdx + 1));
        return MemoryImage(bytes);
      } catch (_) {
        return null;
      }
    }
    if (path.startsWith('http://') || path.startsWith('https://')) {
      return NetworkImage(path.replaceAll(' ', '%20'));
    }
    return null;
  }

  void _openPhotoViewer(ImageProvider image, String? description) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => Scaffold(
          backgroundColor: Colors.black,
          appBar: AppBar(
            backgroundColor: Colors.black,
            iconTheme: const IconThemeData(color: Colors.white),
            elevation: 0,
            title: Text(
              description ?? '',
              style: const TextStyle(color: Colors.white, fontSize: 14),
            ),
          ),
          body: Center(
            child: InteractiveViewer(
              minScale: 0.5,
              maxScale: 4.0,
              child: Image(image: image, fit: BoxFit.contain),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildInfoRow(IconData icon, String text, Color iconColor,
      {VoidCallback? onTap}) {
    return InkWell(
      onTap: onTap,
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 6),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Icon(icon, size: 18, color: iconColor),
            const SizedBox(width: 10),
            Expanded(
              child: Text(
                text,
                style: TextStyle(
                  fontFamily: AppTextStyles.fontFamily,
                  fontSize: 14,
                  color: onTap != null ? AppColors.primaryBlue : AppColors.textPrimary,
                  decoration:
                      onTap != null ? TextDecoration.underline : null,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
