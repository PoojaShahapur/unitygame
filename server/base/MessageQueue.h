#ifndef _MessageQueue_h
#define _MessageQueue_h

#include "zNullCmd.h"
#include "Zebra.h"
#include "zTime.h"
#include "zMisc.h"
#include "zSocket.h"

class MessageQueue
{
protected:
	virtual ~MessageQueue(){};
public:
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD cmdLen)
	{
		return cmdQueue.put((void*)pNullCmd,cmdLen);
	}
	virtual bool cmdMsgParse(const Cmd::t_NullCmd *,const DWORD)
	{
	    return true;
	}
	bool doCmd()
	{
		CmdPair *cmd = cmdQueue.get();
		while(cmd)
		{
			cmdMsgParse((const Cmd::t_NullCmd *)cmd->second,cmd->first);
			cmdQueue.erase();
			cmd = cmdQueue.get();
		}
		if (cmd)
		{
			cmdQueue.erase();
		}
		return true;
	}

private:
	MsgQueue<> cmdQueue;
};
#endif
