#ifndef _PROPERTYINT8_H
#define _PROPERTYINT8_H

#include "Property.hxx"

class PropertyInt8 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYINT8_H  