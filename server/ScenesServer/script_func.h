#ifndef _script_func_h
#define _script_func_h

#include <string>
#include <vector>
#include "zType.h"

class SceneUser;

SceneUser* me();
void set_me(SceneUser* user);

////////////////////////////使用全局变量存储场景服务最近处理过的10条消息////////////////////////////////////////
struct RecentMsg
{
    int cmd;
    int para;
    int size;
    unsigned long long time;
    char name[MAX_NAMESIZE];
    RecentMsg()
    {
	cmd = 0;
	para = 0;
	size = 0;
	time = 0;
	bzero(name, sizeof(name));
    }
};
void updateMsgArray(int cmd, int para, int size, unsigned long long time/*毫秒时间戳*/, char *name);

//////////////////////////////////////////////end of //////////////////////////////////////
bool sys(SceneUser* target,int type,const char* msg);
void globalSys(SceneUser* sender,const char* msg);
//void show_dialog(SceneNpc* npc,const char* menu);
void equip_make(SceneUser* user,DWORD thisid,bool drop,int flag);
//zObject *CreateObject(DWORD objID,DWORD level,DWORD quantity);
//zObject *getObjByTempidFromPackage(SceneUser* user,DWORD thisid);
//zObject *getObjByIdFromPackage(SceneUser* user,DWORD id);
void makerName(const char *name,DWORD thisid);
char * getMeterials(DWORD thisid);
DWORD getIdByThisId(DWORD thisid);
void deleteChar(const char *p);
void infoUserObjUpdate(SceneUser *user,DWORD thisid);
void assign_set(DWORD thisid);
class Vars;

void set_var(Vars* vars,const char* name,int value);
void set_varS(Vars* vars,const char* name,const char * value);

int get_var(Vars* vars,const char* name);
const char * get_varS(Vars* vars,const char* name);

//void refresh_status(SceneNpc* npc);

//void refresh_npc(int id);

//int npc_tempid(SceneNpc* npc);

//int npc_id(SceneNpc* npc);

void refresh_quest(int id);



/** brief 提供对脚本中全局变量的支持
  
*/
class GlobalVars
{
public:
  static Vars* add_g();
  static Vars* add_t();
  static Vars* add_f();
};

bool check_money(SceneUser* user,int money);
bool remove_money(SceneUser* user,int money);
void add_money(SceneUser* user,int money);

bool have_ob(SceneUser* user,int id,int num,int level,int type);
DWORD get_ob(SceneUser* user,int id,int level );
bool del_ob(SceneUser* user,DWORD id);
int  space(SceneUser* user);

Vars * get_familyvar(SceneUser* user,int dummy);
Vars * get_uservar(SceneUser* user,int dummy);
Vars * get_tongvar(SceneUser* user,int dummy);

void add_exp(SceneUser* user,DWORD num,bool addPet,DWORD dwTempID,BYTE byType,bool addCartoon);
int get_time();
double diff_time(int,int);
bool gomap(SceneUser *pUser,const char *para);
bool goTo(SceneUser *pUser,const char *para);
bool addExp(SceneUser *pUser,const char *para);
//extern NFilterModuleArray g_nFMA;

#endif

