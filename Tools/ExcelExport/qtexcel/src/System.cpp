#include "System.hxx"

template<> System* Singleton<System>::msSingleton = 0;

void System::checkEndian()
{
	int testData = 1;
	char* pChar = (char *)&testData;
	if (pChar[0] == 1)
	{
		m_sysEndian = eSys_BIG_ENDIAN;
	}
	else
	{
		m_sysEndian = eSys_LITTLE_ENDIAN;
	}
}

bool System::isEndianDiffFromSys(SysEndian rhv)
{
	return m_sysEndian != rhv;
}