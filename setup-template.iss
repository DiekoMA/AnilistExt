#define AppVersion "0.0.1.0"

[Setup]
AppId=b5c71e57-cb39-46b2-91f7-bc94634785f5
AppName=Anilist
AppVersion={#AppVersion}
AppPublisher=DiekoMA
DefaultDirName={autopf}\Anilist
OutputDir=bin\Release\installer
OutputBaseFilename=Anilist-Setup-{#AppVersion}
Compression=lzma
SolidCompression=yes
MinVersion=10.0.19041

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "bin\Release\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\Anilist"; Filename: "{app}\AnilistExt.exe"

[Registry]
Root: HKCU; Subkey: "SOFTWARE\Classes\CLSID\b5c71e57-cb39-46b2-91f7-bc94634785f5"; ValueData: "Anilist"
Root: HKCU; Subkey: "SOFTWARE\Classes\CLSID\b5c71e57-cb39-46b2-91f7-bc94634785f5\LocalServer32"; ValueData: "{app}\AnilistExt.exe -RegisterProcessAsComServer"