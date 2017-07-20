#ifndef SVR_COMMON_TIMER_QUEUE_ROOT_H
#define SVR_COMMON_TIMER_QUEUE_ROOT_H

#include "timer_queue.h"

// TimerQueueRoot为根队列，单件，
// 各功能的定时器为子队列。
// 用SetParent()设置父队列。
class TimerQueueRoot final : public TimerQueue
{
private:
	TimerQueueRoot() {}  // Singleton. Use TimerQueueRoot::Get()

public:
	// 不可多线程使用！
	static TimerQueueRoot& Get()
	{
		static TimerQueueRoot s_inst;
		return s_inst;
	}
};  // class TimerQueueRoot

#endif  // SVR_COMMON_TIMER_QUEUE_ROOT_H
