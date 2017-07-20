/**
 * File: src/cell/common_player.h
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-30 16:02:12
 * Last Modified Date: 2017-06-30 16:02:12
 * Last Modified By: cxz <cxz@qq.com>
 */
#pragma once
#include <memory>
#include <cmath>
#include <functional>  // for greater<>
#include <map>
#include "pos.h"
#include "game_clt_id.h"
#include "entity.h"

// 每场比赛的统计数据
struct GameStatisticsData{
        uint32_t m_score;
        // 旧得分,用于删除排行榜中旧数据
        uint32_t m_oldScore;
    // 本次存活期间击杀飞机数
    uint32_t m_killNumber;
    uint32_t m_totalKillNumber;
    uint32_t m_destroyNumber;
    uint32_t m_comboNumber;
    uint32_t m_highestComboNumber;

    GameStatisticsData() : m_score(0), m_oldScore(0), m_killNumber(0), m_totalKillNumber(0)
                     , m_destroyNumber(0), m_comboNumber(0), m_highestComboNumber(0)
    {}
};

// lua里给玩家发奖励需要的数据
// 本想继承 GameStatisticsData,奈何 Lua-intf addVariable 时候报错,暂时解决不了,先不继承了
struct LuaRankData{
    uint32_t m_rank;
    uint32_t m_id;
    std::string m_nickname;
    std::string m_account;
    uint32_t m_isai;
    // CGameCltId.m_uBaseSvrId
    uint32_t m_serverid;
    // CGameCltId.m_uBaseRpcCltId
    uint64_t m_rpcid;
        uint32_t m_score;
    uint32_t m_totalKillNumber;
    uint32_t m_destroyNumber;
    uint32_t m_highestComboNumber;
};

/*
 * 2017-07-07 决定传到lua里面
每局结算奖励放lua里发放,(c++里要读奖励配置,要调用user的方法)
放lua里面可以把该类用lua-intf绑定,并定义函数返回 std::vector<PlayerRankData>
lua中读取到第1-N名数据后,依次给玩家发奖励
发放奖励需求可能会经常变,而且也不费CPU,经常会热更新

lua中需要知道名次,玩家id,nickname,account,score,isai,uid
总击杀人数,击杀人数,得分,最高combo数,房间模式
*/
/*
 * 2017-07-05,奖励还是策划配在Excel里,放在c++里读取,增加消息通知网关服给user添加奖励
 * 1. 奖励越来越多,发奖励的函数参数越来越长,终究要定义消息(lua里定义消息)
 * 2. 虽然lua-intf 绑定了,但lua/c++频繁的交互还是费效率(5分钟费1次效率也没关系)
 * 3. csv也能热更新,和lua里配置效果一样
 * 4. 方便后面重载CommonRoom 实现团战/炼狱等房间(传给lua一个type就行)
 */
//struct PlayerRankData{
//};

/*
 * CommonPlayer 为 Player, AIPlayer, CrazyAIPlayer 的基类,该类想涵盖 玩家,AI,疯狂AI等共有的属性和方法
 * CommonPlayer 主要负责:
 * 添加/删除小飞机
 * 移动,停止移动
 * 添加/删除状态(无敌)
 * 死亡处理
 * 击杀处理
 * 发射子弹
 * 分裂
 * 返回大厅
 */


struct Plane;
class CommonRoom;
class Formation;
class Bullet;
class BulletGroup;
using BulletMap = std::map<uint32_t, Bullet>;
class CommonPlayer : public EntityWithQueue, public std::enable_shared_from_this<CommonPlayer>{
    public:
        using PlaneMap = std::map<uint32_t, Plane, std::greater<uint32_t>>;
        using PlaneMapIter = PlaneMap::iterator;
        CGameCltId m_gameClientID;
        std::string m_account;
        std::string m_nickname;
        uint32_t m_angle;//用来发给客户端的
        uint32_t m_skinid;
        uint32_t m_bulletskinid;
        CommonRoom *m_pRoom;
        /*
         * 为啥用map?--2017-07-02
         * 飞机容器的基本需求是:
         * 1.增加时候插入列表头部,保证新加飞机坐标是(0,0),要把所有元素往后移动1位
         * 2.减少时候找到该id的飞机删除
         * 3.分裂时候将列表后半部分所有元素都移除掉.
         *
         * 假设用vector,需求1和需求2的复杂度都为o(N)
         * 用list,需求1复杂度为O(1),需求2复杂度为o(N),最外围的飞机都在list最尾部
         * 用 map,以飞机id为key,key从大到小排列,
         * 那么first就是最后一次插入的,坐标为(0,0).
         * 分裂时将map的 [first+size()/2, end())区间段内飞机全部移除;
         * 减少飞机时,直接erase掉,遍历迭代器重新设置位置.
         * 需求1和需求3的复杂度为o(logN)
         */
        PlaneMap m_planes;

        GameStatisticsData m_gameData;
        bool m_isGod;
        bool m_isStop;
        // 在分裂过程中,收到移动状态改变消息,那就不用还原分裂前的移动状态
        bool m_receiverStopMsgWhenSplit;
        Formation &m_formation;
        // 包裹机群的粗略的圆的半径
        double      m_wrapRadius;
        uint64_t    m_lastShotTime;
        double      m_shotCD;

        // 上次分裂时间,分裂CD，分裂定时器id,分裂期间移动速度
        uint64_t    m_lastSplitTime;
        uint32_t    m_splitTimerID;
        // 出生时候的飞机数量
        uint32_t    m_bornPlaneNum;
        double m_godLastSeconds;

        // 加速定时器id
        uint32_t m_speedupTimerID;
        // 击杀我的玩家id,存下来,待帧击中消息打包出去后,给击杀者奖励
        uint32_t m_killerID;
        // 是否是掉线状态的玩家
        // 掉线后,该玩家不会再移动,就站在那里被打
        bool m_isOffline;
    protected:

    public:
        CommonPlayer(const uint16_t serverid, const uint16_t gameClientID
                , const uint32_t uid, const std::string &acc
                , const std::string &nickname, const uint32_t skinid
                , const uint32_t bulletid, CommonRoom *pRoom, Formation &formation);
        ~CommonPlayer();
        virtual bool isAI() const {return false;}
        Vector2 &getPos(){return m_pos;}
        double getMoveSpeed() {return m_moveSpeed;}

        virtual bool canAddPlane();
        uint32_t addOneSmallPlane();
        virtual void init();
        void notifyGod(bool isGodOver = false);
        virtual void notifyDeath(const std::string &name, bool isOutOfEdge);
        virtual void onKilled(CommonPlayer *pKiller);
        /* 
         * 击杀玩家能进入加速状态 x 秒
         * 如果玩家不在分裂和加速状态,通知客户端改变速度,设置定时器结束加速状态;
         * 如果玩家在分裂状态,判断分裂状态结束时间 >= x,若大于,则return;否则,移除定时器,进入加速状态;
         * 如果玩家在加速状态,移除旧的加速状态定时器,添加新的定时器;
         *
         *
         * 加速状态再加速,旧时间顶替掉新时间;加速状态点分裂,移除加速状态,开始分裂状态;
         * 分裂状态再加速,判时间决定是否更换定时器;分裂状态再分裂,移除旧分裂,开始新分裂.
         *
         * 静止状态打死玩家,还是静止状态;
         * 打死玩家后立即运动,应该还是处在加速状态.
         */
        void speedUp();
        inline bool isInSpeedUp() {return m_speedupTimerID != 0;}
        void onSpeedUpOver();
        void onKillOtherPlayer();
        // 仅仅删除一架小飞机,并重新计算速度
        // isFrameDelete=true 代表不需要通知客户端,自己内部计算即可
        void deleteOneSmallPlane(uint32_t planeid, bool isOutOfEdge = false, bool isFrameDelete = true);
        // 当某架小飞机出界时间到,将该飞机清除
        void onSmallPlaneOutOfEdgeTimeout(uint32_t planeid);
        void HandleMoveMsg(bool is_stop, uint32_t angle);
        void HandleTurnToMsg(uint32_t angle);
        void HandleFireMsg();
        void HandleSmallPlaneDieMsg(uint32_t planeid);
        // 玩家请求排行榜数据
        virtual void HandleReqRankMsg();
        // 获取分数,作为排行榜排名依据
        uint32_t getRankScore() const;
        uint32_t getRankOldScore() const;
        virtual void onSmallPlaneNumberChanged(bool isFrameDelete = false);
        void onMoveSpeedChanged(bool isStartSplit = false);
        // 生成子弹组位置
        void generateBullets(BulletGroup &group);
        // 发射子弹
        void shoot();
        // 设置方向,同时改变 m_dir里的x,y值
        void setAngle(uint32_t angle);
        double getBuleltGroupLifetime();
        /*
         * 分裂需求:
         * 静止,点分裂,分裂完成后静止.
         * 静止,点分裂,拖动摇杆开始动,分裂完成后运动.(拖动摇杆会先发停止消息,再发开始移动)
         * 运动,点分裂,分裂完成后按以前速度运动
         * 分裂时候,点分裂,取消旧分裂,开始新分裂运动
         * 分裂时候点停止,就立马停止,但再开始运动速度还是分裂的速度.
         *
         * 检查CD是否到了,是否已死亡,分裂数量是否够
         * 若能分裂,并且正处在分裂状态,清掉旧的分裂状态,开始新的分裂状态
         * 若能分裂,不在分裂状态,删除掉一半飞机,创建对应数量的能源飞机,将移动速度调大,运动状态设置为move,通知客户端
         * 添加定时器,时间到了后将移动速度设置回正常速度,
         * 如果分裂期间收到移动消息,isStop不需改变;如果没收到,isStop要恢复到分裂之前
         */
        void HandleSplitMsg();
        void onSplitTimeOver(bool isStop);
        // 是否处在分裂状态
        bool isInSplit();
        void cancelSplit();
        inline bool isDead() {return m_planes.size() == 0;}
        inline bool isGod() {return m_isGod;}
        virtual void frameMove(double deltaSeconds);
        // 获取 seconds 秒后的玩家位置,用于AI预判
        Vector2 getPosAfterSeconds(double seconds);
        void onDestroy();
        // 设置分数,同时设置旧分数
        virtual void setScore(uint32_t score);

        // 玩家下线时候处理
        void onOffline();
        void onReconnect();
};
using CommonPlayerSharedPtr = std::shared_ptr<CommonPlayer>;

