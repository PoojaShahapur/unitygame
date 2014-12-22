#ifndef _PROPERTYUNSIGNEDINT64_H
#define _PROPERTYUNSIGNEDINT64_H

#include "Property.hxx"

class PropertyUnsignedInt64 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYUNSIGNEDINT64_H  