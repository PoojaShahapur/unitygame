/*************************************************************************
 Author: wang
 Created Time: 2014年11月06日 星期四 17时08分43秒
 File Name: ScenesServer/Recycle.h
 Description: 
 ************************************************************************/
#ifndef _Recycle_h_
#define _Recycle_h_

#pragma once
#include "Zebra.h"
#include "zTime.h"
#include "zThread.h"

class SceneNpc;

class SceneRecycle : public zThread
{
    public:
	~SceneRecycle() {}
	static SceneRecycle &getMe()
	{
	    if(NULL == instance)
		instance = new SceneRecycle();
	    return *instance;
	}
	static void delInstance()
	{
	    SAFE_DELETE(instance);
	}
	void run();
	void insert(SceneNpc* n, DWORD delay=5);
    private:
	std::queue<SceneNpc *> npcList;
	Timer _3_sec;

	zRWLock rwlock;

	static SceneRecycle *instance;

	SceneRecycle() :zThread("Recycle"), _3_sec(3) { }
};
#endif

