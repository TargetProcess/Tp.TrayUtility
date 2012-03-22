
; version used later (e.g. for creating output file name)
!ifndef VERSION
  !define VERSION '1.0.0.0'
!endif

!ifndef CONFIGURATION
  !define CONFIGURATION 'Release'
!endif

;definition of output file
!ifdef OUTFILE
  OutFile "${OUTFILE}"
!else
  OutFile TPTRAY-${VERSION}-setup.exe
!endif

;product name
!define PRODUCT_NAME 'TP.Tray'
Name "TP.Tray"

InstallDir "$PROGRAMFILES\TargetProcess\${PRODUCT_NAME}"

ReserveFile "bin\${CONFIGURATION}\TPTray.exe"


; use modern user interface
!include "MUI.nsh"

; if something wrong with installation process than user can abort it
!define MUI_ABORTWARNING

; use TP icons instaed of standard
!define MUI_ICON  bugLarge.ico
!define MUI_UNICON unbug.ico

; we need custom pictures during installation process
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP orange.bmp
!define MUI_HEADERIMAGE_UNBITMAP orange.bmp
!define MUI_WELCOMEFINISHPAGE_BITMAP orange2.bmp
!define MUI_UNWELCOMEFINISHPAGE_BITMAP orange2.bmp

; installation scenarios begins from standard wellcome page
!insertmacro MUI_PAGE_WELCOME
; license page.
!insertmacro MUI_PAGE_LICENSE "license.rtf"
!insertmacro MUI_PAGE_DIRECTORY

!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN $INSTDIR\TPTray.exe
!define MUI_FINISHPAGE_SHOWREADME
!define MUI_FINISHPAGE_SHOWREADME_TEXT "Run Tp.Tray automatically on system start." 
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION AutoStartUp
!insertmacro MUI_PAGE_FINISH

; scenario for de-installation
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; installation langugage
!insertmacro MUI_LANGUAGE "English"

!include LogicLib.nsh
!include "FileFunc.nsh"
!insertmacro DirState
!insertmacro un.DirState


; Installation section called with 'Page instfiles'
Section "Install"
	; all files should installed on $INSTDIR
	SetOutPath $INSTDIR

	File "/oname=$PLUGINSDIR\TPTray.exe" "bin\${CONFIGURATION}\TPTray.exe"
	nsExec::Exec /TIMEOUT=20000 "$PLUGINSDIR\TPTray.exe -kill"

	; add all files to a package
	File /r bin\${CONFIGURATION}\*.exe
	File /r bin\${CONFIGURATION}\*.dll
	File /r /x _svn IncludeFiles\*.*
	
	; updating registry
	; write uninstall information (reqired to uninstall TP through Control Panel)
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TP.Tray" "DisplayName" "${PRODUCT_NAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TP.Tray" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TP.Tray" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TP.Tray" "NoRepair" 1

	; uninstaller
	WriteUninstaller "uninstall.exe"
	nsExec::Exec /TIMEOUT=20000 'msiexec /package "$INSTDIR\Microsoft WSE 3.0 Runtime.msi" /quiet'
	call CreateProgramFilesFolder
	delete "$INSTDIR\Microsoft WSE 3.0 Runtime.msi"
SectionEnd

; uninstall section
Section "Uninstall"
	; remove records in registry
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TP.Tray"

; remove TP folder with all files
	nsExec::Exec /TIMEOUT=20000 "$INSTDIR\TPTray.exe -kill"
	RMDir /r "$INSTDIR"
	; remove Start->Programs->TP
	RMDir /r "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}"

	Var /GLOBAL dirState
	${un.DirState} "$SMPROGRAMS\TargetProcess" $dirState
	${If} $dirState == 0
		RMDir /r "$SMPROGRAMS\TargetProcess"
	${EndIf}

	SetShellVarContext All
	Delete "$SMSTARTUP\TP.Tray.lnk"
SectionEnd

; Creates Start->Program->TP and places some URLs there.
Function CreateProgramFilesFolder
	CreateDirectory "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}"
	WriteINIStr "$SMPROGRAMS\${PRODUCT_NAME}\TP.Tray.url" "InternetShortcut" "URL" "http://www.targetprocess.com"
	CreateShortCut "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\TP.Tray.lnk" "$INSTDIR\TPTray.exe"
	CreateShortCut "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
FunctionEnd

Function AutoStartUp
	SetShellVarContext All
	CreateShortCut "$SMSTARTUP\TP.Tray.lnk" "$INSTDIR\TPTray.exe"
FunctionEnd




