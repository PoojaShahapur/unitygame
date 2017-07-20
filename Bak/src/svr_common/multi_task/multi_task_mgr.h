#ifndef __MULTI_TASK_MGR_HEAD__
#define __MULTI_TASK_MGR_HEAD__

#include <boost/asio.hpp>
#include "singleton.h"
#include <functional>

namespace MultiTask
{
	class Machine;
	class TaskBase;

	class Mgr : public Singleton<Mgr>
	{
	public:
		Mgr();

	public:
		void Run(boost::asio::io_service *mainloop, unsigned uThreadNum = 3);
		void AddTask(const std::shared_ptr<TaskBase> &task);
		unsigned GetThreadNum();

	private:
		std::shared_ptr<Machine> m_machine;
	};
};

#endif  //__MULTI_TASK_MGR_HEAD__
