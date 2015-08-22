#ifndef _SKILLSTATUS_MANAGER_H_
#define _SKILLSTATUS_MANAGER_H_
#include "zDatabase.h"
#include "zSceneEntry.h"
#include "HeroCardCmd.h"

class zCard;

/// 技能善恶类型枚举
enum {
  SKILL_GOOD=0,
  SKILL_BAD=1
};

/// 技能状态类型枚举
enum {
  SKILL_TYPE_INITIAVITE=1,// 攻击技能
  SKILL_TYPE_RECOVERY=2,  // 临时被动技能
  SKILL_TYPE_PASSIVENESS=3  // 永久被动技能
};

/// 技能状态处理返回值枚举
enum {
  SKILL_ACTIVE  =  1,//  加到活动MAP中
  SKILL_RECOVERY,    //  加到临时被动MAP中
  SKILL_PASSIVENESS,  //  加到永久被动MAP中
  SKILL_RETURN,    //  不加到任何MAP中
  SKILL_BREAK,    //  不继续投放操作
};

/// 技能状态执行步骤枚举
enum {
  ACTION_STEP_DOPASS=  0,	//δʹ��
  ACTION_STEP_START=  1,	//�ͷ� 
  ACTION_STEP_TIMER,		//ʱ����ѵ
  ACTION_STEP_STOP,		//����ֹͣ
  ACTION_STEP_CLEAR,		//���ⲿ���
  ACTION_STEP_RELOAD,		//δʹ��
};

/// 技能状态最大数目定义
#define SKILLSTATENUMBER 354   /// 技能状态最大编号 [sky]扩充到400

/**
 * \brief  技能状态元素载体
 */
struct SkillStatusCarrier
{
    /// 技能操作
    const SkillStatus *status;
    /// 技能字典
    const zSkillB *skillbase;
    /// 收到的攻击消息
    Cmd::stCardAttackMagicUserCmd revCmd;
    /// 攻击者的指针
    zCard *attacker;
    /**
     * \brief  构造函数，初始化所有属性
     */
    SkillStatusCarrier()
    {
      status = NULL;
      skillbase = NULL;
      attacker = NULL;
    }
};

/**
 * \brief  技能状态元素
 */
struct SkillStatusElement
{
  /// 状态的 id
  DWORD  id;

  /// 状态发生几率
  DWORD percent;

  /// 状态的影响数值(sky 召唤:怪物ID现在已经超过65535)
  DWORD value;


  DWORD value2;

  /// 状态的持续时间
  QWORD qwTime;

  /// 状态执行的步骤标志
  DWORD state;


  ///攻击者的临时ID;
  DWORD dwThisID;	    //����ΨһID

  ///攻击者的ID
  DWORD dwAttackerID;	    //��ɫCHARID

  ///技能ID
  DWORD dwSkillID;

  ///执行时长
  DWORD dwTime;

  ///�⻷
  BYTE halo;

  ///执行步骤
  BYTE  byStep;
  
  ///善恶类型
  BYTE  byGoodnessType;
  
  ///技能的互斥大类
  BYTE  byMutexType;

  ///是否刷新人物属性1为刷新0为否
  BYTE  refresh;

  ///攻击者的类型
  zSceneEntry::SceneEntryType attacktype;

  /**
   * \brief  技能状态元素构造函数初始化所有属性
   */
  SkillStatusElement()
  {
    id      = 0;          //状态的id;
    percent    = 0;
    value    = 0;
    value2    = 0;
    qwTime    = 0;
    state    = 0;
    dwThisID  = 0;
    dwSkillID  = 0;
    dwTime    = 0;
    byStep    = 0;
    refresh    = 0;
    halo = 0;
  }
};

/**
 * \brief  技能状态管理器
 */
class SkillStatusManager
{
private:
  std::map<DWORD,SkillStatusElement> _activeElement;	    //���õļ���״̬(���Ǳ���Ĭ)
    
  std::map<DWORD,SkillStatusElement> _recoveryElement;	    //�����õļ���״̬(������ʱ���ӣ�ʧЧʱ���)

  std::map<DWORD,SkillStatusElement> _passivenessElement;

  /// 类型定义
  typedef std::map<DWORD,SkillStatusElement>::value_type VALUE_TYPE;

  typedef std::map<DWORD,SkillStatusElement>::const_iterator SkillStatus_ConstIter;
  typedef std::map<DWORD,SkillStatusElement>::iterator SkillStatus_Iter;
  
  static const int MAX_SKILL_STATE_NUM = 4*1024;

  typedef BYTE (* SkillStatusFunc)(zCard *,SkillStatusElement &);
  static SkillStatusFunc s_funlist[MAX_SKILL_STATE_NUM];

  /// 技能状态管理器属主
  zCard *entry;	//����
  bool bclearActiveSkillStatus;
public:
  static void initFunctionList();
  static void initFunctionListUnsafe();

public:
    SkillStatusManager();
    ~SkillStatusManager();
    void initMe(zCard *pEntry);
    //void loadSkillStatus(char *buf,DWORD length);
    void getSelectStates(Cmd::stSelectReturnStatesPropertyUserCmd *buf,unsigned long maxSize);
    void sendSelectStates(zCard *pThis,DWORD state,WORD value,WORD time);
    //void saveSkillStatus(char *buf,DWORD &size);
    bool putOperationToMe(const SkillStatusCarrier &carrier,const bool good = false);
    void putPassivenessOperationToMe(const DWORD skillid,const SkillStatus *pSkillStatus);
    void processPassiveness();
    inline BYTE runStatusElement(SkillStatusElement &element);
    void timer();
    void clearActiveSkillStatus();
    void clearBadActiveSkillStatus();
    void addBadSkillStatusPersistTime(const DWORD &value);
    void addBadSkillStatusPersistTimePercent(const DWORD &value);
    void clearMapElement(const BYTE &byMutexType,std::map<DWORD,SkillStatusElement> &myMap,DWORD dwID,bool notify=true);
    WORD getSaveStatusSize();
    void clearRecoveryElement(DWORD value);
    void clearActiveElement(DWORD value);
    void processDeath();
    void clearSkill(DWORD dwSkillID);
};

#endif

