#ifndef _PROPERTYBASE_H
#define _PROPERTYBASE_H

#include "ByteBuffer.hxx"
#include "Platform.hxx"

BEGINNAMESPACE(NSExcelExport)

class PropertyBase
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYBASE_H  