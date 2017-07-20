#pragma once
#include <cstdint>

struct Pos{
    uint32_t m_x;
    uint32_t m_y;

    Pos(const uint32_t x = 0, const uint32_t y = 0) 
        : m_x(x), m_y(y)
    {}

    Pos(const Pos &pos)
        : m_x(pos.m_x), m_y(pos.m_y)
    {}

    const Pos & operator = (const Pos &pos)
    {
        m_x = pos.m_x;
        m_y = pos.m_y;
        return *this;
    }

    bool operator == (const Pos &pos)
    {
        return (m_x == pos.m_x && m_y == pos.m_y);
    }

    bool operator != (const Pos &pos)
    {
        return !this->operator==(pos);
    }
};

struct Vector2{
    double m_x;
    double m_y;

    Vector2(const double x = 0, const double y = 0) 
        : m_x(x), m_y(y)
    {}

    Vector2(const Vector2 &pos)
        : m_x(pos.m_x), m_y(pos.m_y)
    {}

    const Vector2 & operator = (const Vector2 &pos)
    {
        m_x = pos.m_x;
        m_y = pos.m_y;
        return *this;
    }

    bool operator == (const Vector2 &pos)
    {
        return (m_x == pos.m_x && m_y == pos.m_y);
    }

    bool operator != (const Vector2 &pos)
    {
        return !this->operator==(pos);
    }

    // 获取和另一个 Vector2 距离的平方
    inline double squareDistance(const Vector2 &pos)
    {
        return (m_x - pos.m_x) * (m_x - pos.m_x) + (m_y - pos.m_y) * (m_y - pos.m_y);
    }
    inline double squareDistance(const Vector2 &pos) const
    {
        return (m_x - pos.m_x) * (m_x - pos.m_x) + (m_y - pos.m_y) * (m_y - pos.m_y);
    }
};
