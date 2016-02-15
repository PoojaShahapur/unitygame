%~d0

..\Tools\ProtoGen\protogen.exe -i:Cmd\msg_enum.proto -o:..\Client\msg_enum.cs
..\Tools\protoc.exe --proto_path=Cmd -o ..\Client\BaseType.pb Cmd\BaseType.proto