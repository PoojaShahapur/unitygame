#ifndef _ZMISC_H_
#define _ZMISC_H_

#include <string>
#include <cstring>
#include <cctype>
#include <cstdlib>
#include <cstdio>
#include <cerrno>
#include <ctime>
#include <vector>
#include <queue>
#include <dirent.h>
#include <unistd.h>
#include "zType.h"
#include "zMutex.h"
#include <set>
#include <sstream>
#include "Zebra.h"
#include "zRegex.h"
#include <ext/mt_allocator.h> 

#define count_of(entry_v) (sizeof(entry_v)/sizeof((entry_v)[0]))

struct odds_t
{
    unsigned int upNum;
    unsigned int downNum;
};

enum ServerType
{
	UNKNOWNSERVER  =  0, /** 未知服务器类型 */
	SUPERSERVER      =  1, /** 管理服务器 */
	LOGINSERVER     =  10, /** 登陆服务器 */
	RECORDSERVER  =  11, /** 档案服务器 */
	BILLSERVER      =  12, /** 计费服务器 */
	SESSIONSERVER  =  20, /** 会话服务器 */
	SCENESSERVER  =  21, /** 场景服务器 */
	GATEWAYSERVER  =  22, /** 网关服务器 */
	MINISERVER      =  23    /** 小游戏服务器 */
	
};

namespace Zebra
{
    extern __thread unsigned int seedp;
};

class zMisc
{
    public:

//随机产生min~max之间的数字，包裹min和max
static int randBetween(int min,int max)
{
  if (min==max)
    return min;
  else if (min > max)
    return max + (int) (((double) min - (double)max + 1.0) * rand_r(&Zebra::seedp) / (RAND_MAX + 1.0));
  else
    return min + (int) (((double) max - (double)min + 1.0) * rand_r(&Zebra::seedp) / (RAND_MAX + 1.0));
}

//获取几分之的几率
static bool selectByOdds(const DWORD upNum,const DWORD downNum)
{
  DWORD m_rand;
  if (downNum < 1) return false;
  if (upNum < 1) return false;
  if (upNum > downNum - 1) return true;
  m_rand = 1 + (DWORD) ((double)downNum * rand() / (RAND_MAX + 1.0));
  return (m_rand <= upNum);
}

//获取几分之几的几率
static bool selectByt_Odds(const odds_t &odds)
{
  return selectByOdds(odds.upNum,odds.downNum);
}

//获取百分之的几率
static bool selectByPercent(const DWORD percent)
{
  return selectByOdds(percent,100);
}

//获取万分之的几率
static bool selectByTenTh(const DWORD tenth)
{
  return selectByOdds(tenth,10000);
}

//获取十万分之的几率
static bool selectByLakh(const DWORD lakh)
{
  return selectByOdds(lakh,100000);
}

//获取亿分之之的几率
static bool selectByOneHM(const DWORD lakh)
{
  return selectByOdds(lakh,100000000);
}

//获取当前时间字符串，需要给定格式
static void getCurrentTimeString(char *buffer,const int bufferlen,const char *format)
{
  time_t now;
  time(&now);
  strftime(buffer,bufferlen,format,localtime(&now));
}

static char *getTimeString(time_t t,char *buffer,const int bufferlen,const char *format)
{
  strftime(buffer,bufferlen,format,localtime(&t));
  return buffer;
}

static char *getTimeString(time_t t,char *buffer,const int bufferlen)
{
  return getTimeString(t,buffer,bufferlen,"%C/%m/%d %T");
}

template<class T, class U>
      static T lexical_cast(const U& u)    //do not use char*
      {
           std::stringstream sstrm;
           sstrm << u;
           T t;
           sstrm >> t;
           return t;
      }  
};

template <typename T>
struct singleton_default
{
private:
	singleton_default();

public:
	typedef T object_type;

	static object_type & instance()
	{
		return obj;
	}

	static object_type obj;
};
template <typename T>
typename singleton_default<T>::object_type singleton_default<T>::obj;

//手动调用构造函数，不分配内存
template<class _T1> 
inline  void constructInPlace(_T1  *_Ptr)
{
	new (static_cast<void*>(_Ptr)) _T1();
}
#if 0
#define BUFFER_CMD(cmd,name,len) char buffer##name[len];\
	cmd *name=(cmd *)buffer##name;constructInPlace(name);
#endif
/// 声明变长指令
#define BUFFER_CMD(cmd,name,len) CheckedBuf buffer_##name(len, __PRETTY_FUNCTION__);\
    cmd *name=(cmd *)buffer_##name.buffer();\
    constructInPlace(name);

class CheckedBuf
{
    public:
	CheckedBuf(DWORD size, const char* des)
	{
	    size_ = size;
	    des_ = des;
	    buffer_ = new char[size_ + 2];
	    bzero(buffer_, size_ + 2);
	    buffer_[size_] = 20;
	    buffer_[size_ + 1] = 12;
	}
	~CheckedBuf()
	{
	    if(buffer_[size_] != 20 || buffer_[size_ + 1] != 12 )
	    {
		Zebra::logger->error("[�䳤��Ϣд�����] %s",des_.c_str());	
	    }
	    delete[] buffer_;
	}

	char *buffer(){ return buffer_;}
    public:
	DWORD size_;
	std::string des_;
	char *buffer_;
};

typedef std::pair<DWORD,BYTE *> CmdPair;
template <int QueueSize=102400>
class MsgQueue
{
public:
	MsgQueue()
	{
		queueRead=0;
		queueWrite=0;
	}
	~MsgQueue()
	{
		clear();
	}
	typedef std::pair<volatile bool,CmdPair > CmdQueue;
	CmdPair *get()
	{
		CmdPair *ret=NULL;
		if (cmdQueue[queueRead].first)
		{
			ret=&cmdQueue[queueRead].second;
		}
		return ret;
	}
	void erase()
	{
		//SAFE_DELETE_VEC(cmdQueue[queueRead].second.second);
		__mt_alloc.deallocate(cmdQueue[queueRead].second.second,cmdQueue[queueRead].second.first);
		cmdQueue[queueRead].first=false;
		queueRead = (++queueRead)%QueueSize;
	}
	bool put(const void *pNullCmd,const DWORD cmdLen)
	{
		//BYTE *buf = new BYTE[cmdLen];
		BYTE *buf = (BYTE*)__mt_alloc.allocate(cmdLen);
		if (buf)
		{
			bcopy(pNullCmd,buf,cmdLen);
			if (!putQueueToArray() && !cmdQueue[queueWrite].first)
			{
				cmdQueue[queueWrite].second.first = cmdLen;
				cmdQueue[queueWrite].second.second = buf;
				cmdQueue[queueWrite].first=true;
				queueWrite = (++queueWrite)%QueueSize;
				return true;
			}
			else
			{
				queueCmd.push(std::make_pair(cmdLen,buf));
			}
			return true;
		}
		return false;

	}
private:
	void clear()
	{
		while(putQueueToArray())
		{
			while(get())
			{
				erase();
			}
		}
		while(get())
		{
			erase();
		}
	}
	bool putQueueToArray()
	{
		bool isLeft=false;
		while(!queueCmd.empty())
		{
			if (!cmdQueue[queueWrite].first)
			{
				cmdQueue[queueWrite].second = queueCmd.front();;
				cmdQueue[queueWrite].first=true;
				queueWrite = (++queueWrite)%QueueSize;
				queueCmd.pop();
			}
			else
			{
				isLeft = true; 
				break;
			}
		}
		return isLeft;
	}
	__gnu_cxx::__mt_alloc<BYTE> __mt_alloc;
	CmdQueue cmdQueue[QueueSize];
	std::queue<CmdPair> queueCmd;
	DWORD queueWrite;
	DWORD queueRead;
};

template<typename T>
class zCmdCheck : public zNoncopyable
{
    public:
	zCmdCheck()
	{}
	virtual ~zCmdCheck()
	{
	    cmdset.clear();
	}
	bool put(const T& t)
	{
	    CmdSet_Iter iter = cmdset.find(t);
	    if(iter != cmdset.end())
		return false;
	    cmdset.insert(t);
	    return true;
	}
	void erase(const T& t)
	{
	    cmdset.erase(t);
	}
	virtual void list(const char* desc =NULL)
	{
	    std::ostringstream oss;
	    if(desc)
		oss << desc <<": ";
	    CmdSet_Iter iter = cmdset.begin();
	    for(; iter !=cmdset.end();++iter)
	    {
		oss <<*iter<<" ";
	    }
	}
	bool check(const T& t)
	{
	    CmdSet_Iter iter = cmdset.find(t);
	    if(iter == cmdset.end())
		return true;
	    return false;
	}
	int size()
	{
	    return cmdset.size();
	}
    protected:
	typedef std::set<T> CmdSet;
	typedef typename CmdSet::iterator CmdSet_Iter;
	CmdSet cmdset;

};

typedef zCmdCheck<WORD> CmdCheck;

#endif
