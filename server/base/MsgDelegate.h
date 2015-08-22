/*************************************************************************
 Author: wang
 Created Time: 2015��02��12�� ������ 15ʱ00��30��
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
	 * \brief   ����ȷ����ͬƽ̨�ϳ�Ա�����ĳ���
	 */
	void test()
	{}  
    };
};

#define MSG_MF_LEN sizeof(&MSG_CENTER::_TEMP::test)

/**
 * \brief   ��װ��Ա����ָ��
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
	 * \brief   ���巽��
	 */
	template<typename C, typename U, typename FunPtr>
	    bool delegate(C *p, U *u, const Cmd::t_NullCmd *pNullCmd, const DWORD nLen)
	    {
		return (p->*(*reinterpret_cast<FunPtr*>(data)))(u, pNullCmd, nLen);
	    }
	bool isbind;	//�Ƿ��Ѿ���
    private:
	char data[MSG_MF_LEN];
};

/**
 * \brief   ��Ϣ����
*/
class MsgCenter : public Singleton<MsgCenter>
{
	friend class SingletonFactory<MsgCenter>;
    public:
	/**
	 * \brief ����Ϣ�ʹ�������
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
		Zebra::logger->error("[��Ϣ����] �ظ�����Ϣ MSG(cmd=%u,para=%u)", byCmd, byPara);
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
	    Zebra::logger->error("[��Ϣ����] û���ҵ��󶨸���Ϣ�ĺ��� MSG(cmd=%u,para=%u)", byCmd, byPara);
	    return NULL;
	}

    private:
	std::vector<std::vector<MsgDelegate> > msg_map;
};

