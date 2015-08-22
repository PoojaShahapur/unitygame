/*************************************************************************
 Author: wang
 Created Time: 2015年02月12日 星期四 15时00分30秒
 File Name: base/MsgDelegate.h
 Description: 
 ************************************************************************/
#include "zSingleton.h"
#include "zType.h"
#include <vector>
#include "Zebra.h"
#include "zNullCmd.h"

namespace MSG_CENTER
{
    struct _TEMP
    {
	/**
	 * \brief   用于确定不同平台上成员函数的长度
	 */
	void test()
	{}  
    };
};

#define MSG_MF_LEN sizeof(&MSG_CENTER::_TEMP::test)

/**
 * \brief   封装成员函数指针
*/
class MsgDelegate
{
    public:
	MsgDelegate()
	    :isbind(false)
	{
	    bzero(data, MSG_MF_LEN);
	}

	template<typename F>
	    MsgDelegate(const F &arg)
	    :isbind(false)
	    {
		memcpy(&data, &arg, sizeof(F));
	    }
	/**
	 * \brief   具体方法
	 */
	template<typename C, typename U, typename FunPtr>
	    bool delegate(C *p, U *u, const Cmd::t_NullCmd *pNullCmd, const DWORD nLen)
	    {
		return (p->*(*reinterpret_cast<FunPtr*>(data)))(u, pNullCmd, nLen);
	    }
	bool isbind;	//是否已经绑定
    private:
	char data[MSG_MF_LEN];
};

/**
 * \brief   消息中心
*/
class MsgCenter : public Singleton<MsgCenter>
{
	friend class SingletonFactory<MsgCenter>;
    public:
	/**
	 * \brief 将消息和处理函数绑定
	 */
	void bind(const WORD byCmd, const WORD byPara, MsgDelegate &del)
	{
	    if(byCmd >= msg_map.size())
	    {
		msg_map.resize(byCmd + 1);
	    }
	    if(byPara >= msg_map.at(byCmd).size())
	    {
		msg_map.at(byCmd).resize(byPara + 1);
	    }
	    if(msg_map[byCmd][byPara].isbind)
	    {
		Zebra::logger->error("[消息中心] 重复绑定消息 MSG(cmd=%u,para=%u)", byCmd, byPara);
		return;
	    }
	    del.isbind = true;
	    msg_map[byCmd][byPara] = del;
	}
	
	MsgDelegate* find(const WORD byCmd, const WORD byPara)
	{
	    if(byCmd < msg_map.size()
		    && byPara < msg_map[byCmd].size()
		    && msg_map[byCmd][byPara].isbind)
	    {
		return &(msg_map[byCmd][byPara]);
	    }
	    Zebra::logger->error("[消息中心] 没有找到绑定该消息的函数 MSG(cmd=%u,para=%u)", byCmd, byPara);
	    return NULL;
	}

    private:
	std::vector<std::vector<MsgDelegate> > msg_map;
};

