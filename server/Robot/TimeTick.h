#ifndef __TIMETICK_H_
#define __TIMETICK_H_

#include <iostream>
#include <string>

#include "Zebra.h"
#include "zThread.h"
#include "zTime.h"


/**
* \brief 网关定时器线程
*
*/
class ClientTimeTick : public zThread
{

public:

	static zRTime currentTime;

	~ClientTimeTick() {};

	/**
	* \brief 得到唯一实例
	*
	*
	* \return 唯一实例
	*/
	static ClientTimeTick &getInstance()
	{
		if (NULL == instance)
			instance = new ClientTimeTick();

		return *instance;
	}

	/**
	* \brief 释放类的唯一实例
	*
	*/
	static void delInstance()
	{
		SAFE_DELETE(instance);
	}

	void run();

private:

	static ClientTimeTick *instance;

	ClientTimeTick() : zThread("TimeTick") {};


};



#endif
