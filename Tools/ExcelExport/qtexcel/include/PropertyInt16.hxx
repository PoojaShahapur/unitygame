#ifndef _PROPERTYINT16_H
#define _PROPERTYINT16_H

#include "Property.hxx"

class PropertyInt16 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYINT16_H  