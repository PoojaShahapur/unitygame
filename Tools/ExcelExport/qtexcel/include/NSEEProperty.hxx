#ifndef _NSEEPROPERTY_H
#define _NSEEPROPERTY_H

#include "ByteBuffer.hxx"
#include "Platform.hxx"

BEGINNAMESPACE(NSExcelExport)

class NSEEProperty
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

ENDNAMESPACE(NSExcelExport)

#endif	// _NSEEPROPERTY_H  