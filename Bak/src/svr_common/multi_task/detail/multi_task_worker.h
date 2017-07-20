#ifndef __MULTI_TASK_WORK_HEAD__
#define __MULTI_TASK_WORK_HEAD__

#include <memory>
#include <Theron/Theron.h>

namespace MultiTask
{
	class TaskBase;

	class Worker : public Theron::Actor
	{
	public:
		Worker(Theron::Framework &framework);
		void SetSharedObj(void *sharedObj) { m_sharedObj = sharedObj; }

	private:
		void Handler(const std::shared_ptr<TaskBase> &task, const Theron::Address from);

	private:
		void *m_sharedObj;
	};
}

#endif  //__MULTI_TASK_WORK_HEAD__
