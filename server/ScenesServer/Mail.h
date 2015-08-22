/*************************************************************************
 Author: wang
 Created Time: 2015年03月26日 星期四 11时03分54秒
 File Name: ScenesServer/Mail.h
 Description: 
 ************************************************************************/
#ifndef SCENE_MAIL_H_
#define SCENE_MAIL_H_
#include <string>
#include "zType.h"

struct zObject;
namespace Mail
{
    /**
     * \brief	发送邮件 可以带附件可以带道具
     */
    void send(const std::string& sFromName, DWORD dwFromId, const std::string& sToName, DWORD dwToId,
	    BYTE type, DWORD dwMoney=0, const zObject* pObj = NULL, const std::string& sText="", 
	    const zObject* pObj1=NULL, const zObject* pObj2=NULL, const std::string& sTitle="");
    /**
     * \brief	发送邮件 发送纯文本邮件
     */
    void sendSysText(const std::string& sFromName, DWORD dwToId, const std::string& sText);
}
#endif

