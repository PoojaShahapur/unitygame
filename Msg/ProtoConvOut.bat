cd %~dp0
%~d0

..\Tools\ProtoGenExe\protogen.exe -i:Proto\User.proto -o:Cs\User.cs -ns:MyProtoBuf
..\Tools\ProtoGenExe\protoc.exe --proto_path=Proto Proto\User.proto -o Pb\User.pb

pause