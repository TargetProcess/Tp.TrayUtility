del /Q inst\*.*
rmdir inst
mkdir inst
copy .\bin\release\*.dll inst\*.dll
copy .\bin\release\*.exe inst\*.exe
copy tptray.nsi inst\*.*
copy bugLarge.ico inst\*.*
copy unbug.ico inst\*.*
copy orange.bmp inst\*.*
copy orange2.bmp inst\*.*
copy license.rtf inst\*.*
copy *.msi inst\*.*

"c:\program files\nsis\makensis.exe" inst\tptray.nsi
