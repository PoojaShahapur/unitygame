Vs调用luacheck静态检查

参见[金庆的专栏](http://blog.csdn.net/jq0123/article/details/60870390)

1. 安装lua和lfs
	1. 将deps/bin/lua53pp/ 复制到 d:/tools/lua53pp
	1. 复制 scripts\lualibs\lfs.dll 到 d:/tools/lua53pp/Release
	1. 然后使用 d:/tools/lua53pp/Release/lua53pp.exe.

1. 安装luacheck
	1. 下载 luacheck-0.19.0
	2. 解压复制到 d:/tools/luacheck-0.19.0
	3. 运行 install.lua <path>
	
		d:\tools\luacheck-0.19.0>d:\tools\lua53pp\Release\lua53pp.exe install.lua d:\tools\luacheck

1. VS 添加外部工具调用
	* 工具->外部工具->添加
	* 标题：luacheck
	* 命令：d:\tools\luacheck\bin\luacheck.bat
	* 参数：$(SolutionDir) --formatter luacheck.format_vs
	* 初始目录：$(SolutionDir)
	* 选择“使用输出窗口”

1. 更改luacheck输出格式，可以双击定位到出错行。

	luacheck\format.lua 复制为 luacheck\format_vs.lua, 如下更改。

	<pre>
	-   local res = ("%s:%d:%d"):format(file, location.line, location.column)
	+   local res = ("%s(%d):%d"):format(file, location.line, location.column)
	</pre><pre>
	- return format
	+ return formatters.plain
	</pre>

1. 添加配置 .luacheckrc, 示例：


	exclude_files = {
		"lualibs/**/*",
	}
	
	globals = {
	}
	
	read_globals = {
		'c_log',
	}
