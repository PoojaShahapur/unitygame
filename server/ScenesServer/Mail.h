/*************************************************************************
 Author: wang
 Created Time: 2015��03��26�� ������ 11ʱ03��54��
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
     * \brief	�����ʼ� ���Դ��������Դ�����
     */
    void send(const std::string& sFromName, DWORD dwFromId, const std::string& sToName, DWORD dwToId,
	    BYTE type, DWORD dwMoney=0, const zObject* pObj = NULL, const std::string& sText="", 
	    const zObject* pObj1=NULL, const zObject* pObj2=NULL, const std::string& sTitle="");
    /**
     * \brief	�����ʼ� ���ʹ��ı��ʼ�
     */
    void sendSysText(const std::string& sFromName, DWORD dwToId, const std::string& sText);
}
#endif

