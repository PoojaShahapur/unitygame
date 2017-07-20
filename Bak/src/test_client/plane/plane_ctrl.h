#ifndef PLANE_CTRL_H
#define PLANE_CTRL_H

#include <memory>  // for unique_ptr<>

namespace LuaIntf {
class LuaRef;
}

struct lua_State;

// 飞机控制器
class PlaneCtrl
{
public:
	explicit PlaneCtrl(lua_State* L);
	~PlaneCtrl();

public:
	void Init();
	void Fire();
	void TurnTo(float angle);

private:
	lua_State* m_luaState;
	using LuaRef = LuaIntf::LuaRef;
	std::unique_ptr<LuaRef> m_pLuaPlane;
};  // class PlaneCtrl

#endif  // PLANE_CTRL_H
