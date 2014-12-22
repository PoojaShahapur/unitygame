#ifndef _PROPERTYINT64_H
#define _PROPERTYINT64_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyInt64 : public NSEEProperty
{
public:
	int64 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYINT64_H  