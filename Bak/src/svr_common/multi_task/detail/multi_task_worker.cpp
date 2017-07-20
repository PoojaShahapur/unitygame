#include "multi_task_worker.h"

#include "multi_task/task.h"

namespace MultiTask
{
	Worker::Worker(Theron::Framework &framework)
		: Theron::Actor(framework)
		, m_sharedObj(nullptr)
	{
		RegisterHandler(this, &Worker::Handler);
	}

	void Worker::Handler(const std::shared_ptr<TaskBase> &task, const Theron::Address from)
	{
		std::shared_ptr<TaskBase> t(task);
		t->Process(m_sharedObj);
		t->IsCompleted(true);
		Send(t, from);
	}
}
