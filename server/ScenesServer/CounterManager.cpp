/*************************************************************************
 Author: wang
 Created Time: 2014年10月13日 星期一 10时35分17秒
 File Name: ScenesServer/CounterManager.cpp
 Description: 
 ************************************************************************/
#include "CounterManager.h"
#include "TimeTick.h"
#include <math.h>

CounterManager::CounterManager()
{
}

CounterManager::~CounterManager()
{
}

QWORD CounterManager::getCounter(DWORD id)
{
    counter_iter iter = counter_map.find(id);
    if(iter == counter_map.end())
	return 0;
    if(iter->second.type == CHANGE_DAY)
    {
	struct tm tm_cur, tm_last;
	zRTime::getLocalTime(tm_cur, SceneTimeTick::currentTime.sec());
	zRTime::getLocalTime(tm_last, iter->second.last_time);

	if(SceneTimeTick::currentTime.sec() > iter->second.last_time
	    && (tm_cur.tm_yday != tm_last.tm_yday || tm_cur.tm_year != tm_last.tm_year))
	{
	    iter->second.count = 0;
	    iter->second.last_time = SceneTimeTick::currentTime.sec();
	}
    }
    else if(iter->second.type == CHANGE_WEEK)
    {
	DWORD cur = SceneTimeTick::currentTime.sec();
	struct tm tm_cur, tm_last;
	zRTime::getLocalTime(tm_cur, SceneTimeTick::currentTime.sec());
	zRTime::getLocalTime(tm_last, iter->second.last_time);
	WORD cur_wday = tm_cur.tm_wday?tm_cur.tm_wday:7;
	WORD last_wday = tm_last.tm_wday?tm_last.tm_wday:7;

	time_t cur_monday = cur -(cur_wday-1)*24*60*60;
	time_t last_monday = iter->second.last_time -(last_wday-1)*24*60*60;
	struct tm tm_cur_monday, tm_last_monday;
	zRTime::getLocalTime(tm_cur_monday, cur_monday);
	zRTime::getLocalTime(tm_last_monday, last_monday);
	if(cur > iter->second.last_time)
	{
	    if(tm_cur_monday.tm_yday != tm_last_monday.tm_yday 
		    || cur-iter->second.last_time >= 7*24*60*60)
	    {
		iter->second.count = 0;
		iter->second.last_time = cur;
	    }
	}
    }
    else if(iter->second.type == CHANGE_MONTH)
    {
	DWORD cur = SceneTimeTick::currentTime.sec();
	struct tm tm_cur, tm_last;
	zRTime::getLocalTime(tm_cur, SceneTimeTick::currentTime.sec());
	zRTime::getLocalTime(tm_last, iter->second.last_time);
	if(tm_cur.tm_mon != tm_last.tm_mon || tm_cur.tm_year != tm_last.tm_year)
	{
	    iter->second.count = 0;
	    iter->second.last_time = cur;
	}
    }
    return iter->second.count;
}

/**
 * \brief   add count
 * \param id,stand for count type; 
 * \return
*/
bool CounterManager::addCounter(DWORD id, QWORD num)
{
    counter_iter iter = counter_map.find(id);
    if(iter == counter_map.end())
    {
	CounterMember tmp;
	tmp.id = id;
	tmp.type = CHANGE_NONE;
	tmp.count = num;
	tmp.last_time = SceneTimeTick::currentTime.sec();
	counter_map[id] = tmp;
	return true;
    }
    else
    {
	iter->second.count += num;
    }
    return true;
}

bool CounterManager::clearCounter(DWORD id)
{
    counter_iter iter = counter_map.find(id);
    if(iter == counter_map.end())
    {}
    else
    {
	iter->second.count = 0;
    }
    return true;
}

bool CounterManager::addCounterDay(DWORD id, QWORD num)
{
    counter_iter iter = counter_map.find(id);
    if(iter == counter_map.end())
    {
	CounterMember tmp;
	tmp.id = id;
	tmp.type = CHANGE_DAY;
	tmp.count = num;
	tmp.last_time = SceneTimeTick::currentTime.sec();
	counter_map[id] = tmp;
	return true;
    }
    else if(iter->second.type == CHANGE_DAY)
    {
	struct tm tm_cur, tm_last;
	zRTime::getLocalTime(tm_cur, SceneTimeTick::currentTime.sec());
	zRTime::getLocalTime(tm_last, iter->second.last_time);

	if(SceneTimeTick::currentTime.sec() > iter->second.last_time
		&& (tm_cur.tm_yday != tm_last.tm_yday || tm_cur.tm_year != tm_last.tm_year))
	{
	    iter->second.count = num;
	    iter->second.last_time = SceneTimeTick::currentTime.sec();
	}
	else
	{
	    iter->second.count += num;
	}
	return true;
    }
    return false;
}

bool CounterManager::addCounterWeek(DWORD id, QWORD num)
{
    counter_iter iter = counter_map.find(id);
    if(iter == counter_map.end())
    {
	CounterMember tmp;
	tmp.id = id;
	tmp.type = CHANGE_WEEK;
	tmp.count = num;
	tmp.last_time = SceneTimeTick::currentTime.sec();
	counter_map[id] = tmp;
	return true;
    }
    else if(iter->second.type == CHANGE_WEEK)
    {
	DWORD cur = SceneTimeTick::currentTime.sec();
	struct tm tm_cur, tm_last;
	zRTime::getLocalTime(tm_cur, SceneTimeTick::currentTime.sec());
	zRTime::getLocalTime(tm_last, iter->second.last_time);
	WORD cur_wday = tm_cur.tm_wday?tm_cur.tm_wday:7;
	WORD last_wday = tm_last.tm_wday?tm_last.tm_wday:7;

	time_t cur_monday = cur -(cur_wday-1)*24*60*60;
	time_t last_monday = iter->second.last_time -(last_wday-1)*24*60*60;
	struct tm tm_cur_monday, tm_last_monday;
	zRTime::getLocalTime(tm_cur_monday, cur_monday);
	zRTime::getLocalTime(tm_last_monday, last_monday);
	if(cur > iter->second.last_time)
	{
	    if(tm_cur_monday.tm_yday != tm_last_monday.tm_yday 
		    || cur-iter->second.last_time >= 7*24*60*60)
	    {
		iter->second.count = num;
	    }
	}
	else
	{
	    iter->second.count += num;
	}
	iter->second.last_time = cur;
	return true;
    }
    return false;
}

bool CounterManager::addCounterMonth(DWORD id, QWORD num)
{
    counter_iter iter = counter_map.find(id);
    if(iter == counter_map.end())
    {
	CounterMember tmp;
	tmp.id = id;
	tmp.type = CHANGE_MONTH;
	tmp.count = num;
	tmp.last_time = SceneTimeTick::currentTime.sec();
	counter_map[id] = tmp;
	return true;
    }
    else if(iter->second.type == CHANGE_MONTH)
    {
	DWORD cur = SceneTimeTick::currentTime.sec();
	struct tm tm_cur, tm_last;
	zRTime::getLocalTime(tm_cur, SceneTimeTick::currentTime.sec());
	zRTime::getLocalTime(tm_last, iter->second.last_time);
	if(tm_cur.tm_mon == tm_last.tm_mon || tm_cur.tm_year == tm_last.tm_year)
	{
	    iter->second.count += num;
	}
	else
	{
	    iter->second.count = num;
	    iter->second.last_time = cur;
	}
	return true;
    }
    return false;
}

int CounterManager::getSize()
{
    return this->counter_map.size();
}

int CounterManager::save(unsigned char* dest)
{
    int size = this->counter_map.size();
    if(0 == size)
	return 0;
    *((int *)dest) = size;
    int len = sizeof(int);
    counter_iter iter = this->counter_map.begin();
    for(; iter != this->counter_map.end(); ++iter)
    {
	bcopy(&iter->second, dest+len, sizeof(CounterMember));
	len += sizeof(CounterMember);
    }
    return len;
}

int CounterManager::load(unsigned char* src, unsigned int len)
{
    if(!len) return 0;
    unsigned char* begin = src;
    int count = *((int*)begin);
    unsigned int size = sizeof(int);
    while(count-- && (size < len))
    {
	CounterMember tmp;
	bcopy(begin+size, &tmp, sizeof(CounterMember));
	size += sizeof(CounterMember);
	counter_map[tmp.id] = tmp;
    }
    return size;
}

