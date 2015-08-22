#ifndef _NETTYPE_H_
#define _NETTYPE_H_
enum NetType
{
	NetType_near = 0,		//近端路由，电信区连接电信，网通区连接网通
	NetType_far = 1			//远端路由，电信区连接网通，网通区连接电信
};
#endif

