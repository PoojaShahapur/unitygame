#ifndef __LAMBDA_TASK_HEAD__
#define __LAMBDA_TASK_HEAD__

#include <string>
#include <vector>
#include <functional>

#include "task.h"

//support for simple task by lambda funciton

namespace MultiTask
{
	template<class ResultDataType>
	class LambdaTask : public TaskBase
	{
	public:
		using ProcessType = std::function<void(ResultDataType&)>;
		using OnCompletedType = std::function<void(const ResultDataType&)>;

	public:
		LambdaTask(const ProcessType &processHandler, const OnCompletedType &onCompletedHandler = OnCompletedType());
		~LambdaTask();

	public:
		const char *GetName() override { return MULTI_TASK_GETNAME(LambdaTask); }
		void Process(void *sharedObj) override;
		void OnCompleted() override;

	private:
		ProcessType m_process;
		OnCompletedType m_onCompleted;
		ResultDataType m_resultData;
	};

#include "lambda_task.ipp"
};

#endif  //__LAMBDA_TASK_HEAD__
