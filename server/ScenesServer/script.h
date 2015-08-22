#ifndef __NEWQUEST_SCRIPT_H_
#define __NEWQUEST_SCRIPT_H_

#include "zType.h"
#include <string>
#include <ext/hash_set>

/**
  A script that can be executed by a virtual machine.
*/
class LuaScript 
{
public:
  LuaScript( );

  virtual ~LuaScript();

  virtual bool isLoaded();

  virtual void setData( const std::string& rData );

  const std::string& getData() const;

private:
  std::string mFileName;
};

const DWORD LUALIB_BASE    = 0x00000001;
const DWORD LUALIB_TABLE    = 0x00000002;
const DWORD LUALIB_IO      = 0x00000004;
const DWORD LUALIB_STRING    = 0x00000008;
const DWORD LUALIB_MATH    = 0x00000010;
const DWORD LUALIB_DEBUG    = 0x00000020;

struct lua_State;

/** Scripting virtual machine.
  A virtual machine can execute scripting code.
*/
class LuaVM
{
protected:
  lua_State  * mLuaState;
public:
  LuaVM( DWORD libs = LUALIB_BASE|LUALIB_TABLE|LUALIB_STRING|LUALIB_MATH|LUALIB_IO );

  virtual ~LuaVM();

  virtual void execute( const std::string & rData );	//ִ��һ��lua�ű�

  virtual void execute( LuaScript* pScript);		//ִ��һ��lua�ű�

  lua_State* getLuaState() const { return mLuaState; }
};

/** Scripting system interface.
  Provides means to create virtual machines and scripts.
*/
class ScriptingSystemLua 
{
public:
  static ScriptingSystemLua& instance();

  virtual LuaVM* createVM();
  
  LuaVM* getVM(int index);

  void reloadVM();

  virtual LuaScript* createScriptFromFile( const std::string & rFile );

private:
  ScriptingSystemLua();
  virtual ~ScriptingSystemLua();
  

  typedef std::vector< LuaVM* > VMList;
  VMList  mVMs;
  static ScriptingSystemLua* _instance;
  
};

/** Script binder,
  Provides means to export system interfaces to script
*/
class Binder
{
public:
  void bind( LuaVM* pVM );
};

namespace luabind
{
  namespace detail {
    template<class T>
    struct delete_s;
    template<class T>
    struct destruct_only_s;  
  }
}

/** global index,used to decide executing a script or not.
  it's for speed purpose and avoid wasting cpu time
*/
class ScriptQuest
{
public:
  enum {
    NPC_VISIT = 1,
    NPC_KILL = 2,
    OBJ_USE = 4,
    OBJ_GET = 8,
    OBJ_DROP = 16,
  };
    
  static ScriptQuest& get_instance();
  
  void add(int type,int id);
  bool has(int type,int id) const;
  
  //for speed
  void sort();

  
private:
  ScriptQuest() {}  
  ~ScriptQuest() { }


  friend class luabind::detail::delete_s<ScriptQuest>;  
  friend class luabind::detail::destruct_only_s<ScriptQuest>;  

  int hash(int type,int id) const;

  static ScriptQuest*   _instance;
  
  __gnu_cxx::hash_set<int> _sq;
};

#include <luabind/luabind.hpp>
#include "Zebra.h"

class SceneUser;
extern SceneUser* current_user;

inline int execute_script_event(const char* func)
{

  try {
    LuaVM* vm = ScriptingSystemLua::instance().getVM(0);
    int ret = luabind::call_function<int>(vm->getLuaState(),func);
    return ret;
  }
  catch (luabind::error& e)
  {
    Zebra::logger->debug("CATCHED Luabind EXCEPTION:%s\n%s",func,e.what());
    return 0;
  }          
  catch (const char* msg)
  {
    Zebra::logger->debug("CATCHED (...) EXCEPTION:%s\n%s",func,msg);
    return 0;
  }
  
  return 0;  
}


inline int execute_script_event(SceneUser * user,const char* func)
{
  if (user == NULL )
  {
    Zebra::logger->error("��������ű���ǰ�û���ָ��");
    return 0;
  }
  else
    current_user=user;
  try {
    LuaVM* vm = ScriptingSystemLua::instance().getVM(0);
    int ret = luabind::call_function<int>(vm->getLuaState(),func);
    return ret;
  }
  catch (luabind::error& e)
  {
    Zebra::logger->debug("CATCHED Luabind EXCEPTION:%s\n%s",func,e.what());
    return 0;
  }          
  catch (const char* msg)
  {
    Zebra::logger->debug("CATCHED (...) EXCEPTION:%s\n%s",func,msg);
    return 0;
  }
  
  return 0;  
}

template<typename P>
int execute_script_event(SceneUser * user,const char* func,P& p)
{
  if (user == NULL )
  {
    Zebra::logger->error("��������ű���ǰ�û���ָ��");
    return 0;
  }
  else
    current_user=user;
  try {
    LuaVM* vm = ScriptingSystemLua::instance().getVM(0);
    int ret = luabind::call_function<int>(vm->getLuaState(),func,p);
    return ret;
  }
  catch (luabind::error& e)
  {
    Zebra::logger->debug("CATCHED Luabind EXCEPTION:%s\n%s",func,e.what());
    return 0;
  }          
  catch (const char* msg)
  {
    Zebra::logger->debug("CATCHED (...) EXCEPTION:%s\n%s",func,msg);
    return 0;
  }
  
  return 0;  
}

template<typename P1,typename P2>
int execute_script_event(SceneUser * user,const char* func,P1& p1,P2& p2)
{
  if (user == NULL )
  {
    Zebra::logger->error("��������ű���ǰ�û���ָ��");
    return 0;
  }
  else
    current_user=user;
  try {
    LuaVM* vm = ScriptingSystemLua::instance().getVM(0);
    int ret = luabind::call_function<int>(vm->getLuaState(),func,p1,p2);
    return ret;
  }
  catch (luabind::error& e)
  {
    Zebra::logger->debug("CATCHED Luabind EXCEPTION:%s\n%s",func,e.what());
    return 0;
  }          
  catch (const char* msg)
  {
    Zebra::logger->debug("CATCHED (...) EXCEPTION:%s\n%s",func,msg);
    return 0;
  }
  
  return 0;  
}

template<typename P1,typename P2,typename P3>
int execute_script_event(SceneUser * user,const char* func,P1& p1,P2& p2,P3& p3)
{
  if (user == NULL )
  {
    Zebra::logger->error("��������ű���ǰ�û���ָ��");
    return 0;
  }
  else
    current_user=user;
//  Zebra::logger->debug("execute_script_event:%s,%s,%u,%u",func,p1->name,p2,p3);
  try {
    LuaVM* vm = ScriptingSystemLua::instance().getVM(0);
    int ret = luabind::call_function<int>(vm->getLuaState(),func,p1,p2,p3);
    return ret;
  }
  catch (luabind::error& e)
  {
    Zebra::logger->debug("CATCHED Luabind EXCEPTION:%s\n%s",func,e.what());
    return 0;
  }          
  catch (const char* msg)
  {
    Zebra::logger->debug("CATCHED (...) EXCEPTION:%s\n%s",func,msg);
    return 0;
  }
  
  return 0;  
}


template<typename P1,typename P2,typename P3>
int execute_script_event(SceneUser * user,const char* func,const P1 *p1,const P2 *p2,P3& p3)
{
  if (user == NULL )
  {
    Zebra::logger->error("��������ű���ǰ�û���ָ��");
    return 0;
  }
  else
    current_user=user;
//  Zebra::logger->debug("execute_script_event:%s,%s,%u,%u",func,p1->name,p2,p3);
  try {
    LuaVM* vm = ScriptingSystemLua::instance().getVM(0);
    int ret = luabind::call_function<int>(vm->getLuaState(),func,p1,p2,p3);
    return ret;
  }
  catch (luabind::error& e)
  {
    Zebra::logger->debug("CATCHED Luabind EXCEPTION:%s\n%s",func,e.what());
    return 0;
  }          
  catch (const char* msg)
  {
    Zebra::logger->debug("CATCHED (...) EXCEPTION:%s\n%s",func,msg);
    return 0;
  }
  
  return 0;  
}
#endif
