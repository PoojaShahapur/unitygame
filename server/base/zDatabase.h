/*************************************************************************
 Author: wang
 Created Time: 2014��10��20�� ����һ 19ʱ43��35��
 File Name: base/zDatabase.h
    �ر�ע��:��ǰ�����е� struct xxxxBase �е��κ��ֶδ�С����4�ı�������
 Description: 
 ************************************************************************/
#ifndef _zDatabase_h_
#define _zDatabase_h_

#include "zEntry.h"
#include "zMisc.h"

struct ObjectBase
{
    const DWORD getUniqueID() const
    {
	return id;
    }
    union
    {
	DWORD dwField0;	
	DWORD id;
    };
    char name[64];  
    DWORD maxnum;
    DWORD kind;
    DWORD color;
};

struct zObjectB : public zEntry
{
    DWORD maxnum;
    DWORD kind;	    //����
    typedef std::vector<DWORD> JobVector;
    JobVector jobs;
    BYTE sex;
    WORD level;
    std::vector<DWORD> blus;
    std::vector<DWORD> golds;
    std::vector<DWORD> greens;
    std::vector<DWORD> suits;
    std::vector<DWORD> needobject;

    struct leechdom_t
    {
	BYTE id;
	WORD effect;
	WORD time;
    }leechdom;
    WORD needlevel;
    WORD maxhp;
    WORD maxmp;
    WORD maxsp;


    WORD pdamage;				// ��С������      // ��Ӧ��ƥ �﹥�ӳ�
    WORD maxpdamage;			// ��󹥻���
    WORD mdamage;				// ��С����������  // ��Ӧ��ƥ ħ���ӳ�
    WORD maxmdamage;			// �����������

    WORD pdefence;				// ���            // ��Ӧ��ƥ ����ӳ�
    WORD mdefence;				// ħ��            // ��Ӧ��ƥ ħ���ӳ�
    BYTE damagebonus;			// �˺��ӳ�x% from ���߻�����

    WORD akspeed;				// �����ٶ�
    WORD mvspeed;				// �ƶ��ٶ�
    WORD atrating;				// ������
    WORD akdodge;				// �����
    
    DWORD color;
    struct socket
    {
	WORD odds;
	BYTE min;
	BYTE max;
    }hole;

    BYTE recast;
    BYTE recastlevel;
    WORD recastcost;

    WORD make;
    struct skills
    {
	DWORD id;
	DWORD level;
    };
    skills need_skill;

    struct material
    {
	DWORD gold;
	struct stuff
	{
	    DWORD id;
	    DWORD number;
	    DWORD level;
	};
	std::vector<std::vector<stuff> > stuffs;

    };
    material need_material;

    BYTE setpos;
    DWORD durability;
    DWORD price;
    DWORD crystalPrice;
    DWORD bluerating;
    DWORD goldrating;
    BYTE width;
    BYTE timeeffect;

    union
    {
	DWORD cardpoint;
	DWORD cointtype;
    };
    WORD bang;
    DWORD holyrating;
    DWORD coolSeconds;
    QWORD modelid;
    DWORD impression;
    DWORD effect;
    DWORD dropSendInfo;
    char classify[32];
    WORD useTimesLimitPerDay;
    void fill(ObjectBase &data);
};

//------------------------------------
// NpcBase
//------------------------------------
struct NpcBase
{
    const DWORD getUniqueID() const
    {
	return dwField0;
    }
    DWORD  dwField0;    // ���
    char  strField1[64];    // ����
    DWORD  dwField2;    // ����
    DWORD  dwField3;    // �ȼ�
    DWORD  dwField4;    // distance
    DWORD  dwField5;    // adistance
    DWORD  dwField6;    // hp
#if 0
    DWORD  dwField5;    // ����ֵ

    DWORD  dwField6;    // ��
    DWORD  dwField7;    // ��
    DWORD  dwField8;    // ����
    DWORD  dwField9;    // ����
    DWORD  dwField10;    // ����
    DWORD  dwField11;    // ����

    DWORD  dwField12;    // ��ɫ
    DWORD  dwField13;    // ai
    DWORD  dwField14;    // �ƶ����
    DWORD  dwField15;    // �������
    DWORD  dwField16;    // ��С���������
    DWORD  dwField17;    // ������������
    DWORD  dwField18;    // ��С����������
    DWORD  dwField19;    // �����������
    DWORD  dwField20;    // ��������
    DWORD  dwField21;    // ���е���
    char  strField22[1024];    // ��������
    DWORD  dwField23;    // ��С��������
    DWORD  dwField24;    // ���������
    DWORD  dwField25;    // ��С������
    DWORD  dwField26;    // ��󹥻���
    DWORD  dwField27;    // ����
    char  strField28[4096];    // Я����Ʒ
    DWORD  dwField29;    // ����֮ʯ����
    char  strField30[1024];    // ʹ�ü���
    char  strField31[1024];    // ״̬
    DWORD  dwField32;    // �����
    DWORD  dwField33;    // ������
    DWORD  dwField34;    // ͼƬ
    DWORD  dwField35;    // Ʒ��
    DWORD  dwField36;    // �������
    DWORD  dwField37;    // ֽ����ͼƬ
    char  strField38[64];    // ��Ѫ
    DWORD  dwField39;    // �����Ʊ�־
    DWORD  dwField40;    // �����Ʊ�־
    DWORD  dwField41;    // sky ��Ʒ����
#endif
};

struct CarryObject
{
    DWORD id;
    int   rate;
    WORD colorPara;
    int   minnum;
    int   maxnum;
    CarryObject()
    {
	id = 0;
	rate = 0;
	minnum = 0;
	maxnum = 0;
	colorPara = 0;
    }
};

typedef std::vector<CarryObject> NpcLostObject;

struct NpcCarryObject : private zNoncopyable
{
    NpcCarryObject() {};
    bool set(const char *objects)
    {
	bool retval = true;
	//mlock.lock();
	cov.clear();
	if (strcmp(objects,"0"))
	{
	    std::vector<std::string> obs;
	    Zebra::stringtok(obs,objects,";");
	    for(std::vector<std::string>::const_iterator it = obs.begin(); it != obs.end(); it++)
	    {
		std::vector<std::string> rt;
		Zebra::stringtok(rt,*it,":");
		if (3 == rt.size())
		{
		    CarryObject co;
		    co.id = atoi(rt[0].c_str());
		    co.rate = atoi(rt[1].c_str());
		    std::vector<std::string> nu;
		    Zebra::stringtok(nu,rt[2],"-");
		    if (2 == nu.size())
		    {
			co.minnum = atoi(nu[0].c_str());
			co.maxnum = atoi(nu[1].c_str());
			cov.push_back(co);
		    }
		    else
			retval = false;
		}
		else
		    retval = false;
	    }
	}
	//mlock.unlock();
	return retval;
    }

    /**
     * \brief ��Ʒ���䴦��
     * \param nlo npcЯ����Ʒ����
     * \param value �����ʴ��۱�
     * \param value1 ����������
     * \param value2 ���ӵ���������
     */
    void lost(NpcLostObject &nlo,int value,int value1,int value2,int vcharm,int vlucky,int player_level,int DropRate,int DropRateLevel)
    {
	//mlock.lock();
	if (vcharm>1000) vcharm=1000;
	if (vlucky>1000) vlucky=1000;
	for(std::vector<CarryObject>::const_iterator it = cov.begin(); it != cov.end(); it++)
	{
	    //Zebra::logger->debug("%u,%u,%u,%u",(*it).id,(*it).rate,(*it).minnum,(*it).maxnum);
	    switch((*it).id)
	    {
		case 665:
		    {
			int vrate = (int)(((*it).rate/value)*(1+value1/100.0f)*(1+value2/100.0f)*(1+vcharm/1000.0f)*(1+vlucky/1000.0f));
			if (zMisc::selectByTenTh(vrate))
			{
			    nlo.push_back(*it);
			}
		    }
		    break;
		default:
		    {
			int vrate = (int)(((*it).rate/value)*(1+value1/100.0f)*(1+vcharm/1000.0f)*(1+vlucky/1000.0f));
			if (player_level<= DropRateLevel)
			{
			    if (zMisc::selectByTenTh(vrate * DropRate))
			    {
				nlo.push_back(*it);
			    }
			}
			else
			{
			    if (zMisc::selectByTenTh(vrate))
			    {
				nlo.push_back(*it);
			    }
			}
		    }
		    break;
	    }
	}
	//mlock.unlock();
    }
    /**
     * \brief ȫ����Ʒ���䴦��
     * \param nlo npcЯ����Ʒ����
     * \param value �����ʴ��۱�
     * \param value1 ����������
     * \param value2 ���ӵ���������
     */
    void lostAll(NpcLostObject &nlo)
    {
	for(std::vector<CarryObject>::const_iterator it = cov.begin(); it != cov.end(); it++)
	{
	    nlo.push_back(*it);
	}
    }

    /**
     * \brief װ����Ʒȫ�����䴦��(�̹�ר��)
     * \param nlo npcЯ����Ʒ����
     * \param value �����ʴ��۱�
     * \param value1 ����������
     * \param value2 ���ӵ���������
     */
    void lostGreen(NpcLostObject &nlo,int value=1,int value1=0,int value2=0,int vcharm = 0,int vlucky = 0);
    private:
    std::vector<CarryObject> cov;
    //zMutex mlock;
};


    struct aTypeS{
	aTypeS()
	{
	    byValue[0] = 0;
	    byValue[1] = 0;
	}
	union {
	    struct {
		BYTE byAType;
		BYTE byAction;
	    };
	    BYTE byValue[2];
	};
    };

enum
{
    NPC_TYPE_HUMAN    = 0,///����
    NPC_TYPE_NORMAL    = 1,/// ��ͨ����
    NPC_TYPE_BBOSS    = 2,/// ��Boss����
    NPC_TYPE_LBOSS    = 3,/// СBoss����
    NPC_TYPE_BACKBONE  = 4,/// ��Ӣ����
    NPC_TYPE_GOLD    = 5,/// �ƽ�����
    NPC_TYPE_TRADE    = 6,/// ��������
    NPC_TYPE_TASK    = 7,/// ��������
    NPC_TYPE_GUARD    = 8,/// ʿ������
    NPC_TYPE_PET    = 9,/// ��������
    NPC_TYPE_BACKBONEBUG= 10,/// ��������
    NPC_TYPE_SUMMONS  = 11,/// �ٻ�����
    NPC_TYPE_TOTEM    = 12,/// ͼ������
    NPC_TYPE_AGGRANDIZEMENT = 13,/// ǿ������
    NPC_TYPE_ABERRANCE  = 14,/// ��������
    NPC_TYPE_STORAGE  = 15,/// �ֿ�����
    NPC_TYPE_ROADSIGN  = 16,/// ·������
    NPC_TYPE_TREASURE  = 17,/// ��������
    NPC_TYPE_WILDHORSE  = 18,/// Ұ������
    NPC_TYPE_MOBILETRADE  = 19,/// ����С��
    NPC_TYPE_LIVENPC  = 20,/// ����npc����ս��������ʱ��ʧ��
    NPC_TYPE_DUCKHIT  = 21,/// ���²��ܴ��npc
    NPC_TYPE_BANNER    = 22,/// ��������
    NPC_TYPE_TRAP    = 23,/// ��������
    NPC_TYPE_MAILBOX  =24,///����
    NPC_TYPE_AUCTION  =25,///��������Ա
    NPC_TYPE_UNIONGUARD  =26,///�������
    NPC_TYPE_SOLDIER  =27,///ʿ����ֻ���������
    NPC_TYPE_UNIONATTACKER  =28,///����ʿ��
    NPC_TYPE_SURFACE = 29,/// �ر�����
    NPC_TYPE_CARTOONPET = 30,/// ������
    NPC_TYPE_PBOSS = 31,/// ��ɫBOSS
    NPC_TYPE_RESOURCE = 32, /// ��Դ��NPC

    //sky���
    NPC_TYPE_GHOST	= 999,  /// Ԫ����NPC
    NPC_TYPE_ANIMON   = 33,   /// ���������
    NPC_TYPE_GOTO	= 34,	///���͵�
    NPC_TYPE_RESUR  = 35,	///�����
    NPC_TYPE_UNFIGHTPET	= 36, ///��ս������
    NPC_TYPE_FIGHTPET	= 37, ///ս������
    NPC_TYPE_RIDE		= 38, ///����
    NPC_TYPE_TURRET	= 39, /// ����
    NPC_TYPE_BARRACKS = 40, /// ��Ӫ
    NPC_TYPE_CAMP = 41,		/// ����
};

enum
{
    NPC_ATYPE_NEAR    = 1,/// �����빥��
    NPC_ATYPE_FAR    = 2,/// Զ���빥��
    NPC_ATYPE_MFAR    = 3,/// ����Զ�̹���
    NPC_ATYPE_MNEAR    = 4,/// ����������
    NPC_ATYPE_NOACTION  = 5,    /// �޹�������
    NPC_ATYPE_ANIMAL    = 6  /// ������
};

///npcʹ��һ�����ܵ�����
struct npcSkill
{
    DWORD id;///����id
    int needLevel;///����id
    int rate;///ʹ�ü���
    int coefficient;///����ϵ��

    npcSkill():id(0),needLevel(0),rate(0),coefficient(0){}
    npcSkill(const npcSkill &skill)
    {
	id = skill.id;
	needLevel = skill.needLevel;
	rate = skill.rate;
	coefficient = skill.coefficient;
    }
    npcSkill& operator = (const npcSkill &skill)
    {
	id = skill.id;
	needLevel = skill.needLevel;
	rate = skill.rate;
	coefficient = skill.coefficient;
	return *this;
    }
};

struct npcRecover
{
    DWORD start;
    BYTE type;
    DWORD num;

    npcRecover()
    {
	start = 0;
	type = 0;
	num = 0;
    }

    void parse(const char * str)
    {
	if (!str) return;

	std::vector<std::string> vec;

	vec.clear();
	Zebra::stringtok(vec,str,":");
	if (3==vec.size())
	{
	    start = atoi(vec[0].c_str());
	    type = atoi(vec[1].c_str());
	    num = atoi(vec[2].c_str());
	}
    }
};

/**
 * \brief Npc�����������
 *
 */
struct zNpcB : public zEntry
{
    DWORD  kind;        // ����
    DWORD  level;        // �ȼ�
    DWORD  hp;          // ����ֵ
    DWORD  exp;        // ����ֵ
    DWORD  str;        // ����
    DWORD   inte;        // ����
    DWORD   dex;        // ����
    DWORD   men;        // ����
    DWORD   con;        // ����
    DWORD   cri;        // ����
    DWORD  color;        // ��ɫ
    DWORD  ai;          // ai
    DWORD  distance;      // �ƶ����
    DWORD  adistance;      // �������
    DWORD  pdefence;      // ��С���������
    DWORD  maxpdefence;    // ������������
    DWORD  mdefence;      // ��С����������
    DWORD  maxmdefence;    // �����������
    DWORD  five;        // ��������
    DWORD   fivepoint;      // ���е���
    std::vector<aTypeS> atypelist;  // ��������
    DWORD  mdamage;      // ��С��������
    DWORD  maxmdamage;      // ���������
    DWORD  damage;        // ��С������
    DWORD  maxdamage;      // ��󹥻���
    DWORD  skill;        // ����
    //char  object[1024 + 1];  // Я����Ʒ
    NpcCarryObject nco;
    DWORD  ChangeNpcID;     //soulrate;      //sky NPC����ID
    char  skills[1024];    // ʹ�ü���
    char  state[1024];    // ״̬
    DWORD  dodge;        // �����
    DWORD  rating;        // ������
    DWORD  pic;        // ͼƬ
    DWORD  trait;        //Ʒ��
    DWORD  bear_type;      //�������
    DWORD  pet_pic;      //����ͼƬ
    npcRecover recover;
    DWORD  flags;      //�����Ʊ�־��Ŀǰ��һ�����ɲ��ɱ������ɱ
    DWORD  allyVisit;      //�ɱ��˹����ʵĵȼ� 0�����ɷ��� 1��1���ɷ��� 2��2���ɷ���
    DWORD radix;

    DWORD kokGroupID;
    DWORD kokAttGroupID;
    DWORD kok3DZoom;
    DWORD kok3DInterval;
    DWORD lingqiexp;
    DWORD lingqiPK;
    char tips[32];
    DWORD hurtType;
    DWORD hurtValue;
    DWORD tiredType;
    DWORD delayClearBodyTime;
    DWORD elementHurt;
    DWORD allHitCount;
    DWORD skillPKType;
    std::map<int,std::vector<npcSkill> > skillMap;

    //DWORD  Need_Probability; //sky ��Ʒ����

    bool parseSkills(const char * str)
    {
	skillMap.clear();
	strncpy(skills,str,sizeof(skills));

	bool ret = false;
	std::vector<std::string> type_v;
	Zebra::stringtok(type_v,str,";");
	if (type_v.size()>0)
	{
	    std::vector<std::string> type_sub_v,skill_v,prop_v;
	    std::vector<std::string>::iterator type_it,skill_it;

	    for (type_it=type_v.begin();type_it!=type_v.end();type_it++)
	    {
		type_sub_v.clear();
		Zebra::stringtok(type_sub_v,type_it->c_str(),":");
		if (2==type_sub_v.size())
		{
		    int type = atoi(type_sub_v[0].c_str());

		    std::vector<npcSkill> oneTypeSkills;
		    skill_v.clear();
		    Zebra::stringtok(skill_v,type_sub_v[1].c_str(),",");
		    for (skill_it=skill_v.begin();skill_it!=skill_v.end();skill_it++)
		    {
			prop_v.clear();
			Zebra::stringtok(prop_v,skill_it->c_str(),"-");
			if (4==prop_v.size())
			{
			    npcSkill oneSkill;
			    oneSkill.id = atoi(prop_v[0].c_str());
			    oneSkill.needLevel = atoi(prop_v[1].c_str());
			    oneSkill.rate = atoi(prop_v[2].c_str());
			    oneSkill.coefficient = atoi(prop_v[3].c_str());

			    oneTypeSkills.push_back(oneSkill);
			}
		    }
		    if (oneTypeSkills.size()>0)
		    {
			skillMap[type] = oneTypeSkills;
			ret = true;
		    }
		}
	    }
	}
	return ret;
    }

    /**
     * \brief �����������ȡ��һ��npc���ܵ�����
     *
     * \param type ��������
     * \param skill ����ֵ��ȡ�õļ�������
     * \return �Ƿ�ȡ�óɹ�
     */
    bool getRandomSkillByType(int type,npcSkill &skill)
    {
	if (skillMap.find(type)==skillMap.end()) return false;

	skill = skillMap[type][zMisc::randBetween(0,skillMap[type].size()-1)];
	return true;
    }

    /**
     * \brief ȡ�����п��õļ���ID
     *
     *
     * \param list ����ID�б�
     * \return bool �Ƿ��м���
     */
    bool getAllSkills(std::vector<DWORD> & list,WORD level)
    {
	std::map<int,std::vector<npcSkill> >::iterator type_it;
	std::vector<npcSkill>::iterator skill_it;
	for (type_it=skillMap.begin();type_it!=skillMap.end();type_it++)
	{
	    for (skill_it=type_it->second.begin();skill_it!=type_it->second.end();skill_it++)
		if (level>=skill_it->needLevel)
		    list.push_back(skill_it->id);
	}
	return list.size()>0;
    }

    /**
     * \brief ����һ��npc����
     * \param type ���ܷ���
     * \param id Ҫ���ӵļ���id
     * \param rate ʩ�ż���
     * \param coefficient ϵ��
     */
    void addSkill(int type,DWORD id,int needLevel,int rate,int coefficient = 0)
    {
	npcSkill s;
	s.id = id;
	s.needLevel = needLevel;
	s.rate = rate;
	s.coefficient = coefficient;
	skillMap[type].push_back(s);
    }

    /**
     * \brief ɾ��һ��npc����
     *
     *
     * \param id Ҫɾ���ļ���id
     * \return npcû�иü����򷵻�false
     */
    bool delSkill(DWORD id)
    {
	std::map<int,std::vector<npcSkill> >::iterator v_it;
	for (v_it=skillMap.begin();v_it!=skillMap.end();v_it++)
	{
	    std::vector<npcSkill> v = v_it->second;
	    std::vector<npcSkill>::iterator s_it;
	    for (s_it=v.begin();s_it!=v.end();s_it++)
	    {
		if (s_it->id==id)
		{
		    v.erase(s_it);
		    return true;
		}
	    }
	}
	return false;
    }

    /**
     * \brief ����npc�Ĺ�������
     *
     *
     * \param data ������ַ���
     * \param size �ַ�����С
     */
    void setAType(const char *data,int size)
    {

	//Zebra::logger->error("address = %x",data);
	if(NULL == data)
	{
	    fprintf(stderr,"data == NULL");
	    return;
	}
	atypelist.clear();
	size = 1024;

	char Buf[1024];
	bzero(Buf,size);
	strncpy(Buf,data,size);
	std::vector<std::string> v_fir;
	Zebra::stringtok(v_fir,Buf,":");
	for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
	{
	    std::vector<std::string> v_sec;
	    Zebra::stringtok(v_sec,iter->c_str(),"-");

	    if (v_sec.size() != 2)
	    {
		return;
	    }

	    aTypeS aValue;
	    std::vector<std::string>::iterator iter_1 = v_sec.begin();

	    for(int i=0; i<2; i++)
	    {
		aValue.byValue[i] = (BYTE)atoi(iter_1->c_str());
		iter_1 ++;
	    }
	    atypelist.push_back(aValue);
	}
	return;
    }

    /**
     * \brief ȡ��npc�Ĺ������ͺͶ�������
     *
     *
     * \param type ��� ��������
     * \param action
     */
    void getATypeAndAction(BYTE &type,BYTE &action)
    {    
	int size = atypelist.size();
	if (size == 0)
	{
	    type = NPC_ATYPE_NEAR;
	    action = 4 ;//Cmd::AniTypeEnum::Ani_Attack;//Cmd::Ani_Attack
	    return;
	}
	int num = zMisc::randBetween(0,size-1);
	type = atypelist[num].byAType;
	action = atypelist[num].byAction;
    }

    /**
     * \brief ���ݱ���ж������������zNpcB�ṹ
     *
     *
     * \param npc �ӱ��ж���������
     */
    void fill(NpcBase &data)
    {
	id= data.dwField0;
	strncpy(name, data.strField1, MAX_NAMESIZE);
	kind= data.dwField2;
	level= data.dwField3;
	distance = data.dwField4;
	adistance = data.dwField5;
	hp= data.dwField6;
//#if 0
//#endif
    }

    zNpcB() : zEntry()
    {
	id=          0;
	bzero(name,sizeof(name));
	kind=        0;
	level=        0;
	hp=        0;
	exp=        0;
	str=        0;
	inte=        0;
	dex=        0;
	men=        0;
	con=        0;
	cri=        0;
	color=        0;
	ai=        0;
	distance=      0;
	adistance=       0;
	pdefence=      0;
	maxpdefence=    0;
	mdefence=      0;
	maxmdefence=    0;
	five=        0;
	fivepoint=      0;
	atypelist.clear();
	mdamage=      0;
	maxmdamage=      0;
	damage=        0;
	maxdamage=      0;
	skill=        0;
	//bzero(object,sizeof(object));
	ChangeNpcID=      0;
	bzero(skills,sizeof(skills));
	bzero(state,sizeof(state));
	dodge=        0;
	rating=        0;
	pic=        0;
	trait=        0;
	bear_type=      0;
	pet_pic=      0;
	flags=        0;
	allyVisit=      0;
	//Need_Probability = 0;
    }

};
#if 0

//------------------------------------
// SkillBase
//------------------------------------
/**
* \brief ���ݼ������ͺ͵ȼ�����һ����ʱΨһ���
*
*/
#define skill_hash(type,level) ((type - 1) * 100 + level)

struct SkillBase
{
	const DWORD getUniqueID() const
	{
		return skill_hash(dwField0,dwField2);
	}

	DWORD  dwField0;      // ����ID
	char  strField1[32];    // ��������
	DWORD  dwField2;      // ���ܵȼ�
	DWORD  dwField3;      // ����ϵ��
	DWORD  dwField4;      // ��������
	DWORD  dwField5;      // ��Ҫ���߼��ܵ���
	DWORD  dwField6;      // ǰ�Ἴ��һ
	DWORD  dwField7;      // ǰ�Ἴ��һ�ȼ�
	DWORD  dwField8;      // ǰ�Ἴ�ܶ�
	DWORD  dwField9;      // ǰ�Ἴ�ܶ��ȼ�
	DWORD  dwField10;      // ǰ�Ἴ����
	DWORD  dwField11;      // ǰ�Ἴ�����ȼ�
	DWORD  dwField12;      // ���ʱ��
	DWORD  dwField13;      // ������ʽ
	DWORD  dwField14;      // �ܷ�����ʹ��
	DWORD  dwField15;      // ��Ҫ��Ʒ
	char  strField16[128];  // ��Ҫ����
	DWORD  dwField17;      // ��������ֵ
	DWORD  dwField18;      // ���ķ���ֵ
	DWORD  dwField19;      // ��������ֵ
	DWORD  dwField20;      // �˺��ӳ�
	char  strField21[1024];  // Ч��
	DWORD  dwField22;      // ������Ʒ����
	DWORD  dwField23;      // ��Ʒ��������
};//���� SkillBase �ɹ����� 1 ����¼

#define BENIGNED_SKILL_STATE 2
#define BAD_SKILL_STATE 4
#define NONE_SKILL_STATE 1 

struct SkillElement
{
	SkillElement()
	{
		id = 0;
		value = 0;
		percent = 0;
		time = 0;
		state = 0;
	}
	union {
		struct {
			DWORD id;
			DWORD percent;
			DWORD value;
			DWORD time;
			DWORD state;
		};
		DWORD element[5];
	};
	static SkillElement *create(SkillElement elem);
};
struct SkillStatus
{
	SkillStatus()
	{
		for(int i = 0 ; i < (int)(sizeof(status) / sizeof(WORD)) ; i ++)
		{
			status[i] = 0;
		}
	}
	union {
		struct {
			WORD id;//����id
			WORD target;//Ŀ��
			WORD center;//���ĵ�
			WORD range;//��Χ
			WORD mode;//����ģʽ
			WORD clear;//�ܷ����
			WORD isInjure;//�Ƿ���Ҫ�˺�����
		};
		WORD status[7];
	};
	std::vector<SkillElement> _StatusElementList;
};
struct zSkillB : public zEntry
{
	bool has_needweapon(const WORD weapontype) const
	{
		std::vector<WORD>::const_iterator iter;
		if (weaponlist.empty()) return true;
		for(iter = weaponlist.begin(); iter != weaponlist.end(); iter++)
		{
			if (*iter == weapontype) return true;
		}
		return false;
	}

	bool set_weaponlist(const char *data)
	{
		weaponlist.clear(); 
		std::vector<std::string> v_fir;
		stringtok(v_fir,data,":");
		for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
		{
			WORD weaponkind = (WORD)atoi(iter->c_str());
			weaponlist.push_back(weaponkind);
		}
		return true;
	}

	bool set_skillState(const char *data)
	{
		skillStatus.clear(); 
		std::vector<std::string> v_fir;
		stringtok(v_fir,data,".");
		for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
		{
			//Zebra::logger->debug("%s",iter->c_str());
			std::vector<std::string> v_sec;
			stringtok(v_sec,iter->c_str(),":");
			/*
			if (v_sec.size() != 2)
			{
			return false;
			}
			// */
			SkillStatus status;
			std::vector<std::string>::iterator iter_1 = v_sec.begin() ;
			std::vector<std::string> v_thi;
			stringtok(v_thi,iter_1->c_str(),"-");
			if (v_thi.size() != 7)
			{
				//Zebra::logger->debug("����!=7");
				continue;
				//return false;
			}
			std::vector<std::string>::iterator iter_2 = v_thi.begin() ;
			for(int i = 0 ; i < 7 ; i ++)
			{
				status.status[i] = (WORD)atoi(iter_2->c_str());
				//Zebra::logger->debug("status.status[%ld]=%ld",i,status.status[i]);
				iter_2 ++;
			}
			iter_1 ++;
			if (iter_1 == v_sec.end())
			{
				//Zebra::logger->debug("�ղ���");
				skillStatus.push_back(status);
				continue;
			}
			std::vector<std::string> v_fou;
			stringtok(v_fou,iter_1->c_str(),";");
			std::vector<std::string>::iterator iter_3 = v_fou.begin() ;
			for( ; iter_3 != v_fou.end() ; iter_3 ++)
			{
				std::vector<std::string> v_fiv;
				stringtok(v_fiv,iter_3->c_str(),"-");
				if (v_fiv.size() != 5)
				{
					//Zebra::logger->debug("Ԫ�ظ�������");
					continue;
					//return false;
				}
				std::vector<std::string>::iterator iter_4 = v_fiv.begin() ;
				SkillElement element;
				for(int i = 0 ; i < 5 ; i ++)
				{
					element.element[i] = (DWORD)atoi(iter_4->c_str());
					//Zebra::logger->debug("element.element[%u]=%u",i,element.element[i]);
					iter_4 ++;
				}
				status._StatusElementList.push_back(element);
			}
			skillStatus.push_back(status);
		}
		return true;
	}
	DWORD  skillid;            //����ID
	DWORD  level;              //���ܵȼ�
	DWORD  kind;              //����ϵ��
	DWORD  subkind;            //��������
	DWORD  needpoint;            //��Ҫ���߼��ܵ���
	DWORD  preskill1;            //ǰ�Ἴ��1
	DWORD  preskilllevel1;          //ǰ�Ἴ�ܼ���1
	DWORD  preskill2;            //ǰ�Ἴ��2
	DWORD  preskilllevel2;          //ǰ�Ἴ�ܼ���2
	DWORD  preskill3;            //ǰ�Ἴ��3
	DWORD  preskilllevel3;          //ǰ�Ἴ�ܼ���3
	DWORD  dtime;              //���ʱ��
	DWORD  usetype;            //������ʽ
	DWORD  ride;              //�ɷ�����ʹ��
	DWORD  useBook;            //��Ҫ��Ʒ
	DWORD  spcost;              //��������ֵ
	DWORD  mpcost;              //���ķ���ֵ
	DWORD  hpcost;              //��������ֵ
	DWORD  damnum;              //�˺��ӳ�
	DWORD  objcost;            //������Ʒ����
	DWORD  objnum;              //������Ʒ����
	std::vector<SkillStatus> skillStatus;  //Ч��
	std::vector<WORD> weaponlist;      //�����б�



	void fill(const SkillBase &data)
	{
		id=skill_hash(data.dwField0,data.dwField2);
		skillid=data.dwField0;                //����ID
		strncpy(name,data.strField1,MAX_NAMESIZE);
		level      = data.dwField2;          //���ܵȼ�
		kind      = data.dwField3;          //����ϵ��
		subkind      = data.dwField4;          //��������
		needpoint    = data.dwField5;          //��Ҫ���߼��ܵ���
		preskill1    = data.dwField6;          //ǰ�Ἴ��1
		preskilllevel1  = data.dwField7;;          //ǰ�Ἴ�ܼ���1
		preskill2    = data.dwField8;          //ǰ�Ἴ��2
		preskilllevel2  = data.dwField9;          //ǰ�Ἴ�ܼ���2
		preskill3    = data.dwField10;          //ǰ�Ἴ��3
		preskilllevel3  = data.dwField11;          //ǰ�Ἴ�ܼ���3
		dtime      = data.dwField12;          //���ʱ��
		usetype      = data.dwField13;          //������ʽ
		ride      = data.dwField14;          //�ɷ�����ʹ��
		useBook      = data.dwField15;          //ѧϰ��Ҫ��Ʒ
		set_weaponlist(data.strField16);          //��Ҫ����
		spcost      = data.dwField17;          //��������ֵ
		mpcost      = data.dwField18;          //���ķ���ֵ
		hpcost      = data.dwField19;          //��������ֵ
		damnum      = data.dwField20;          //�˺��ӳ�
		set_skillState(data.strField21);
		objcost      = data.dwField22;          //������Ʒ����
		objnum      = data.dwField23;          //������Ʒ����
	}


	zSkillB() : zEntry()
	{
		id = 0;
		skillid = 0;
		bzero(name,sizeof(name));        //˵��
		level      = 0;          //���ܵȼ�
		kind      = 0;          //����ϵ��
		subkind      = 0;          //��������
		needpoint    = 0;          //��Ҫ���߼��ܵ���
		preskill1    = 0;          //ǰ�Ἴ��1
		preskilllevel1  = 0;          //ǰ�Ἴ�ܼ���1
		preskill2    = 0;          //ǰ�Ἴ��2
		preskilllevel2  = 0;          //ǰ�Ἴ�ܼ���2
		preskill3    = 0;          //ǰ�Ἴ��3
		preskilllevel3  = 0;          //ǰ�Ἴ�ܼ���3
		dtime      = 0;          //���ʱ��
		usetype      = 0;          //������ʽ
		ride      = 0;          //�ɷ�����ʹ��
		useBook      = 0;          //��Ҫ��Ʒ
		spcost      = 0;          //��������ֵ
		mpcost      = 0;          //���ķ���ֵ
		hpcost      = 0;          //��������ֵ
		damnum      = 0;          //�˺��ӳ�
		objcost      = 0;          //������Ʒ����
		objnum      = 0;          //������Ʒ����
	}

};
#endif

struct CardBase
{
    const DWORD getUniqueID() const
    {
	return id;
    }
    union
    {
	DWORD dwField0;
	DWORD id;
    };
    char name[64];
    DWORD type;		    //����
    DWORD occupation;	    //ְҵ
    DWORD race;		    //����
    DWORD kind;		    //Ʒ��
    DWORD mpcost;	    //����
    DWORD damage;	    //������
    DWORD hp;		    //Ѫ��
    DWORD dur;		    //�;�
    DWORD source;	    //��Դ
    
    DWORD taunt;		//����(1,0)
    DWORD charge;		//���(1,0)
    DWORD windfury;		//��ŭ(1,0)
    DWORD sneak;	    //Ǳ��(1,0)
    DWORD shield;	    //ʥ��(1,0)
    DWORD antimagic;	    //ħ��(1,0)
    DWORD magicDamAdd;	    //�����˺�����(X)
    DWORD overload;	    //����(X)
    DWORD magicID;	    //����ID(skill)
    DWORD needTarget;   //��ҪĿ��
    DWORD shoutID;	    //ս��ID(skill)
    DWORD shoutTarget;	    //ս����ҪĿ��
};

struct ConditionStatus
{
    ConditionStatus()
    {
	for(int i = 0 ; i < (int)(sizeof(status) / sizeof(WORD)) ; i ++)
	{
	    status[i] = 0;
	}
    }
    union {
	struct {
	    WORD range;	    //��Χ(���÷�Χ,�μ�ö��)
	    WORD condition; //ɸѡĿ��������ö��
	    WORD mode;	    //1,����ʩ���߱���;0,������ʩ����
	};
	WORD status[3];
    };
    ConditionStatus & operator= (const ConditionStatus &other)
    {
	range = other.range;
	condition = other.condition;
	mode = other.mode;
	return *this;
    }
};

struct zCardB : public zEntry
{
    DWORD type;		    //����
    DWORD occupation;	    //ְҵ
    DWORD race;		    //����
    DWORD kind;		    //Ʒ��
    DWORD mpcost;	    //����
    DWORD damage;	    //������
    DWORD hp;		    //Ѫ��
    DWORD dur;		    //�;�
    DWORD source;	    //��Դ
    
    BYTE taunt;		    //����(1,0)
    BYTE charge;	    //���(1,0)
    BYTE windfury;	    //��ŭ(1,0)

    BYTE sneak;		    //Ǳ��(1,0)
    BYTE shield;	    //ʥ��(1,0)
    BYTE antimagic;        //ħ��(1,0)
    BYTE magicDamAdd;	    //�����˺�����(X)
    BYTE overload;	    //����(X)

    DWORD magicID;	    //����ID(skill)
    DWORD needTarget;   //��ҪĿ��
    DWORD shoutID;	    //ս��ID(skill)
    DWORD shoutTarget;	    //ս����ҪĿ��

    void fill(CardBase &data);
};


struct SkillBase
{
	const DWORD getUniqueID() const
	{
		return id;
	}
	union
	{
	    DWORD  dwField0;      // ����ID
	    DWORD id;
	};
	char  name[32];    // ��������
	char  function[256];  // Ч��
	DWORD conditionType;
	DWORD conditionID;
	DWORD skillAID;  // trueЧ��
	DWORD skillBID;  // falseЧ��
};//���� SkillBase �ɹ����� 1 ����¼

struct SkillElement
{
    SkillElement()
    {
	id = 0;
	value = 0;
	value2 = 0;
	percent = 0;
	halo = 0;
	state = 0;
    }
    union {
	struct {
	    DWORD id;
	    DWORD percent;
	    DWORD value;
	    DWORD value2;
	    DWORD halo;
	    DWORD state;
	};
	DWORD element[6];
    };
    static SkillElement *create(SkillElement elem);
};
struct SkillStatus
{
    SkillStatus()
    {
	for(int i = 0 ; i < (int)(sizeof(status) / sizeof(WORD)) ; i ++)
	{
	    status[i] = 0;
	}
    }
    union {
	struct {
	    WORD id;//����id
	    WORD attack;    //1,����;2,Ⱥ��,3,���Ŀ��(����),4,���Ŀ��(����)
	    WORD num;	    //��(attack==3 || attack==4)���ֶ���Ч  
	    WORD range;	    //��Χ(���÷�Χ,�μ�ö��)
	    WORD mode;	    //1,����ʩ���߱���;0,������ʩ����
	    WORD condition; //ɸѡĿ��������ö��
	    WORD useHand;    //�Ƿ�ʹ����ѡĿ��(1,ʹ��;0,��ʹ��)
	};
	WORD status[7];
    };
    std::vector<SkillElement> _StatusElementList;
};
struct zSkillB : public zEntry
{


    bool set_skillState(const char *data, std::vector<SkillStatus> &_skillStatus)
    {
	_skillStatus.clear(); 
	std::vector<std::string> v_fir;
	Zebra::stringtok(v_fir,data,".");
	for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
	{
	    //Zebra::logger->debug("%s",iter->c_str());
	    std::vector<std::string> v_sec;
	    Zebra::stringtok(v_sec,iter->c_str(),":");
	    /*
	       if (v_sec.size() != 2)
	       {
	       return false;
	       }
	    // */
	    SkillStatus status;
	    std::vector<std::string>::iterator iter_1 = v_sec.begin() ;
	    std::vector<std::string> v_thi;
	    Zebra::stringtok(v_thi,iter_1->c_str(),"-");
	    if (v_thi.size() != 7)
	    {
		//Zebra::logger->debug("����!=7");
		continue;
		//return false;
	    }
	    std::vector<std::string>::iterator iter_2 = v_thi.begin() ;
	    for(int i = 0 ; i < 7 ; i ++)
	    {
		status.status[i] = (WORD)atoi(iter_2->c_str());
		//Zebra::logger->debug("status.status[%ld]=%ld",i,status.status[i]);
		iter_2 ++;
	    }
	    iter_1 ++;
	    if (iter_1 == v_sec.end())
	    {
		//Zebra::logger->debug("�ղ���");
		_skillStatus.push_back(status);
		continue;
	    }
	    std::vector<std::string> v_fou;
	    Zebra::stringtok(v_fou,iter_1->c_str(),";");
	    std::vector<std::string>::iterator iter_3 = v_fou.begin() ;
	    for( ; iter_3 != v_fou.end() ; iter_3 ++)
	    {
		std::vector<std::string> v_fiv;
		Zebra::stringtok(v_fiv,iter_3->c_str(),"-");
		if (v_fiv.size() != 6)
		{
		    //Zebra::logger->debug("Ԫ�ظ�������");
		    continue;
		    //return false;
		}
		std::vector<std::string>::iterator iter_4 = v_fiv.begin() ;
		SkillElement element;
		for(int i = 0 ; i < 6 ; i ++)
		{
		    element.element[i] = (DWORD)atoi(iter_4->c_str());
		    //Zebra::logger->debug("element.element[%u]=%u",i,element.element[i]);
		    iter_4 ++;
		}
		status._StatusElementList.push_back(element);
	    }
	    _skillStatus.push_back(status);
	}
	return true;
    }
    DWORD   skillid;            //����ID
    std::vector<SkillStatus> skillStatus;  //Ч��
    DWORD   conditionType;
    DWORD   conditionID;
    DWORD skillAID;  //AЧ��
    DWORD skillBID;  //BЧ��

    zSkillB() : zEntry()
    {
	id = 0;
	skillid = 0;
	bzero(name,sizeof(name));        //˵��
    }

    void fill(const SkillBase &data);

};

struct StateBase
{
    const DWORD getUniqueID() const
    {
	return id;
    }
    union
    {
	DWORD dwField0;
	DWORD id;
    };
    char name[32];
    DWORD mainBuff;
};

struct zStateB : public zEntry
{
    DWORD mainBuff;		    //��BUFF
    zStateB() : zEntry()
    {
	id = 0;
	mainBuff = 0;
	bzero(name,sizeof(name));        //˵��
    }

    void fill(StateBase &data);
};

#endif

