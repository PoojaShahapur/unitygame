#ifndef _PROPERTYINS_H
#define _PROPERTYINS_H

#include "PropertyBase.hxx"
#include "ByteBuffer.hxx"
#include "Platform.hxx"

BEGINNAMESPACE(NSExcelExport)

// 定义除 string 之外的 CBO 
template <class T>
class PropertyIns : PropertyBase
{
public:
	T m_propData;

protected:
	virtual void srz2BU(ByteBuffer& byteBuffer)
	{
		byteBuffer.append<T>(&m_propData, 1);		// 这个地方直接使用
	}

public:
	// 序列化服务器
	virtual void srz2BUServer(ByteBuffer& byteBuffer)
	{
		srz2BU(byteBuffer);
	}

	// 序列化桌面客户端
	virtual void srz2BUDesktop(ByteBuffer& byteBuffer)
	{
		srz2BU(byteBuffer);
	}

	// 序列化 web
	virtual void srz2BUWeb(ByteBuffer& byteBuffer)
	{
		srz2BU(byteBuffer);
	}

	// 序列化移动设备
	virtual void srz2BUMobile(ByteBuffer& byteBuffer)
	{
		srz2BU(byteBuffer);
	}
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYINS_H 