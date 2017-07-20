#ifndef SVR_COMMON_TIMER_QUEUE_H
#define SVR_COMMON_TIMER_QUEUE_H

#include <chrono>
#include <cstdint>
#include <functional>

#include <boost/noncopyable.hpp>
#include <boost/multi_index_container.hpp>
#include <boost/multi_index/member.hpp>
#include <boost/multi_index/composite_key.hpp>
#include <boost/multi_index/hashed_index.hpp>
#include <boost/multi_index/ordered_index.hpp>

class TimerQueue : boost::noncopyable
{
public:
	TimerQueue();
	virtual ~TimerQueue();

public:
	using Action = std::function<void()>;
	using TimerId = uint64_t;
	using TimePoint = std::chrono::steady_clock::time_point;

public:
	// 添加定时器，分为单次定时器和重复定时器. uMsFromNow是多少ms后开始。
	// uIntervalMs为间隔时间，不应该太小，建议最小100ms。
	TimerId InsertSingleAt(const TimePoint& tpStart, const Action & act)
	{
		return InsertRepeatAt(tpStart, 0, act);
	}
	TimerId InsertSingleFromNow(unsigned int uMsFromNow, const Action & act)
	{
		return InsertRepeatFromNow(uMsFromNow, 0, act);
	}
	TimerId InsertRepeatNow(unsigned int uIntervalMs, const Action & act)
	{
		return InsertRepeatFromNow(0, uIntervalMs, act);
	}
	TimerId InsertRepeatAt(const TimePoint& tpStart, unsigned int uIntervalMs,
		const Action & act);
	TimerId InsertRepeatFromNow(unsigned int uMsFromNow, unsigned int uIntervalMs,
		const Action & act)
	{
		return InsertRepeatAt(std::chrono::steady_clock::now() +
			std::chrono::milliseconds(uMsFromNow), uIntervalMs, act);
	}

	// 删除定时器
	void Erase(TimerId id);
	// 剩余毫秒值
	unsigned int GetLeftMs(TimerId id) const;

public:
	// 设置父队列
	void SetParent(TimerQueue * p);

public:
	// 由外部定时器定时执行动作。
	// 建议动作间隔为100ms以上。
	void Tick();

private:
	void TickOne();
	void SetNextTime();
	void EraseParentTimer();

private:
	struct TimerItem
	{
		TimePoint tp;
		TimerId id;
		Action act;
		unsigned int uIntervalMs;  // 0表示单次
	};

private:
	void InsertItem(const TimerItem & itm);
	TimerItem Pop();

private:
	struct TimeOrder {};

	using TimerIdIdx = boost::multi_index::hashed_unique<
		boost::multi_index::tag<TimerId>,
		boost::multi_index::member<TimerItem, TimerId, &TimerItem::id>
	>;
	using TimeOrderIdx = boost::multi_index::ordered_unique<
		boost::multi_index::tag<TimeOrder>,
		boost::multi_index::composite_key<
			TimerItem,
			boost::multi_index::member<TimerItem, TimePoint, &TimerItem::tp>,
			boost::multi_index::member<TimerItem, TimerId, &TimerItem::id>
		>  // composite_key
	>;  // ordered_unique

	using TimerItemSet = boost::multi_index::multi_index_container<
		TimerItem,
		boost::multi_index::indexed_by<TimeOrderIdx, TimerIdIdx>
	>;  // multi_index_container

	using SetByTime = TimerItemSet::index<TimeOrder>::type;
	using SetById = TimerItemSet::index<TimerId>::type;

private:
	TimerItemSet m_set;

private:
	TimerId m_qwNextId = 1;  // 0 is illegal
	TimePoint m_tpNext;

	TimerQueue * m_pParent = nullptr;
	TimerId m_idParentTimer = 0;  // Used to erase timer from parent

#ifndef NDEBUG
	uint32_t m_uValidFlag = 0x5a5a5a5a;
	bool IsValid() const { return 0x5a5a5a5a == m_uValidFlag; }
#endif
};  // class TimerQueue

#endif  // SVR_COMMON_TIMER_QUEUE_H
