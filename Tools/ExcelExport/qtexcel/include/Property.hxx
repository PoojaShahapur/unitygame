#ifndef _PROPERTY_H
#define _PROPERTY_H

#include "ByteBuffer.hxx"

class Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTY_H  