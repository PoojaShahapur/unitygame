REM Generate server protobuf cpp files to svr_common/pb/svr/.
REM Generate descriptors to scripts/pb/descriptors_svr.pb.

set PROTOC=..\..\deps\bin\protoc-3.2.0-win32\bin\protoc.exe
%PROTOC% -I. --cpp_out=..\svr_common\pb\svr ^
  --descriptor_set_out=..\..\scripts\pb\descriptors_svr.pb ^
  *.proto
  
pause
  