class MediaMovel {
  final double confirmed;
  final double deaths;

  MediaMovel({this.confirmed, this.deaths});

  factory MediaMovel.fromJson(Map<String, dynamic> json) {
    return MediaMovel(
        confirmed: json['confirmed'] / 1, deaths: json['deaths'] / 1);
  }
}
