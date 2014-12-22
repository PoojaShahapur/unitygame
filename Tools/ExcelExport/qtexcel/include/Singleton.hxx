#ifndef _SINGLETON_H
#define _SINGLETON_H

#include <string.h>

template<class T>
class Singleton
{
private:
	static T* msSingleton;

public:
	static T* getSingletonPtr()
	{
		if(msSingleton == NULL)
		{
			msSingleton = new T();
		}
		return msSingleton;
	}
};

#endif				// SINGLETON_H