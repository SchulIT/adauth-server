# AD Auth Server

Mithilfe des Active Directory Authentication Server (AD Auth Server) kann eine Authentifizierung mithilfe eines lokalen Active Directories stattfinden. Der Server wird für den Identity Provider benötigt, um eine Anmeldung mit den Anmeldedaten aus dem Schulnetzwerk zu ermöglichen.

# Protokoll

## Ping-Anfrage

Anfrage:

```json
{
    "action": "ping"
}
```

Antwort:

```json
{
    "success": true
}
```

## Anmeldung

Anfrage:

```json
{
    "action": "auth",
    "username": "m.mustermann",
    "password": "secret-password"
}

Antwort (Anmeldung erfolgreich):

```json
{
    "success": true,
    "username": "m.mustermann",
    "firstname": "Max",
    "lastname": "Mustermann",
    "display_name": "Mustermann, Max",
    "unique_id": "1",
    "ou": "OU=Students,DC=schulit,DC=lokal",
    "groups": [
        "Students",
        "8A"
    ]
}
```

Antwort (Anmeldung nicht erfolgreich):

```json
{
    "success": false,
    "username": null,
    "firstname": null,
    "lastname": null,
    "display_name": null,
    "external_id": null,
    "ou": null,
    "groups": [ ]
}
```

# Installation & Konfiguration

## Schritt 1: Installation

Die Installationsdateien findet man unter [Releases](https://github.com/adauth-server/releases).

## Schritt 2: Active Directory Benutzer erstellen

Damit der Server die Daten aus dem Active Directory auslesen kann, muss ein **normaler** AD-Benutzer angelegt werden (kein Administrator o.ä.). OU und Gruppen-Mitgliedschaften sind egal. 

## Schritt 3: TLS-Zertifikat erstellen

Um die Verbindung zwischen Client und dem Server zu verschlüsseln, sollte man TLS im Server aktivieren. Dazu benötigt es jedoch ein Zertifikat, welches man entweder mit OpenSSL oder ADCS erzeugen kann (im Grunde geht aber auch jede andere Möglichkeit).

### OpenSSL (selbstsigniert)

Folgendes Kommando erzeugt ein selbst-signiertes Zertifikat mithilfe von OpenSSL. Das Zertifikat ist 10 Jahre gültig. Die "PEM pass phrase" sollte leer gelassen werden.

    $ openssl genrsa -out private.pem 4096
    $ openssl req -x509 -new -key private.pem -out public.pem -days 3650

Beispiel:

```PS
PS C:\ProgramData\SchulIT\AD Auth Server> openssl genrsa -out private.pem 4096
Generating RSA private key, 4096 bit long modulus (2 primes)
................................++++
........................++++
e is 65537 (0x010001)
PS C:\ProgramData\SchulIT\AD Auth Server> openssl req -x509 -new -key private.pem -out public.pem -days 3650
You are about to be asked to enter information that will be incorporated
into your certificate request.
What you are about to enter is what is called a Distinguished Name or a DN.
There are quite a few fields but you can leave some blank
For some fields there will be a default value,
If you enter '.', the field will be left blank.
-----
Country Name (2 letter code) [AU]:DE
State or Province Name (full name) [Some-State]:Nordrhein-Westfalen
Locality Name (eg, city) []:Aachen
Organization Name (eg, company) [Internet Widgits Pty Ltd]:SchulIT
Organizational Unit Name (eg, section) []:IT
Common Name (e.g. server FQDN or YOUR name) []:dc01.schulit.lokal
Email Address []:it@schulit.lokal
```

**Info:** Die eingetragenen Werte sollten nach eigenem Belieben angepasst werden (die Werte sind im Grunde egal). Es empfiehlt sich, als `Common Name` den Computernamen (samt Domäne) einzutragen.

Anschließend muss noch eine PFX-Datei erstellt werden, die der Server einlesen kann. Sie enthält den privaten Schlüssel sowie das Zertifiakt:

    $ openssl pkcs12 -export -in public.pem -inkey private.pem -out server.pfx

Das Kommando fragt ein `Export Password` ab. Dieses wird genutzt, um das Zertifikat und den privaten Schlüssel in der PFX-Datei zu verschlüsseln. Es muss später auch in der Konfigurationsdatei hinterlegt werden.

**Anschließend die Datei `private.pem` löschen!** Die Datei `public.pem` kann, muss aber nicht gelöscht werden.

Die Datei `server.pfx` kann in einem beliebigen Verzeichnis liegen. Es empfiehlt sich, sie im Verzeichnis `C:\ProgramData\SchulIT\AD Auth Server` abzulegen. 

### Active Directory Certificate Services (ADCS)

TODO :-)

## Server konfigurieren

Die Konfiguration des Servers wird in der Datei `settings.json` vorgenommen, welche sich im Verzeichnis `C:\ProgramData\SchulIT\AD Auth Server` befindet. Alternativ kann die Konfiguration über die GUI vorgenommen werden (Startmenü: AD Auth Server > AD Auth Server GUI).

### server.ipv6

Ist dieser Wert auf `true`, so lauscht der Server nur auf IPv6-Anfragen. Ist er `false`, so lauscht er auf IPv4-Anfragen (Standard).

### server.port

Legt den Port des Servers fest (Standard: `55117`).

### tls.enabled

Legt fest, ob TLS aktiviert ist (Standard: `false`). Dieser Wert sollte in einem Produktivsystem auf `true` gesetzt sein.

### tls.cert

Pfad zur PFX-Datei (s.o.), welche das Zertifikat und den zugehörigen privaten Schlüssel enthält. 

### tls.psk

Passwort für die PFX-Datei.

### ldap.dc

Hostname des Domänencontrollers.

### ldap.domain

Name der Domäne.

### ldap.username

Benutzername des AD Benutzers für den AD Auth Server (s.o.).

### ldap.password

Passwort des AD Benutzers für den AD Auth Server (s.o.).

### unique_id

Attributname, in der eine eindeutige ID gespeichert ist. Diese kommt in der Regel aus der Schulverwaltung und wird benötigt, wenn SchulIT Dienste Benutzer und Schüler bzw. Lehrer erkennt. Falls dies nicht benötigt wird, kann der Wert auf `null` gesetzt werden.

# Schritt 4: Server konfigurieren

Diehe Abschnitt "Konfigurationsdatei".

# Schritt 5: Dienst aktivieren

Der Dienst kann entweder über die GUI (Startmenü: AD Auth Server > AD Auth Server GUI) aktiviert werden oder händisch unter "Dienste" in Windows.

# Schritt 6: Port-Weiterleitung einrichten

Zu guter letzt muss noch die Port-Weiterleitung im Router eingerichtet werden.

# Sicherheit

Um die Sicherheit des Servers zu erhöhen, sollten folgende Dinge beachtet werden.

## Konfigurationsdatei

Die Konfigurationsdatei sollte so mit Berechtigungen ausgestattet werden, dass die Datei nur von Administratoren geändert und gelesen werden kann. 

## Netzwerk

Der Server implementiert keine Maßnahmen, um DDos-Attacken zu verhindern. Dies sollte auf Netzwerk-Ebene geschehen.

Als ersten Schritt sollte die Port-Weiterleitung im Router so angelegt werden, dass der Port nur für die IP-Adresse(n) des Servers freigeschaltet ist, der auch tatsächlich Anfragen an den Server stellen darf.

# Client-Bibliotheken

Aktuell gibt es eine [Client-Bibliothek für PHP](https://github.com/schulit/adauth) sowie das zugehörige [Bundle für Symfony](https://github.com/schulit/adauth-bundle).

# Lizenz

Der Quelltext steht (abgesehen von den [Icons](ICONS_LICENSE.md)) unter der [MIT License](LICENSE.md).