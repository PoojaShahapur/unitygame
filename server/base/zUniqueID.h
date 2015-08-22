#ifndef _zUniqueID_h_
#define _zUniqueID_h_

#include <pthread.h>
#include <list>
#include <set>
#include <ext/pool_allocator.h>

#include "zType.h"
#include "zMutex.h"
#include "zNoncopyable.h"
#include "zMisc.h"

/**
* \brief zUniqueID模板
* 本模板实现了唯一ID生成器，并保证线程安全。
* 可以用各种长度的无符号整数作为ID。
*/
template <class T>
class zUniqueID:private zNoncopyable
{
private:
	zMutex mutex;
	//std::list<T,__pool_alloc<T> > ids;
	std::set<T> ids;
	T maxID;
	T minID;
	T curMaxID;
	void init(T min,T max)
	{
		minID=min;
		maxID=max;
		curMaxID=minID;
	}

public:
	/**
	* \brief 默认构造函数 
	* 开始ID为1，最大有效ID为(T)-2,无效ID为(T)-1
	*/
	zUniqueID()
	{
		init(1,(T)-1);
	}

	/**
	* \brief 构造函数 
	* 用户自定义起始ID，最大有效ID为(T)-2,无效ID为(T)-1
	* \param startID 用户自定义的起始ID
	*/
	zUniqueID(T startID)
	{
		init(startID,(T)-1);
	}

	/**
	* \brief 构造函数 
	* 用户自定义起始ID，及最大无效ID,最大有效ID为最大无效ID-1
	* \param startID 用户自定义的起始ID
	* \param endID 用户自定义的最大无效ID
	*/
	zUniqueID(T startID,T endID)
	{
		init(startID,endID);
	}

	/**
	* \brief 析构函数 
	* 回收已分配的ID内存。
	*/
	~zUniqueID()
	{
		mutex.lock();
		ids.clear();
		mutex.unlock();
	}

	/**
	* \brief 得到最大无效ID 
	* \return 返回最大无效ID
	*/
	T invalid()
	{
		return maxID;
	}

	/**
	* \brief 测试这个ID是否被分配出去
	* \return 被分配出去返回true,无效ID和未分配ID返回false
	*/
	bool hasAssigned(T testid)
	{
		mutex.lock();
		if (testid<maxID && testid>=minID)
		{
			typename std::set<T>::iterator iter = ids.find(testid);
			if(iter != ids.end())
			{
					mutex.unlock();
					return false;
			}
			/*
			for(int i=0,n=ids.size() ;i<n;i++)
			{
			if (ids[i]==testid)
			{
			mutex.unlock();
			return false;
			}
			}
			// */
			mutex.unlock();
			return true;
		}
		mutex.unlock();
		return false;
	}

	/**
	* \brief 得到一个唯一ID 
	* \return 返回一个唯一ID，如果返回最大无效ID，比表示所有ID都已被用，无可用ID。
	*/
	T get()
	{
		T ret;
		mutex.lock();
		if (maxID>curMaxID)
		{
			ret=curMaxID;
			curMaxID++;
		}
		else
			ret=maxID;
		if (ret == maxID && !ids.empty())
		{
		    DWORD index = zMisc::randBetween(0, ids.size()-1);
		    typename std::set<T>::iterator iter = ids.begin();
		    std::advance(iter, index);
		    ret = *iter;
		    ids.erase(iter);
		}
		mutex.unlock();
		return ret;
	}

	/**
	* \brief 一次得到多个ID，这些ID都是相邻的,并且不回被放回去 
	* \param size 要分配的ID个数
	* \param count 实际分配ID的个数
	* \return 返回第一个ID，如果返回最大无效ID，比表示所有ID都已被用，无可用ID。
	*/
	T get(int size,int & count)
	{
		T ret;
		mutex.lock();
		if (maxID>curMaxID)
		{
			count=(maxID-curMaxID)>size?size:(maxID-curMaxID);
			ret=curMaxID;
			curMaxID+=count;
		}
		else
		{
			count=0;
			ret=maxID;
		}
		mutex.unlock();
		return ret;
	}

	/**
	* \brief 将ID放回ID池，以便下次使用。 
	* 
	* 放回的ID必须是由get函数得到的。并且不能保证放回的ID,没有被其他线程使用。
	* 所以用户要自己保证还在使用的ID不会被放回去。以免出现ID重复现象。
	* \param id 由get得到的ID.
	*/
	void put(T id)
	{
		mutex.lock();
		if (id<maxID && id>=minID)
		{
			typename std::set<T>::iterator iter = ids.find(id);
			if(iter == ids.end())
			{
			    ids.insert(id);
			}
		}
		mutex.unlock();
	}
};

typedef zUniqueID<DWORD> zUniqueDWORDID;
#endif
