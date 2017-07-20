#include <cmath>//std::pow, floor
#include "plane.h"
#include "log.h"
#include "common_player.h"

const char LOG_NAME[] = "Plane";
Plane::Plane(const uint32_t id, CommonPlayer *player)
    : Entity(id, "plane"), m_pPlayer(player), m_arriveDest(false)
{}

void Plane::setDestPos(const Vector2 &dest)
{
    if (m_pos.m_x == dest.m_x && m_pos.m_y == dest.m_y)
    {
        m_arriveDest = true;
    }
    else
    {
        double vectorLen = std::sqrt(
                std::pow(m_pos.m_x - dest.m_x, 2) + std::pow(m_pos.m_y - dest.m_y, 2)
                );
        m_dir.m_x = (dest.m_x - m_pos.m_x) / vectorLen;
        m_dir.m_y = (dest.m_y - m_pos.m_y) / vectorLen;
        m_destPos = dest;
        m_arriveDest = false;
    }
}

const Vector2 Plane::getWorldPos()
{
    return Vector2(m_pPlayer->getPos().m_x + m_pos.m_x
            , m_pPlayer->getPos().m_y + m_pos.m_y);
}
const Vector2 Plane::getWorldPos() const
{
    return Vector2(m_pPlayer->getPos().m_x + m_pos.m_x
            , m_pPlayer->getPos().m_y + m_pos.m_y);
}

void Plane::frameMove(double deltaTime)
{
    if (!m_arriveDest)
    {
        Vector2 newPos(
            m_pos.m_x + m_pPlayer->getMoveSpeed() * deltaTime * m_dir.m_x,
            m_pos.m_y + m_pPlayer->getMoveSpeed() * deltaTime * m_dir.m_y
            );
        double multiplex = (m_destPos.m_x - newPos.m_x) * (m_destPos.m_x - m_pos.m_x);
        double multipley = (m_destPos.m_y - newPos.m_y) * (m_destPos.m_y - m_pos.m_y);
        if (multiplex <= 1e-6 && multipley <= 1e-6)
        {
            m_pos = m_destPos;
            m_arriveDest = true;
            //LOG_INFO(Fmt("planeid=%d,moveto=(%f,%f)") % m_id % m_destPos.m_x % m_destPos.m_y);
        }
        else
        {
            double movex = m_pPlayer->getMoveSpeed() * deltaTime * m_dir.m_x;
            double movey = m_pPlayer->getMoveSpeed() * deltaTime * m_dir.m_y;
            /*
            LOG_INFO(Fmt("planeid=%d,dir=(%f,%f),(%f,%f)-->(%f,%f),multixy=(%f,%f)") 
                    % m_id % m_dir.m_x % m_dir.m_y % m_pos.m_x % m_pos.m_y
                    % newPos.m_x % newPos.m_y % multiplex % multipley);
                    */
            m_pos = newPos;
        }
    }
}
