/**
 * File: src/cell/formation.cpp
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-29 16:27:27
 * Last Modified Date: 2017-06-29 18:23:33
 * Last Modified By: cxz <cxz@qq.com>
 */
#include <cmath>
#include "formation.h"
#include "pos.h"
#include "sin_cos_map.h"
#include <cmath>

const double PI  = std::acos(-1);
Formation::Formation(const double radius)
    : m_moduleRadius(radius)
{
}

void Formation::initFormationMap(uint32_t N)
{
    for (uint32_t planeNum = 1; planeNum <= N; ++planeNum)
    {
        calculateAllPlanePos(planeNum, m_formationMap[planeNum]);
    }
}



void Formation::initFormationRadiusMap(uint32_t N)
{
    for (uint32_t planeNum = 1; planeNum <= N; ++planeNum)
    {
        uint32_t circle = getTotalCircleNumByPlaneNum(planeNum);
        m_radiusMap[planeNum] = 2 * m_moduleRadius * (circle - 1) + m_moduleRadius;
    }
}

HexagonFormation::HexagonFormation(const double radius)
    : Formation(radius)
{}


uint32_t HexagonFormation::getThisCirclePlaneNum(uint32_t circle)
{
    if (circle > 1)
    {
        return 6 * (circle - 1);
    }

    return 1;
}

uint32_t HexagonFormation::getTotalCircleNumByPlaneNum(uint32_t planeNum)
{
    if (1 == planeNum)
    {
        return 1;
    }

    uint32_t currentNum = 1;
    uint32_t circleNum = 1;
    while (currentNum < planeNum)
    {
        ++circleNum;
        currentNum += getThisCirclePlaneNum(circleNum);
    }

    return circleNum;
}

/*
 * 从第2圈开始,遍历每圈分别计算,每圈的计算思路:
 * 1. 将6个顶点编号,该圈六边形最右边的顶点序号为0,按逆时针方向增加,算出6个特殊顶点的坐标
 * 2. 六边形的每条边内,顶点间横坐标相差 radius 个单位;纵坐标相差 sqrt(3)*radius 个单位
 */
void HexagonFormation::calculateAllPlanePos(uint32_t planeNum, std::vector<Vector2> &posVec)
{
    // 如果就1架飞机,刚好占1圈,刚好处在 (0.0, 0.0)
    posVec.emplace_back(0.0, 0.0);
    if (1 == planeNum)
    {
        return;
    }

    // 算出圈数N,从第2圈到第N圈分别计算
    uint32_t totalCircleNum = getTotalCircleNumByPlaneNum(planeNum);
    // 已经算过的飞机数量,用来终止循环
    uint32_t alreadyCalculateNum = 2;
    double sqrt3 = std::sqrt(3);
    for (uint32_t circle = 2; circle <= totalCircleNum; ++circle)
    {
        // 顶点0-5在该圈所有飞机中的序号
        uint32_t indexOfNode[] = {
            0, 1 * (circle - 1), 2 * (circle - 1),
            3 * (circle - 1), 4 * (circle - 1), 5 * (circle - 1)
        };

        // 顶点0-5在该圈的坐标
        Vector2 posOfNode[] = {
            Vector2(2 * m_moduleRadius * (circle - 1), 0),
            Vector2(m_moduleRadius * (circle - 1), sqrt3 * m_moduleRadius * (circle - 1)),
            Vector2(0 - m_moduleRadius * (circle - 1), sqrt3 * m_moduleRadius * (circle - 1)),
            Vector2(0 - 2 * m_moduleRadius * (circle - 1), 0),
            Vector2(0 - m_moduleRadius * (circle - 1), 0 - sqrt3 * m_moduleRadius * (circle - 1)),
            Vector2(m_moduleRadius * (circle - 1), 0 - sqrt3 * m_moduleRadius * (circle - 1)),
        };

        if (alreadyCalculateNum > planeNum)
        {
            break;
        }

        // 该变量代表当前圈的第几个飞机
        uint32_t thisCircleIndex = 0;
        uint32_t thisCircleNum = getThisCirclePlaneNum(circle);
        while (thisCircleIndex < thisCircleNum)
        {
            double x = 0.0;
            double y = 0.0;
            // 对于每条边,找到我是这条边的第几个顶点
            // 下面分别处理 第1-6条边
            if (thisCircleIndex >= indexOfNode[0] && thisCircleIndex < indexOfNode[1])
            {
                uint32_t myIndexInThisSide = thisCircleIndex - indexOfNode[0];
                x = posOfNode[0].m_x - m_moduleRadius * myIndexInThisSide;
                y = posOfNode[0].m_y + sqrt3 * m_moduleRadius * myIndexInThisSide;
            }
            else if (thisCircleIndex >= indexOfNode[1] && thisCircleIndex < indexOfNode[2])
            {
                uint32_t myIndexInThisSide = thisCircleIndex - indexOfNode[1];
                x = posOfNode[1].m_x - 2 * m_moduleRadius * myIndexInThisSide;
                y = posOfNode[1].m_y;
            }
            else if (thisCircleIndex >= indexOfNode[2] && thisCircleIndex < indexOfNode[3])
            {
                uint32_t myIndexInThisSide = thisCircleIndex - indexOfNode[2];
                x = posOfNode[2].m_x - m_moduleRadius * myIndexInThisSide;
                y = posOfNode[2].m_y - sqrt3 * m_moduleRadius * myIndexInThisSide;
            }
            else if (thisCircleIndex >= indexOfNode[3] && thisCircleIndex < indexOfNode[4])
            {
                uint32_t myIndexInThisSide = thisCircleIndex - indexOfNode[3];
                x = posOfNode[3].m_x + m_moduleRadius * myIndexInThisSide;
                y = posOfNode[3].m_y - sqrt3 * m_moduleRadius * myIndexInThisSide;
            }
            else if (thisCircleIndex >= indexOfNode[4] && thisCircleIndex < indexOfNode[5])
            {
                uint32_t myIndexInThisSide = thisCircleIndex - indexOfNode[4];
                x = posOfNode[4].m_x + 2 * m_moduleRadius * myIndexInThisSide;
                y = posOfNode[4].m_y;
            }
            else
            {
                uint32_t myIndexInThisSide = thisCircleIndex - indexOfNode[5];
                x = posOfNode[5].m_x + m_moduleRadius * myIndexInThisSide;
                y = posOfNode[5].m_y + sqrt3 * m_moduleRadius * myIndexInThisSide;
            }
            posVec.emplace_back(x, y);
            ++alreadyCalculateNum;
            ++thisCircleIndex;
            if (alreadyCalculateNum > planeNum)
            {
                break;
            }
        }
    }
}

CircleFormation::CircleFormation(const double radius)
    : Formation(radius)
{}

uint32_t CircleFormation::getThisCirclePlaneNum(uint32_t circle)
{
    if (1 == circle)
    {
        return 1;
    }

    // 这个7是实践试出来的数
    return 7 * (circle - 1) - 1;
}
uint32_t CircleFormation::getTotalCircleNumByPlaneNum(uint32_t planeNum)
{
    if (1 == planeNum)
    {
        return 1;
    }

    uint32_t circleNum = 1;
    uint32_t currentNum = 1;
    while (currentNum < planeNum)
    {
        ++circleNum;
        currentNum += getThisCirclePlaneNum(circleNum);
    }

    return circleNum;
}

void CircleFormation::calculateAllPlanePos(uint32_t planeNum, std::vector<Vector2> &posVec)
{
    posVec.emplace_back(0.0, 0.0);
    if (1 == planeNum)
    {
        return;
    }

    // 算出圈数N,从第2圈到第N圈分别计算
    uint32_t totalCircleNum = getTotalCircleNumByPlaneNum(planeNum);
    // 已经算过的飞机数量,用来终止循环
    uint32_t alreadyCalculateNum = 2;
    double sqrt3 = std::sqrt(3);
    for (uint32_t circle = 2; circle <= totalCircleNum; ++circle)
    {
        if (alreadyCalculateNum > planeNum)
        {
            break;
        }

        uint32_t thisCircleIndex = 0;
        uint32_t thisCircleNum = getThisCirclePlaneNum(circle);
        // 本圈内每个飞机之间的夹角
        const double kOneAngle = 360.0 / getThisCirclePlaneNum(circle);
        while (thisCircleIndex < thisCircleNum)
        {
            double angle = kOneAngle * (thisCircleIndex);
            double rad = angle * PI / 180.0f;
            double x = std::cos(rad) * (circle - 1) * 2 * m_moduleRadius;
            double y = std::sin(rad) * (circle - 1) * 2 * m_moduleRadius;
            posVec.emplace_back(x, y);
            ++alreadyCalculateNum;
            ++thisCircleIndex;
            if (alreadyCalculateNum > planeNum)
            {
                break;
            }
        }
    }
}
