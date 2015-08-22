#ifndef __TIMETICK_H_
#define __TIMETICK_H_

#include <iostream>
#include <string>

#include "Zebra.h"
#include "zThread.h"
#include "zTime.h"


/**
* \brief ���ض�ʱ���߳�
*
*/
class ClientTimeTick : public zThread
{

public:

	static zRTime currentTime;

	~ClientTimeTick() {};

	/**
	* \brief �õ�Ψһʵ��
	*
	*
	* \return Ψһʵ��
	*/
	static ClientTimeTick &getInstance()
	{
		if (NULL == instance)
			instance = new ClientTimeTick();

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

	static ClientTimeTick *instance;

	ClientTimeTick() : zThread("TimeTick") {};


};



#endif
