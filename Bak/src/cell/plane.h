#pragma once
#include "pos.h"
#include "entity.h"

/*
 * 该类存在时,玩家一定存在,所以这里用了raw pointer,而非 shared_ptr
 * 玩家进房间或者复活后,创建该类;
 * 玩家离开游戏或者房间结束时候,销毁该类.(std::map负责销毁)
 *
 * m_pos 为局部坐标;m_dir为局部方向
 * 飞机类的 m_moveSpeed 不使用,直接取玩家的移动速度.
 */

class CommonPlayer;

struct Plane : public Entity{
        CommonPlayer *m_pPlayer;
        // 是否到达目标位置
        bool m_arriveDest;
        // 目标位置
        Vector2 m_destPos;

    public:
        Plane(const uint32_t id, CommonPlayer *player);

        // 设置我的目标位置
        void setDestPos(const Vector2 &dest);

        const Vector2 getWorldPos();
        const Vector2 getWorldPos() const;

        void frameMove(double deltaTime);
};
