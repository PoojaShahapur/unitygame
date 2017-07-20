#include "httpclient_task.h"
#include "log.h"
#include <curl/curl.h>


HttpClientTask::HttpClientTask(const std::string &baseUrl, const std::string &reqData, const std::string &reqMethod)
	: m_baseUrl(baseUrl)
	, m_reqData(reqData)
	, m_reqMethod(reqMethod)
{
}

HttpClientTask::~HttpClientTask()
{

}

size_t write_data(void *ptr, size_t size, size_t nmemb, std::string *p_result) {

	size_t n = (size * nmemb);
	p_result->append((const char*)ptr, n);

	return n;
}

void HttpClientTask::Process(void *sharedObj)
{
	CURL *curl = curl_easy_init();
	if (!curl) {
		LOG_WARN_TO("HttpClientTask", Fmt("task process failure, curl init failed"));
		return;
	}

	std::string().swap(m_result);

	CURLcode res;
	curl_easy_setopt(curl, CURLOPT_URL, m_baseUrl.c_str());
	curl_easy_setopt(curl, CURLOPT_FOLLOWLOCATION, 1L); 
	curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, 0L);
	curl_easy_setopt(curl, CURLOPT_SSL_VERIFYHOST, 0L);
	curl_easy_setopt(curl, CURLOPT_NOSIGNAL, 1L);	
	curl_easy_setopt(curl, CURLOPT_WRITEDATA, &m_result);
	curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, write_data);

	if (m_reqMethod == "POST") {
		curl_easy_setopt(curl, CURLOPT_POSTFIELDS, m_reqData.c_str());
	}
	res = curl_easy_perform(curl);
	if (res != CURLE_OK) {
		LOG_WARN_TO("HttpClientTask", curl_easy_strerror(res));
	}
	curl_easy_cleanup(curl);
}

void HttpClientTask::OnCompleted()
{
	if (m_cb)
	{
		m_cb(m_result);
	}
}