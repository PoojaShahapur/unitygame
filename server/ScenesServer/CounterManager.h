/*************************************************************************
 Author: wang
 Created Time: 2014��10��13�� ����һ 10ʱ29��47��
 File Name: ScenesServer/CounterManager.h
 Description: 
 ************************************************************************/
#ifndef _CounterManager_h_
#define _CounterManager_h_

#include "zType.h"
#include "CharBase.h"
#include <map>

class SceneUser;

class CounterManager
{
    public:
	CounterManager();
	~CounterManager();
	QWORD getCounter(DWORD id);

	bool addCounterDay(DWORD id, QWORD num=1);
	bool addCounterWeek(DWORD id, QWORD num=1);
	bool addCounterMonth(DWORD id, QWORD num=1);
	bool addCounter(DWORD id, QWORD num=1);
	bool clearCounter(DWORD id);
	int getSize();
	int save(unsigned char* dest);
	int load(unsigned char* src, unsigned int len);
    protected:
	std::map<DWORD, CounterMember> counter_map;
	typedef std::map<DWORD, CounterMember>::iterator counter_iter;
};
#endif

