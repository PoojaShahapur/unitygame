cd %~dp0
%~d0

..\Tools\ProtoGenExe\protogen.exe -i:Proto\Test.proto -o:Cs\Test.cs
..\Tools\ProtoGenExe\protoc.exe Proto\Test.proto -o Pb\Test.pb
..\Tools\ProtoGenExe\protoc.exe Proto\login.proto -o Pb\login.pb
..\Tools\ProtoGenExe\protoc.exe Proto\rpc.proto -o Pb\rpc.pb

pause