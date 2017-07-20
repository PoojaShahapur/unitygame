// Author: Jin Qing (http://blog.csdn.net/jq0123)

#include "game_clt_id.h"

#include "log.h"
#include "util.h"  // for Util

#include <boost/lexical_cast.hpp>
#include <cassert>

const char LOG_NAME[] = "CGameCltId";

// 因为导出到Lua, 所以须容错。
CGameCltId::CGameCltId(uint16_t uBaseSvrId, uint64_t uBaseRpcCltId)
	: m_uBaseSvrId(uBaseSvrId ? uBaseSvrId : Util::GetMySvrId()),
	m_uBaseRpcCltId(uBaseRpcCltId)
{
	assert(m_uBaseSvrId);
	if (!uBaseSvrId)
		LOG_WARN("Base server ID is 0 in CGameCltId().");
}

CGameCltId::~CGameCltId()
{
	assert(!((m_nValid = 0)));  // to check dangling point
}

uint16_t CGameCltId::GetBaseSvrId() const
{
	return m_uBaseSvrId;
}

uint64_t CGameCltId::GetBaseRpcCltId() const
{
	assert(IsValid());
	return m_uBaseRpcCltId;
}

std::string CGameCltId::ToString() const
{
	assert(IsValid());

	using std::string;
	using boost::lexical_cast;
	return lexical_cast<string>(m_uBaseSvrId) + "." +
		lexical_cast<string>(m_uBaseRpcCltId);
}

bool CGameCltId::Equals(const CGameCltId& gameCltId) const
{
	assert(IsValid());
	return m_uBaseSvrId == gameCltId.m_uBaseSvrId &&
		m_uBaseRpcCltId == gameCltId.m_uBaseRpcCltId;
}

CGameCltId& CGameCltId::operator=(const CGameCltId& clt)
{
    this->m_uBaseSvrId = clt.m_uBaseSvrId;
    this->m_uBaseRpcCltId = clt.m_uBaseRpcCltId;
    return *this;
}

// 是否本服
bool CGameCltId::IsLocal() const
{
	assert(IsValid());
	assert(m_uBaseSvrId);
	return Util::IsMySvrId(m_uBaseSvrId);
}

