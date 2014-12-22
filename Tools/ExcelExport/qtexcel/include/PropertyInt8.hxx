#ifndef _PROPERTYINT8_H
#define _PROPERTYINT8_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyInt8 : public NSEEProperty
{
public:
	int8 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYINT8_H  