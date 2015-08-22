#ifndef _OBJECT_H
#define _OBJECT_H
#include "zType.h"
//************************************************************//
//	说明所有的equipItem分为道具和装备,添加类型时,请注意		  //
//************************************************************//
enum enumItemType
{
    ItemType_None,
    ItemType_Resource = 16, //16代表一般道具
    ItemType_Leechdom,  //17代表药品类
    ItemType_FoodRes,   //18代表食物原料类
    ItemType_Food,      //19代表食物类
    ItemType_Tools,     //20代表劳动工具类
    ItemType_Arrow,     //21代表配合弓使用的箭支类
    //ItemType_BattleHorse,   //22战马
    ItemType_Pack = 23,      //23代表包裹类
    ItemType_Money,     //24代表金钱类
    ItemType_Scroll,    //25代表转移卷轴类
    ItemType_Move,      //26代表特殊移动道具类
    ItemType_LevelUp,   //27代表道具升级需要的材料类
    ItemType_CaptureWeapon, //28代表驯服宠物用武器
    ItemType_Union, //29代表创建帮会需要的道具.
    ItemType_Tonic, //30表示自动补药类道具.
    ItemType_Gift,  //31代表礼品类物品.
    ItemType_Other,
    ItemType_MASK = 33,     //33代表蒙面巾
    ItemType_Quest = 34,
    //ItemType_HORSE = 35,
    ItemType_SOULSTONE = 37, //37代表魂魄石类
    ItemType_Wedding = 38, //38代表婚礼类
    ItemType_Change = 41,   //41 代表合成道具
    ItemType_Auto = 42,   //42 代表自动练功
    ItemType_SkillUp = 43,   //43 代表技能升级道具
    ItemType_Book = 44, //44代表书籍
    ItemType_Store = 45,   //45 代表仓库
    ItemType_Renew = 46,   //46 代表洗点道具
    ItemType_Repair = 47, //47代表修复宝石类
    ItemType_GeniusSkillBookPage = 49, //天赋书残页
    ItemType_DoubleExp = 52, //52代表双倍经验类型
    ItemType_Honor = 53, //53代表荣誉之星类型
    ItemType_TONG = 54,  // 佣兵团召集令（帮主令）
    ItemType_FAMILY = 55,  // 家族召集令（家族令）
    ItemType_Adonment = 56, //56代表装饰品
    ItemType_SpecialBook = 57, //57代表特殊书籍
    ItemType_GreatLeechdom = 58, //58大计量药品
    ItemType_ClearProperty = 59, //59洗点道具
    ItemType_UseSkill = 60, // 附带技能类道具
    ItemType_Amulet = 61, // 护身符类道具
    ItemType_GreatLeechdomMp = 62,//62大计量自动补兰道具
    ItemType_TEAM = 64, // 冒险队伍召集令
    ItemType_KING = 65, // 界域召集令(国王令)
    ItemType_OneFiveExp = 66,  //1.5倍经验道具
    ItemType_OneTwoFiveExp = 67,  //1.25倍经验道具
    ItemType_OneSevenFiveExp = 68,  //1.75倍经验道具
    ItemType_NewOneFiveExp = 69,  //耐久按分算的1.5倍经验道具
    ItemType_CanNotDestroy = 70,    //不可销毁的道具
    ItemType_EGObject = 71,     //71代表恶搞类道具
    ItemType_Stuff = 72,		//72代表原料
    ItemType_Material = 73,     //73代表材料
    ItemType_TwoExp = 74,       //双倍经验道具
    ItemType_New_OneFiveExp = 75,  //新1.5倍经验道具
    ItemType_New_OneSevenFiveExp = 76,  //新1.75倍经验道具
    //ItemType_immortalmedicines = 77,  //77道具类型
	ItemType_immortalmedicines = 77,        //坐骑饲料类型
    ItemType_SeaWarItem  = 79,  //海战道具
    ItemType_HuShenFu  = 80,  //护身符
    ItemType_SkillObject = 81,  //技能类型的道具
    ItemType_GeniusSkillBook = 83,  //天赋技能书
	ItemType_HouseGardenSeed        = 95,   //房屋花园种子
	ItemType_HouseGardenMaterial    = 96,   //房屋花园物品
	ItemType_HouseItem              = 98,   //房屋物品
    ItemType_Furniture = 99,  //家具
	//=============================================装备==========================================================================
    ItemType_ClothBody =101,		//101代表布质加生命类服装		
    ItemType_FellBody =102,         //102代表皮甲加魔防类服装
    ItemType_MetalBody =103,        //103代表金属铠甲加物防类服装
    ItemType_Blade =104,            //104代表武术刀类武器
    ItemType_Sword =105,            //105代表武术剑类武器
    ItemType_Axe =106,				//106代表武术斧类武器（这种斧子装备在右手，也可装备在左手)
    ItemType_Hammer =107,           //107代表武术斧类武器
    ItemType_Staff =108,            //108代表法术杖类武器
    ItemType_Crossbow =109,         //109代表箭术弓类武器
    ItemType_Fan =110,				//110代表美女扇类
    ItemType_Stick =111,            //111代表召唤棍类武器
    ItemType_Shield =112,			//112代表盾牌类
    ItemType_Helm =113,				//113代表角色头盔类
    ItemType_Caestus =114,			//114代表角色腰带类
    ItemType_Cuff = 115,			//115代表角色护腕类
    ItemType_Shoes = 116,			//116代表角色鞋子类
    ItemType_Necklace = 117,		//117代表角色项链类
    ItemType_Fing = 118,			//118代表角色戒指类
    ItemType_FashionBody = 119,     //119代表时装
	//==============================================end============================================================================
    ItemType_Flower = 120,      //120代表鲜花,采集手套...
    //ItemType_BMW = 121,     //119代表宝马

	//==========================================时装==============================================================================
    ItemType_Fashion140 = 122,				//122代表140级时装
    ItemType_TransFashion140 = 123,			//123代表140级透明时装
    ItemType_Fashion = 124,					//124代表配置版时装
    ItemType_ClearFashion = 125,			//125代表表格配置版透明时装
	//===========================================end===============================================================================
    ItemType_GiftBox = 126,     //宝盒开奖用的宝箱
    ItemType_GiftBox_Key = 127,     //宝盒开奖用的钥匙
    ItemType_Add_Exploit = 128,     //增加功勋道具
    ItemType_UnionItems = 129,  //帮会道具
    //ItemType_HorseShoe = 130,       // 马蹄铁道具
	ItemType_HorseSoul = 130,               // 坐骑灵魂
    //ItemType_HorseRope = 131,       // 马绳
    //ItemType_HorseSaddle = 132,     // 马鞍道具
    //ItemType_HorseSafe = 133,       // 护马
    //ItemType_HorseIron = 134,       // 马镫

	//=========================================装备============================================================================
    ItemType_Dagger = 136,					// 匕首
    ItemType_NewSword = 137,				// 新剑
    ItemType_breastplate = 138,				// 护心镜
    //ItemType_HorseFashion = 140,			// 马时装

	
	ItemType_BothHandAxe = 139,				//双手斧（这种斧之装备在右手)
    ItemType_BoxingGlove = 140,				//拳套
    ItemType_MusicBow = 141,				//琴弓
    ItemType_MagicBook = 142,				//魔法书
    ItemType_BothHandHammer = 143,			//双手巨锤
    ItemType_CrystalHead = 144,				//水晶球
    ItemType_HumanSkeleton= 145,			//骷髅头
    ItemType_MagicSword = 147,				// 魔法剑
    ItemType_Talon = 148,					// 拳刃
    ItemType_Lance = 149,					// 长矛,枪
    ItemType_Mace = 150,					// 槌
    ItemType_Earing = 151,					// 耳环
    ItemType_Arm = 152,						// 臂环
    ItemType_Knee = 153,					// 护膝
    ItemType_Cloak = 154,					// 披风
    ItemType_Shoulder = 155,				// 肩甲
    ItemType_FamilyCloak = 156,				//家族披风
    ItemType_FashionHelm = 160,				// 时装头饰
    ItemType_FashionCloak = 161,			// 时装披风
    //ItemType_AccessoryJewel = 162,			// 附加饰品
	ItemType_AutoPick = 162,	// 自动拣物道具
    ItemType_HpJewel = 163,					// 生命饰品
    ItemType_MpJewel = 164,					// 魔力饰品

	//==========================================end==============================================================================
    ItemType_Reload = 170,          // 填充物
    ItemType_MountElse = 171,       // 放射诱导器
    ItemType_Offline = 180,         //离线工具
    ItemType_Autoget = 190,         //自动拾取工具

    ItemType_PerfectLevelup = 201,  // 钻石
    ItemType_AntiquityStone = 203,  //上古神石
    ItemType_AntiquityStone_2 = 204,    //补天灵石
    ItemType_AntiquityStone_3 = 205,    //补天石

    ItemType_Medal = 206,       //勋章, Medal 装备后可增加人物属性
    ItemType_ActiveReward = 207,  // 活动赠送物品
    ItemType_Tactics = 208, // 战术道具
    ItemType_TankPartPic = 209, // 战车配件图纸
    ItemType_TankPark = 210,    // 战车配件
    ItemType_ElemGem = 211, // 镶嵌元素宝石类,镶嵌到攻击类装备
    ItemType_ElemEss = 212, // 镶嵌元素精石类,镶嵌到防御类装备
    ItemType_GiftBag = 213,  // 保留大礼包类型
    ItemType_HomePetFood = 214, //保留宠物食物类型
    ItemType_HoleStuff = 215,   //打孔所用的道具类型

    ItemType_MakePattern = 216, //制造技能所需的图纸或者配方类型.
    ItemType_PastorBar =217,    //牧师用的权杖类
    ItemType_FashionPattern = 218, //制造时装所需的图纸类型
    ItemType_FineSword = 219,//细剑
    ItemType_TreasureMap = 220, //藏宝图
    ItemType_Tank = 221, //战车牌
    ItemType_HorseCard = 222,   //马牌  

    ItemType_QuestTrigger = 223,    //触发任务道具
	ItemType_GemPiece = 224,        //宝石碎片 精石碎片 类道具

	ItemType_HairDyes = 225,        //染发剂类道具
	ItemType_DearObject = 226,      //珍贵物品  卖给NPC时按原价处以3
	ItemType_QuestTriggerCool = 227, //触发任务道具,有冷却时间,每次减100耐久度
	ItemType_GoldToMoney = 228,      //用于把水晶币兑换成金属币的道具类型
    ItemType_LingQi     =229,   //灵器
	ItemType_TankObj = 230,		// 战车加血道具
	ItemType_tFuMo = 231,		//  附魔用品
	ItemType_tCloak = 232,		// 团披风
	ItemType_tCoGift = 233,     //互动礼物
	ItemType_EmotionTalk = 234, //自定义情绪泡泡
	ItemType_Fire = 235,		// 个人篝火
	ItemType_Wine = 236,		// 烤火时喝的酒

	ItemType_LingQiFeedUp   = 237,  //灵器养成类
    ItemType_AutoRepair = 238, //自动修理饰品
	ItemType_Exp         = 239,  //经验珠，使用后直接给经验
	ItemType_UnionMoney         = 240,  //给团直接加钱道具


	//新的勋章类型,原有的206放弃
	ItemType_ForeverMedal   = 241,		//永久型勋章
	ItemType_TimeEraseMedal = 242,		//时限型勋章(到期删除) 
	ItemType_TimeInvalidMedal = 243,	//时限型勋章(到期失去作用)
	ItemType_AutoFireSkill = 244,  //自动释放技能补时间道具
	ItemType_FunctionObject = 245, //功能性道具，比如右键点击弹出另外一个对话框，功能主要在另一个对话框处理的
	ItemType_LingQiAddExp = 246, //灵器增加经验道具
	ItemType_DoubleExpObject = 247, //双倍经验槽道具
	 ItemType_EmployGather = 248,  //雇佣采集凭证
	ItemType_AddRentStoreTime = 249, //租赁仓库充时道具
	ItemType_AddPersonAttribute = 250, //捡到直接增加玩家生命、魔法，攻击等属性的物品
	
	ItemType_FishingRob = 251,  /// 鱼竿
	ItemType_FishingBait = 252,  /// 鱼饵
	ItemType_FuwenMozhu  = 253,  ///符纹魔珠，给装备加符纹能量
	ItemType_MerchAddit     = 254,  ///佣兵团商城辅料
	ItemType_UnionObject     = 255,  ///城邦物品
	ItemType_ResetEvolveTimes  = 256,  ///重置灵器进化次数和值
	ItemType_LingYuanZhu       = 257,  ///灵元珠
	ItemType_RongLingFen       = 258,  ///融灵粉
	ItemType_ChuanChengNotFull   = 259,  ///传承之书,未充满的
	ItemType_ChuanChengFull   = 260,  ///传承之书,充满的
	ItemType_EquipAid   = 261,  //装备辅助道具
	ItemType_StarKeeper   = 262,  //符纹（星星）保留道具 (保星石)
	ItemType_UnionSeed = 263,   // 团种子
	ItemType_SkillBookNotFull = 264,  // 未完成的技能书
	ItemType_SkillBookBlank = 265,    // 空白的技能书
	ItemType_AddLingQiMinusExp = 266,  //为灵器经验为负时增加经验
	ItemType_ClearDrunkennessTime = 267,     //解酒药 
	ItemType_LingYuanZhu_CanAdd = 268,       //可叠加的灵元珠
	ItemType_Qianneng_Obj       = 269,      //潜能类道具
	ItemType_FuWenQiangHua			= 270,		//符文强化类
	ItemType_JieZhanMedal           = 271,      //界战指挥官印记
	ItemType_LingQi_Model           = 272,        //灵器模型类
	ItemType_FlyItem                        = 273,          //饰品类道具翅膀
	ItemType_DragonEquip                    = 274,          //龙装备类型
    ItemType_Dragon                         = 275,         //宠物龙
    ItemType_DragonFood                     = 276,          //龙宠食料
	ItemType_NewGiftBox                     = 277,          //新宝箱道具
	ItemType_Bugle                          = 278,          //喊话小喇叭道具
	ItemType_EvolveHorseScroll              = 279,          //坐骑进化卷轴
	ItemType_EvolveHorseStone               = 280,          //坐骑进化熔石
	ItemType_HappyCard						= 281,			//龙珠抽奖 欢乐卡片
	ItemType_FuWenStone               = 282,          //升星升月石头
	ItemType_AddRing                = 283,      //加数环道具
	ItemType_GemHammer				= 284,		//宝石锤
	ItemType_Fight				    = 285,		//战争道具
	ItemType_CrystalCertificate     = 286,  //水晶币兑换券(绑定水晶币道具增加道具)
	ItemType_PointOpenGiftBox       = 287,  // (宝箱)点数打开宝箱道具，每次打开消耗对应点数，复用耐久字段
	ItemType_PointOpenGiftBag       = 288,  // (礼包)点数打开礼包道具，每次打开消耗对应点数，复用耐久字段
	ItemType_PurpleDef   = 289,  // 紫装提升道具 提升防具
    ItemType_PurpleJew   = 290,  // 紫装提升道具 提升饰品
    ItemType_PurpleWeapon  = 291,  // 紫装提升道具 提升武器
    ItemType_PurplePiece = 292,  // 紫装提升碎片
	ItemType_LingQi_ExpZhu = 293,  //灵器经验珠，打怪时经验倍数
	ItemType_Exp_To_LingQiExp = 294,  //人物经验到灵器经验
	ItemType_GiftBox_WenMing = 295,  //装备文明宝箱
	ItemType_DecayLingQi_ReRound = 296,      //蜕变灵器变回10级道具

	ItemType_OffPurple = 297,  // 取下紫装道具


/////////////////////////////我是一个华丽的分割线///////////////////////////////////////////////////////////////////////////////
	ItemType_GiftBag_Card	    =	300,	//卡牌玩法，卡牌礼包
};

enum LeechdomType  //药水作用
{
	Leechdom_default = 0,
	Leechdom_dam = 1,          //增加伤害力
	Leechdom_def = 2,          //增加防御力
	Leechdom_poison = 3,       //药物使人持续中毒
	Leechdom_sppersist = 4,    //物品使人体力值保持当前值不变
	Leechdom_spup = 5,         //药物增加人体力值百分比
	Leechdom_spcostdown = 6,   //减慢体力消耗速度
	Leechdom_spresumeup = 7,   //加快体力恢复速度
	Leechdom_hp = 8,           //一次性恢复生命
	Leechdom_hppersist = 9,    //持续恢复生命
	Leechdom_mp = 10,          //一次性恢复法术值
	Leechdom_mppersist = 11,   //持续恢复法术
	Leechdom_sp = 12,          //一次性恢复体力值
	Leechdom_hp5 = 13,         //一次性恢复生命
	Leechdom_hppersist5 = 14,   //持续恢复生命
	Leechdom_hpmax = 15,       //消耗耐久一次加满HP
	Leechdom_mpmax = 16,       //消耗耐久一次加满MP
	Leechdom_chocolate = 17,    //巧克力

	Leechdom_hpmax25 = 18,     //消耗耐久一次加满HP,冷却.5秒
	Leechdom_mpmax25 = 19,     //消耗耐久一次加满MP，冷却.5秒

	Leechdom_UnionItemsAttack_3 = 22,	//帮会道具，攻击类，.5秒冷却
	Leechdom_UnionItemsHP_3 = 23,		//帮会道具，加血类，.5秒冷却
	Leechdom_hpmax25_3500 = 24,  //消耗耐久，一次加血，冷却.5秒
	Leechdom_hpmax25_4000 = 25,  //消耗耐久，一次加血，冷却.5秒
	Leechdom_hpmax25_4500 = 26,  //消耗耐久，一次加血，冷却.5秒
	Leechdom_hpmax25_5000 = 27,  //消耗耐久，一次加血，冷却.5秒
	Leechdom_hpmax25_6000 = 28,  //消耗耐久，一次加血，冷却.5秒
	Leechdom_hpmax25_4300 = 29,
	Leechdom_hpmax25_4600 = 30,
	Leechdom_hpmax25_4900 = 31,
	Leechdom_hpmax25_5200 = 32,
	Leechdom_hpmax25_5500 = 33,
	Leechdom_hpmax25_8000 = 34,
	Leechdom_hppercent = 35,     //百分比恢复生命 
	Leechdom_mppercent = 36,     //百分比恢复魔法
	// 大包生命药水, 相当于多个持续性药水, 每使用次扣点耐久
	Leechdom_hpManyPersist = 37,
	Leechdom_mpManyPersist = 38,
	Leechdom_hppercent_dutypet = 39,  // 使命召唤任务的召唤兽恢复血量百分比
};

#if 0
enum {				//setpos定义
	EQUIPCELLTYPE_NONE=0,       /// 不能装备
    EQUIPCELLTYPE_HELM=1,       /// 头盔
    EQUIPCELLTYPE_BODY=2,       /// 服装
    EQUIPCELLTYPE_HANDR=3,      /// 右手
    EQUIPCELLTYPE_HANDL=4,      /// 左手
    EQUIPCELLTYPE_NECKLACE=5,   /// 项链
    EQUIPCELLTYPE_GLOVES=6,     /// 手套,手镯
    EQUIPCELLTYPE_RING=7,       /// 戒指
    EQUIPCELLTYPE_BELT=8,       /// 腰带
    EQUIPCELLTYPE_SHOES=9,      /// 鞋子
	EQUIPCELLTYPE_PACKAGE=11,	/// 额外包裹
    EQUIPCELLTYPE_FASHION=15,   /// 时装
    EQUIPCELLTYPE_EAR=16,       /// 耳部, 装备耳环
    EQUIPCELLTYPE_SHOULDER=17,  /// 肩部, 装备肩甲
    EQUIPCELLTYPE_ARM=18,       /// 胳膊位置, 装备臂环
    EQUIPCELLTYPE_KNEE=19,      /// 膝部, 装备护膝
    EQUIPCELLTYPE_AUTOPHY=20,   /// 自动补体力值装备
    EQUIPCELLTYPE_AUTOMAG=21,   /// 自动补魔法值装备
    EQUIPCELLTYPE_CLOAK=22,         /// 披风
    EQUIPCELLTYPE_FASHIONCLOAK=23,  /// 时装披风
    EQUIPCELLTYPE_FASHIONHELM=24,   /// 时装头饰
    EQUIPCELLTYPE_SBEAST=25,        /// 其它装备物,有附加功能(类似有灵魂有生命的东西)
    EQUIPCELLTYPE_MEDAL=26,         /// 新增格子，用来放置称号系统的勋章
	EQUIPCELLTYPE_HANDLR = 29,      ///标识左右手都可以装备的物品,如双手斧
};
#endif

#if 0
/// 定义物品格子类型
enum{
    OBJECTCELLTYPE_NONE,        /// 不是格子，用于丢弃或捡到物品
    OBJECTCELLTYPE_COMMON,      /// 普通物品格子
    OBJECTCELLTYPE_EQUIP,       /// 装备格子
    OBJECTCELLTYPE_MOUSE,       /// 鼠标
    OBJECTCELLTYPE_TRADE,       /// 自己的交易格子
    OBJECTCELLTYPE_OTHERTRADE,  /// 对方的交易格子
    OBJECTCELLTYPE_BANK,        /// 银行
    OBJECTCELLTYPE_SELL,        /// 卖
    OBJECTCELLTYPE_STORE,       /// 仓库
    OBJECTCELLTYPE_EQUIPSHOW,   /// 非自己穿着的装备
    OBJECTCELLTYPE_PACKAGE,    /// 包裹的格子
    OBJECTCELLTYPE_MAKE,       /// 合成、升级，镶嵌的格子
    OBJECTCELLTYPE_MYSHOP,      /// 自己摊位的格子
    OBJECTCELLTYPE_OTHERSSHOP,  /// 别的玩家摊位的格子
    OBJECTCELLTYPE_MAIL,        /// 邮件系统的格子
    OBJECTCELLTYPE_COUNTRY_SAVEBOX, /// 国家倉库
    OBJECTCELLTYPE_PET,       /// 宠物包裹
    OBJECTCELLTYPE_GIFTBOX,     //宝盒的格子  //KOK20090325宝箱功能
    OBJECTCELLTYPE_GOD_EQUIP = 18,   //神器
    OBJECTCELLTYPE_RENT_STORE,  //租借的仓库
    OBJECTCELLTYPE_STONE = 20,  //改造千年寒铁
	OBJECTCELLTYPE_FSTORAGE = 21,	// 家具仓库
	OBJECTCELLTYPE_DAILYGIFT = 22,	// 每日礼包物品
	OBJECTCELLTYPE_MAGICSTOVE = 23, // 魔力炉
	OBJECTCELLTYPE_PUBSTORAGE = 24,	// 房屋里的公用储物箱
	OBJECTCELLTYPE_PRISTORAGE = 25,	// 房屋里的个人储物箱
	OBJECTCELLTYPE_NEWGIFTBOX = 26, // 新宝箱物品格子
	OBJECTCELLTYPE_RENT_STORE3 = 27, // 第三仓库
};
#endif

//包裹类型
enum {
	MAIN_PACK = 1,
	LEFT_PACK = 2,  //装备包裹的子包裹
	RIGHT_PACK = 4, //装备包裹的子包裹
	EQUIP_PACK = 8, 
	STORE_PACK = 16,
	LM_PACK = 32, 
	RM_PACK = 64,
	PET_PACK = 128,
};

//最大打孔的数量==================================================
#define	EQUIP_MAX_GROOVENUM	6
//神印类型定义:
enum
{
        GODSEAL_NONE  = 0,    //无
        GODSEAL_LIGHT = 1,   //光明套装
        GODSEAL_CHAOS = 2,   //混沌套装
        GODSEAL_DARK  = 4,   //黑暗套装
        
		GODSEAL_SUN  = 100,      //旭日套装
        GODSEAL_SPIRIT = 101,    //圣灵套装
        GODSEAL_THORNS = 102,    //荆棘套装
        GODSEAL_FLAME = 103,     //烈焰套装
        GODSEAL_ASSAULT = 104,   //强袭套装
        GODSEAL_DEFENCE = 105,   //城防军套装
        GODSEAL_CROSS = 106,      //十字军套装
		GODSEAL_GUYONGJUN = 107,  //雇佣军套装
		GODSEAL_ESSENCE = 108,    //精英套装
        GODSEAL_WARD = 109,       //守护套装
		GODSEAL_MOON = 110,      //明月套装
        GODSEAL_BLOOD = 111,     //血色套装
        GODSEAL_PURPLE = 112,    //紫金套装
		GODSEAL_CANGQIONG = 113, //苍穹套装
		GODSEAL_MINGSHEN = 114,  //冥神套装
		GODSEAL_SHEHUN = 115,    //摄魂套装
		GODSEAL_ZIYAN = 116,     //紫炎套装
		GODSEAL_GUANGHUI = 117,  //光辉套装
		GODSEAL_TANXI = 118,     //叹息套装
		GODSEAL_QIANCHENG = 119,   //虔诚套装
		GODSEAL_SHENGGUANG = 120,  //圣光套装
		GODSEAL_KUANSHU = 121,     //宽恕套装		
		GODSEAL_SHENGJIE = 122,    //圣洁套装
		GODSEAL_BENLEI = 123,      //奔雷套装
		GODSEAL_XUANMO = 124,      //炫魔套装
		GODSEAL_MINGYAN = 125,     //冥眼套装
		GODSEAL_FENGBAO = 126,     //风暴套装
		GODSEAL_HEIYI = 127,       //黑翼套装
		GODSEAL_MINGHUO = 128,     //冥炎套装
		GODSEAL_ZIDI = 129,        //紫帝套装
		GODSEAL_KUANGSHEN = 130,   //狂神套装
};

enum
{
	GODSEAL_Attr_Dpdam = 1,				//物理伤害减免 %0.2f%%
	GODSEAL_Attr_Dmdam = 2,				//魔法伤害减免 %0.2f%%
	GODSEAL_Attr_Bdam = 3,				//伤害增加 %0.2f%%
	GODSEAL_Attr_Rdam = 4,				//伤害反弹 %0.2f%%
	GODSEAL_Attr_Ignoredef = 5,			//忽视防御 %d
	GODSEAL_Attr_ExcellentAttack = 6,	//卓越一击 %d
	GODSEAL_Attr_Blife = 7,				//生命     %0.2f%%

	GODSEAL_Attr_IncDam_Num = 20,				//伤害增加%d点
	GODSEAL_Attr_DecDam_Num = 21,				//受伤减少%d点
	GODSEAL_Attr_IncPhicAttack_NUM = 22,		//物理攻击力增加x点
	GODSEAL_Attr_IncMagicAttack_NUM = 23,	//魔法攻击力增加x点
	GODSEAL_Attr_IncPhicAttack = 24,		//物理攻击力提升x%
	GODSEAL_Attr_IncMagicAttack = 25,	//魔法攻击力提升x%
	GODSEAL_Attr_IncAllAttrs_NUM = 26,	//全属性提升x点
	GODSEAL_Attr_IncCruel = 27,			//暴击提升x%
	GODSEAL_Attr_GainExp = 28,			//杀相差10级以内的怪增加%d点经验
	GODSEAL_Attr_IncPhicDefend_NUM = 29,	//物理防御力增加x点
	GODSEAL_Attr_IncMagicDefend_NUM = 30,	//魔法防御力增加x点
	GODSEAL_Attr_IncCruelDefend_NUM = 31,		//防暴击增加x点
	GODSEAL_Attr_IncEvadeRate = 32,			//躲避率增加x点
	GODSEAL_Attr_MobExpPercent = 33,		//杀怪经验增加x%
	

	GODSEAL_Attr_AttackAndDefend = 40,	//增加双攻x点,减少双防y点
	GODSEAL_Attr_DefendAndAttack = 41,	//增加双防x点,减少双攻y点
	GODSEAL_Attr_RetrieveLife = 42,		//每x秒生命恢复y点
	GODSEAL_Attr_RetrieveMagical = 43	//每x秒魔力恢复y点

};
enum
{
        AUCTION_WEAPOM_SWORD = 1,
        AUCTION_WEAPOM_AXE = 2,
        AUCTION_WEAPOM_BOW = 3,
        AUCTION_WEAPOM_STICK = 4,
        AUCTION_WEAPOM_WAND = 5,
        AUCTION_WEAPOM_FAN = 6,
        AUCTION_WEAPOM_DAGGER = 7,//利刃
        AUCTION_WEAPOM_NEW_SWORD = 8,//剑
        AUCTION_EQUIP_HEAD = 11,
        AUCTION_EQUIP_BODY = 12,
        AUCTION_EQUIP_WRIST = 13,
        AUCTION_EQUIP_SHIELD = 14,
        AUCTION_EQUIP_WAIST = 15,
        AUCTION_EQUIP_FOOT = 16,
        AUCTION_EQUIP_MIRROR = 17,//镜 
        AUCTION_ACCESSORY_NECKLACE = 21,
        AUCTION_ACCESSORY_RING = 22,
        AUCTION_ACCESSORY_ADORNMENT = 23,
        AUCTION_BOOK_FIGHTER = 31,
        AUCTION_BOOK_ARCHER = 32,
        AUCTION_BOOK_WIZARD = 33,
        AUCTION_BOOK_SUMMONER = 34,
        AUCTION_BOOK_PRIEST = 35,
        AUCTION_BOOK_SPECIAL = 36,
        AUCTION_BOOK_ASSASSIN = 37,//刺客
        AUCTION_BOOK_GUARD = 38,//卫士
        AUCTION_BOOK_GEN = 39,//天赋
        AUCTION_OTHER_GEM = 41,
        AUCTION_OTHER_ITEM = 42,
        AUCTION_OTHER_MATERIAL = 43,
        AUCTION_OTHER_LEECHDOM = 44,
        AUCTION_OTHER_SOUL = 45,
        AUCTION_OTHER_BMW = 46,    //马铠  
        AUCTION_OTHER_XIUFUSHI = 47,    //修复石
        AUCTION_OTHER_ZHUFUSHI = 48,    //祝福石
        AUCTION_OTHER_DINGXINGSHI = 49    //定星石
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
	DWORD dwLocation;   // 格子类型  1
	DWORD dwTableID;    // 包袱ID    ->0
	WORD  x;		//行
	WORD  y;		//列

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

//t_Object中lure的定义
#define OBJECT_LURE_BUYFROMMARKET 0x1	//表示从商城购买
#define OBJECT_LURE_MADED	0x2			//表示该物品是打造出来的, 而不是打怪掉落, 魔力炉熔炼等方式产出的
#define OBJECT_LURE_JingJie 0x4			//表示该物品具有警戒属性(使装备者的移动速度固定为步行速度的50%), 目前只有时装具有这个属性
//服务器端发送的物品信息
typedef struct t_Object
{
	DWORD qwThisID;   //物品唯一id
	DWORD dwObjectID;  //物品表中的编号
	DWORD type;	//物品类别id Add for Kok
	char strName[MAX_NAMESIZE]; //名称
	
	stObjectLocation pos;	// 位置
	DWORD dwNum;	// 数量
	BYTE upgrade;//物品升级等级
	//BYTE kind;	//物品类型, 0普通, 1蓝色, 2金色, 4神圣, 8套装
	BYTE kind;	//change for kok by zgm 物品类型, 0白色, 1蓝色, 2黄色, 4绿色 // 后面是新增加的心魂的颜色 5绿阶二, 6绿阶三, 6绿阶四, 8绿阶五
	BYTE subkind;//子品质 (+1~+9)
	DWORD exp;  //道具经验
	
	WORD needlevel;				// 需要等级

	WORD maxhp;					// 最大生命值      // 对应马匹 HP加成
	WORD maxmp;					// 最大法术值      // 对应马匹 MP加成
	WORD maxsp;					// 最大体力值	   // 对应心魂的当前能量值	

	WORD pdamage;				// 最小攻击力      // 对应马匹 物攻加成
	WORD maxpdamage;			// 最大攻击力
	WORD mdamage;				// 最小法术攻击力  // 对应马匹 魔攻加成
	WORD maxmdamage;			// 最大法术攻击力

	WORD pdefence;				// 物防            // 对应马匹 物防加成
	WORD mdefence;				// 魔防            // 对应马匹 魔防加成
	BYTE damagebonus;			// 伤害加成x% from 道具基本表
	BYTE damage;				// 增加伤害值x％from 神圣装备表
		
	WORD akspeed;				// 攻击速度
	WORD mvspeed;				// 移动速度
	WORD atrating;				// 命中率
	WORD akdodge;				// 躲避率

	DWORD color;				// 颜色	

	WORD str;  // 力量                               // 对应马匹 力量 // 时装将力量和智力重用为增加HP和MP上限的百分比
	WORD inte;  // 智力                              // 对应马匹 智力 // 时装将力量和智力重用为增加HP和MP上限的百分比
	WORD dex;  // 敏捷                               // 对应马匹 敏捷
	WORD spi;  // 精神                               // 对应马匹 精神
	WORD con;  // 体质                               // 对应马匹 体质

	WORD kar;  //运气                                // 对应马匹 运气


//注释原有五行属性字段,kok中装备按位置确定其套装类型ouyx
//	WORD fivetype;  // 五行属性
//	WORD fivepoint; // 五行属性
	BYTE godseal;   //神印类型定义在Command.h中
	
	WORD hpr;  // 生命值恢复
	WORD mpr;  // 法术值恢复
	WORD flag;  //标志字段

	WORD holy;  //神圣一击	
	WORD bang;      //重击->暴击
	WORD bangdef;  //防暴击
	WORD pdam;  // 增加物理攻击力  100最大  百分比
	WORD pdef;  // 增加物理防御力  100最大  百分比
	WORD mdam;  // 增加魔法攻击力  100最大  百分比
	WORD mdef;  // 增加魔法防御力  100最大  百分比
	
	WORD poisondef; //抗毒增加
	WORD lulldef; //抗麻痹增加
	WORD reeldef; //抗眩晕增加
	WORD evildef; //抗噬魔增加
	WORD bitedef; //抗噬力增加
	WORD chaosdef; //抗混乱增加
	WORD colddef; //抗冰冻增加
	WORD petrifydef; //抗石化增加
	WORD blinddef; //抗失明增加
	WORD stabledef; //抗定身增加
	WORD slowdef; //抗减速增加
	WORD luredef; //抗诱惑增加

	WORD durpoint; //恢复装备耐久度点数
	WORD dursecond; //恢复装备耐久度时间单位

	struct skillbonus {
		WORD id; //技能id
		WORD point; // 技能点数
	} skill[10]; //技能加成

	struct skillsbonus {
		WORD id; //技能id
		WORD point; // 技能点数
	} skills;	//全系技能加成

	WORD poison; //中毒增加
	WORD lull; //麻痹增加
	WORD reel; //眩晕增加
	WORD evil; //噬魔增加
	WORD bite; //噬力增加
	WORD chaos; //混乱增加
	WORD cold; //冰冻增加
	WORD petrify; //石化增加
	WORD blind; //失明增加
	WORD stable; //定身增加
	WORD slow; //减速增加
	WORD lure; //修改为专用的标志位,按位取值; 第一位用于标记是否是从商城购买的物品,等等, 详细定义见上, 如OBJECT_LURE_BUYFROMMARKET
	
	struct leech
	{
		BYTE odds;    //x
		WORD effect;	//y
	};
	leech hpleech; //x%吸收生命值y
	leech mpleech; //x%吸收法术值y

	BYTE hptomp; //转换生命值为法术值x％
	BYTE dhpp; //物理伤害减少x%	
	BYTE dmpp; //法术伤害值减少x%		

	//BYTE incgold; //紫装标志
	//BYTE doublexp; //x%双倍经验		
	//BYTE mf; //增加掉宝率x%
	
	BYTE bind;  //装备是否绑定 0非绑定  1绑定

//万王套装相关属性修改, ouyx ---------------start
	/*注释原代码
	union {
		BYTE _five_props[5];
		struct {
			//五行套装相关属性
			BYTE dpdam; //物理伤害减少%x
			BYTE dmdam; //法术伤害减少%x
			BYTE bdam; //增加伤害x%
			BYTE rdam; //伤害反射%x
			BYTE ignoredef; //%x忽视目标防御
		};
	};
	*/
	union {  //为了保存精确的百分数,将万分之几进行运算存储
		 WORD _seven_props[7];
		struct {
			//套装相关属性
			WORD dpdam; //物理伤害减少x%%
			WORD dmdam; //法术伤害减少x%%
			WORD bdam; //增加伤害x%%
			WORD rdam; //伤害反射x%%
			WORD ignoredef; //x%%忽视目标防御
			WORD excellentAttack;  //卓越一击
			WORD blife;  //增加生命值x%%
		};
	};

//	WORD fiveset[5]; //五行套装, 按顺序排列
//---------------------------------------------end

	//...
	BYTE width;  //宽度
	BYTE timeeffect; //时效状态, 0 -- 普通物品, 没有时效性质; 1 -- 有时效性质, 还没过期; 2 -- 有时效性质, 已过期
	DWORD dur; 	//当前耐久度
	DWORD maxdur; //最大耐久度
	
	// 新版本该字段被分为四个字节使用,从低位->高位, 第一字节==255为有效的孔,但没放宝石,==0为无效的孔,不能放宝石,>0&&!=255则表示放了一个宝石,并且值就是宝石的编号
	// 第二个字节保留
	// 第三个字节表示该宝石增加的值
	// 第四个字节表示该宝石增加的属性类别
	//DWORD socket[6]; //孔
	
	// Change for KoK by hb. ----------start-----------
	// 尽量兼容以前模式, 从低位到高位, 第一字节==255为有效的孔,但没放宝石,==0为无效的孔,不能放宝石,>0&&!=255则表示放了一个宝石,值就是宝石的编号
	// 第二字节的低位为孔的颜色, 为Cmd::ICE等~5个数之一, 高位为宝石的颜色, 同样为Cmd::ICE等~5个数之一
	// 第三第四字节为宝石的加成或者吸收的属性值
	DWORD socket[6]; //孔Changed for kok 改为孔
	// -------end-----
	
	// 额外激活属性字段,如果镶嵌的宝石颜色和孔的颜色一样就可能出现额外激活属性,一件装备最多出现种
	// 从低位到高位,第一字节为表示出现了激活属性,但还没激活.1表示激活了额外属性,第二字节表示额外属性的种类
	// 第三第四字节为加成或者吸收的属性值 add for kok by zgm
	DWORD extra_active[2];
	
	//stSocket socket[7];	// 孔, Changed for kok by hb 采用更清晰的定义
	DWORD price;     //价格
	//DWORD cardpoint; //代先生说：cardpoint这个变量的高位表示转生次数!
	char maker[MAX_NAMESIZE]; //打造者

	// Add for Kok by hb 取有符号数, 正数表示加成,负数表示吸收
    WORD  aIce;        // 冰元素伤害加成
    WORD  aThunder;    // 雷元素伤害加成
    WORD  aWind;       // 风元素伤害加成
    WORD  aElec;       // 电元素伤害加成
    WORD  aFire;       // 火元素伤害加成

	WORD mIce;			// 冰元素伤害吸收
	WORD mThunder;		// 雷元素伤害吸收
	WORD mWind;			// 雷元素伤害吸收
	WORD mElec; 		// 雷元素伤害吸收
	WORD mFire;			// 火元素伤害吸收

	QWORD modelid;      //模型编号add for kok

	////////////////////////////////////////
	/// 魔力炉附魔加成
	WORD charmadd_pdamage;                  /// 魔力炉(附魔变化值)
	WORD charmadd_mdamage;                  /// 魔力炉(附魔变化值)
	WORD charmadd_pdefence;                 /// 魔力炉(附魔变化值)
	WORD charmadd_mdefence;                 /// 魔力炉(附魔变化值)
	////////////////////////////////////////

	////////////////////////////////////////
	/// 装备强化影响加成
	union
	{
	    BYTE subupgrade;        /// 物品升级阶附属等级(装备强化冲星)
	    BYTE moon;
	};
	////////////////////////////////////////

	struct
	{
	    DWORD jobEnergy;  // 职业装备:职业能量
	    WORD  energyJob;  // 职业装备:能量的职业类型, 定义同CharBase.job  客户端对应职业装备中职业需求中的职业显示
	};

	BYTE sealStars;//星星总数(封印+upgrade), 0表示无封印
	BYTE sealMoons;//月亮总数(封印+subupgrade), 0表示无封印

	BYTE broadcast; // 礼包领出时，公告类型 新宝箱使用 WDS 20100811
	BYTE mAllPointPer;//全属性百分比
	BYTE purpleFlag;//紫装等级
	BYTE purpleValue;//紫装数值 百分比
	BYTE nextUseBYTE5;//预留字段，下次使用改名
	BYTE nextUseBYTE6;//预留字段，下次使用改名

	WORD purplePdamage;     //紫装加成物攻
	WORD purpleMaxpdamage;  //紫装加成最大物攻

	WORD purpleMdamage;     //紫装加成魔攻
	WORD purpleMaxmdamage;  //紫装加成最大魔攻

	WORD purplePdefence;    //紫装加成物防
	WORD purpleMdefence;    //紫装加成魔防

	DWORD nextUseDWORD2;//预留字段, 下次使用改名
	DWORD nextUseDWORD3;//预留字段, 下次使用改名
	DWORD nextUseDWORD4;//预留字段, 下次使用改名
	DWORD nextUseDWORD5;//预留字段, 下次使用改名

};

/**
 * \brief   手游 服务器和客户端通信道具数据
*/
typedef struct t_Object_mobile
{
	DWORD dwThisID;   //物品唯一id
	DWORD dwObjectID;  //物品表中的编号
	stObjectLocation pos;	// 位置
	DWORD dwNum;	// 数量

};

#pragma pack()
namespace Object
{
    static stObjectLocation INVALID_POS;
}
#endif
