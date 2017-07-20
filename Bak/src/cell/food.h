#pragma once
#include <memory>
#include "pos.h"

enum FoodType{
    FoodType_Energy = 0,//能源
    FoodType_SplitPlane = 1,//分裂飞机
};

/*
 * 该类暂不继承 Entity,待整理
 */

// 包括：地图内的正常能源,以及玩家分裂产生的能源飞机
struct Food{
    uint32_t m_id;
    Vector2 m_pos;
    // 食物类型,
    uint32_t m_ftype;
    uint32_t m_skinid;
    // 能源的角度,0-359
    uint32_t m_angle;

    /*
    Food() : m_id(0), m_posx(0), m_posy(0), m_ftype(FoodType_Energy)
             , m_skinid(0), m_angle(0)
    {
    }
    */

    Food(const uint32_t id, const double x, const double y, const uint32_t type = 0,
            const uint32_t skinid = 0, const uint32_t angle = 0)
        : m_id(id), m_pos(x, y), m_ftype(type), m_skinid(skinid), m_angle(angle)
    {
    }

    virtual bool isSplitPlane() {return false;}
};

struct SplitPlane : Food {
    // ownerid 代表是我是哪个玩家分裂出来的能源飞机
    // 目前该字段用来在玩家进入房间时候,将某个玩家的所有能源飞机用一条消息发送给客户端
    // 因为飞机的角度和皮肤都是一样的
    uint32_t m_ownerid;

    SplitPlane(const uint32_t id, const double x, const double y
            , const uint32_t skinid, const uint32_t angle, const uint32_t ownerid)
        : Food(id, x, y, FoodType_SplitPlane, skinid, angle), m_ownerid(ownerid)
    {}
    bool isSplitPlane() {return true;}
};

using FoodSharedPtr= std::shared_ptr<Food>;
using EnergySharedPtr = std::shared_ptr<SplitPlane>;
