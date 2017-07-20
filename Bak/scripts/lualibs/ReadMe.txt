本目录是Lua扩展库。

lualibs在base/init.lua 和 cell/init.lua 中添加到 package.path。

mobdebug.lua
	调试
serpent.lua
	Lua serializer and pretty printer.
	https://github.com/pkulchenko/serpent
protobuf.lua
protobuf.dll .so
	google protocol buffers library
	https://github.com/cloudwu/pbc
	Build from deps\pbc\binding\lua53.

lfs.dll, lfs.so
	luafilesystem
	https://github.com/keplerproject/luafilesystem.git
	deps/lualibs/luafilesystem_...

hotfix
	Lua 热更新
	Lua 5.2/5.3 hotfix. Hot update functions and keep old data.
	https://github.com/jinq0123/hotfix
