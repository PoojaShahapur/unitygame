#include "SceneMsgCmdHandle.h"
///////////////////////////////////////////////
//
//code[SceneMsgCmdHandle.cpp] defination by codemokey
//
//
///////////////////////////////////////////////
#include "MsgDelegate.h"
#include "SceneTask.h"



SceneMsgCmdHandle::SceneMsgCmdHandle()
{}

SceneMsgCmdHandle::~SceneMsgCmdHandle()
{}

bool SceneMsgCmdHandle::registerSceneMsgCmd()
{
    MsgDelegate *mdl = new MsgDelegate(&SceneTask::psstReqAllCardTujianDataUserCmd);
    MsgCenter::getMe().bind(162, 4, *mdl);
    return true;
}
