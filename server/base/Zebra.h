#ifndef __Zebra_h_
#define __Zebra_h_
#include "zLogger.h"
#include "zProperties.h"
#include "zVersion.h"
namespace Zebra
{
	/**
	* \brief ��Ϸʱ��
	*
	*/
	extern volatile QWORD qwGameTime;

	/**
	* \brief ��־ָ��
	*
	*/
	extern zLogger *logger;

	/**
	* \brief ��ȡȫ�ֱ���������
	*
	*/
	extern zProperties global;
};

#endif

