#ifndef __HTTPCLIENT_HEAD__
#define __HTTPCLIENT_HEAD__

#include <string>

int boost_http_sync_client(const std::string& server, const std::string& port, const std::string& method, const std::string& path, const std::string& data,
	std::string& out_response_status_line, std::string& out_response_header, std::string& out_response_data);


//////////////////////////////////////////////////////////////////////////  
//可以解析下列三种类型的URL:  
//http://yunhq.sse.com.cn:32041/v1/sh1/snap/204001?callback=jQuery_test&select=name%2Clast%2Cchg_rate%2Cchange%2Camount%2Cvolume%2Copen%2Cprev_close%2Cask%2Cbid%2Chigh%2Clow%2Ctradephase  
//http://hq.sinajs.cn/list=sh204001  
//https://www.baidu.com  
//////////////////////////////////////////////////////////////////////////  
int parse_url(const std::string& url, std::string& out_server, std::string& out_port, std::string& out_path);


#endif  //__MONGODB_QUERY_TASK_HEAD__
