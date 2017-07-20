/*----------------------------------------------------------------
// Copyright (C) 2016 巨人网络
//
// 模块名：asio_session_in_mgr
// 创建者：Jin Qing (http://blog.csdn.net/jq0123)
// 修改者列表：
// 创建日期：2017.2.20
// 模块描述：Asio连入会话管理
//----------------------------------------------------------------*/

#include "asio_session_in_mgr.h"

#include "asio_session_in.h"  // for CAsioSessionIn
#include "log.h"

const char LOG_NAME[] = "CAsioSessionInMgr";

CAsioSessionInMgr::CAsioSessionInMgr()
{
}

CAsioSessionInMgr::~CAsioSessionInMgr()
{
}

void CAsioSessionInMgr::InsertSession(CAsioSessionIn& rSession)
{
	m_mapSessions[rSession.GetSessionId()] = &rSession;
}

void CAsioSessionInMgr::EraseSession(CAsioSessionIn& rSession)
{
	m_mapSessions.erase(rSession.GetSessionId());
}

CAsioSessionIn* CAsioSessionInMgr::GetSessionIn(uint64_t uSessionId) const
{
	auto iter = m_mapSessions.find(uSessionId);
	if (iter == m_mapSessions.end())
		return nullptr;
	return (*iter).second;
}
