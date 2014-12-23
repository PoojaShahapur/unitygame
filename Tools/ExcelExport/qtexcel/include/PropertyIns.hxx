#ifndef _PROPERTYINS_H
#define _PROPERTYINS_H

#include "PropertyBase.hxx"
#include "ByteBuffer.hxx"
#include "Platform.hxx"

BEGINNAMESPACE(NSExcelExport)

template <class T>
class PropertyIns : PropertyBase
{
public:
	T m_propData;
public:
	virtual void srz2BU(ByteBuffer& byteBuffer)
	{
		byteBuffer.append<T>(&m_propData, 1);		// 这个地方直接使用
	}
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYINS_H 