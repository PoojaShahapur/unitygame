#ifndef _PROPERTYUNSIGNEDINT32_H
#define _PROPERTYUNSIGNEDINT32_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyUnsignedInt32 : public NSEEProperty
{
public:
	uint32 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYUNSIGNEDINT32_H  