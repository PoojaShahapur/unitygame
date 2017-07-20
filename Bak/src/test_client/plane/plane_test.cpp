#include "plane_test.h"

#include "behaviac_generated/types/behaviac_types.h"  // for PlaneAgent
#include "log.h"
#include "plane_ctrl.h"  // for PlaneCtrl

#include <LuaIntf/LuaIntf.h>

const char LOG_NAME[] = "PlaneTest";

PlaneTest::PlaneTest(lua_State* L)
	: m_pLuaState(L),
	m_pPlaneCtrl(new PlaneCtrl(L))
{
	assert(L);
	bool bOk = InitPlayer();
	if (!bOk)
	{
		LOG_ERROR("Failed to init player.");
		throw("Failed to init player.");
	}
	m_pPlaneCtrl->Init();
	m_pPlaneAgent->SetPlaneCtrl(m_pPlaneCtrl.get());

	Bind();
}

PlaneTest::~PlaneTest()
{
	CleanupPlayer();
}

bool PlaneTest::InitPlayer()
{
	m_pPlaneAgent = behaviac::Agent::Create<PlaneAgent>();
	bool bRet = m_pPlaneAgent->btload("BT_Test");
	m_pPlaneAgent->btsetcurrent("BT_Test");
	m_pPlaneAgent->SetActive(false);  // 进入房间后才激活
	return bRet;
}

void PlaneTest::CleanupPlayer()
{
	behaviac::Agent::Destroy(m_pPlaneAgent);
}

void PlaneTest::Bind()
{
	PlaneAgent& rAgent = *m_pPlaneAgent;
	LuaIntf::LuaBinding(m_pLuaState).beginModule("c_plane")
		.addFunction("set_active", [&rAgent](bool active) {
			rAgent.SetActive(active);  // 进入房间后激活
		})
		.addFunction("set_reaching_top_or_bottom", [&rAgent]() {
			rAgent.SetReachingTopOrBottom();
		})
		.addFunction("set_reaching_left_or_right", [&rAgent]() {
			rAgent.SetReachingLeftOrRight();
		})
	.endModule();
}

