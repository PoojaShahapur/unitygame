cd %~dp0
%~d0

ProtoGenExe\protogen.exe --proto_path=. -i:..\Msg\Proto\User.proto -o:..\Msg\Cs\User.cs
ProtoGenExe\protoc.exe --proto_path=. -o ..\Msg\Pb\User.pb ..\Msg\Proto\User.proto

pause