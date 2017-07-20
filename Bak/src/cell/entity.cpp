#include "entity.h"
#include "timer_queue/timer_queue_root.h"

Entity::Entity(const uint32_t id, const std::string &name
        , const double speed,const Vector2 &pos , const Vector2 &dir)
    : m_id(id), m_name(name), m_moveSpeed(speed), m_pos(pos),m_dir(dir)
{
}

EntityWithQueue::EntityWithQueue(const uint32_t id, const std::string &name)
    : Entity(id, name), m_pTimerQueue(new TimerQueue)
{
	TimerQueueRoot& rTimerQueueRoot = TimerQueueRoot::Get();
	m_pTimerQueue->SetParent(&rTimerQueueRoot);
}

void EntityWithQueue::onDestroy()
{
    m_pTimerQueue.reset(new TimerQueue);
	TimerQueueRoot& rTimerQueueRoot = TimerQueueRoot::Get();
	m_pTimerQueue->SetParent(&rTimerQueueRoot);
}
