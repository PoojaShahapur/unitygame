#ifndef __HTTPCLIENT_TASK_HEAD__
#define __HTTPCLIENT_TASK_HEAD__

#include <string>
#include <vector>
#include <functional>
#include "multi_task/task.h"

class HttpClientTask : public MultiTask::TaskBase
{
public:
	HttpClientTask(const std::string &baseUrl, const std::string &reqData = "", const std::string &reqMethod = "GET");
	~HttpClientTask();

	using QueryCBType = std::function<void(const std::string &)>;
	std::shared_ptr<MultiTask::TaskBase> SetQueryCB(const QueryCBType & cb) { m_cb = cb; return shared_from_this(); }

public:
	const char *GetName() override { return "HttpClientTask"; }
	void Process(void *sharedObj) override;
	void OnCompleted() override;

private:
	std::string m_baseUrl;
	std::string m_reqData;
	std::string m_reqMethod;
	std::string m_result;
	QueryCBType m_cb;
};

#define HttpClient_Get(url) std::make_shared<HttpClientTask>(url)
#define HttpClient_Post(url, data) std::make_shared<HttpClientTask>((url), (data), "POST")

#endif  //__MONGODB_QUERY_TASK_HEAD__
