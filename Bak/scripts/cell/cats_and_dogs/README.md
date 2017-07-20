# 猫狗大战脚本代码说明

* 多数休闲游戏只需用Lua编写逻辑
* 一般应该集中在一个目录内，可以分子目录
* 猫狗大战是全区匹配的，所以匹配服和战斗服是不同的

## 文件列表

文件名						| 功能
----------------------------|-----------------
matcher.lua                 | 匹配器
on_matched.lua              | 匹配结果处理器
player.lua                  | 玩家
README.md                   | 本文
room.lua                    | 房间
room_mgr.lua                | 房间管理
svc_cats_and_dogs.lua       | 猫狗大战服务

## 实现步骤

建议: 先实现单服版本，然后再扩展成集群版本。

1. 定义协议
	1. cats_and_dogs.proto
	2. cats_and_dogs_push.proto
1. 实现服务器端的服务: svc_cats_and_dogs.lua
1. 设置服务的初始路由
	* 将服务请求转发到特定的功能服
		* 功能服是由功能名唯一标识的服务器进程
	* c_rpc_router.set_svc_function(服务名，功能名)
	* 也可以用 set_mthd_function() 设置方法的路由
1. 配匹逻辑在match.lua中实现
1. 战斗逻辑在room中实现
1. 每个房间有2个Player对象，实现为player.lua
1. 房间管理器(room_mgr.lua)实现房间的添加删除和查找
