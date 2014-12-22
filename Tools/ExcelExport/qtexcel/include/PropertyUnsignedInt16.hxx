#ifndef _PROPERTYUNSIGNEDINT16_H
#define _PROPERTYUNSIGNEDINT16_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyUnsignedInt16 : public NSEEProperty
{
public:
	uint16 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYUNSIGNEDINT16_H  