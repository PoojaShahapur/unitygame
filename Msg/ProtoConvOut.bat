cd %~dp0
%~d0

..\Tools\ProtoGenExe\protogen.exe -i:Proto\User.proto -o:Cs\User.cs
..\Tools\ProtoGenExe\protoc.exe Proto\User.proto -o Pb\User.pb

pause