#ifndef _PROPERTYUNSIGNEDINT32_H
#define _PROPERTYUNSIGNEDINT32_H

#include "Property.hxx"

class PropertyUnsignedInt32 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYUNSIGNEDINT32_H  