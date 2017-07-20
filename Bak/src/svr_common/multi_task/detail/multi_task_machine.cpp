#include "multi_task_machine.h"

#include "multi_task_dispatcher.h"
#include "multi_task_worker.h"
#include "multi_task/task.h"
#include "log.h"

#include <boost/asio.hpp>  // for post()

#ifdef WIN32
#ifdef _DEBUG
#pragma comment(lib,"Therond.lib")
#else
#pragma comment(lib,"Theron.lib")
#endif
#endif

namespace MultiTask
{
	Machine::Machine(unsigned uThreadNum)
		: m_pmainloop(nullptr)
		, m_framework(uThreadNum)
		, m_threadnum(uThreadNum)
	{

	}

	Machine::~Machine()
	{
		m_pmainloop = nullptr;
	}

	void Machine::Run(boost::asio::io_service *mainloop)
	{
		assert(mainloop);
		m_pmainloop = mainloop;
		m_receiver.RegisterHandler(this, &Machine::OnTaskCompleted);
	}

	void Machine::OnTaskCompleted(const std::shared_ptr<TaskBase> &task, const Theron::Address from)
	{
		m_pmainloop->post([task]()
		{
			task->OnCompleted();
		});
	}

	void Machine::AddTask(const std::shared_ptr<TaskBase> &task)
	{
		auto itr = m_dispatchers.find(task->GetName());
		if (itr == m_dispatchers.end())
		{
			auto dispatcher = std::make_shared<Dispatcher>(
				m_receiver,
				m_framework,
				m_threadnum,
				std::bind(&TaskBase::CreateSharedObj, task, std::placeholders::_1)
				);
			itr = m_dispatchers.insert(std::make_pair(task->GetName(), dispatcher)).first;
		}
		if (itr == m_dispatchers.end())
		{
			LOG_ERROR_TO("multimask", "AddTask Failed, Memory Error!");
			return;
		}
		m_framework.Send(task, m_receiver.GetAddress(), itr->second->GetAddress());
	}
}