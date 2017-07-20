#include "multi_task_mgr.h"

#include "detail/multi_task_machine.h"
#include "task.h"

namespace MultiTask
{
	Mgr::Mgr()
	{
	}

	void Mgr::Run(boost::asio::io_service *mainloop, unsigned uThreadNum)
	{
		m_machine = std::make_shared<MultiTask::Machine>(uThreadNum);
		m_machine->Run(mainloop);
	}

	void Mgr::AddTask(const std::shared_ptr<TaskBase> &task)
	{
		m_machine->AddTask(task);
	}

	unsigned Mgr::GetThreadNum()
	{
		return m_machine->GetThreadNum();
	}
}