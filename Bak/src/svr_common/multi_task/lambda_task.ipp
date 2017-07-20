#include "lambda_task.h"

template<class ResultDataType>
LambdaTask<ResultDataType>::LambdaTask(const ProcessType &processHandler, const OnCompletedType &onCompletedHandler)
	: m_process(processHandler)
	, m_onCompleted(onCompletedHandler)
{

}

template<class ResultDataType>
LambdaTask<ResultDataType>::~LambdaTask()
{

}

template<class ResultDataType>
void LambdaTask<ResultDataType>::Process(void *)
{
	m_process(m_resultData);
}

template<class ResultDataType>
void LambdaTask<ResultDataType>::OnCompleted()
{
	if(m_onCompleted)
	{
		m_onCompleted(m_resultData);
	}
}