#ifndef _QUEST_H_
#define _QUEST_H_
#include "zType.h"
#include "Command.h"
#include "zTime.h"
#include <ext/hash_map>
#include <list>
#include <string>
#include <fstream>
#include <map>

class SceneUser;

#define HAS_TASK_DIALOG "function IsHasTask()\n\treturn true\nend\n"
#define NO_TASK_DIALOG "function IsHasTask()\n\treturn false\nend\nfunction TaskDialog()\nend\n"
/**
 * \brief 任务
 *
 * 该类封装了对任务的抽象
 *
 */
class Quest
{
public:
  enum {
    FINISHED_SAVE = -1,
    FINISHED_NOT_SAVE = -2,
    FINISHED = -3,
    DOING = 0,
  };
  
  static bool execute(SceneUser& user,Cmd::stQuestUserCmd* cmd,DWORD len);
  
  static int notify(SceneUser& user);
  
  static int abandon(SceneUser& user,DWORD id);

  /**     
   * \brief  返回任务名称
   *
   * \return 任务名称
   */  
  const std::string& title() const
  {
    return _title;
  }
  
  /**     
   * \brief  返回任务描述
   *
   * \return 任务描述
   */  
  const std::string& description() const  
  {
    return _description;
  }

  static const std::string FINISHED_NAME;
    
private:
  friend class QuestTable;
    
  DWORD _id;
  
  std::string _title;
  std::string _description;
};

/**
 * \brief 任务变量
 *
 * 该类封装了对任务变量的抽象
 *
 */
class Vars
{
public:
  //typedef   void (*callback_func) ();
  struct VarsCallback
  {
      virtual bool exec(const std::string& name)=0;
      virtual ~VarsCallback() {}
  };
  /**     
   * \brief  构造函数
   *
   * 初始化相关变量
   *      
   * \param quest_id: 任务id
   */      
  Vars(DWORD quest_id = 0) : _vars(50),_quest_id(quest_id),_timeout(0),_start_time(0),_update(0)
  { }
  
  /**     
   * \brief 析构函数
   *
   */    
  ~Vars()
  { }

#if 0
  void callback(callback_func func)
  {
    _func = func;
  }
#endif

  void doEvery(VarsCallback& callback) const
  {
      const_var_iterator iter=_vars.begin();
      for(;iter != _vars.end(); ++iter)
      {
	  if(!callback.exec(iter->first))
	      break;
      }
  }
 
  void del_var(const std::string& var)
  {
      var_iterator it=_vars.find(var);
      if(it != _vars.end())
      {
	  Zebra::logger->debug("删除变量 %s,值:%u",var.c_str(), atoi(it->second.value().c_str()));
	  _vars.erase(it);
      }
      else
      {
	  Zebra::logger->debug("删除变量%s 未找到",var.c_str());
      }
  }
  template<typename T>
  bool get_value(const std::string& name,T& value)
  {
      var_iterator it = _vars.find(name);
      if (it != _vars.end()) {
	  if(name == "")
	  {
	      Zebra::logger->error("试图从任务变量为空的var中取值，任务ID:%u",_quest_id);
	  }
	  else
	  {
	      std::stringstream os(it->second.value());
	      os >> value;
	  }
	  return true;
      }

      return false;
  }
  /**     
   * \brief  设置变量
   *
   * 根据Operation类型设置变量值
   *      
   * \param op: 操作类型
   * \param name: 变量名称
   * \param action: 要改变的变量值
   * \param tmp: 变量是否需要存储
   * \return 当前总是返回0
   */  
  template <typename Operation>
  int set_value(Operation op,const std::string& name,typename Operation::value_type const & action,int tmp=0,SceneUser *user=NULL)
  {
    std::stringstream os;
    
    var_iterator it = _vars.find(name);
    if (it != _vars.end()) {
      os << it->second.value();
      tmp = it->second.tmp();

    }else {
      //haven't this var before,give a default value  
      os << 0;
    }
        
    typename Operation::value_type value;
    os >> value;

    //value=action;
    op(value,action,user);

    os.clear();
    
    os << value;
    std::string new_value;
    os >> new_value;

    if(name == "")
    {
	Zebra::logger->error("试图给任务变量为空的var中赋值，任务ID:%u",_quest_id);
    }
    else
    {
	_vars[name] = VAR(new_value,tmp);
    }
    return 0;
  }

  /**     
   * \brief  判断变量是否有效
   *
   * 根据Operation类型判断变量值是否满足条件要求
   *      
   * \param op: 操作类型
   * \param name: 变量名称
   * \param condition: 测试值
   * \return 有效返回true,否则返回false
   */    
  template <typename Operation>
  bool is_valid(Operation op,const std::string& name,typename Operation::value_type const & condition) const
  {
    const_var_iterator it = _vars.find(name);
    if (it != _vars.end()) {
      std::stringstream os(it->second.value());
      
      typename Operation::value_type value;
      os >> value;
            
      return op(value,condition);
    }
    

    return !Operation::NEED_EXIST;      
  }
  
  /**     
   * \brief  返回任务id
   *
   * \return 任务id
   */  
  DWORD quest_id() const
  {
    return _quest_id;
  }
  
  int set_timer();
  int set_timer(int start);
  
  bool is_timeout(int timeout) const;
  int start_time() const;
  
  int save_timer(BYTE* dest) const;
  int load_timer(BYTE* dest);

  int save(BYTE* dest) const;
  int save(std::vector<unsigned char> &buf, int &len) const;
  int load(BYTE* dest);
  
  int notify(SceneUser& user) const;
  int notify(SceneUser& user,const std::string& name) const;
  
  int state() const;

  int update() const;
  void update(int value);

  std::string info() const;

  bool reserve(); 
private:
  class VAR
  {
  public:
    VAR(const std::string& value="",int tmp=0) : _value(value),_tmp(tmp)
    { }
    
    ~VAR()
    { }
    
    const std::string& value() const
    {
      return _value;
    }

    const int state() const
    {
	return _state;
    }

    const void state(int state)
    {
	_state = state;
    }
    
    bool is_tmp() const
    {
      return _tmp==1;  
    }
    
    bool tmp() const
    {
      return _tmp;
    }
    
  private:
    std::string _value;
    int _state;
    int _tmp;  
  };  
  struct key_hash : public std::unary_function<const std::string,size_t>
  {
    size_t operator()(const std::string &x) const
    {
	__gnu_cxx::hash<const char *> H;
      return H(x.c_str());
    }
  };
  typedef __gnu_cxx::hash_map<std::string, VAR, key_hash> VARS;
  typedef VARS::iterator var_iterator;
  typedef VARS::const_iterator const_var_iterator;  
    
  VARS _vars;  
  DWORD _quest_id;
  int _timeout;
  int _start_time;
  int _update;

  //callback_func _func;
};

/**
 * \brief 全局变量
 *
 * 该类封装了对全局任务变量的抽象
 *
 */

template <int dummy>
class GlobalVarImpl
{
public:
  typedef GlobalVarImpl<dummy> self_t;
  enum {
    MAX_BUF_SIZE = 4096*8,
  };
  
  static self_t& instance()
  {
    if (!_instance) {
      /***********************
      static self_t new_instance; 
      _instance = &new_instance;
      ************************/
      //here can not use as above
      _instance = new self_t;
    }
    
    return *_instance;
  }    

  Vars* vars(QWORD id) const
  {
    const_vars_iterator it = _vars.find(id);
    if   (it != _vars.end() ) {
      return it->second;
    }
    
    return NULL;  
  }

  Vars* add(QWORD id)
  {
    Vars* v = vars(id);

    if (!v) {
      v = new Vars(id);
      _vars[id] = v;
    }

    return v;
  }

  void update()
  {
    time_t t = time(NULL);
    const struct tm * const now = localtime(&t);

    if (_day != now->tm_mday) {
      _day = now->tm_mday;
      destroy();
    }

  }

  bool save() const
  {
    std::ofstream of(_file.c_str(),std::ios::binary);
    int count = _vars.size();
    saveBuf.clear();
    memcpy(&saveBuf[0], &count, sizeof(count));
    int len = sizeof(count);
    
    for (const_vars_iterator it=_vars.begin(); it!=_vars.end(); ++it) 
    {
	if(saveBuf.capacity() < (DWORD)(len+MAX_BUF_SIZE))
	{
	    saveBuf.resize(len + MAX_BUF_SIZE*5);
	}
	QWORD id = it->first;
	memcpy(&saveBuf[len], &id, sizeof(id));
	len += sizeof(id);
	it->second->save(saveBuf, len);
    }

    if (len > 0) of.write((char*)(&saveBuf[0]), len);
      
    of.flush();
    of.close();
    
    return true;
  }

  bool load()
  {
    bool ret = false;
    std::ifstream inf(_file.c_str(),std::ios::binary);

    //获得文件大小
    inf.seekg(0,std::ios::end);
    int length = inf.tellg();
    inf.seekg(0,std::ios::beg);

    if (length > 0)
    {
      char *buf = new char[length];
      if (buf)
      {
        bzero(buf,length);

        inf.read(buf,length);
        int size =*((int*)buf);
        int len = sizeof(int);

        while (size-- > 0) {
          QWORD id = *((QWORD*)(buf+len));
          len += sizeof(id);
          Vars* vars = new Vars;
          len += vars->load((BYTE*)buf+len);
          _vars[id] = vars;
        }
        SAFE_DELETE_VEC(buf);
        ret = true;
      }
      else
        Zebra::logger->fatal("加载任务变量文件分配内存失败：%u",length);
    }

    inf.close();
    return ret;
  }

/*
  void set(QWORD id,const std::string& name,int value)
  {
    Vars* v = vars(id);

    if (!v) {
      v = new Vars(id);
      _vars[id] = v;
    }

    Op::Set<int> op;
    v->set_value(op,name,value);
  }
*/

  std::string show() const
  {
    std::ostringstream os;
    for (const_vars_iterator it=_vars.begin(); it!=_vars.end(); ++it) {
      os << "变量(" << it->first << ")" << "\n";
      os << it->second->info() << "\n";
    }
    return os.str();
  }

  static void server_id(int id) 
  {
    SERVER_ID = id;
  }

  static int server_id()
  {
    return SERVER_ID;
  }
  
private:  
  void destroy()
  {  
    for (vars_iterator it=_vars.begin(); it!=_vars.end(); ) {
      if (it->second->reserve())
      {
	  ++it;
      }
      else 
      {
        SAFE_DELETE(it->second);
        _vars.erase(it++);
      }
    }
    saveBuf.clear();
  }
  
  /**     
   * \brief  构造函数
   *
   * 初始化相关变量
   *      
   */ 
  GlobalVarImpl()
  { 
    time_t t = time(NULL);
    struct tm now;
    zRTime::getLocalTime(now, t);
    t+=60;
    _day = now.tm_mday;
    _week = now.tm_wday;

    std::ostringstream os;
//    os << ScenesService::getInstance().getServerID();
    os << Zebra::global["vardir"];
    os << SERVER_ID << "_";
    switch(dummy)
    {
	case 5:
	    os << "countryVar";
	    break;
	default:
	    os << dummy;
	    break;
    }
    os << ".sv";
    _file = os.str();
    saveBuf.resize(MAX_BUF_SIZE*100);
  }
  
  /**     
   * \brief 析构函数
   *
   */    
  ~GlobalVarImpl()
  { 
    destroy();
  }
  
  typedef std::map<QWORD,Vars*> VARS;  //
  typedef VARS::iterator vars_iterator;
  typedef VARS::const_iterator const_vars_iterator;
  
  VARS _vars;
  
  static self_t* _instance;
  int _day;
  int _week;
  std::string _file;

  static int SERVER_ID;
  mutable std::vector<unsigned char> saveBuf;
};

template <int dummy>
GlobalVarImpl<dummy>* GlobalVarImpl<dummy>::_instance = NULL;

template <int dummy>
int GlobalVarImpl<dummy>::SERVER_ID = 0;

typedef GlobalVarImpl<1> GlobalVar;
typedef GlobalVarImpl<2> TongVar;
typedef GlobalVarImpl<3> FamilyVar;
typedef GlobalVarImpl<4> UsersVar;
typedef GlobalVarImpl<5> CountrysVar;
typedef GlobalVarImpl<6> TeamVar;
typedef GlobalVarImpl<7> NewGlobalVar;
typedef GlobalVarImpl<8> WeekVar;

#define ALLVARS(function) \
  GlobalVar::instance().function(); \
  TongVar::instance().function(); \
  FamilyVar::instance().function(); \
  UsersVar::instance().function(); \
  UserVar::instance().function(); \
  CountrysVar::instance().function(); \
  TeamVar::instance().function(); \
  WeekVar::instance().function();

/*************************************************************
NOTE: This macro used initial the filename,according by server id,
           it must be call FIRST,even BEFORE the INSTANCE.
*************************************************************/
#define ALLVARS1(function,arg1) \
  GlobalVar::function(arg1); \
  TongVar::function(arg1); \
  FamilyVar::function(arg1); \
  CountrysVar::function(arg1); \
  UsersVar::function(arg1); \
  TeamVar::function(arg1); \
  UserVar::function(arg1); \
  NewGlobalVar::function(); \
  WeekVar::function();


/**
 * \brief 用户变量
 *
 * 该类封装了对用户任务变量的抽象
 *
 */
class UserVar
{
public:
  enum {
    MAX_BUF_SIZE = 4096*8,
  };
  static UserVar& instance();

  Vars* vars(DWORD id,QWORD key) const;
  
  Vars* add(DWORD id,QWORD key);

  void update()
  {
//    time_t t = time(NULL);
//    const struct tm * const now = localtime(&t);
//
//    if (_day != now->tm_mday) {
//      _day = now->tm_mday;
//      destroy();
//    }

  }

  bool save() const;
  bool load();

/*
  void set(QWORD id,const std::string& name,int value)
  {
    Vars* v = vars(id);

    if (!v) {
      v = new Vars(id);
      _vars[id] = v;
    }

    Op::Set<int> op;
    v->set_value(op,name,value);
  }
*/

  std::string show() const
  {
    std::ostringstream os;
    for (const_vars_iterator it=_vars.begin(); it!=_vars.end(); ++it) {
      os << "变量(" << it->first << ")" << "\n";
      //os << it->second->info() << "\n";
    }
    return os.str();
  }

  static void server_id(int id) 
  {
    SERVER_ID = id;
  }

  static int server_id()
  {
    return SERVER_ID;
  }
private:  
  /**     
   * \brief  构造函数
   *
   * 初始化相关变量
   *      
   */ 
  UserVar() : _vars(50)
  {
    std::ostringstream os;
//    os << ScenesService::getInstance().getServerID();
    os << Zebra::global["vardir"];
    os << SERVER_ID << "_";
    os << "userv" << ".sv";
    _file = os.str();
  }
  
  /**     
   * \brief 析构函数
   *
   */    
  ~UserVar()
  { }
  
  class VAR
  {
  public:  
    Vars* vars(QWORD key) const
    {
      const_var_iterator it = _vars.find(key);
      if   (it != _vars.end() ) {
        return it->second;
      }
      
      return NULL;
    }    
    
    Vars* add(DWORD id,QWORD key)
    {
      Vars* v = vars(key);
      if (!v) v = new Vars(id);
    
      _vars[key] = v;
    
      return v;        
    }
    
    /**     
     * \brief 存储变量
     *
     *存储任务变量
     *      
     * \param dest: 任务档案
     * \return 存储的二进制档案长度
     */
    int save(BYTE* dest) const;

    /**     
     * \brief 读取变量
     *
     *读取任务变量
     *      
     * \param dest: 任务档案
     * \return 读取的二进制档案长度
     */
    int load(BYTE* dest);
  private:
    std::map<QWORD,Vars*> _vars;
    typedef std::map<QWORD,Vars*>::const_iterator const_var_iterator;
  };
  
  typedef __gnu_cxx::hash_map<DWORD,VAR*> VARS;  //
  typedef VARS::iterator vars_iterator;
  typedef VARS::const_iterator const_vars_iterator;
  std::string _file;
  
  VARS _vars;
  
  static UserVar* _instance;
  static int SERVER_ID;
};

/**
 * \brief 任务列表
 *
 * 该类封装了对用户身上的任务列表的操作
 *
 */
class QuestList
{
public:
  enum {
    MAX_NUM = 20,
  };
  
  /**     
   * \brief  构造函数
   *
   * 初始化相关变量
   *      
   */ 
  QuestList() 
  { }

  /**     
   * \brief 析构函数
   *
   */    
  ~QuestList()
  { }
  
  Vars* vars(DWORD id) const;
  
  void add_quest(DWORD id,const Vars& vars,SceneUser& user,bool notify = true);
  int count() const;
  
  bool set_menu(const std::string& menu, const Vars* vars);
  int get_menu(char* menu,int& status);
  
  void add_menu(const std::string& menu);
  
  int save(BYTE* dest) const;
  //int load(BYTE* dest,unsigned long &dest_size);
  int load(BYTE* dest);

  int state(DWORD id) const;
  int start_time(DWORD id) const;
  
  int notify(SceneUser& user) const;
  
  int abandon(SceneUser& user,DWORD id,bool force = false,bool destroy = true);

  // 清除所有任务
  void clear(SceneUser* pUser);

  int update(SceneUser& user,bool refresh);

  std::string info(int id=0) const;

private:
  
  //std::string _menu;
  typedef std::map<DWORD, std::string> MENU;
  typedef MENU::iterator quest_menu;
  typedef MENU::const_iterator const_quest_menu;
  MENU _menu;

  std::list<std::string> _subs;
  
  typedef __gnu_cxx::hash_map<DWORD,Vars> QUESTS;  //questid,vars
  typedef QUESTS::iterator quest_iterator;
  typedef QUESTS::const_iterator const_quest_iterator;
  
  QUESTS _quests;

};
#endif

