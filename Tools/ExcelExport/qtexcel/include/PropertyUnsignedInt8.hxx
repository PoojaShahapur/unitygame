#ifndef _PROPERTYUNSIGNEDINT8_H
#define _PROPERTYUNSIGNEDINT8_H

#include "Property.hxx"

class PropertyUnsignedInt8 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYUNSIGNEDINT8_H  