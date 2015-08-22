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
	* \brief �ͷ����Ψһʵ��
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

