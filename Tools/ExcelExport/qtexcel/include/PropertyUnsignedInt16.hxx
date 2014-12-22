#ifndef _PROPERTYUNSIGNEDINT16_H
#define _PROPERTYUNSIGNEDINT16_H

#include "Property.hxx"

class PropertyUnsignedInt16 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYUNSIGNEDINT16_H  