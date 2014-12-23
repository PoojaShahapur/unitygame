#ifndef _PROPERTYSTR_H
#define _PROPERTYSTR_H

#include "PropertyIns.hxx"
#include <string>

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyStr : public PropertyIns<std::string>
{
public:
	size_t m_cfgLen;			// 配置表中的长度

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYSTR_H  