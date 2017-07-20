// 飞机控制器
#include "plane_ctrl.h"

#include <LuaIntf/LuaIntf.h>
#include <iostream>

PlaneCtrl::PlaneCtrl(lua_State* L)
	: m_luaState(L),
	m_pLuaPlane(new LuaRef)
{
	assert(L);
}

PlaneCtrl::~PlaneCtrl()
{
}

void PlaneCtrl::Init()
{
	LuaRef require(m_luaState, "require");
	try
	{
		*m_pLuaPlane = require.call<LuaRef>("plane.test_plane");
	}
	catch (const LuaIntf::LuaException& e)
	{
		std::cerr << "Failed to require test_plane, "
			<< e.what() << std::endl;
		throw e;
	}
}

void PlaneCtrl::Fire()
{
	try
	{
		m_pLuaPlane->dispatchStatic("fire");
	}
	catch (const LuaIntf::LuaException& e)
	{
		std::cerr << "Failed to call lua test_plane.fire(), "
			<< e.what() << std::endl;
	}
}

void PlaneCtrl::TurnTo(float angle)
{
	try
	{
		m_pLuaPlane->dispatchStatic("turn_to", angle);
	}
	catch (const LuaIntf::LuaException& e)
	{
		std::cerr << "Failed to call lua test_plane.turn_to(), "
			<< e.what() << std::endl;
	}
}

