#ifndef __Zebra_h_
#define __Zebra_h_
#include "zLogger.h"
#include "zProperties.h"
#include "zVersion.h"
namespace Zebra
{
	/**
	* \brief 游戏时间
	*
	*/
	extern volatile QWORD qwGameTime;

	/**
	* \brief 日志指针
	*
	*/
	extern zLogger *logger;

	/**
	* \brief 存取全局变量的容器
	*
	*/
	extern zProperties global;
};

#endif

