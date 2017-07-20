#ifndef __MULTI_TASK_MACHINE_HEAD__
#define __MULTI_TASK_MACHINE_HEAD__

#include "asio/asio_fwd.h"  // for io_service

#include <Theron/Theron.h>
#include <unordered_map>
#include <string>
#include <memory>

/**
使用Theron实现多任务管理器。该实现参照了Theron-6.00.02\Tutorial\FileReader
使用了Theron线程池，让每个task对象并发、异步完成
Detail目录下内容可编进lib，且对外不可见
*/

namespace MultiTask
{
	class Worker;
	class Dispatcher;
	class TaskBase;

	class Machine
	{
	public:
		Machine(unsigned uThreadNum);
		virtual ~Machine();

	public:
		void Run(boost::asio::io_service *mainloop);
		void AddTask(const std::shared_ptr<TaskBase> &task);

	public:
		Theron::Framework &GetFramework() { return m_framework; }
		boost::asio::io_service *GetMainLoop() { return m_pmainloop; }
		unsigned GetThreadNum() { return m_threadnum; }
		Theron::Receiver &GetReceiver() { return m_receiver; }

	private:
		void OnTaskCompleted(const std::shared_ptr<TaskBase> &task, const Theron::Address from);

		Theron::Framework m_framework;
		boost::asio::io_service *m_pmainloop;
		unsigned m_threadnum;

	private:
		std::unordered_map<std::string, std::shared_ptr<Dispatcher>> m_dispatchers;
		Theron::Receiver m_receiver;
	};
}

#endif  //__MULTI_TASK_MACHINE_HEAD__
