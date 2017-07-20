#ifndef __TASK_HEAD__
#define __TASK_HEAD__

#include <memory>
#include <cassert>

/**
使用方法：
1）启动多任务管理器，如下例子
	MultiTask::Mgr::get_mutable_instance().Run(&mainio, 4);

2）继承 TaskBase，实现对应接口
	a、重载TaskBase::Process，实现具体需要异步执行的任务逻辑
	b、重载TaskBase::OnCompleted，实现 异步任务完成后的逻辑。该函数在主线程中被调用
	c、对于具有访问公共对象的任务（如涉及mysql连接等任务），需要重载TaskBase::CreateSharedObj
	   返回mysql访问封装类对象。该对象会作为TaskBase::Process的参数sharedObj传入

具体可参见 readfile_task.h中的异步读取文件的例子

*/

namespace MultiTask
{
	class TaskBase : public std::enable_shared_from_this<TaskBase>
	{
	public:
		TaskBase();
		virtual ~TaskBase();
		bool IsCompleted() { return m_completed; }
		void IsCompleted(bool value) { m_completed = value; }

	public:

		//以下虚函数必须实现
		virtual const char *GetName() { assert(0); return ""; };
		virtual void Process(void *sharedObj) = 0;
		virtual void OnCompleted() = 0;

		/**
		非必须实现。可应用于创建共享资源对象。避免多线程问题
		实现该方法，则内部每个worker会对应一个SharedObj
		如实现mysql、mongodb等，则需要实现该方法，让每个worker对应一个访问数据库的连接
		Process函数中的参数sharedObj，即为CreateSharedObj所创建
		*/
		virtual void *CreateSharedObj(unsigned index) { return nullptr; }

	public:
		virtual void Go();

	protected:
		bool m_completed;
	};
}

#define MULTI_TASK_GETNAME(CLASS) #CLASS

#endif  //__TASK_HEAD__
