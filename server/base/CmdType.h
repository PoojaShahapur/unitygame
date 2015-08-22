#ifndef _CMD_TYPE_H_
#define _CMD_TYPE_H_

#include "zType.h"

namespace Cmd{

//BEGIN_ONE_CMD

/// 空指令
const BYTE NULL_USERCMD      = 0;
/// 登陆指令
const BYTE LOGON_USERCMD    = 104;
/// 时间指令
const BYTE TIME_USERCMD      = 2;
/// 数据指令
const BYTE DATA_USERCMD      = 3;
/// 道具指令
const BYTE PROPERTY_USERCMD    = 4;
/// 地图指令
const BYTE MAPSCREEN_USERCMD    = 5;
/// 移动指令
const BYTE MOVE_USERCMD      = 6;
/// 建造指令
const BYTE BUILD_USERCMD    = 8;
/// 打造指令
const BYTE MAKEOBJECT_USERCMD    = 10;
/// 复活指令
const BYTE RELIVE_USERCMD    = 12;
/// 聊天指令
const BYTE CHAT_USERCMD      = 14;
/// 离开指令
const BYTE LEAVEONLINE_USERCMD    = 15;
/// 交易指令
const BYTE TRADE_USERCMD    = 17;
/// 魔法指令
const BYTE MAGIC_USERCMD    = 18;
/// 帮会指令
const BYTE UNION_USERCMD    = 21;
/// 国家指令
const BYTE COUNTRY_USERCMD    = 22;
/// 任务指令
const BYTE TASK_USERCMD      = 23;
/// 选择指令
const BYTE SELECT_USERCMD    = 24;
//  社会关系指令
const BYTE RELATION_USERCMD        = 25;
//  门派关系指令
const BYTE SCHOOL_USERCMD    = 26;
//  家族关系指令
const BYTE SEPT_USERCMD      = 27;
// 战斗指令
const BYTE DARE_USERCMD                 = 28;
// 宠物指令
const BYTE PET_USERCMD                  = 29;
// 获取服务器列表
const BYTE PING_USERCMD      = 30;
// 金币指令
const BYTE GOLD_USERCMD      = 31;
// 答题指令
const BYTE QUIZ_USERCMD      = 32;
// NPC争夺战指令
const BYTE NPCDARE_USERCMD    = 33;
// 与GM工具交互的指令
const BYTE GMTOOL_USERCMD    = 34;
// 邮件指令
const BYTE MAIL_USERCMD      = 35;
// 拍卖指令
const BYTE AUCTION_USERCMD    = 36;
// 卡通宠物指令
const BYTE CARTOON_USERCMD    = 37;
// 股票指令
const BYTE STOCK_SCENE_USERCMD    = 38;
const BYTE STOCK_BILL_USERCMD    = 39;
// 投票指令
const BYTE VOTE_USERCMD      = 40;
// 军队指令
const BYTE ARMY_USERCMD      = 41;
// 护宝任务指令
const BYTE GEM_USERCMD      = 42;
// 监狱系统指令
const BYTE PRISON_USERCMD    = 43;
// 礼官指令
const BYTE GIFT_USERCMD      = 44;
// 国家同盟指令
const BYTE ALLY_USERCMD      = 45;
// 小游戏指令
const BYTE MINIGAME_USERCMD    = 46;
// 推荐人系统指令
const BYTE RECOMMEND_USERCMD  = 47;
// 财产保护系统指令
const BYTE SAFETY_USERCMD    = 48;
//箱子指令
const BYTE SAFETY_COWBOX = 49;

//转生指令

const BYTE TURN_USERCMD = 50;

const BYTE HOTSPRING_USERCMD = 51;

const BYTE REMAKEOBJECT_USERCMD	= 52;

//训马指令 [sky]
const BYTE HORSETRAINING_USERCMD	= 53;

//跨区战场指令
const BYTE BATTLE_USERCMD	= 54;

// sky 战场-副本-竞技场指令
const BYTE ARENA_USERCMD	= 55;

//英雄排行指令
const BYTE HEROLIST_USERCMD		= 56;

//炉石传说卡牌玩法
const BYTE HERO_CARD_USERCMD	= 162;
}
#endif


