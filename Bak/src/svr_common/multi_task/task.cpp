#include "task.h"

#include "multi_task_mgr.h"

namespace MultiTask
{
	TaskBase::TaskBase()
		:m_completed(false)
	{
	}

	TaskBase::~TaskBase()
	{

	}

	void TaskBase::Go()
	{
		Mgr::get_mutable_instance().AddTask(shared_from_this());
	}
}