#ifndef _PROPERTYSTR_H
#define _PROPERTYSTR_H

#include "NSEEProperty.hxx"
#include <string>

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyStr : public NSEEProperty
{
public:
	std::string m_propData;
	size_t m_cfgLen;			// 配置表中的长度

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYSTR_H  