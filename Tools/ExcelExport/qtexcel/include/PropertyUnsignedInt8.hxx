#ifndef _PROPERTYUNSIGNEDINT8_H
#define _PROPERTYUNSIGNEDINT8_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyUnsignedInt8 : public NSEEProperty
{
public:
	uint8 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYUNSIGNEDINT8_H  