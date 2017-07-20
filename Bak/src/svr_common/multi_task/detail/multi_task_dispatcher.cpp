#include "multi_task_dispatcher.h"

#include "multi_task_worker.h"
#include "multi_task/task.h"

namespace MultiTask
{
	Dispatcher::Dispatcher(Theron::Receiver &receiver, Theron::Framework &framework, const int workerCount, const std::function<void*(unsigned)> &createSharedObj)
		: Theron::Actor(framework)
		, m_receiver(receiver)
	{
		for (int i = 0; i < workerCount; ++i)
		{
			std::shared_ptr<Worker> worker = std::make_shared<Worker>(framework);
			worker->SetSharedObj(createSharedObj(i));
			m_workers.push_back(worker);
			m_frees.push(m_workers.back()->GetAddress());
		}
		RegisterHandler(this, &Dispatcher::Handler);
	}

	void Dispatcher::Handler(const std::shared_ptr<TaskBase> &task, const Theron::Address from)
	{
		if (task->IsCompleted())
		{
			Send(task, m_receiver.GetAddress());
			m_frees.push(from);
		}
		else
		{
			m_tasks.push(task);
		}

		while (!m_tasks.empty() && !m_frees.empty())
		{
			Send(m_tasks.front(), m_frees.front());
			m_frees.pop();
			m_tasks.pop();
		}
	}
}
