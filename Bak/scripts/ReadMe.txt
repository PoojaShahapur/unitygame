C++ ��lua�ű���
	luaL_dostring
		����̨����
		Debug����
	lua_pcall()
	EntityMethodCall() -> lua_pcall()
	CLuaCallback?

C++������lua��ģ������ c_ ǰ׺�� �� c_logger��

������ LuaBinding, �磺
	LuaIntf::LuaBinding(L).beginModule("c_logger")
		.addFunction("debug", &debug)

�����ܷ�Ŀ¼������:
	cell/cats_and_dogs è����ս
����ɾ�����ܻ�����ɾ�����Ŀ¼���ɡ�
package.path������cell, ������cell/cats_and_dogs��
�������밴��ģ����أ���:
	require("cats_and_dogs.matcher")

�����svc_ǰ׺����svc_login.lua, svc_cats_and_dogs.lua
