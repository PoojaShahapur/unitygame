cd %~dp0
%~d0

ProtoGen\protogen.exe -i:..\Msg\Proto\User.proto -o:..\Msg\Cs\User.cs
ProtoGen\protoc.exe --proto_path=. -o ..\Msg\Pb\User.pb ..\Msg\Proto\User.proto

pause