/**
 * File: src/cell/entity.h
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-30 16:26:25
 * Last Modified Date: 2017-06-30 16:26:25
 * Last Modified By: cxz <cxz@qq.com>
 */
#pragma once

#include <memory>
#include <string>
#include "pos.h"
/*
 * 场景内实体类,封装了所有场景内物件都需要的属性
 * 物件id
 * 物件名
 * 中心位置
 * 方向
 * 移动速度
 * 待整合 : 皮肤id
 */

class TimerQueue;

// 实体基类,
// 该类存在的目的:将 Room,Player,AIPlayer等共有的方法提取出来
struct Entity{
    uint32_t m_id;
    Vector2 m_pos;
    Vector2 m_dir;
    double m_moveSpeed;
    std::string m_name;

    Entity(const uint32_t id, const std::string &name
            , const double speed = 0.0f,const Vector2 &pos = Vector2()
            , const Vector2 &dir = Vector2());

};

struct EntityWithQueue : Entity
{
    EntityWithQueue(const uint32_t id, const std::string &name);
	std::unique_ptr<TimerQueue> m_pTimerQueue;
    virtual void onDestroy();
};
