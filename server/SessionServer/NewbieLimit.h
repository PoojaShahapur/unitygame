/*************************************************************************
 Author: wang
 Created Time: 2014��12��23�� ���ڶ� 11ʱ53��57��
 File Name: SessionServer/NewbieLimit.h
 Description: 
 ************************************************************************/
#ifndef _NEWBIELIMIT_H
#define _NEWBIELIMIT_H

#include <string>
class NewbieLimit
{
    public:
	NewbieLimit(void);
	virtual ~NewbieLimit(void);
	static std::string getNewbieMapName(const std::string &sCountryName);
	static void enable(bool bEnable);
    private:
	static bool _bEnabled;	//default is open
};

#endif
