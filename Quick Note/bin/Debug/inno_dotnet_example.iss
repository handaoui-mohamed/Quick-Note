;Example .NET Setup Script
;written in Inno Setup 5.1.9 (ISPP 5.1.8.0)
;2008 Matthew Kleinwaks
;www.ZerosAndTheOne.com
;change these comments as needed for your own app

#define IncludeFramework true
#define IsExternal ""
#define SourceFileDir "c:\work\myapp"

[setup]
;name of your application
AppName=MyApp
;repeat name of application. (otherwise you get
;multiple entries in add/remove programs)
AppVerName=MyApp
;app publisher name
AppPublisher=zerosandtheone.com
;app publisher website URL
AppPublisherURL=http://www.zerosandtheone.com
;app publisher support URL
AppSupportURL=http://www.zerosandtheone.com/support
;app publisher updates URL
AppUpdatesURL=http://www.zerosandtheone.com
;default directory {pf} is a constant for
;program files. See INNO help for all constants
DefaultDirName={pf}\zerosandtheone\MyApp
;default group name in the programs
; section of the start menu
DefaultGroupName=MyApp
;Boolean to disable allowing user to customize
;start menu entry during installation
DisableProgramGroupPage=yes
;Boolean to warn if directory user picks
;already exists
DirExistsWarning=no
;directory where uninstaller exe will be
;this will be where our app is
;the constant we use is {app}
UninstallFilesDir={app}
;Location of the license file
LicenseFile={#SourceFileDir}\eula.rtf
;file to show before install (I show sys requirements)
InfoBeforeFile={#SourceFileDir}\sysreq.rtf
;file to show after install (I show readme)
InfoAfterFile={#SourceFileDir} eadme.txt
;Custom image to show on left side of installer
WizardImageFile={#SourceFileDir}installlogo.bmp
;Icon for uninstall in add/remove programs
;I use whatever my apps icon is
UninstallDisplayIcon={app}\MyApp.exe
;Version number of your installer (not your app)
VersionInfoVersion=1.0.0.0
;If IncludeFramework, append _FW to end of compiled setup;
I do this to make it easy to compile a version with and
;without the framework included
#if IncludeFramework
  OutputBaseFilename=setup_FW
#else
  OutputBaseFilename=Setup
#endif
;Directory where setup.exe will be compiled to
OutputDir={#SourceFileDir}\setup

[files]
Source: {#SourceFileDir}\MyApp.exe;      DestDir: {app}; Flags: ignoreversion {#IsExternal}
source: {#SupportFilesDir}readme.txt;    DestDir: {app}; Flags: ignoreversion {#IsExternal}
#if IncludeFramework
  Source: {#SourceFileDir}\dotnetfx.exe; DestDir: {tmp}; Flags: ignoreversion {#IsExternal} ; Check: NeedsFramework
#endif

[icons]
Name: {group}\MyApp;        Filename: {app}\MyApp.exe; WorkingDir: {app}
Name: {group}\Remove MyApp; Filename: {uninstallexe};  WorkingDir: {app}
Name: {userdesktop}\MyApp;  Filename: {app}\MyApp.exe; WorkingDir: {app}

[Run]
#if IncludeFramework
  Filename: {tmp}\dotnetfx.exe; Parameters: "/q:a /c:""install /l /q"""; WorkingDir: {tmp}; Flags: skipifdoesntexist; StatusMsg: "Installing .NET Framework if needed"
#endif
Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -remgroup ""MyApp"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "Setting Program Access Permissions..."
Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -addgroup 1.2 -url ""file://{app}/*"" FullTrust -name ""MyApp"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "Setting Program Access Permissions..."

[UninstallRun]
Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -remgroup ""MyApp"""; Flags: skipifdoesntexist runhidden;

[code]

// Indicates whether .NET Framework 2.0 is installed.
function IsDotNET20Detected(): boolean;
var
    success: boolean;
    install: cardinal;
begin
    success := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727', 'Install', install);
    Result := success and (install = 1);
end;

//RETURNS OPPOSITE OF IsDotNet20Detected FUNCTION
//Remember this method from the Files section above
function NeedsFramework(): Boolean;
begin
  Result := (IsDotNET20Detected = false);
end;

//CHECKS TO SEE IF CLIENT MACHINE IS WINDOWS 95
function IsWin95 : boolean;
begin
  Result := (InstallOnThisVersion('4.0,0', '4.1.1998,0') = irInstall);
end;

//CHECKS TO SEE IF CLIENT MACHINE IS WINDOWS NT4
function IsWinNT : boolean;
begin
  Result := (InstallOnThisVersion('0,4.0.1381', '0,4.0.1381') = irInstall);
end;

//GETS VERSION OF IE INSTALLED ON CLIENT MACHINE
function GetIEVersion : String;
var
  IE_VER: String;
begin
  {First check if Internet Explorer is installed}
  if RegQueryStringValue(HKLM,'SOFTWARE\Microsoft\Internet Explorer','Version',IE_VER) then
      Result := IE_VER
else
    {No Internet Explorer at all}
    result := '';
end;

//GETS THE VERSION OF WINDOWS INSTALLER DLL
function GetMSIVersion(): String;
begin
    GetVersionNumbersString(GetSystemDir+'\msi.dll', Result);
end;

//LAUNCH DEFAULT BROWSER TO WINDOWS UPDATE WEBSITE
procedure GoToWindowsUpdate;
var
  ErrorCode: Integer;
begin
  if (MsgBox('Would you like to go to the Windows Update site now?' + chr(13) + chr(13) + '(Requires Internet Connection)'
            , mbConfirmation, MB_YESNO) = IDYES) then
      ShellExec('open', 'http://windowsupdate.microsoft.com','', '', SW_SHOW, ewNoWait, ErrorCode);
end;

//IF SETUP FINISHES WITH EXIT CODE OF 0, MEANING ALL WENT WELL
//THEN CHECK FOR THE PRESENCE OF THE REGISTRY FLAG TO INDICATE THE
//.NET FRAMEWORK WAS INSTALLED CORRECTLY
//IT CAN FAIL WHEN CUST DOESN'T HAVE CORRECT WINDOWS INSTALLER VERSION
function GetCustomSetupExitCode(): Integer;
begin
  if (IsDotNET20Detected = false) then
    begin
      MsgBox('.NET Framework was NOT installed successfully!',mbError, MB_OK);
      result := -1
    end
end;

function InitializeSetup: Boolean;
var
  Version: TWindowsVersion;
  IE_VER: String;
  MSI_VER: String;
  NL: Char;
  NL2: String;
begin

  NL := Chr(13);
  NL2 := NL + NL;

  // Get Version of Windows from API Call
  GetWindowsVersionEx(Version);

  // On Windows 2000, check for SP3

  if Version.NTPlatform and
     (Version.Major = 5) and
     (Version.Minor = 0) and
     (Version.ServicePackMajor < 3) then
  begin
    SuppressibleMsgBox('When running on Windows 2000, Service Pack 3 is required.' + NL +
                       'Visit' + NL2 +
                       ' *** http://windowsupdate.microsoft.com ***' + NL2 +
                       'to get the needed Windows Updates,' + NL +
                       'and then reinstall this program',
                        mbCriticalError, MB_OK, MB_OK);
    GoToWindowsUpdate;
    Result := False;
    Exit;
  end;

  // On Windows XP, check for SP2
  if Version.NTPlatform and
     (Version.Major = 5) and
     (Version.Minor = 1) and
     (Version.ServicePackMajor < 2) then
  begin
    SuppressibleMsgBox('When running on Windows XP, Service Pack 2 is required.' + NL +
                       'Visit' + NL2 + ' *** http://windowsupdate.microsoft.com ***' + NL2 +
                       'to get the needed Windows Updates,' + NL +
                       'and then reinstall this program',
                       mbCriticalError, MB_OK, MB_OK);
    GoToWindowsUpdate;
    Result := False;
    Exit;
  end;

  //IF WINDOWS 95 OR NT DON'T INSTALL
  if (IsWin95) or (IsWinNT) then
  begin
    SuppressibleMsgBox('This program can not run on Windows 95 or Windows NT.',
      mbCriticalError, MB_OK, MB_OK);
    Result := False;
    Exit;
  end;

  //CHECK MSI VER, NEEDS TO BE 3.0 ON ALL SUPPORTED SYSTEM EXCEPT 95/ME, WHICH NEEDS 2.0)
  MSI_VER := GetMSIVersion
  if ((Version.NTPlatform) and (MSI_VER < '3')) or ((Not Version.NTPlatform) and (MSI_VER < '2')) then
    begin
      SuppressibleMsgBox('You do not have all the required Windows Updates to install this program.' + NL +
                         'Visit *** http://windowsupdate.microsoft.com *** to get the needed Windows Updates,' + NL +
                         'and then reinstall this program',
                         mbCriticalError, MB_OK, MB_OK);
      GoToWindowsUpdate;
      Result := False;
      Exit;
  end;

  //CHECK THE IE VERSION (NEEDS TO BE 5.01+)
  IE_VER := GetIEVersion;
  if IE_VER < '5.01' then
    begin
      if IE_VER = '' then
        begin
          MsgBox('Microsoft Internet Explorer 5.01 or higher is required to run this program.' + NL2 +
                 'You do not currently have Microsoft Internet Explorer installed, or it is not working correctly.' + NL +
                 'Obtain a newer version at www.microsoft.com and then run setup again.', mbInformation, MB_OK);
        end
      else
        begin
          MsgBox('Microsoft Internet Explorer 5.01 or higher is required to run this program.' + NL2 +
                 'You are using version ' + IE_VER + '.' + NL2 +
                 'Obtain a newer version at www.microsoft.com and then run setup again.', mbInformation, MB_OK);
        end

      GoToWindowsUpdate;
      result := false;
      exit;
  end;

  //MAKE SURE USER HAS ADMIN RIGHTS BEFORE INSTALLING
  if IsAdminLoggedOn then
    begin
      result := true
        exit;
    end
  else
    begin
      MsgBox('You must have admin rights to perform this installation.' + NL +
             'Please log on with an account that has administrative rights,' + NL +
            'and run this installation again.', mbInformation, MB_OK);
        result := false;
    end
  end;
end.
