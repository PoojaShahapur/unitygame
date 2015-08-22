#ifndef _OBJECT_H
#define _OBJECT_H
#include "zType.h"
//************************************************************//
//	˵�����е�equipItem��Ϊ���ߺ�װ��,�������ʱ,��ע��		  //
//************************************************************//
enum enumItemType
{
    ItemType_None,
    ItemType_Resource = 16, //16����һ�����
    ItemType_Leechdom,  //17����ҩƷ��
    ItemType_FoodRes,   //18����ʳ��ԭ����
    ItemType_Food,      //19����ʳ����
    ItemType_Tools,     //20�����Ͷ�������
    ItemType_Arrow,     //21������Ϲ�ʹ�õļ�֧��
    //ItemType_BattleHorse,   //22ս��
    ItemType_Pack = 23,      //23���������
    ItemType_Money,     //24�����Ǯ��
    ItemType_Scroll,    //25����ת�ƾ�����
    ItemType_Move,      //26���������ƶ�������
    ItemType_LevelUp,   //27�������������Ҫ�Ĳ�����
    ItemType_CaptureWeapon, //28����ѱ������������
    ItemType_Union, //29�����������Ҫ�ĵ���.
    ItemType_Tonic, //30��ʾ�Զ���ҩ�����.
    ItemType_Gift,  //31������Ʒ����Ʒ.
    ItemType_Other,
    ItemType_MASK = 33,     //33���������
    ItemType_Quest = 34,
    //ItemType_HORSE = 35,
    ItemType_SOULSTONE = 37, //37�������ʯ��
    ItemType_Wedding = 38, //38���������
    ItemType_Change = 41,   //41 ����ϳɵ���
    ItemType_Auto = 42,   //42 �����Զ�����
    ItemType_SkillUp = 43,   //43 ��������������
    ItemType_Book = 44, //44�����鼮
    ItemType_Store = 45,   //45 ����ֿ�
    ItemType_Renew = 46,   //46 ����ϴ�����
    ItemType_Repair = 47, //47�����޸���ʯ��
    ItemType_GeniusSkillBookPage = 49, //�츳���ҳ
    ItemType_DoubleExp = 52, //52����˫����������
    ItemType_Honor = 53, //53��������֮������
    ItemType_TONG = 54,  // Ӷ�����ټ�������
    ItemType_FAMILY = 55,  // �����ټ�������
    ItemType_Adonment = 56, //56����װ��Ʒ
    ItemType_SpecialBook = 57, //57���������鼮
    ItemType_GreatLeechdom = 58, //58�����ҩƷ
    ItemType_ClearProperty = 59, //59ϴ�����
    ItemType_UseSkill = 60, // �������������
    ItemType_Amulet = 61, // ����������
    ItemType_GreatLeechdomMp = 62,//62������Զ���������
    ItemType_TEAM = 64, // ð�ն����ټ���
    ItemType_KING = 65, // �����ټ���(������)
    ItemType_OneFiveExp = 66,  //1.5���������
    ItemType_OneTwoFiveExp = 67,  //1.25���������
    ItemType_OneSevenFiveExp = 68,  //1.75���������
    ItemType_NewOneFiveExp = 69,  //�;ð������1.5���������
    ItemType_CanNotDestroy = 70,    //�������ٵĵ���
    ItemType_EGObject = 71,     //71�����������
    ItemType_Stuff = 72,		//72����ԭ��
    ItemType_Material = 73,     //73�������
    ItemType_TwoExp = 74,       //˫���������
    ItemType_New_OneFiveExp = 75,  //��1.5���������
    ItemType_New_OneSevenFiveExp = 76,  //��1.75���������
    //ItemType_immortalmedicines = 77,  //77��������
	ItemType_immortalmedicines = 77,        //������������
    ItemType_SeaWarItem  = 79,  //��ս����
    ItemType_HuShenFu  = 80,  //�����
    ItemType_SkillObject = 81,  //�������͵ĵ���
    ItemType_GeniusSkillBook = 83,  //�츳������
	ItemType_HouseGardenSeed        = 95,   //���ݻ�԰����
	ItemType_HouseGardenMaterial    = 96,   //���ݻ�԰��Ʒ
	ItemType_HouseItem              = 98,   //������Ʒ
    ItemType_Furniture = 99,  //�Ҿ�
	//=============================================װ��==========================================================================
    ItemType_ClothBody =101,		//101�����ʼ��������װ		
    ItemType_FellBody =102,         //102����Ƥ�׼�ħ�����װ
    ItemType_MetalBody =103,        //103����������׼�������װ
    ItemType_Blade =104,            //104����������������
    ItemType_Sword =105,            //105����������������
    ItemType_Axe =106,				//106���������������������ָ���װ�������֣�Ҳ��װ��������)
    ItemType_Hammer =107,           //107����������������
    ItemType_Staff =108,            //108��������������
    ItemType_Crossbow =109,         //109���������������
    ItemType_Fan =110,				//110������Ů����
    ItemType_Stick =111,            //111�����ٻ���������
    ItemType_Shield =112,			//112���������
    ItemType_Helm =113,				//113�����ɫͷ����
    ItemType_Caestus =114,			//114�����ɫ������
    ItemType_Cuff = 115,			//115�����ɫ������
    ItemType_Shoes = 116,			//116�����ɫЬ����
    ItemType_Necklace = 117,		//117�����ɫ������
    ItemType_Fing = 118,			//118�����ɫ��ָ��
    ItemType_FashionBody = 119,     //119����ʱװ
	//==============================================end============================================================================
    ItemType_Flower = 120,      //120�����ʻ�,�ɼ�����...
    //ItemType_BMW = 121,     //119������

	//==========================================ʱװ==============================================================================
    ItemType_Fashion140 = 122,				//122����140��ʱװ
    ItemType_TransFashion140 = 123,			//123����140��͸��ʱװ
    ItemType_Fashion = 124,					//124�������ð�ʱװ
    ItemType_ClearFashion = 125,			//125���������ð�͸��ʱװ
	//===========================================end===============================================================================
    ItemType_GiftBox = 126,     //���п����õı���
    ItemType_GiftBox_Key = 127,     //���п����õ�Կ��
    ItemType_Add_Exploit = 128,     //���ӹ�ѫ����
    ItemType_UnionItems = 129,  //������
    //ItemType_HorseShoe = 130,       // ����������
	ItemType_HorseSoul = 130,               // �������
    //ItemType_HorseRope = 131,       // ����
    //ItemType_HorseSaddle = 132,     // ������
    //ItemType_HorseSafe = 133,       // ����
    //ItemType_HorseIron = 134,       // ����

	//=========================================װ��============================================================================
    ItemType_Dagger = 136,					// ذ��
    ItemType_NewSword = 137,				// �½�
    ItemType_breastplate = 138,				// ���ľ�
    //ItemType_HorseFashion = 140,			// ��ʱװ

	
	ItemType_BothHandAxe = 139,				//˫�ָ������ָ�֮װ��������)
    ItemType_BoxingGlove = 140,				//ȭ��
    ItemType_MusicBow = 141,				//�ٹ�
    ItemType_MagicBook = 142,				//ħ����
    ItemType_BothHandHammer = 143,			//˫�־޴�
    ItemType_CrystalHead = 144,				//ˮ����
    ItemType_HumanSkeleton= 145,			//����ͷ
    ItemType_MagicSword = 147,				// ħ����
    ItemType_Talon = 148,					// ȭ��
    ItemType_Lance = 149,					// ��ì,ǹ
    ItemType_Mace = 150,					// �
    ItemType_Earing = 151,					// ����
    ItemType_Arm = 152,						// �ۻ�
    ItemType_Knee = 153,					// ��ϥ
    ItemType_Cloak = 154,					// ����
    ItemType_Shoulder = 155,				// ���
    ItemType_FamilyCloak = 156,				//��������
    ItemType_FashionHelm = 160,				// ʱװͷ��
    ItemType_FashionCloak = 161,			// ʱװ����
    //ItemType_AccessoryJewel = 162,			// ������Ʒ
	ItemType_AutoPick = 162,	// �Զ��������
    ItemType_HpJewel = 163,					// ������Ʒ
    ItemType_MpJewel = 164,					// ħ����Ʒ

	//==========================================end==============================================================================
    ItemType_Reload = 170,          // �����
    ItemType_MountElse = 171,       // �����յ���
    ItemType_Offline = 180,         //���߹���
    ItemType_Autoget = 190,         //�Զ�ʰȡ����

    ItemType_PerfectLevelup = 201,  // ��ʯ
    ItemType_AntiquityStone = 203,  //�Ϲ���ʯ
    ItemType_AntiquityStone_2 = 204,    //������ʯ
    ItemType_AntiquityStone_3 = 205,    //����ʯ

    ItemType_Medal = 206,       //ѫ��, Medal װ�����������������
    ItemType_ActiveReward = 207,  // �������Ʒ
    ItemType_Tactics = 208, // ս������
    ItemType_TankPartPic = 209, // ս�����ͼֽ
    ItemType_TankPark = 210,    // ս�����
    ItemType_ElemGem = 211, // ��ǶԪ�ر�ʯ��,��Ƕ��������װ��
    ItemType_ElemEss = 212, // ��ǶԪ�ؾ�ʯ��,��Ƕ��������װ��
    ItemType_GiftBag = 213,  // �������������
    ItemType_HomePetFood = 214, //��������ʳ������
    ItemType_HoleStuff = 215,   //������õĵ�������

    ItemType_MakePattern = 216, //���켼�������ͼֽ�����䷽����.
    ItemType_PastorBar =217,    //��ʦ�õ�Ȩ����
    ItemType_FashionPattern = 218, //����ʱװ�����ͼֽ����
    ItemType_FineSword = 219,//ϸ��
    ItemType_TreasureMap = 220, //�ر�ͼ
    ItemType_Tank = 221, //ս����
    ItemType_HorseCard = 222,   //����  

    ItemType_QuestTrigger = 223,    //�����������
	ItemType_GemPiece = 224,        //��ʯ��Ƭ ��ʯ��Ƭ �����

	ItemType_HairDyes = 225,        //Ⱦ���������
	ItemType_DearObject = 226,      //�����Ʒ  ����NPCʱ��ԭ�۴���3
	ItemType_QuestTriggerCool = 227, //�����������,����ȴʱ��,ÿ�μ�100�;ö�
	ItemType_GoldToMoney = 228,      //���ڰ�ˮ���Ҷһ��ɽ����ҵĵ�������
    ItemType_LingQi     =229,   //����
	ItemType_TankObj = 230,		// ս����Ѫ����
	ItemType_tFuMo = 231,		//  ��ħ��Ʒ
	ItemType_tCloak = 232,		// ������
	ItemType_tCoGift = 233,     //��������
	ItemType_EmotionTalk = 234, //�Զ�����������
	ItemType_Fire = 235,		// ��������
	ItemType_Wine = 236,		// ����ʱ�ȵľ�

	ItemType_LingQiFeedUp   = 237,  //����������
    ItemType_AutoRepair = 238, //�Զ�������Ʒ
	ItemType_Exp         = 239,  //�����飬ʹ�ú�ֱ�Ӹ�����
	ItemType_UnionMoney         = 240,  //����ֱ�Ӽ�Ǯ����


	//�µ�ѫ������,ԭ�е�206����
	ItemType_ForeverMedal   = 241,		//������ѫ��
	ItemType_TimeEraseMedal = 242,		//ʱ����ѫ��(����ɾ��) 
	ItemType_TimeInvalidMedal = 243,	//ʱ����ѫ��(����ʧȥ����)
	ItemType_AutoFireSkill = 244,  //�Զ��ͷż��ܲ�ʱ�����
	ItemType_FunctionObject = 245, //�����Ե��ߣ������Ҽ������������һ���Ի��򣬹�����Ҫ����һ���Ի������
	ItemType_LingQiAddExp = 246, //�������Ӿ������
	ItemType_DoubleExpObject = 247, //˫������۵���
	 ItemType_EmployGather = 248,  //��Ӷ�ɼ�ƾ֤
	ItemType_AddRentStoreTime = 249, //���޲ֿ��ʱ����
	ItemType_AddPersonAttribute = 250, //��ֱ���������������ħ�������������Ե���Ʒ
	
	ItemType_FishingRob = 251,  /// ���
	ItemType_FishingBait = 252,  /// ���
	ItemType_FuwenMozhu  = 253,  ///����ħ�飬��װ���ӷ�������
	ItemType_MerchAddit     = 254,  ///Ӷ�����̳Ǹ���
	ItemType_UnionObject     = 255,  ///�ǰ���Ʒ
	ItemType_ResetEvolveTimes  = 256,  ///������������������ֵ
	ItemType_LingYuanZhu       = 257,  ///��Ԫ��
	ItemType_RongLingFen       = 258,  ///�����
	ItemType_ChuanChengNotFull   = 259,  ///����֮��,δ������
	ItemType_ChuanChengFull   = 260,  ///����֮��,������
	ItemType_EquipAid   = 261,  //װ����������
	ItemType_StarKeeper   = 262,  //���ƣ����ǣ��������� (����ʯ)
	ItemType_UnionSeed = 263,   // ������
	ItemType_SkillBookNotFull = 264,  // δ��ɵļ�����
	ItemType_SkillBookBlank = 265,    // �հ׵ļ�����
	ItemType_AddLingQiMinusExp = 266,  //Ϊ��������Ϊ��ʱ���Ӿ���
	ItemType_ClearDrunkennessTime = 267,     //���ҩ 
	ItemType_LingYuanZhu_CanAdd = 268,       //�ɵ��ӵ���Ԫ��
	ItemType_Qianneng_Obj       = 269,      //Ǳ�������
	ItemType_FuWenQiangHua			= 270,		//����ǿ����
	ItemType_JieZhanMedal           = 271,      //��սָ�ӹ�ӡ��
	ItemType_LingQi_Model           = 272,        //����ģ����
	ItemType_FlyItem                        = 273,          //��Ʒ����߳��
	ItemType_DragonEquip                    = 274,          //��װ������
    ItemType_Dragon                         = 275,         //������
    ItemType_DragonFood                     = 276,          //����ʳ��
	ItemType_NewGiftBox                     = 277,          //�±������
	ItemType_Bugle                          = 278,          //����С���ȵ���
	ItemType_EvolveHorseScroll              = 279,          //�����������
	ItemType_EvolveHorseStone               = 280,          //���������ʯ
	ItemType_HappyCard						= 281,			//����齱 ���ֿ�Ƭ
	ItemType_FuWenStone               = 282,          //��������ʯͷ
	ItemType_AddRing                = 283,      //����������
	ItemType_GemHammer				= 284,		//��ʯ��
	ItemType_Fight				    = 285,		//ս������
	ItemType_CrystalCertificate     = 286,  //ˮ���Ҷһ�ȯ(��ˮ���ҵ������ӵ���)
	ItemType_PointOpenGiftBox       = 287,  // (����)�����򿪱�����ߣ�ÿ�δ����Ķ�Ӧ�����������;��ֶ�
	ItemType_PointOpenGiftBag       = 288,  // (���)������������ߣ�ÿ�δ����Ķ�Ӧ�����������;��ֶ�
	ItemType_PurpleDef   = 289,  // ��װ�������� ��������
    ItemType_PurpleJew   = 290,  // ��װ�������� ������Ʒ
    ItemType_PurpleWeapon  = 291,  // ��װ�������� ��������
    ItemType_PurplePiece = 292,  // ��װ������Ƭ
	ItemType_LingQi_ExpZhu = 293,  //���������飬���ʱ���鱶��
	ItemType_Exp_To_LingQiExp = 294,  //���ﾭ�鵽��������
	ItemType_GiftBox_WenMing = 295,  //װ����������
	ItemType_DecayLingQi_ReRound = 296,      //�ɱ��������10������

	ItemType_OffPurple = 297,  // ȡ����װ����


/////////////////////////////����һ�������ķָ���///////////////////////////////////////////////////////////////////////////////
	ItemType_GiftBag_Card	    =	300,	//�����淨���������
};

enum LeechdomType  //ҩˮ����
{
	Leechdom_default = 0,
	Leechdom_dam = 1,          //�����˺���
	Leechdom_def = 2,          //���ӷ�����
	Leechdom_poison = 3,       //ҩ��ʹ�˳����ж�
	Leechdom_sppersist = 4,    //��Ʒʹ������ֵ���ֵ�ǰֵ����
	Leechdom_spup = 5,         //ҩ������������ֵ�ٷֱ�
	Leechdom_spcostdown = 6,   //�������������ٶ�
	Leechdom_spresumeup = 7,   //�ӿ������ָ��ٶ�
	Leechdom_hp = 8,           //һ���Իָ�����
	Leechdom_hppersist = 9,    //�����ָ�����
	Leechdom_mp = 10,          //һ���Իָ�����ֵ
	Leechdom_mppersist = 11,   //�����ָ�����
	Leechdom_sp = 12,          //һ���Իָ�����ֵ
	Leechdom_hp5 = 13,         //һ���Իָ�����
	Leechdom_hppersist5 = 14,   //�����ָ�����
	Leechdom_hpmax = 15,       //�����;�һ�μ���HP
	Leechdom_mpmax = 16,       //�����;�һ�μ���MP
	Leechdom_chocolate = 17,    //�ɿ���

	Leechdom_hpmax25 = 18,     //�����;�һ�μ���HP,��ȴ.5��
	Leechdom_mpmax25 = 19,     //�����;�һ�μ���MP����ȴ.5��

	Leechdom_UnionItemsAttack_3 = 22,	//�����ߣ������࣬.5����ȴ
	Leechdom_UnionItemsHP_3 = 23,		//�����ߣ���Ѫ�࣬.5����ȴ
	Leechdom_hpmax25_3500 = 24,  //�����;ã�һ�μ�Ѫ����ȴ.5��
	Leechdom_hpmax25_4000 = 25,  //�����;ã�һ�μ�Ѫ����ȴ.5��
	Leechdom_hpmax25_4500 = 26,  //�����;ã�һ�μ�Ѫ����ȴ.5��
	Leechdom_hpmax25_5000 = 27,  //�����;ã�һ�μ�Ѫ����ȴ.5��
	Leechdom_hpmax25_6000 = 28,  //�����;ã�һ�μ�Ѫ����ȴ.5��
	Leechdom_hpmax25_4300 = 29,
	Leechdom_hpmax25_4600 = 30,
	Leechdom_hpmax25_4900 = 31,
	Leechdom_hpmax25_5200 = 32,
	Leechdom_hpmax25_5500 = 33,
	Leechdom_hpmax25_8000 = 34,
	Leechdom_hppercent = 35,     //�ٷֱȻָ����� 
	Leechdom_mppercent = 36,     //�ٷֱȻָ�ħ��
	// �������ҩˮ, �൱�ڶ��������ҩˮ, ÿʹ�ôο۵��;�
	Leechdom_hpManyPersist = 37,
	Leechdom_mpManyPersist = 38,
	Leechdom_hppercent_dutypet = 39,  // ʹ���ٻ�������ٻ��޻ָ�Ѫ���ٷֱ�
};

#if 0
enum {				//setpos����
	EQUIPCELLTYPE_NONE=0,       /// ����װ��
    EQUIPCELLTYPE_HELM=1,       /// ͷ��
    EQUIPCELLTYPE_BODY=2,       /// ��װ
    EQUIPCELLTYPE_HANDR=3,      /// ����
    EQUIPCELLTYPE_HANDL=4,      /// ����
    EQUIPCELLTYPE_NECKLACE=5,   /// ����
    EQUIPCELLTYPE_GLOVES=6,     /// ����,����
    EQUIPCELLTYPE_RING=7,       /// ��ָ
    EQUIPCELLTYPE_BELT=8,       /// ����
    EQUIPCELLTYPE_SHOES=9,      /// Ь��
	EQUIPCELLTYPE_PACKAGE=11,	/// �������
    EQUIPCELLTYPE_FASHION=15,   /// ʱװ
    EQUIPCELLTYPE_EAR=16,       /// ����, װ������
    EQUIPCELLTYPE_SHOULDER=17,  /// �粿, װ�����
    EQUIPCELLTYPE_ARM=18,       /// �첲λ��, װ���ۻ�
    EQUIPCELLTYPE_KNEE=19,      /// ϥ��, װ����ϥ
    EQUIPCELLTYPE_AUTOPHY=20,   /// �Զ�������ֵװ��
    EQUIPCELLTYPE_AUTOMAG=21,   /// �Զ���ħ��ֵװ��
    EQUIPCELLTYPE_CLOAK=22,         /// ����
    EQUIPCELLTYPE_FASHIONCLOAK=23,  /// ʱװ����
    EQUIPCELLTYPE_FASHIONHELM=24,   /// ʱװͷ��
    EQUIPCELLTYPE_SBEAST=25,        /// ����װ����,�и��ӹ���(����������������Ķ���)
    EQUIPCELLTYPE_MEDAL=26,         /// �������ӣ��������óƺ�ϵͳ��ѫ��
	EQUIPCELLTYPE_HANDLR = 29,      ///��ʶ�����ֶ�����װ������Ʒ,��˫�ָ�
};
#endif

#if 0
/// ������Ʒ��������
enum{
    OBJECTCELLTYPE_NONE,        /// ���Ǹ��ӣ����ڶ��������Ʒ
    OBJECTCELLTYPE_COMMON,      /// ��ͨ��Ʒ����
    OBJECTCELLTYPE_EQUIP,       /// װ������
    OBJECTCELLTYPE_MOUSE,       /// ���
    OBJECTCELLTYPE_TRADE,       /// �Լ��Ľ��׸���
    OBJECTCELLTYPE_OTHERTRADE,  /// �Է��Ľ��׸���
    OBJECTCELLTYPE_BANK,        /// ����
    OBJECTCELLTYPE_SELL,        /// ��
    OBJECTCELLTYPE_STORE,       /// �ֿ�
    OBJECTCELLTYPE_EQUIPSHOW,   /// ���Լ����ŵ�װ��
    OBJECTCELLTYPE_PACKAGE,    /// �����ĸ���
    OBJECTCELLTYPE_MAKE,       /// �ϳɡ���������Ƕ�ĸ���
    OBJECTCELLTYPE_MYSHOP,      /// �Լ�̯λ�ĸ���
    OBJECTCELLTYPE_OTHERSSHOP,  /// ������̯λ�ĸ���
    OBJECTCELLTYPE_MAIL,        /// �ʼ�ϵͳ�ĸ���
    OBJECTCELLTYPE_COUNTRY_SAVEBOX, /// ���҂}��
    OBJECTCELLTYPE_PET,       /// �������
    OBJECTCELLTYPE_GIFTBOX,     //���еĸ���  //KOK20090325���书��
    OBJECTCELLTYPE_GOD_EQUIP = 18,   //����
    OBJECTCELLTYPE_RENT_STORE,  //���Ĳֿ�
    OBJECTCELLTYPE_STONE = 20,  //����ǧ�꺮��
	OBJECTCELLTYPE_FSTORAGE = 21,	// �Ҿֿ߲�
	OBJECTCELLTYPE_DAILYGIFT = 22,	// ÿ�������Ʒ
	OBJECTCELLTYPE_MAGICSTOVE = 23, // ħ��¯
	OBJECTCELLTYPE_PUBSTORAGE = 24,	// ������Ĺ��ô�����
	OBJECTCELLTYPE_PRISTORAGE = 25,	// ������ĸ��˴�����
	OBJECTCELLTYPE_NEWGIFTBOX = 26, // �±�����Ʒ����
	OBJECTCELLTYPE_RENT_STORE3 = 27, // �����ֿ�
};
#endif

//��������
enum {
	MAIN_PACK = 1,
	LEFT_PACK = 2,  //װ���������Ӱ���
	RIGHT_PACK = 4, //װ���������Ӱ���
	EQUIP_PACK = 8, 
	STORE_PACK = 16,
	LM_PACK = 32, 
	RM_PACK = 64,
	PET_PACK = 128,
};

//����׵�����==================================================
#define	EQUIP_MAX_GROOVENUM	6
//��ӡ���Ͷ���:
enum
{
        GODSEAL_NONE  = 0,    //��
        GODSEAL_LIGHT = 1,   //������װ
        GODSEAL_CHAOS = 2,   //������װ
        GODSEAL_DARK  = 4,   //�ڰ���װ
        
		GODSEAL_SUN  = 100,      //������װ
        GODSEAL_SPIRIT = 101,    //ʥ����װ
        GODSEAL_THORNS = 102,    //������װ
        GODSEAL_FLAME = 103,     //������װ
        GODSEAL_ASSAULT = 104,   //ǿϮ��װ
        GODSEAL_DEFENCE = 105,   //�Ƿ�����װ
        GODSEAL_CROSS = 106,      //ʮ�־���װ
		GODSEAL_GUYONGJUN = 107,  //��Ӷ����װ
		GODSEAL_ESSENCE = 108,    //��Ӣ��װ
        GODSEAL_WARD = 109,       //�ػ���װ
		GODSEAL_MOON = 110,      //������װ
        GODSEAL_BLOOD = 111,     //Ѫɫ��װ
        GODSEAL_PURPLE = 112,    //�Ͻ���װ
		GODSEAL_CANGQIONG = 113, //�����װ
		GODSEAL_MINGSHEN = 114,  //ڤ����װ
		GODSEAL_SHEHUN = 115,    //�����װ
		GODSEAL_ZIYAN = 116,     //������װ
		GODSEAL_GUANGHUI = 117,  //�����װ
		GODSEAL_TANXI = 118,     //̾Ϣ��װ
		GODSEAL_QIANCHENG = 119,   //����װ
		GODSEAL_SHENGGUANG = 120,  //ʥ����װ
		GODSEAL_KUANSHU = 121,     //��ˡ��װ		
		GODSEAL_SHENGJIE = 122,    //ʥ����װ
		GODSEAL_BENLEI = 123,      //������װ
		GODSEAL_XUANMO = 124,      //��ħ��װ
		GODSEAL_MINGYAN = 125,     //ڤ����װ
		GODSEAL_FENGBAO = 126,     //�籩��װ
		GODSEAL_HEIYI = 127,       //������װ
		GODSEAL_MINGHUO = 128,     //ڤ����װ
		GODSEAL_ZIDI = 129,        //�ϵ���װ
		GODSEAL_KUANGSHEN = 130,   //������װ
};

enum
{
	GODSEAL_Attr_Dpdam = 1,				//�����˺����� %0.2f%%
	GODSEAL_Attr_Dmdam = 2,				//ħ���˺����� %0.2f%%
	GODSEAL_Attr_Bdam = 3,				//�˺����� %0.2f%%
	GODSEAL_Attr_Rdam = 4,				//�˺����� %0.2f%%
	GODSEAL_Attr_Ignoredef = 5,			//���ӷ��� %d
	GODSEAL_Attr_ExcellentAttack = 6,	//׿Խһ�� %d
	GODSEAL_Attr_Blife = 7,				//����     %0.2f%%

	GODSEAL_Attr_IncDam_Num = 20,				//�˺�����%d��
	GODSEAL_Attr_DecDam_Num = 21,				//���˼���%d��
	GODSEAL_Attr_IncPhicAttack_NUM = 22,		//������������x��
	GODSEAL_Attr_IncMagicAttack_NUM = 23,	//ħ������������x��
	GODSEAL_Attr_IncPhicAttack = 24,		//������������x%
	GODSEAL_Attr_IncMagicAttack = 25,	//ħ������������x%
	GODSEAL_Attr_IncAllAttrs_NUM = 26,	//ȫ��������x��
	GODSEAL_Attr_IncCruel = 27,			//��������x%
	GODSEAL_Attr_GainExp = 28,			//ɱ���10�����ڵĹ�����%d�㾭��
	GODSEAL_Attr_IncPhicDefend_NUM = 29,	//�������������x��
	GODSEAL_Attr_IncMagicDefend_NUM = 30,	//ħ������������x��
	GODSEAL_Attr_IncCruelDefend_NUM = 31,		//����������x��
	GODSEAL_Attr_IncEvadeRate = 32,			//���������x��
	GODSEAL_Attr_MobExpPercent = 33,		//ɱ�־�������x%
	

	GODSEAL_Attr_AttackAndDefend = 40,	//����˫��x��,����˫��y��
	GODSEAL_Attr_DefendAndAttack = 41,	//����˫��x��,����˫��y��
	GODSEAL_Attr_RetrieveLife = 42,		//ÿx�������ָ�y��
	GODSEAL_Attr_RetrieveMagical = 43	//ÿx��ħ���ָ�y��

};
enum
{
        AUCTION_WEAPOM_SWORD = 1,
        AUCTION_WEAPOM_AXE = 2,
        AUCTION_WEAPOM_BOW = 3,
        AUCTION_WEAPOM_STICK = 4,
        AUCTION_WEAPOM_WAND = 5,
        AUCTION_WEAPOM_FAN = 6,
        AUCTION_WEAPOM_DAGGER = 7,//����
        AUCTION_WEAPOM_NEW_SWORD = 8,//��
        AUCTION_EQUIP_HEAD = 11,
        AUCTION_EQUIP_BODY = 12,
        AUCTION_EQUIP_WRIST = 13,
        AUCTION_EQUIP_SHIELD = 14,
        AUCTION_EQUIP_WAIST = 15,
        AUCTION_EQUIP_FOOT = 16,
        AUCTION_EQUIP_MIRROR = 17,//�� 
        AUCTION_ACCESSORY_NECKLACE = 21,
        AUCTION_ACCESSORY_RING = 22,
        AUCTION_ACCESSORY_ADORNMENT = 23,
        AUCTION_BOOK_FIGHTER = 31,
        AUCTION_BOOK_ARCHER = 32,
        AUCTION_BOOK_WIZARD = 33,
        AUCTION_BOOK_SUMMONER = 34,
        AUCTION_BOOK_PRIEST = 35,
        AUCTION_BOOK_SPECIAL = 36,
        AUCTION_BOOK_ASSASSIN = 37,//�̿�
        AUCTION_BOOK_GUARD = 38,//��ʿ
        AUCTION_BOOK_GEN = 39,//�츳
        AUCTION_OTHER_GEM = 41,
        AUCTION_OTHER_ITEM = 42,
        AUCTION_OTHER_MATERIAL = 43,
        AUCTION_OTHER_LEECHDOM = 44,
        AUCTION_OTHER_SOUL = 45,
        AUCTION_OTHER_BMW = 46,    //����  
        AUCTION_OTHER_XIUFUSHI = 47,    //�޸�ʯ
        AUCTION_OTHER_ZHUFUSHI = 48,    //ף��ʯ
        AUCTION_OTHER_DINGXINGSHI = 49    //����ʯ
};


#define BOW_ARROW_ITEM_TYPE 21

namespace Object
{
    enum
    {
	INVALID_LOC = (DWORD)-1,
	INVALID_TAB = (DWORD)-1,
	INVALID_X = (WORD)-1,
	INVALID_Y = (WORD)-1,
    };
}
#pragma pack(1)

struct	stObjectLocation
{
	DWORD dwLocation;   // ��������  1
	DWORD dwTableID;    // ����ID    ->0
	WORD  x;		//��
	WORD  y;		//��

	friend class Package;
	friend class Packages;
    public:
	stObjectLocation()
	    :dwLocation(Object::INVALID_LOC), dwTableID(Object::INVALID_TAB),x(Object::INVALID_X),y(Object::INVALID_Y)
	{}

	stObjectLocation(const stObjectLocation& loc)
	    :dwLocation(loc.dwLocation),dwTableID(loc.dwTableID),x(loc.x),y(loc.y)
	{}

	stObjectLocation(DWORD loc, DWORD tab, WORD _x, WORD _y)
	    :dwLocation(loc), dwTableID(tab), x(_x), y(_y)
	{}

	void operator = (const stObjectLocation& loc)
	{
	    dwLocation = loc.dwLocation;
	    dwTableID = loc.dwTableID;
	    x = loc.x;
	    y = loc.y;
	}

	bool operator == (const stObjectLocation& st) const
	{
	    return (dwLocation==st.dwLocation && dwTableID==st.dwTableID && x==st.x && y==st.y);
	}

	bool operator!= (const stObjectLocation& st) const
	{
	    return !(*this == st);
	}

	void setLoc(DWORD loc)
	{
	    dwLocation = loc;
	}

	void setX(DWORD ix)
	{
	    x = ix;
	}
	
	void setY(DWORD iy)
	{
	    y = iy;
	}

	void setXY(WORD ix, WORD iy)
	{
	    x = ix;
	    y = iy;
	}

	void setAll(DWORD loc, DWORD tab, WORD ix, WORD iy)
	{
	    dwLocation = loc;
	    dwTableID = tab;
	    x = ix;
	    y = iy;
	}

	DWORD tab() const
	{
	    return dwTableID;
	}

	void tab(DWORD t)
	{
	    dwTableID = t;
	}

	DWORD loc() const
	{
	    return dwLocation;
	}

	WORD xpos() const
	{
	    return x;
	}

	WORD ypos() const
	{
	    return y;
	}
};

//t_Object��lure�Ķ���
#define OBJECT_LURE_BUYFROMMARKET 0x1	//��ʾ���̳ǹ���
#define OBJECT_LURE_MADED	0x2			//��ʾ����Ʒ�Ǵ��������, �����Ǵ�ֵ���, ħ��¯�����ȷ�ʽ������
#define OBJECT_LURE_JingJie 0x4			//��ʾ����Ʒ���о�������(ʹװ���ߵ��ƶ��ٶȹ̶�Ϊ�����ٶȵ�50%), Ŀǰֻ��ʱװ�����������
//�������˷��͵���Ʒ��Ϣ
typedef struct t_Object
{
	DWORD qwThisID;   //��ƷΨһid
	DWORD dwObjectID;  //��Ʒ���еı��
	DWORD type;	//��Ʒ���id Add for Kok
	char strName[MAX_NAMESIZE]; //����
	
	stObjectLocation pos;	// λ��
	DWORD dwNum;	// ����
	BYTE upgrade;//��Ʒ�����ȼ�
	//BYTE kind;	//��Ʒ����, 0��ͨ, 1��ɫ, 2��ɫ, 4��ʥ, 8��װ
	BYTE kind;	//change for kok by zgm ��Ʒ����, 0��ɫ, 1��ɫ, 2��ɫ, 4��ɫ // �����������ӵ��Ļ����ɫ 5�̽׶�, 6�̽���, 6�̽���, 8�̽���
	BYTE subkind;//��Ʒ�� (+1~+9)
	DWORD exp;  //���߾���
	
	WORD needlevel;				// ��Ҫ�ȼ�

	WORD maxhp;					// �������ֵ      // ��Ӧ��ƥ HP�ӳ�
	WORD maxmp;					// �����ֵ      // ��Ӧ��ƥ MP�ӳ�
	WORD maxsp;					// �������ֵ	   // ��Ӧ�Ļ�ĵ�ǰ����ֵ	

	WORD pdamage;				// ��С������      // ��Ӧ��ƥ �﹥�ӳ�
	WORD maxpdamage;			// ��󹥻���
	WORD mdamage;				// ��С����������  // ��Ӧ��ƥ ħ���ӳ�
	WORD maxmdamage;			// �����������

	WORD pdefence;				// ���            // ��Ӧ��ƥ ����ӳ�
	WORD mdefence;				// ħ��            // ��Ӧ��ƥ ħ���ӳ�
	BYTE damagebonus;			// �˺��ӳ�x% from ���߻�����
	BYTE damage;				// �����˺�ֵx��from ��ʥװ����
		
	WORD akspeed;				// �����ٶ�
	WORD mvspeed;				// �ƶ��ٶ�
	WORD atrating;				// ������
	WORD akdodge;				// �����

	DWORD color;				// ��ɫ	

	WORD str;  // ����                               // ��Ӧ��ƥ ���� // ʱװ����������������Ϊ����HP��MP���޵İٷֱ�
	WORD inte;  // ����                              // ��Ӧ��ƥ ���� // ʱװ����������������Ϊ����HP��MP���޵İٷֱ�
	WORD dex;  // ����                               // ��Ӧ��ƥ ����
	WORD spi;  // ����                               // ��Ӧ��ƥ ����
	WORD con;  // ����                               // ��Ӧ��ƥ ����

	WORD kar;  //����                                // ��Ӧ��ƥ ����


//ע��ԭ�����������ֶ�,kok��װ����λ��ȷ������װ����ouyx
//	WORD fivetype;  // ��������
//	WORD fivepoint; // ��������
	BYTE godseal;   //��ӡ���Ͷ�����Command.h��
	
	WORD hpr;  // ����ֵ�ָ�
	WORD mpr;  // ����ֵ�ָ�
	WORD flag;  //��־�ֶ�

	WORD holy;  //��ʥһ��	
	WORD bang;      //�ػ�->����
	WORD bangdef;  //������
	WORD pdam;  // ������������  100���  �ٷֱ�
	WORD pdef;  // �������������  100���  �ٷֱ�
	WORD mdam;  // ����ħ��������  100���  �ٷֱ�
	WORD mdef;  // ����ħ��������  100���  �ٷֱ�
	
	WORD poisondef; //��������
	WORD lulldef; //���������
	WORD reeldef; //��ѣ������
	WORD evildef; //����ħ����
	WORD bitedef; //����������
	WORD chaosdef; //����������
	WORD colddef; //����������
	WORD petrifydef; //��ʯ������
	WORD blinddef; //��ʧ������
	WORD stabledef; //����������
	WORD slowdef; //����������
	WORD luredef; //���ջ�����

	WORD durpoint; //�ָ�װ���;öȵ���
	WORD dursecond; //�ָ�װ���;ö�ʱ�䵥λ

	struct skillbonus {
		WORD id; //����id
		WORD point; // ���ܵ���
	} skill[10]; //���ܼӳ�

	struct skillsbonus {
		WORD id; //����id
		WORD point; // ���ܵ���
	} skills;	//ȫϵ���ܼӳ�

	WORD poison; //�ж�����
	WORD lull; //�������
	WORD reel; //ѣ������
	WORD evil; //��ħ����
	WORD bite; //��������
	WORD chaos; //��������
	WORD cold; //��������
	WORD petrify; //ʯ������
	WORD blind; //ʧ������
	WORD stable; //��������
	WORD slow; //��������
	WORD lure; //�޸�Ϊר�õı�־λ,��λȡֵ; ��һλ���ڱ���Ƿ��Ǵ��̳ǹ������Ʒ,�ȵ�, ��ϸ�������, ��OBJECT_LURE_BUYFROMMARKET
	
	struct leech
	{
		BYTE odds;    //x
		WORD effect;	//y
	};
	leech hpleech; //x%��������ֵy
	leech mpleech; //x%���շ���ֵy

	BYTE hptomp; //ת������ֵΪ����ֵx��
	BYTE dhpp; //�����˺�����x%	
	BYTE dmpp; //�����˺�ֵ����x%		

	//BYTE incgold; //��װ��־
	//BYTE doublexp; //x%˫������		
	//BYTE mf; //���ӵ�����x%
	
	BYTE bind;  //װ���Ƿ�� 0�ǰ�  1��

//������װ��������޸�, ouyx ---------------start
	/*ע��ԭ����
	union {
		BYTE _five_props[5];
		struct {
			//������װ�������
			BYTE dpdam; //�����˺�����%x
			BYTE dmdam; //�����˺�����%x
			BYTE bdam; //�����˺�x%
			BYTE rdam; //�˺�����%x
			BYTE ignoredef; //%x����Ŀ�����
		};
	};
	*/
	union {  //Ϊ�˱��澫ȷ�İٷ���,�����֮����������洢
		 WORD _seven_props[7];
		struct {
			//��װ�������
			WORD dpdam; //�����˺�����x%%
			WORD dmdam; //�����˺�����x%%
			WORD bdam; //�����˺�x%%
			WORD rdam; //�˺�����x%%
			WORD ignoredef; //x%%����Ŀ�����
			WORD excellentAttack;  //׿Խһ��
			WORD blife;  //��������ֵx%%
		};
	};

//	WORD fiveset[5]; //������װ, ��˳������
//---------------------------------------------end

	//...
	BYTE width;  //���
	BYTE timeeffect; //ʱЧ״̬, 0 -- ��ͨ��Ʒ, û��ʱЧ����; 1 -- ��ʱЧ����, ��û����; 2 -- ��ʱЧ����, �ѹ���
	DWORD dur; 	//��ǰ�;ö�
	DWORD maxdur; //����;ö�
	
	// �°汾���ֶα���Ϊ�ĸ��ֽ�ʹ��,�ӵ�λ->��λ, ��һ�ֽ�==255Ϊ��Ч�Ŀ�,��û�ű�ʯ,==0Ϊ��Ч�Ŀ�,���ܷű�ʯ,>0&&!=255���ʾ����һ����ʯ,����ֵ���Ǳ�ʯ�ı��
	// �ڶ����ֽڱ���
	// �������ֽڱ�ʾ�ñ�ʯ���ӵ�ֵ
	// ���ĸ��ֽڱ�ʾ�ñ�ʯ���ӵ��������
	//DWORD socket[6]; //��
	
	// Change for KoK by hb. ----------start-----------
	// ����������ǰģʽ, �ӵ�λ����λ, ��һ�ֽ�==255Ϊ��Ч�Ŀ�,��û�ű�ʯ,==0Ϊ��Ч�Ŀ�,���ܷű�ʯ,>0&&!=255���ʾ����һ����ʯ,ֵ���Ǳ�ʯ�ı��
	// �ڶ��ֽڵĵ�λΪ�׵���ɫ, ΪCmd::ICE��~5����֮һ, ��λΪ��ʯ����ɫ, ͬ��ΪCmd::ICE��~5����֮һ
	// ���������ֽ�Ϊ��ʯ�ļӳɻ������յ�����ֵ
	DWORD socket[6]; //��Changed for kok ��Ϊ��
	// -------end-----
	
	// ���⼤�������ֶ�,�����Ƕ�ı�ʯ��ɫ�Ϳ׵���ɫһ���Ϳ��ܳ��ֶ��⼤������,һ��װ����������
	// �ӵ�λ����λ,��һ�ֽ�Ϊ��ʾ�����˼�������,����û����.1��ʾ�����˶�������,�ڶ��ֽڱ�ʾ�������Ե�����
	// ���������ֽ�Ϊ�ӳɻ������յ�����ֵ add for kok by zgm
	DWORD extra_active[2];
	
	//stSocket socket[7];	// ��, Changed for kok by hb ���ø������Ķ���
	DWORD price;     //�۸�
	//DWORD cardpoint; //������˵��cardpoint��������ĸ�λ��ʾת������!
	char maker[MAX_NAMESIZE]; //������

	// Add for Kok by hb ȡ�з�����, ������ʾ�ӳ�,������ʾ����
    WORD  aIce;        // ��Ԫ���˺��ӳ�
    WORD  aThunder;    // ��Ԫ���˺��ӳ�
    WORD  aWind;       // ��Ԫ���˺��ӳ�
    WORD  aElec;       // ��Ԫ���˺��ӳ�
    WORD  aFire;       // ��Ԫ���˺��ӳ�

	WORD mIce;			// ��Ԫ���˺�����
	WORD mThunder;		// ��Ԫ���˺�����
	WORD mWind;			// ��Ԫ���˺�����
	WORD mElec; 		// ��Ԫ���˺�����
	WORD mFire;			// ��Ԫ���˺�����

	QWORD modelid;      //ģ�ͱ��add for kok

	////////////////////////////////////////
	/// ħ��¯��ħ�ӳ�
	WORD charmadd_pdamage;                  /// ħ��¯(��ħ�仯ֵ)
	WORD charmadd_mdamage;                  /// ħ��¯(��ħ�仯ֵ)
	WORD charmadd_pdefence;                 /// ħ��¯(��ħ�仯ֵ)
	WORD charmadd_mdefence;                 /// ħ��¯(��ħ�仯ֵ)
	////////////////////////////////////////

	////////////////////////////////////////
	/// װ��ǿ��Ӱ��ӳ�
	union
	{
	    BYTE subupgrade;        /// ��Ʒ�����׸����ȼ�(װ��ǿ������)
	    BYTE moon;
	};
	////////////////////////////////////////

	struct
	{
	    DWORD jobEnergy;  // ְҵװ��:ְҵ����
	    WORD  energyJob;  // ְҵװ��:������ְҵ����, ����ͬCharBase.job  �ͻ��˶�Ӧְҵװ����ְҵ�����е�ְҵ��ʾ
	};

	BYTE sealStars;//��������(��ӡ+upgrade), 0��ʾ�޷�ӡ
	BYTE sealMoons;//��������(��ӡ+subupgrade), 0��ʾ�޷�ӡ

	BYTE broadcast; // ������ʱ���������� �±���ʹ�� WDS 20100811
	BYTE mAllPointPer;//ȫ���԰ٷֱ�
	BYTE purpleFlag;//��װ�ȼ�
	BYTE purpleValue;//��װ��ֵ �ٷֱ�
	BYTE nextUseBYTE5;//Ԥ���ֶΣ��´�ʹ�ø���
	BYTE nextUseBYTE6;//Ԥ���ֶΣ��´�ʹ�ø���

	WORD purplePdamage;     //��װ�ӳ��﹥
	WORD purpleMaxpdamage;  //��װ�ӳ�����﹥

	WORD purpleMdamage;     //��װ�ӳ�ħ��
	WORD purpleMaxmdamage;  //��װ�ӳ����ħ��

	WORD purplePdefence;    //��װ�ӳ����
	WORD purpleMdefence;    //��װ�ӳ�ħ��

	DWORD nextUseDWORD2;//Ԥ���ֶ�, �´�ʹ�ø���
	DWORD nextUseDWORD3;//Ԥ���ֶ�, �´�ʹ�ø���
	DWORD nextUseDWORD4;//Ԥ���ֶ�, �´�ʹ�ø���
	DWORD nextUseDWORD5;//Ԥ���ֶ�, �´�ʹ�ø���

};

/**
 * \brief   ���� �������Ϳͻ���ͨ�ŵ�������
*/
typedef struct t_Object_mobile
{
	DWORD dwThisID;   //��ƷΨһid
	DWORD dwObjectID;  //��Ʒ���еı��
	stObjectLocation pos;	// λ��
	DWORD dwNum;	// ����

};

#pragma pack()
namespace Object
{
    static stObjectLocation INVALID_POS;
}
#endif
