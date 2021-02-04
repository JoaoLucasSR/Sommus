import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:sommus_mobile/Model/MediaMovel.dart';
import 'package:intl/intl.dart';
import 'dart:developer';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, this.title}) : super(key: key);
  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  Future<MediaMovel> mediaMovel1;
  Future<MediaMovel> mediaMovel2;
  Future<MediaMovel> mediaMovel3;

  Future<MediaMovel> fetchMediaMovel(int ordem) async {
    http.Response response;
    if (ordem == 1) {
      response = await http.get('http://10.0.2.2:5000/api/MediaMovel?date=' +
          DateFormat("yyyy-MM-dd'T00:00:00Z'").format(DateTime.now()));
    } else if (ordem == 2) {
      response = await http.get('http://10.0.2.2:5000/api/MediaMovel?date=' +
          DateFormat("yyyy-MM-dd'T00:00:00Z'")
              .format(DateTime.now().subtract(Duration(days: 7))));
    } else {
      response = await http.get('http://10.0.2.2:5000/api/MediaMovel?date=' +
          DateFormat("yyyy-MM-dd'T00:00:00Z'")
              .format(DateTime.now().subtract(Duration(days: 14))));
    }

    if (response.statusCode == 200) {
      log(response.body);
      return MediaMovel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('Falha na requisição');
    }
  }

  @override
  void initState() {
    super.initState();
    mediaMovel1 = fetchMediaMovel(1);
    mediaMovel2 = fetchMediaMovel(2);
    mediaMovel3 = fetchMediaMovel(3);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            FutureBuilder<MediaMovel>(
              future: mediaMovel1,
              builder: (context, snapshot) {
                if (snapshot.hasData) {
                  return Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text("Esta Semana"),
                      Text(
                          "Confirmados: " + snapshot.data.confirmed.toString()),
                      Text("Mortos: " + snapshot.data.deaths.toString()),
                    ],
                  );
                } else if (snapshot.hasError) {
                  return Text("${snapshot.error}");
                }

                return CircularProgressIndicator();
              },
            ),
            SizedBox(
              height: 15,
            ),
            FutureBuilder<MediaMovel>(
              future: mediaMovel2,
              builder: (context, snapshot) {
                if (snapshot.hasData) {
                  return Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text("Semana Passada"),
                      Text(
                          "Confirmados: " + snapshot.data.confirmed.toString()),
                      Text("Mortos: " + snapshot.data.deaths.toString()),
                    ],
                  );
                } else if (snapshot.hasError) {
                  return Text("${snapshot.error}");
                }

                return CircularProgressIndicator();
              },
            ),
            SizedBox(
              height: 15,
            ),
            FutureBuilder<MediaMovel>(
              future: mediaMovel3,
              builder: (context, snapshot) {
                if (snapshot.hasData) {
                  return Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text("Semana Retrasada"),
                      Text(
                          "Confirmados: " + snapshot.data.confirmed.toString()),
                      Text("Mortos: " + snapshot.data.deaths.toString()),
                    ],
                  );
                } else if (snapshot.hasError) {
                  return Text("${snapshot.error}");
                }

                return CircularProgressIndicator();
              },
            ),
          ],
        ),
      ),
    );
  }
}
