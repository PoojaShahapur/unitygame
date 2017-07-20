/**
 * File: src/cell/bullet_group.h
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-30 14:10:23
 * Last Modified Date: 2017-06-30 14:10:23
 * Last Modified By: cxz <cxz@qq.com>
 */
#pragma once
#include <map>
#include <vector>
#include "pos.h"
#include "entity.h"

class BulletGroup;
struct Bullet : Entity{
    const BulletGroup &m_group;
    uint32_t m_correspondPlaneID;

    Bullet(const uint32_t id, const uint32_t planeid
            , const Vector2& pos, const BulletGroup &group)
        : Entity(id, "bullet", 0.0, pos), m_group(group), m_correspondPlaneID(planeid)
    {
    }

    const Vector2 getWorldPos();
};
using BulletMap = std::map<uint32_t, Bullet>;

/*
 * 子弹组类:控制一堆半径为r的子弹群,以方向dir和速度speed移动x秒
 * 子弹组一经创建,就与任何玩家无关.由Room检测击中,销毁.
 *
 * 子弹组的产生和销毁是由Room控制,Room会在添加子弹组时,设置定时器销毁
 * 子弹组是Room内实体,实体的唯一id都是由Room生成的;实体进入场景也是由Room广播出去
 * shooterid用于通知新上线的客户端产生子弹组,
 * m_wrapRadius 是子弹组的包裹半径,一出生就生成好的,用于优化帧击中检测
 * pos,angle,speed,lifetime标记子弹组从哪个位置,以什么速度和方向,移动多久
 * m_map 为所有子弹集合,与玩家的小飞机一一对应,由玩家在发射子弹时候填充
 *
 * 为优化帧击中检测,子弹组在创建的时候,筛选出可能攻击到的能源,500ms可能攻击到的玩家
 * 每帧遍历时候,只遍历该子弹组可能攻击到的实体列表
 */


class CommonPlayer;
class BulletGroup : public Entity{
    public:
        uint32_t m_shooterid;
        uint32_t m_angle;//发给客户端用的
        double m_wrapRadius;
        double m_lifeTime;//后面进来玩家要看到子弹,并看到子弹消失
        BulletMap m_map;

        // 可能会击中的能源id集合,子弹组创建时候会填充该集合
        std::vector<uint32_t> m_possibleEnergies;
        // 500ms内可能会击中的玩家集合
        std::vector<uint32_t> m_possibleEnemies;
        // 下次填充 m_possibleEnemies 的时间戳
        uint64_t m_nextCheckEnemyMillSeconds;

        // 按照玩家的小飞机阵型,依次生成子弹
        void removeBulletByID(uint32_t bulletid);
        void frameMove(double deltaTime);

        // 可以用正方形包裹住子弹组路径,
        // 该函数获取正方形的最小坐标(left,bottom)和最大坐标(right,top)
        void getWrapSquareCoordinate(double &left, double &bottom, double &right, double &top);
        // 获取大正方形的 最小,最大坐标
        void getBiggerWrapSquareCoordinate(double &left, double &bottom
                , double &right, double &top, double playerFastestSpeed, double moveMillSeconds);

        BulletGroup(const uint32_t id, const uint32_t shooterid
                , const Vector2& pos ,const Vector2 &dir
                , const double radius, const double speed, const double t, const uint32_t angle)
            : Entity(id, "group", speed, pos, dir), m_shooterid(shooterid), m_wrapRadius(radius)
              , m_lifeTime(t), m_angle(angle)
        {}
};
