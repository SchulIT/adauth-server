# Active Directory Authentication Server

[![Build Status](https://dev.azure.com/schulit/Active%20Directory%20Authentication%20Server/_apis/build/status/SchulIT.adauth-server?branchName=master)](https://dev.azure.com/schulit/Active%20Directory%20Authentication%20Server/_build/latest?definitionId=8&branchName=master)
![GitHub](https://img.shields.io/github/license/schulit/adauth-server?style=flat-square)
![.NET 6.0](https://img.shields.io/badge/.NET%206.0-brightgreen?style=flat-square)

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
    "username": "m.mustermann@example.de",
    "password": "secret-password"
}
```

Antwort (Anmeldung erfolgreich):

```json
{
    "success": true,
    "username": "m.mustermann@example.de",
    "firstname": "Max",
    "lastname": "Mustermann",
    "display_name": "Mustermann, Max",
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
    "ou": null,
    "groups": [ ]
}
```

### Passwort ändern

Mit dieser Funktion ist es möglich, dass der Benutzer das Passwort eigenständig ändert. Voraussetzung ist, dass das alte Passwort bekannt ist.

Anfrage:

```json
{
    "action": "change_password",
    "username": "m.mustermann@example.de",
    "old_password": "altesPW",
    "new_password": "neuesPW"
}
```

Antwort:

```json
{
    "success": true|false,
    "message": "Information über den Status der Anfrage (Fehlermeldung oder Erfolg)"
}
```

### Passwort zurücksetzen

Mit dieser Funktion kann ein Benutzer mit entsprechenden Rechten (s.u.) das Passwort eines Benutzers zurücksetzen.

Anfrage:

```json
{
    "action": "reset_password",
    "username": "m.mustermann@example.de",
    "new_password": "neuesPW",
    "admin_username": "resetpw@example.de",
    "admin_password": "yourSecretPassword"
}
```

Antwort:

```json
{
    "success": true|false,
    "message": "Information über den Status der Anfrage (Fehlermeldung oder Erfolg)"
}
```


# Installation & Konfiguration

## Schritt 1: Installation

Die Installationsdateien findet man unter [Releases](https://github.com/adauth-server/releases).

## Schritt 2: Active Directory Benutzer erstellen

Damit der Server die Daten aus dem Active Directory auslesen kann, muss ein **normaler** AD-Benutzer angelegt werden (kein Administrator o.ä.). OU und Gruppen-Mitgliedschaften sind egal. 

Möchte man auch Passwörter zurücksetzen können, wird ein Account mit entsprechender Berechtigung benötigt. Das sind u.a.
auch Domänenadmins, allerdings wird nicht empfohlen, diesen Account zum Zurücksetzen von Passwörtern zu verwenden.
Stattdessen sollte ein separater **normaler** Benutzer (kein Administrator o.ä.) angelegt werden (dessen 
Anmeldeinformationen auch an die Lehrkraft verteilt wird, die Passwörter zurücksetzen darf). Anschließend muss diesem 
Benutzer das Recht zum Zurücksetzen von Passwörtern erteilt werden. Dazu in "Active Directory Benutzer- und Computer" die OU
auswählen, in der Passwörter zurückgesetzt werden dürfen sollen: 

1. Rechtsklick auf die OU -> Aufgaben -> Objektverwaltung zuweisen
2. Den Benutzer auswählen, der Passwörter zurücksetzen darf
3. Aufgabe "Setzt Passwörter zurück und erzwingt Kennwortänderung bei der nächsten Anmeldung"
4. Bestätigen

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

Legt den Port des Servers fest (Standard: `55117`). Achtung: Unter Windows ist gehört dieser Port zu einer Liste blockierter Ports, die nach dem Update [KB4074588](https://support.microsoft.com/en-us/topic/february-13-2018-kb4074588-os-build-16299-248-b4e2ca66-dd7a-6fd5-a8f3-dc6683d4922b) entanden ist.

### tls.enabled

Legt fest, ob TLS aktiviert ist (Standard: `true`). Dieser Wert sollte in einem Produktivsystem auf `true` gesetzt sein.

### tls.cert

Pfad zur PFX-Datei (s.o.), welche das Zertifikat und den zugehörigen privaten Schlüssel enthält. 

### tls.psk

Passwort für die PFX-Datei.

### ldap.dc

Hostname des Domänencontrollers.

### ldap.domain_fqdn

Vollqualifizierter Domänenname

### ldap.domain_netbios

NetBIOS Name der Domäne.

### ldap.username

Benutzername des AD Benutzers für den AD Auth Server (s.o.).

### ldap.password

Passwort des AD Benutzers für den AD Auth Server (s.o.).

### ldap.ssl

Legt fest, ob LDAP over SSL verwendet werden soll.

### ldap.tls

Legt fest, ob LDAP mit STARTTLS verwendet werden soll (empfohlen). Dies wird benötigt, wenn auch Änderungen von Passwörtern vorgenommen werden sollen.

### ldap.certificate_thumbnail

Fingerabdruck des TLS-Zertifikats für SSL oder STARTTLS-Verbindungen.

### ldap.username_property

* `0`: Der UPN des Benutzers wird als Benutzername verwendet (empfohlen)
* `1`: Der sAMAccountName des Benutzers wird als Benutzername verwendet

### ldap.allowed_upn_suffixes

Eine Liste von UPN-Suffixen, für die der Server Anmeldungen akzeptiert (optional). Achtung: Diese Option ist (noch) ungetestet.

# Schritt 4: Server konfigurieren

Diehe Abschnitt "Konfigurationsdatei".

# Schritt 5: Dienst aktivieren

Der Dienst kann entweder über die GUI (Startmenü: SchulIT -> Active Directory Authentication Server Configuration Utility) aktiviert werden oder händisch unter "Dienste" in Windows.

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