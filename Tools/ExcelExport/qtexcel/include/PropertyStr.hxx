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

protected:
	virtual void srz2BU(ByteBuffer& byteBuffer);

public:
	// 序列化 web
	virtual void srz2BUWeb(ByteBuffer& byteBuffer);
	// 序列化移动设备
	virtual void srz2BUMobile(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYSTR_H  