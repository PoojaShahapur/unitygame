#pragma once

#include <string>
#include <map>
#include <vector>
#include "entity.h"
#include "rankmgr.h"
#include "common_player.h"
//#include "bullet_group.h"
#include "food.h"
#include "rpc/rpc_callback.h"

namespace google{
    namespace protobuf{
        class Message;
    };
};

namespace plane
{
    class EnterRoomResponse;
};

enum RoomMode
{
    RoomMode_Normal = 0,
    RoomMode_Purgatory = 1,
    RoomMode_Team = 2,
};

struct MVPCalculateData
{
    uint32_t m_uid;
    // 排名加起来的和
    uint32_t m_rankSum;

    MVPCalculateData(){}

    MVPCalculateData(const uint32_t id, const uint32_t combo)
        :m_uid(id),m_rankSum(combo)
    {}
};

// 能源飞机子弹都被服务器认为是一个圆,圆的半径默认为 0.5m
const double moduleRadius = 0.5f;
// 检查敌人的时间间隔,暂定为 500ms
const uint32_t checkEnemyMillSecondsInterval = 500;
const uint32_t sessionServerID = 1;

// 每局游戏内的排行榜管理器
using CommonPlayerSharedPtr = std::shared_ptr<CommonPlayer>;
using AllPlayerMap = std::map<std::string, CommonPlayerSharedPtr>;
using PlayerMap = std::map<uint32_t, CommonPlayerSharedPtr>;

class BulletGroup;
using BulletGroupMap= std::map<uint32_t, BulletGroup>;

using FoodMap = std::map<uint32_t, FoodSharedPtr>;

// 各种各样房间的基类
//class CommonRoom : public Entity{
//class CommonRoom : public EntityWithQueue, std::enable_shared_from_this<CommonRoom>{
class CommonRoom : public EntityWithQueue{
    friend class TeamRoom;
    friend class PurgatoryRoom;
    private:
        // 房间创建时候的时戳(毫秒)
        uint64_t m_roomCreateMillSeconds;
        // 房间结束时的时戳
        uint64_t m_roomEndMillSeconds;
        // 下一个要产生的 entity的id
        uint32_t m_nextEntityID;
        // 玩家的位置上次更新时候的时戳
        uint64_t m_playersLastUpdateMillSeconds;
        // 子弹组上次更新位置时的时戳
        uint64_t m_bulletGroupsLastUpdateMillSeconds;
        // 所有玩家指针，包括AI
    public:
        // 房间的帧数
        uint32_t m_currentFrame;
        uint32_t m_roomMode;
        std::string m_voiceToken;
        BulletGroupMap m_bulletGroupMap;
        // 排行榜管理器
        OneRoomRankManager<CommonPlayer> m_rankManager;
        static AllPlayerMap m_playerSharedPtrMap;
    private:
        FoodMap m_foodSharedPtrMap;
        PlayerMap m_players;

        //////////////////////
        // 跟房间配置相关的变量
        // 房间可容纳人数
        uint32_t m_holdPlayerNum;
        // 房间剩余秒数小于该秒数时候,禁止加入
        uint32_t m_forbidJoinSeconds;
        // 房间内产生的能源个数
        uint32_t m_energyNumber;
        // 需要击中多少架飞机才能给自己加1架飞机
        uint32_t m_needShotNumber;
        double m_moveSpeedK;
        double m_moveSpeedA;
        double m_moveSpeedB;
        double m_shotMinSeconds;
        double m_shotMaxSeconds;
        double m_shotAddCoef;

        // 下个时间片(1s后)应该产生的能源数
        uint32_t m_needCreateEnergyNumber;

        // 玩家最快速度,用来计算包裹子弹组路径的大正方形
        double m_playerFastestSpeed;
    public:
        // 加速持续时间,将策划配置的小数乘以1000
        uint32_t m_speedupLastMillSeconds;
        // 玩家最大可拥有飞机数量
        uint32_t    m_maxPlayerPlaneNumber;
        // 从被打死,到弹出复活界面的时间间隔
        double m_reliveSeconds;
        double m_shotDelaySeconds;
        double m_bulletSpeedCoef;
        double m_splitLifeTimeCoef;
        // 子弹组生命时长的3个系数
        double      m_lifeTimeK;
        uint32_t    m_lifeTimeA;
        double      m_lifeTimeB;
        // 分裂相关变量
        uint32_t m_splitLastMillSeconds;
        double m_splitCD;
        double m_splitSpeedFactor;
        Pos m_min;
        Pos m_max;

    public:
        CommonRoom(const uint32_t id);
        virtual ~CommonRoom();
        Pos randomEntityBornPos();
        CommonPlayer *getPlayerByID(uint32_t id);

        void broadcast(const std::string& sService, const std::string& sMethod 
                , const google::protobuf::Message& req, const RpcCallback &cb = RpcCallback());
        void luaBroadcast(const std::string& sService, const std::string& sMethod, const std::string &req);
        void broadcastPlayerEnter(CommonPlayer *pPlayer);
        // 创建普通能源
        FoodSharedPtr createFood(uint32_t x, uint32_t y);

        // 获取实体的出生点

        // 创建飞机残骸
        FoodSharedPtr createSplitPlane(double x, double y, uint32_t skinid, uint32_t angle, uint32_t ownerid);
        /*
         * 2017-07-05 frameCheckHit() 帧击中检测,目前房间里最复杂的一个函数.
         * 目的: 检测子弹有没有打中玩家,有没有打中能源,打中了要做处理
         * 注意:一定要按时间先后顺序将消息发送给客户端
         * 将子弹组看作一个圆,发射子弹就是该圆 按指定的方向从起点运动到终点
         *
         * 一些常数,2017-07-05
         * 飞机移动的最快速度,2架飞机按分裂,速度为:
         * 分裂系数 * 2架飞机速度 = 1.75 * (70 / (2+8) + 3) = 17.5 m/s
         * 此时发射子弹,子弹速度为:
         * 子弹系数 * 移动速度 = 17.5 * 2.5 = 43.75 m/s
         *
         * 飞机有20架时的分裂速度为: 1.75 * (70 / (20+8) + 3) = 8.75m/s
         * 此时发子弹,速度为 21.875 m/s
         * 
         * 实现思路如下:
         * 1. 发射子弹组时,计算该子弹组覆盖到的能源和玩家范围,存入潜在能源和敌人列表;
         * 2. 每帧检测子弹组是否和潜在能源或敌人相交,相交再检测具体哪个子弹击中哪架飞机/能源
         *
         * 如何找到潜在能源?
         * 1. 当能源出生,或者玩家分裂时候,遍历所有子弹组将能源/能源飞机添加到子弹组的潜在能源列表中
         * 2. 能源修改为集中出生,每秒钟出生一批能源,该批能源一次性检测能否添加到子弹组的潜在能源列表中.
         *
         * 如何找到潜在敌人?
         * 1. 发射子弹时,算下0-500ms子弹运动路径形成的包裹正方形,
         * 以速度 17.5m/s 运动 500ms 内能到达该包裹正方形的玩家，即为潜在敌人.
         * 画一个大正方形,边长为 2 * 17.5m/s * 0.5s + 子弹组包裹正方形边长,在该大正方形范围内的玩家都为潜在敌人
         * 2. 当玩家进入房间,遍历所有子弹组,添加到对应的子弹组潜在敌人列表中
         * 假设将玩家添加到所有子弹组列表,子弹组有80个,那500ms内要比较 33/2*80~=1200次
         * 假设玩家进入时候,检测从当前时间到下一个500ms的这段时间,每个子弹组的大正方形是否包含该玩家,
         * 假设有1/4的子弹组的大正方形包含玩家,则需要比较80次+33/2*20=380次
         *
         * 玩家有20架飞机,分裂时候发了一发子弹,存活时间为2s,子弹路径从正方形的顶点到对角,
         * 该正方形边长约为 21.875 * 2 + 半径2 * 2 ~= 48m
         *
         * 1. 获得 2s内可能进入该区域的玩家集合,
         * 17.5*2=35m,所以该正方形上下左右各加35m范围内的玩家即为玩家集合
         * 地图为 256m*256m.想筛选出潜在敌人,需要遍历1次找边长为 118范围内的玩家,假设玩家平均分布,即约有1/4玩家成为潜在敌人
         * 2s内1个子弹组需要检查66次1/4的玩家,假设地图内有40个玩家,40个玩家同时发子弹,那2s内for循环要比较 66*40*10 次
         * 2. 将2s分成4个500ms, 可能的玩家范围 17.5*0.5~=9m, 可能的子弹范围21.875*0.5+ 半径2*2 ~=15
         * ,每500ms需要遍历1次找边长为 33m 范围内的玩家,约有 1/64玩家成为潜在敌人
         * 2s需要遍历4次, 4 * 40玩家 + 40子弹组 * 66 * 40/64
         *
         *
         * 但实际上,玩家一直在移动,而且经常会聚集在某个范围内,同屏范围内玩家会超过10个,
         * 所以时间粒度到底要多细,是个未知数.目前先尝试500ms去算一次潜在敌人.但潜在能源是子弹出生时候,全部算完.
         */
        void frameCheckHit();
        void frameUpdate();
        void broadcastAllPlayerPos();

        // 获取下一个要产生的实体id
        uint32_t getNextEntityID(){return m_nextEntityID++;}
        // 根据小飞机数量获得移动速度
        double getMoveSpeedByPlaneNumber(uint32_t number);
        void getEnterRoomResponse();
        // 获取游戏还有多少秒结束
        uint32_t getGameOverLeftSeconds();
        // 获取还能进几个玩家
        uint32_t getLeftCanInPlayerNumber();
        double getShotCDByPlaneNumber(uint32_t planeNumber);
        //PlayerSharedPtr getPlayerByGameCltID(std::string gameClientString);
        virtual void init();
        void onDestroy();
        void onPlayerExit(CommonPlayerSharedPtr pPlayer);
        void eraseOneAIPlayer();
        // 添加一个AI玩家
        void addOneAIPlayer();

        // 获得离某个玩家最近的玩家id,目标玩家和pPlayer之间的距离要小于range
        uint32_t getNearestPlayerid(CommonPlayer *pPlayer, uint32_t range = -1);

        // 玩家退出后,清除掉 game_clt_id_str 到玩家的映射
        // 清除玩家的飞机,定时器
        // lua里判断如果房间人数为0,会移除该房间
        void playerExit(CommonPlayerSharedPtr pPlayer);
        // todo,这个函数参数太长了,得改
        void playerEnter(const uint16_t serverid, const uint64_t gameClientID, const std::string &account
            , const uint32_t uid, const std::string nickname, const uint32_t skinid, const uint32_t bulletid);
        // 玩家断线重连上来,返回 EnterRoomResponse
        virtual std::string playerReconnect(const uint16_t serverid, const uint64_t gameClientID, const uint32_t uid);
        // 所有的玩家填充至 EnterRoomResponse 消息
        void fulfillPlayersEnterRoomResponse(plane::EnterRoomResponse &response);
        void fulfillFoodsEnterRoomResponse(plane::EnterRoomResponse &response);

        // 创建和删除子弹组
        void newBulletGroup(CommonPlayer *p);
        void deleteBulletGroup(uint32_t id);
        // 判断两个玩家是否为敌人
        // 2017-07-14,现在改成了玩家指针从开始进入就在,一直到本局结束才析构,所以下面可以用CommonPlayer*
        // 但一旦允许玩家子弹在,人不在,下面就只能用id来对比了,而且子弹组敌人相关的检测要修改
        virtual bool isEnemy(CommonPlayer *pPlayer1, CommonPlayer *pPlayer2);
        // 填充该子弹组可能会击中的能源id列表
        // 遍历当前地图所有能源,依次判断能源是否在子弹组路径的正方形范围内
        void fillBulletGroupPossibleEnergies(BulletGroup &group);
        // 填充该子弹组可能会击中的玩家id列表
        // 遍历地图所有玩家,判断玩家是否在子弹组的大正方形范围内
        void fillBulletGroupPossibleEnemies(BulletGroup &group);
        // 当玩家进游戏时,遍历所有子弹组检查玩家是否是该子弹组的潜在敌人
        void refreshBulletEnemiesOnPlayerEnter(CommonPlayer *pPlayer);
        // 遍历所有子弹组,判断 这一批能源 是否需要 添加到该子弹组的潜在能源列表中
        void batchCheckEnergy(std::vector<FoodSharedPtr> &ptrs);
        // 每隔一段时间,定时创建能源
        void batchCreateEnergy();

        // 通知所有玩家排行榜数据
        virtual void notifyAllPlayerRankData();

        // 1. 给每个玩家发送结算界面数据
        // 2. 请求user给每个玩家发奖励
        void sendRankResultData();
        void getAllPlayerGameClientIDStrings(std::vector<std::string> &ret);
        virtual void getAllPlayerRankData(std::vector<LuaRankData> &ret);
        uint32_t getMVPPlayerUid();

        // 是否靠近结束
        bool isTimeNearOver();

        uint32_t GetPlayersCount();
        inline uint32_t GetRoomID() {return m_id;}
        inline uint32_t GetRoomMode() {return m_roomMode;}
};
