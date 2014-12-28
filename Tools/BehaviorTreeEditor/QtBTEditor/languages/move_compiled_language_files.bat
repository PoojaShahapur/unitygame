set CUR_DIR=%~dp0
cd /d %CUR_DIR%

copy *.qm ..\..\RunPath\languages
del *.qm
pause
