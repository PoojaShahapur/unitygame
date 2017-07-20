#include "timer_queue.h"

#include "log.h"

const char LOG_NAME[] = "TimerQueue";

#ifndef NDEBUG
#include <thread>
static std::thread::id s_mainThreadId = std::this_thread::get_id();
static bool IsMainThread() { return s_mainThreadId == std::this_thread::get_id(); }
#endif

TimerQueue::TimerQueue()
{
	assert(IsMainThread());
	SetNextTime();
}

TimerQueue::~TimerQueue()
{
	assert(IsMainThread());
	EraseParentTimer();
	assert(0 == (m_uValidFlag = 0));
}

TimerQueue::TimerId TimerQueue::InsertRepeatAt(
	const TimePoint& tpStart, unsigned int uIntervalMs, const Action & act)
{
	assert(IsMainThread());
	assert(IsValid());
	TimerItem itm = {tpStart, ++m_qwNextId, act, uIntervalMs };
	InsertItem(itm);
	return itm.id;
}

void TimerQueue::Erase(TimerId id)
{
	assert(IsMainThread());
	assert(IsValid());
	SetById & rSet = m_set.get<TimerId>();
	SetById::iterator itr = rSet.find(id);
	if (itr == rSet.end())
	{
		LOG_WARN(Fmt("Erase() can not find ID: %u. this=%p") % id % this);
		return;
	}

	// Set next time if the front item is erased.
	TimePoint tp = (*itr).tp;
	TimerId idErase = (*itr).id;  // Used to set next time.
	TimerId idFront = m_set.get<TimeOrder>().begin()->id;
	rSet.erase(itr);
	if (idErase == idFront)
		SetNextTime();
}

// 剩余毫秒值
unsigned int TimerQueue::GetLeftMs(TimerId id) const
{
	assert(IsMainThread());
	assert(IsValid());
	const SetById & rSet = m_set.get<TimerId>();
	SetById::const_iterator itr = rSet.find(id);
	if (itr == rSet.end()) return 0;  // 可能已经触发
	TimePoint tp = (*itr).tp;
	TimePoint tpNow = std::chrono::steady_clock::now();
	return std::chrono::duration_cast<std::chrono::milliseconds>(
		tp - tpNow).count();
}

void TimerQueue::SetParent(TimerQueue * p)
{
	assert(IsValid());
	EraseParentTimer();
	m_pParent = p;
	SetNextTime();
}

void TimerQueue::Tick()
{
	assert(IsMainThread());
	assert(IsValid());
	TimePoint tpNow = std::chrono::steady_clock::now();
	while (tpNow >= m_tpNext && !m_set.empty())
		TickOne();
}

void TimerQueue::TickOne()
{
	assert(IsValid());
	BOOST_ASSERT(!m_set.empty());
	TimerItem itm = Pop();
	if (itm.uIntervalMs)
	{
		itm.tp += std::chrono::milliseconds(itm.uIntervalMs);
		InsertItem(itm);
	}

	// 必须放在最后。允许act()中删除自身，如调用lua gc.
	itm.act();
}

TimerQueue::TimerItem TimerQueue::Pop()
{
	BOOST_ASSERT(!m_set.empty());
	SetByTime & rQ = m_set.get<TimeOrder>();
	SetByTime::iterator itr = rQ.begin();
	TimerItem itm = *itr;
	rQ.erase(itr);
	BOOST_ASSERT(itm.tp == m_tpNext);
	SetNextTime();
	return itm;
}

void TimerQueue::SetNextTime()
{
	EraseParentTimer();

	if (m_set.empty())
	{
		m_tpNext = std::chrono::steady_clock::now() +
			std::chrono::hours(24 * 365 * 10);
		return;
	}
	TimePoint tpNew = (*m_set.get<TimeOrder>().begin()).tp;
	m_tpNext = tpNew;
	if (NULL == m_pParent)
		return;
	m_idParentTimer = m_pParent->InsertSingleAt(m_tpNext, [this](){
		assert(IsMainThread());
		assert(IsValid());
		m_idParentTimer = 0;  // 省去m_pParent->Erase()
		TickOne();
	});
	BOOST_ASSERT(m_idParentTimer);  // 0 is illegal ID.
}

void TimerQueue::EraseParentTimer()
{
	if (m_pParent && m_idParentTimer)
		m_pParent->Erase(m_idParentTimer);
	m_idParentTimer = 0;
}

void TimerQueue::InsertItem(const TimerItem & itm)
{
	m_set.insert(itm);
	if (itm.tp < m_tpNext)
		SetNextTime();
}

