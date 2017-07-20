/**
 * File: src/cell/bullet_group.cpp
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-30 14:10:13
 * Last Modified Date: 2017-06-30 14:10:13
 * Last Modified By: cxz <cxz@qq.com>
 */
#include "bullet_group.h"
#include "sin_cos_map.h"

const Vector2 Bullet::getWorldPos()
{
    return Vector2(m_group.m_pos.m_x + m_pos.m_x
            , m_group.m_pos.m_y + m_pos.m_y);
}

void BulletGroup::removeBulletByID(uint32_t bulletid)
{
    auto it = m_map.find(bulletid);
    if (it != m_map.end())
    {
        m_map.erase(it);
    }
}

void BulletGroup::frameMove(double deltaTime)
{
    m_pos.m_x = m_pos.m_x + m_moveSpeed * deltaTime * m_dir.m_x;
    m_pos.m_y = m_pos.m_y + m_moveSpeed * deltaTime * m_dir.m_y;
}

void BulletGroup::getWrapSquareCoordinate(double &left, double &bottom, double &right, double &top)
{
    //如果方向有x轴向左的分量,那么left = 子弹组的终点x - 子弹组半径, right = 子弹组起点x + 子弹组半径
    //如果有x轴向右,left = 子弹组的起点x - 子弹组半径,right = 子弹组终点x + 子弹组半径
    double endx = m_pos.m_x + m_moveSpeed * m_lifeTime * m_dir.m_x;
    double endy = m_pos.m_y + m_moveSpeed * m_lifeTime * m_dir.m_y;
    double smallerx = m_pos.m_x;
    double biggerx = endx;
    if (m_dir.m_x < 0)
    {
        smallerx = endx;
        biggerx = m_pos.m_x;
    }
    left = smallerx - m_wrapRadius;
    right = biggerx + m_wrapRadius;

    double smallery = m_pos.m_y;
    double biggery = endy;
    if (m_dir.m_y < 0)
    {
        smallery = endy;
        biggery = m_pos.m_y;
    }
    bottom = smallery - m_wrapRadius;
    top = biggery + m_wrapRadius;
}

void BulletGroup::getBiggerWrapSquareCoordinate(double &left, double &bottom
                , double &right, double &top, double playerFastestSpeed, double moveMillSeconds)
{
    double moveDistance = playerFastestSpeed * moveMillSeconds;
    getWrapSquareCoordinate(left, bottom, right, top);
    left -= moveDistance;
    bottom -= moveDistance;
    right += moveDistance;
    top += moveDistance;
}
