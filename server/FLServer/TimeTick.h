#ifndef _FLTimeTick_h_
#define _FLTimeTick_h_
#include "zThread.h"
#include "zType.h"

class FLTimeTick : public zThread
{
public:

	~FLTimeTick() {};

	static FLTimeTick &getInstance()
	{
		if (NULL == instance)
			instance = new FLTimeTick();

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

	static FLTimeTick *instance;

	FLTimeTick() : zThread("TimeTick")
	{
	}

};
#endif

