/// Extras attached to an event: agenda files, minutes-of-meeting files, photos.
/// Matches iOS PastEventDetailViewController which shows agenda, MOM and
/// photo gallery for a past event (source tables on production:
/// event_agendas, event_minutes, event_photos).
///
/// API: POST Event/GetEventExtras { eventID } →
///   { status, agendas: [{ id, fileName }],
///              minutes: [{ id, fileName }],
///              photos:  [{ id, photoPath, description }] }
class EventExtrasResult {
  EventExtrasResult({
    this.status,
    this.agendas = const [],
    this.minutes = const [],
    this.photos = const [],
  });

  final String? status;
  final List<EventDocFile> agendas;
  final List<EventDocFile> minutes;
  final List<EventExtraPhoto> photos;

  factory EventExtrasResult.fromJson(Map<String, dynamic> json) {
    List<T> parseList<T>(
      dynamic raw,
      T Function(Map<String, dynamic>) fromJson,
    ) {
      if (raw is! List) return const [];
      return raw
          .whereType<Map<String, dynamic>>()
          .map(fromJson)
          .toList(growable: false);
    }

    return EventExtrasResult(
      status: json['status']?.toString(),
      agendas: parseList(json['agendas'], EventDocFile.fromJson),
      minutes: parseList(json['minutes'], EventDocFile.fromJson),
      photos: parseList(json['photos'], EventExtraPhoto.fromJson),
    );
  }
}

class EventDocFile {
  EventDocFile({this.id, this.fileName});

  final String? id;
  final String? fileName;

  factory EventDocFile.fromJson(Map<String, dynamic> json) {
    return EventDocFile(
      id: json['id']?.toString(),
      fileName: json['fileName']?.toString(),
    );
  }
}

class EventExtraPhoto {
  EventExtraPhoto({this.id, this.photoPath, this.description});

  final String? id;
  final String? photoPath;
  final String? description;

  factory EventExtraPhoto.fromJson(Map<String, dynamic> json) {
    return EventExtraPhoto(
      id: json['id']?.toString(),
      photoPath: json['photoPath']?.toString(),
      description: json['description']?.toString(),
    );
  }
}
