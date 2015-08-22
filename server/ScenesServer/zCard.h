#ifndef _zCard_h_
#define _zCard_h_
#include "zDatabase.h"
#include "SaveObject.h"
#include "CardManager.h"
#include "zTime.h"
#include "zSceneEntry.h"
#include "Card.h"
#include "HeroCardCmd.h"
#include "SkillStatusManager.h"

class ChallengeGame;

struct zCard:zEntry
{
  friend class GlobalCardIndex;
  public:
    t_Card data;
    zCardB *base;
    t_CardPK pk;

    /////////////////////////π‚ª∑ Ù–‘º«¬º//////////////////////////////////////
    std::map<DWORD, t_haloInfo>	haloInfoMap;	    //ø®≈∆qwThisID<---->t_haloInfo
    /////////////////////////end of π‚ª∑ Ù–‘º«¬º/////////////////////////////////////////////////

    static zCard *create(zCardB *objbase,DWORD gameID,BYTE level = 0);
    static void destroy(zCard*& ob);
    static void logger(QWORD createid,DWORD objid,char *objname,DWORD num,DWORD change,DWORD type,DWORD srcid,char *srcname,DWORD dstid,char *dstname,const char *action,zCardB *base,BYTE kind, DWORD gameID);
    static zCard *create(zCard *objsrc);
    static zCard *load(const SaveObject *o);
    bool getSaveData(SaveObject *save);
    
    const stObjectLocation &reserve() const;
    void restore(const stObjectLocation &loc);
      
    union
    {
      struct 
      {
        DWORD createtime;
        DWORD dwCreateThisID;  
      };
      QWORD createid;
    };

  private:
    friend class CardSlots;
    friend class CardSlot;
    friend class BattleSlot;
    friend class HandSlot;
    friend class EquipSlot;

    zCard();
    ~zCard();

    bool free() const;
    void free(bool flag);

    void fill(t_Card& d);    
    void generateThisID();
    bool inserted;
  public:
    DWORD playerID;		//πÈ Ù
    DWORD opTimeSequence;	//≤Ÿ◊˜ ±º‰–Ú¡–
    DWORD gameID;
    DWORD playingTime;		//…œ≥° ±º‰
  public:
    //////////////////////////////////ø®≈∆’Ω∂∑œ‡πÿ/////////////////////////////////////////////
    bool issetCardState(const int state)
    {
	return Cmd::isset_state(data.state, state);
    }
    bool setCardState(const int state)
    {
	if(!issetCardState(state))
	{
	    Cmd::set_state(data.state, state);
	    Zebra::logger->debug("[…Ë÷√◊¥Ã¨] state:%u", state);
	    return true;
	}
	return false;
    }
    bool clearCardState(const int state)
    {
	Cmd::clear_state(data.state, state);
	return true;
    }
    ////////////////////////////“ªœµ¡–µƒcondition//////////////////////////////////////////
    bool injured();		// ‹…Àµƒ
    bool damageGreat2();	//π•ª˜¡¶¥Û”⁄µ»”⁄2
    bool damageLess3();		//π•ª˜¡¶–°”⁄3

    void addAttackTimes();
    BYTE getAttackTimes();
    
    bool hasMagic();
    bool addMagic(DWORD skillID);
    bool clearMagic();

    bool hasShout();
    bool addShout(DWORD skillID);
    bool clearShout();

    bool hasDeadLanguage();
    bool addDeadLanguage(DWORD skillID);
    bool clearDeadLanguage();

    bool hasRoundSID();
    bool addRoundSID(DWORD skillID, bool tmp=false);
    bool clearRoundSID();

    bool hasRoundEID();
    bool addRoundEID(DWORD skillID, bool tmp=false);
    bool clearRoundEID();

    bool hasEnemyRoundSID();
    bool addEnemyRoundSID(DWORD skillID, bool tmp=false);
    bool clearEnemyRoundSID();

    bool hasEnemyRoundEID();
    bool addEnemyRoundEID(DWORD skillID, bool tmp=false);
    bool clearEnemyRoundEID();

    bool isDie();
    bool isAwake();
    bool setAwake();
    bool isCharge();
    bool clearCharge();
    bool hasTaunt();
    bool clearTaunt();
    bool hasWindfury();
    bool clearWindfury();
    bool isSneak();
    bool breakSneak();
    bool hasShield();
    bool breakShield();
    bool isFreeze();
    bool clearFreeze();
    bool setFreeze();
    void resetAttackTimes();
    bool checkAttackTimes();
    bool isHero();
    bool isAttend();
    bool isEquip();
    bool isMagicCard();
    bool isHeroMagicCard();
    bool hasDamage();
    void setDamage(DWORD dam);
    bool addDamage(DWORD dam);
    bool subDamage(DWORD dam);
    bool multiplyDamage(DWORD para);
    bool multiplyHpBuff(DWORD para);
    bool addMagicDam(DWORD dam);
    void silentMe();
    bool hasArmor();
    bool addArmor(DWORD value);
    bool addDur(DWORD value);
    bool subDur(DWORD value = 1);
    void restoreLife(DWORD hp=0);
    bool toDie();
    bool addTaunt();
    bool subTaunt();
    bool setHpBuff(DWORD hp);
    bool addHpBuff(DWORD hp);
    bool clearHpBuff(DWORD hp);
    bool subMaxHp(DWORD hp);
    bool exchangeHp(zCard* pDef);
    bool exchangeHpDamage();
    bool addMpCost(DWORD mp);
    bool subMpCost(DWORD mp);
    
    bool isEquipOpen();
    bool addCharge();
    bool subCharge();
    bool addShield();
    bool addWindfury();
    bool subWindfury();
    bool addSneak();
    bool canNotAsFashuTarget();	    //≤ªƒ‹±ª◊˜Œ™∑® ı≈∆∫Õ”¢–€ººƒ‹µƒƒø±Í(ƒß√‚)
    bool hasHaloID();
    DWORD getHaloDamage();
    DWORD getHaloMaxHp();
    DWORD getHaloIncMpCost();
    DWORD getHaloDecMpCost();
    bool preAttackMe(zCard *pAtt, const Cmd::stCardAttackMagicUserCmd *rev=NULL);
    bool AttackMe(zCard *pAtt, const Cmd::stCardAttackMagicUserCmd *rev);
    DWORD directDamage(zCard *pAtt, zCard *pDef, DWORD hp, const BYTE addMagic=0, bool isnormal=false);
    DWORD doNormalPK(zCard *pAtt, zCard *pDef);
    bool processDeath(zCard *pAtt, zCard*& pDef);
    bool refreshCard(DWORD pAttThisID);
    bool isEnemy(zCard* entry);
    bool drawCards(DWORD num, const DWORD dwSkillID);		//≥È»°num’≈≈∆
    bool addHeroMp(DWORD value);		//‘ˆº”∑®¡¶ÀÆæß
    bool addHeroMaxMp(DWORD value);		//‘ˆº”∑®¡¶ÀÆæß…œœﬁ
    bool summonAttend(DWORD cardID, DWORD num, DWORD dwMagicType);	//’ŸªΩnum∏ˆcardIDÀÊ¥”
    bool randomDropHand(DWORD playerID, DWORD num);		//ÀÊª˙∂™∆˙num∏ˆ ÷≈∆
    
    SkillStatusManager skillStatusM;
    SkillStatusCarrier carrier;

    void showCurrentEffect(const DWORD state,bool isShow);
    bool addOneHaloInfo(DWORD pAttThisID, t_haloInfo info);
    bool clearOneHaloInfo(DWORD pAttThisID);	//«Â≥˝(÷ª«Â≥˝ Ù–‘)
    bool removeOneHaloInfo(DWORD pAttThisID);	//…æ≥˝(«Â≥˝ Ù–‘+…æ≥˝»›∆˜‘™Àÿ)
};

struct CardSlotCallback
{
  virtual bool exec(zCard * o)=0;
  virtual ~CardSlotCallback(){};
};

class CardSlot:private zNoncopyable
{
  public:
    zCard** container;

  protected:
    virtual bool add(zCard *object,bool find);
    virtual bool remove(zCard *object);
    virtual bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);    
  public:
    CardSlot(WORD type,DWORD id,WORD w,WORD h);
    virtual ~CardSlot();

    virtual bool getObjectByZone(zCard **ret,WORD x,WORD y);
    virtual bool getObjectByID(zCard **ret,DWORD id);
    virtual void execEvery(CardSlotCallback &callback);

    virtual WORD space() const;
    virtual WORD size() const;
    virtual WORD getObjectNum() const;
    virtual WORD getObjectNumByBaseID(DWORD) const;
    WORD type() const;
    DWORD id() const;
    
    void setSpace(WORD);
    virtual bool getObjectByRandom(zCard **ret);
    virtual DWORD getLeftObjectThisID(WORD x, WORD y);
    virtual DWORD getRightObjectThisID(WORD x, WORD y);
  private:
    friend class CardSlots;
    
    virtual void removeAll();  
    bool find_space(WORD &x,WORD &y) const;
    virtual int position(WORD x,WORD y) const;

    virtual WORD width() const
    {
	return _width;
    }
    virtual WORD height() const
    {
	return _height;
    }
  public:
    virtual bool sort();
    virtual DWORD typeWeight(WORD type);
    virtual bool needSwap(zCard* ob1, zCard* ob2);
    virtual bool swap(WORD index1, WORD index2);

    virtual bool trim(SceneUser* pUser, WORD startIndex=0);

    WORD _type;
    DWORD _id;
    WORD _width;
    WORD _height;

    WORD _space;
    WORD _size;
    
};

#define MAX_TAB_NUM 5
#define MIN_TAB_NUM 2

/**
 * \brief   ÷˜’Ω≥°ø®≤€
*/
class BattleSlot:public CardSlot
{
    public:
	enum {
	    WIDTH = 5,
	    HEIGHT = 1,
	};

	BattleSlot();
	~BattleSlot();

	BYTE TabNum; //sky Áé©ÂÆ∂ÂèØ‰ª•‰ΩøÁî®ÁöÑÂåÖË£πÈ°µÊï∞

	bool getObjectByZone(zCard **ret,WORD x,WORD y);

	bool skillReduceObject(SceneUser* pThis,DWORD kind,DWORD num);

    private:
	bool add(zCard *object,bool find);
	bool remove(zCard *object);
	bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);

};

/**
 * \brief    ÷≈∆ø®≤€
*/
class HandSlot:public CardSlot
{
    public:
	enum {
	    WIDTH = 10,
	    HEIGHT = 1,
	};

	HandSlot();
	~HandSlot();

	BYTE TabNum; //sky Áé©ÂÆ∂ÂèØ‰ª•‰ΩøÁî®ÁöÑÂåÖË£πÈ°µÊï∞

	bool getObjectByZone(zCard **ret,WORD x,WORD y);

	bool skillReduceObject(SceneUser* pThis,DWORD kind,DWORD num);

    private:
	bool add(zCard *object,bool find);
	bool remove(zCard *object);
	bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);

};

/**
 * \brief   Œ‰∆˜ø®≤€
*/
class EquipSlot:public CardSlot
{
    public:
	enum {
	    WIDTH = 1,
	    HEIGHT = 1,
	};

	EquipSlot();
	~EquipSlot();

	BYTE TabNum; //sky Áé©ÂÆ∂ÂèØ‰ª•‰ΩøÁî®ÁöÑÂåÖË£πÈ°µÊï∞

	bool getObjectByZone(zCard **ret,WORD x,WORD y);

	bool skillReduceObject(SceneUser* pThis,DWORD kind,DWORD num);

    private:
	bool add(zCard *object,bool find);
	bool remove(zCard *object);
	bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);

};

/**
 * \brief   ººƒ‹ø®≤€
*/
class SkillSlot:public CardSlot
{
    public:
	enum {
	    WIDTH = 1,
	    HEIGHT = 1,
	};

	SkillSlot();
	~SkillSlot();

	BYTE TabNum; //sky Áé©ÂÆ∂ÂèØ‰ª•‰ΩøÁî®ÁöÑÂåÖË£πÈ°µÊï∞

	bool getObjectByZone(zCard **ret,WORD x,WORD y);

	bool skillReduceObject(SceneUser* pThis,DWORD kind,DWORD num);

    private:
	bool add(zCard *object,bool find);
	bool remove(zCard *object);
	bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);

};

/**
 * \brief   ”¢–€ø®≤€
*/
class HeroSlot:public CardSlot
{
    public:
	enum {
	    WIDTH = 1,
	    HEIGHT = 1,
	};

	HeroSlot();
	~HeroSlot();

	BYTE TabNum; //sky Áé©ÂÆ∂ÂèØ‰ª•‰ΩøÁî®ÁöÑÂåÖË£πÈ°µÊï∞

	bool getObjectByZone(zCard **ret,WORD x,WORD y);

	bool skillReduceObject(SceneUser* pThis,DWORD kind,DWORD num);

    private:
	bool add(zCard *object,bool find);
	bool remove(zCard *object);
	bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);

};

/**
 * \brief   ƒπµÿø®≤€
*/
class TombSlot:public CardSlot
{
    public:
	enum {
	    WIDTH = 500,
	    HEIGHT = 1,
	};

	TombSlot();
	~TombSlot();

	BYTE TabNum; //sky Áé©ÂÆ∂ÂèØ‰ª•‰ΩøÁî®ÁöÑÂåÖË£πÈ°µÊï∞

	bool getObjectByZone(zCard **ret,WORD x,WORD y);

	bool skillReduceObject(SceneUser* pThis,DWORD kind,DWORD num);

    private:
	bool add(zCard *object,bool find);
	bool remove(zCard *object);
	bool checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y);

};

template<typename T>
class Type2Type2
{
public:
  typedef T BASE;
};


class CardSlots
{
  private:
    ChallengeGame *owner;

    CardSlot** getCardSlot(int packs);
    
  public:
    enum {
      MAIN1_PACK = 1,
      MAIN2_PACK = 2,
      HAND1_PACK = 4,
      HAND2_PACK = 8,
      EQUIP1_PACK = 16,
      EQUIP2_PACK = 32,
      SKILL1_PACK = 64,
      SKILL2_PACK = 128,
      HERO1_PACK = 256,
      HERO2_PACK = 512,
      TOMB_PACK	= 1024,

    };
    
    GameCardM gcm;	//“ª∏ˆ’Ωæ÷÷–µƒÀ˘”–ø®π‹¿Ì
    //1∑Ωø®≤€
    BattleSlot main1;	//÷˜’Ω≥°≤€
    HandSlot hand1;	// ÷≈∆≤€
    EquipSlot equip1;	//Œ‰∆˜≤€
    SkillSlot skill1;	//ººƒ‹≤€
    HeroSlot hero1;	//”¢–€≤€

    //¡ÌÕ‚“ª∑Ω
    BattleSlot main2;	
    HandSlot hand2;
    EquipSlot equip2;
    SkillSlot skill2;	
    HeroSlot hero2;

    TombSlot record;	//ƒπµÿ

    CardSlots(ChallengeGame* _owner);
    ~CardSlots();
    
    bool moveObject(SceneUser *pUser,zCard*& srcObj,stObjectLocation &dst);
    bool moveObjectToScene(zCard *o,const zPos &pos,DWORD overdue_msecs=0,const unsigned long dwID=0);
    bool removeObject(DWORD playerID, zCard*& srcObj,bool notify = true,bool del = true, BYTE opType=0);
    bool addObject(zCard *srcObj,bool needFind,int packs = 0);
    bool replaceObject(DWORD playerID, zCard*& oldObj, DWORD newObjID);
    CardSlot* getCardSlot(DWORD type,DWORD id);
    ChallengeGame *getOwner()
    {
      return owner;
    }
    
    template<typename T>
    void execEvery(CardSlot* pack,T t)
    {
      typename T::BASE cb(this);
      pack->execEvery(cb);
    }
    
    template<typename T,typename P1>
    void execEvery(CardSlot* pack,T t,P1 p1)
    {
      typename T::BASE cb(this,p1);
      pack->execEvery(cb);
    }
    
    zCard *getGold();
    DWORD getGoldNum();
};


#endif

