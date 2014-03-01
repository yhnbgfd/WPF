; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0B21FC76-AC37-41E4-869C-65894F2973B1}
AppName=�����������������
AppVersion=2.2
;AppVerName=����������������� 2.2
AppPublisher=StoneAnt, Inc.
DefaultDirName={pf}\�����������������
DefaultGroupName=�����������������
AllowNoIcons=yes
OutputBaseFilename=�����������������
SetupIconFile=D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\Resources\Pyramid_Logo_white_128x128.ico
Password=123123
Compression=lzma
SolidCompression=yes

[Languages]
Name: "chinese"; MessagesFile: "compiler:Languages\Chinese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\������.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\Data\Data.db"; DestDir: "{app}\\Data"; Flags: ignoreversion
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\Data\templt.xls"; DestDir: "{app}\\Data"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\�����������������"; Filename: "{app}\������.exe"
Name: "{commondesktop}\�����������������"; Filename: "{app}\������.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\�����������������"; Filename: "{app}\������.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\������.exe"; Description: "{cm:LaunchProgram,�����������������}"; Flags: nowait postinstall skipifsilent
