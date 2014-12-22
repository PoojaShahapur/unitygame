#ifndef _PROPERTYDOUBLE_H
#define _PROPERTYDOUBLE_H

#include "NSEEProperty.hxx"
#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class PropertyDouble : public NSEEProperty
{
public:
	double m_propData;

public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYDOUBLE_H  