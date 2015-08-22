/*************************************************************************
 Author: 
 Created Time: 2014年09月26日 星期五 11时50分02秒
 File Name: base/zSyncEvent.h
 Description: 
 ************************************************************************/
#ifndef _zSyncEvent_h_
#define _zSyncEvent_h_

#include "zNoncopyable.h"
#include "zMutex.h"
#include "zCond.h"

class zSyncEvent : private zNoncopyable
{
    public:
	zSyncEvent(const bool initstate = false) :state(initstate) {};

	~zSyncEvent() {};

	void signal()
	{
	    mutex.lock();
	    while(state)
	    {
		cond2.wait(mutex);
	    }
	    state = true;
	    cond1.signal();
	    mutex.unlock();
	}

	void wait()
	{
	    mutex.lock();
	    while(!state)
	    {
		cond1.wait(mutex);
	    }
	    state = false;
	    cond2.signal();
	    mutex.unlock();
	}
    private:

	volatile bool state;
	zMutex mutex;
	zCond cond1, cond2;
};
#endif


