#ifndef __MULTI_TASK_DISPATCHER_HEAD__
#define __MULTI_TASK_DISPATCHER_HEAD__

#include <Theron/Theron.h>
#include <vector>
#include <queue>
#include <memory>
#include <functional>

namespace MultiTask
{
	class Worker;
	class TaskBase;

	class Dispatcher : public Theron::Actor
	{
	public:
		Dispatcher(Theron::Receiver &receiver, Theron::Framework &framework, const int workerCount, const std::function<void*(unsigned)> &createSharedObj);

	private:
		void Handler(const std::shared_ptr<TaskBase> &task, const Theron::Address from);

		std::vector<std::shared_ptr<Worker>> m_workers;
		std::queue<Theron::Address> m_frees;
		std::queue<std::shared_ptr<TaskBase>> m_tasks;
		Theron::Receiver &m_receiver;
	};
}

#endif  //__MULTI_TASK_DISPATCHER_HEAD__
