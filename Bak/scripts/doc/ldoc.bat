REM Generate lua document.

REM Remove old html files.
rmdir /S /Q html

set LUA=..\..\deps\bin\lua53pp\Release\lua53pp.exe
set LDOC=..\..\tools\lua\LDoc-1.4.6\ldoc.lua
%LUA% -e "package.cpath = '../lualibs/?.dll;'..package.cpath" ^
      -e "package.path = '../../tools/lua/Penlight-1.5.2/lua/?.lua;'..package.path" ^
      %LDOC% .

pause
