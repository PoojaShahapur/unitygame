/**
 * File: src/cell/aiplayer.h
 * Author: cxz <cxz@qq.com>
 * Date: 2017-07-08 22:22:23
 * Last Modified Date: 2017-07-08 22:22:23
 * Last Modified By: cxz <cxz@qq.com>
 */
#pragma once
#include "common_player.h"

/*
 * 该AI为普通版本的AI,普通AI该干啥呢?
 * 没玩家的时候发育,有玩家靠近的时候攻击.
 * AI身上存一个map m_distanceToEnergy
 * key:AI到该能源的距离; value: 该能源的id
 * 每10秒钟Room会遍历所有能源,遍历所有AI,将该AI10秒钟能走到位置的能源id添加到 m_distanceToEnergy
 *
 * AI初始为发育状态
 * 遍历AI身上的 m_distanceToEnergy,找到第一个仍存在的能源,判断距离够的话,直接设置方向并射击;
 * 射击完后立马走向到下一个能源，更改朝向,继续移动.
 * 如果该AI搜索到了敌人,进入攻击状态
 * 攻击状态下不停根据玩家的位置换方向,CD到了就射击,直到打死玩家或者被玩家打死,攻击状态才结束.
 */

enum AIState{
    AIState_Eat = 0,
    AIState_Attack = 1,
};

class AIPlayer : public CommonPlayer
{
    friend class PurgatoryAIPlayer;
    public:
        AIPlayer(const uint32_t uid, const std::string &acc
            , const std::string &nickname, const uint32_t skinid
            , const uint32_t bulletid, CommonRoom *pRoom, Formation &formation);
        ~AIPlayer();
        bool isAI() const {return true;}

        bool isCloseToEdge(const Vector2 &pos);

        void frameMove(double deltaSeconds);

        void init();

        void findNearestEnemy();

        void changeDirOnTimer();

        // 更换状态
        void changeState(uint32_t state, uint32_t enemyid = 0);

        void changeDirWhenAttack();

        void onKilled(CommonPlayer *pKiller);

        void notifyDeath(const std::string &name, bool isOutOfEdge);

        // 当阶段改变时触发
        void onStageChanged(uint32_t searchRange, uint32_t shotcdTimes);

        uint32_t m_searchRange;
        // 射击CD的倍数
        uint32_t m_shotcdTimes;
    private:
        // 已经设置出界后回来的方向了
        bool m_alreadySetNextAngle;
        uint32_t m_nextAngle;
        uint32_t m_state;
        // AI一出生就开始发射子弹,一直到死亡
        uint32_t m_shotTimerID;
        // 当射击定时器创建时候的射击CD
        double m_shotCDWhenTimerCreated;
        uint32_t m_changeDirTimerID;
        uint32_t m_attackChangeDirTimerID;
        uint32_t m_searchEnemyTimerID;
        // 要攻击的玩家id
        uint32_t m_enemyPlayerID;
        // 找敌人定时器触发的次数,5次就重新搜索一次
        uint32_t m_findEnemyTriggerTimes;
        std::map<double, uint32_t> m_distanceToEnergy;
};

class Team;
class TeamAIPlayer : public AIPlayer
{
    public:
        Team *m_pTeam;
        TeamAIPlayer(const uint32_t uid, const std::string &acc
            , const std::string &nickname, const uint32_t skinid
            , const uint32_t bulletid, CommonRoom *pRoom, Formation &formation);
        void setScore(uint32_t score);
};

