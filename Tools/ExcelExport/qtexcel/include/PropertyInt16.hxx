#ifndef _PROPERTYINT16_H
#define _PROPERTYINT16_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyInt16 : public NSEEProperty
{
public:
	int16 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYINT16_H  