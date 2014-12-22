#ifndef _PROPERTYFLOAT_H
#define _PROPERTYFLOAT_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyFloat : public NSEEProperty
{
public:
	float m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYFLOAT_H  