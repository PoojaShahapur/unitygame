#ifndef PLANE_TEST_H
#define PLANE_TEST_H

#include <memory>

class PlaneAgent;
class PlaneCtrl;
struct lua_State;

// 飞机测试
class PlaneTest
{
public:
	explicit PlaneTest(lua_State* L);
	~PlaneTest();

private:
	bool InitPlayer();
	void CleanupPlayer();
	void Bind();

private:
	lua_State* m_pLuaState;
	std::unique_ptr<PlaneCtrl> m_pPlaneCtrl;
	PlaneAgent* m_pPlaneAgent;
};  // class PlaneTest

#endif  // PLANE_TEST_H
