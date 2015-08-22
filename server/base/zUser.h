#ifndef _zUser_h_
#define _zUser_h_
#include "zSceneEntry.h"
#include "Zebra.h"
/**
* \brief ��ɫ����������
*/
/**
* \brief ��ɫ������,�д�����
*/
struct zUser:public zSceneEntry
{
	zUser():zSceneEntry(SceneEntry_Player)
	{
	}
	void lock()
	{
		//Zebra::logger->debug("lockuser");
		mlock.lock();
	}

	void unlock()
	{
		//Zebra::logger->debug("unlockuser");
		mlock.unlock();
	}

private:
	zMutex mlock;
};

#endif

