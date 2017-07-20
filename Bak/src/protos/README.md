# server proto

服务器内部消息与服务定义。

更改proto文件后，请运行generate.bat生成代码到, 
生成代码在 [../svr_common/pb/svr](../svr_common/pb/svr).

客户端与服务器之间的协议定义在 SVN Common/proto/, 有另外的生成脚本。
Common 需从公司 SVN 获取：

    https://192.168.150.238/svn/GiantCode/TechDept/Future/Common

应该是一个服务一个proto文件，这样生成的代码也是按服务分开的。
文件名为服务名, 如服务名 TestCpp, 文件名为 test_cpp.proto。

内部协议都在svr名字空间中。

按功能组织子目录。package名字也要带上其子目录名。
