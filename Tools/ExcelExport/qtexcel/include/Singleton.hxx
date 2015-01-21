#ifndef _SINGLETON_H
#define _SINGLETON_H

#include <string.h>

#include "MemLeakCheck.hxx"

#include "Platform.hxx"
BEGIN_NAMESPACE

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

END_NAMESPACE

//#include "UnMemLeakCheck.hxx"

#endif				// SINGLETON_H