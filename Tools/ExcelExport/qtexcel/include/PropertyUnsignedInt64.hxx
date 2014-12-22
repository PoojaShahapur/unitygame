#ifndef _PROPERTYUNSIGNEDINT64_H
#define _PROPERTYUNSIGNEDINT64_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyUnsignedInt64 : public NSEEProperty
{
public:
	uint64 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYUNSIGNEDINT64_H  