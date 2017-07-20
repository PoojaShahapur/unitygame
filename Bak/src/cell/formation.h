/**
 * File: src/cell/formation.h
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-29 15:48:15
 * Last Modified Date: 2017-06-29 15:48:15
 * Last Modified By: cxz <cxz@qq.com>
 */
#pragma once
//#include <algorithm>
//#include <memory>
#include <vector>
#include <map>
#include "singleton.h"  // for Singleton<>

// 注意,目前的数值都是基于模型半径为 0.5 的前提下换算而来
// 一旦模型半径变了,就要重新算下数值,否则客户端表现会很怪异

/*
 * 阵型类的作用:
 * 在将飞机看成一个圆的前提下:
 * 1. 给定飞机数量N,计算出该阵型各个飞机应该摆放的位置
 * 2. 给定飞机数量N,计算出该阵型包裹圆的半径
 */ 

struct Vector2;
class Formation{
    public:
        Formation(const double radius);
        // key : 飞机数量
        // value : 该飞机数量下,每个飞机应摆放的位置集合(一共有key个元素)
        using FormationMap = std::map<uint32_t, std::vector<Vector2>>;
        // key : 飞机数量
        // value : 该飞机数量下,包裹圆的半径
        using FormationRadiusMap = std::map<uint32_t, double>;

        // 获取阵型ID
        virtual uint16_t getFormationID(){return 0;}

        // 根据圈数,获得这圈应该有几架飞机
        virtual uint32_t getThisCirclePlaneNum(uint32_t circle) {return 0;}

        // 根据飞机总数,获得一共需要排列多少圈
        virtual uint32_t getTotalCircleNumByPlaneNum(uint32_t planeNum) {return 0;}

        // 计算出 planeNum 个飞机分别应该摆放的位置
        virtual void calculateAllPlanePos(uint32_t planeNum, std::vector<Vector2> &posVec){}

        ///////////// 所有阵型共有方法
        // 根据每个小飞机的圆形模型半径的大小,以及飞机的数量,
        // 算出每个飞机应该在该阵型的哪个相对坐标
        void initFormationMap(uint32_t N);
        // 提前算好该阵型从1-N架飞机对应的包裹圆半径
        void initFormationRadiusMap(uint32_t N);
        // 根据飞机总数,获得包裹圆的半径
        double getWrapRadius(uint32_t planeNum) {return m_radiusMap[planeNum];}

        // get方法
        FormationMap & getFormation() {return m_formationMap;}
    private:
        FormationMap m_formationMap;
        FormationRadiusMap m_radiusMap;

    protected:
        // 一架飞机的模型半径
        double m_moduleRadius;
};

// 子类中的函数,请看基类定义和说明
// 六边形阵型
class HexagonFormation : public Formation, public Singleton<HexagonFormation>{
    public:
        HexagonFormation(const double radius = 0.55f);
        uint16_t getFormationID(){return 2;}
        void calculateAllPlanePos(uint32_t planeNum, std::vector<Vector2> &posVec);
        uint32_t getThisCirclePlaneNum(uint32_t circle);
        uint32_t getTotalCircleNumByPlaneNum(uint32_t planeNum);
};

// 圆形阵型
class CircleFormation : public Formation, public Singleton<CircleFormation>{
    public:
        CircleFormation(const double radius = 0.6f);
        uint16_t getFormationID(){return 1;}
        void calculateAllPlanePos(uint32_t planeNum, std::vector<Vector2> &posVec);
        uint32_t getThisCirclePlaneNum(uint32_t circle);
        uint32_t getTotalCircleNumByPlaneNum(uint32_t planeNum);
};
