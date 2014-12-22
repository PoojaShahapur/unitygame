#ifndef _PROPERTYINT32_H
#define _PROPERTYINT32_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyInt32 : public NSEEProperty
{
public:
	int32 m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYINT32_H  