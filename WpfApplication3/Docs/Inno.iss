; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0B21FC76-AC37-41E4-869C-65894F2973B1}
AppName=金字塔财务管理工具
AppVersion=2.2
;AppVerName=金字塔财务管理工具 2.2
AppPublisher=StoneAnt, Inc.
DefaultDirName={pf}\金字塔财务管理工具
DefaultGroupName=金字塔财务管理工具
AllowNoIcons=yes
OutputBaseFilename=金字塔财务管理工具
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
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\金字塔.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\Data\Data.db"; DestDir: "{app}\\Data"; Flags: ignoreversion
Source: "D:\WorkSpaces\VisualStudio\WPF\WpfApplication3\bin\Release\Data\templt.xls"; DestDir: "{app}\\Data"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\金字塔财务管理工具"; Filename: "{app}\金字塔.exe"
Name: "{commondesktop}\金字塔财务管理工具"; Filename: "{app}\金字塔.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\金字塔财务管理工具"; Filename: "{app}\金字塔.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\金字塔.exe"; Description: "{cm:LaunchProgram,金字塔财务管理工具}"; Flags: nowait postinstall skipifsilent

