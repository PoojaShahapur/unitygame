#ifndef _SCENEMSGCMDHANDLE_H_
#define _SCENEMSGCMDHANDLE_H_
#include "zType.h"
#include "zSingleton.h"
///////////////////////////////////////////////
//
//code[SceneMsgCmdHandle.h] defination by codemokey
//
//
///////////////////////////////////////////////

class SceneMsgCmdHandle : public Singleton<SceneMsgCmdHandle>
{
    public:
        friend class SingletonFactory<SceneMsgCmdHandle>;
        SceneMsgCmdHandle();
        ~SceneMsgCmdHandle();

	bool registerSceneMsgCmd();
};

#endif //_SCENEMSGCMDHANDLE_H_

