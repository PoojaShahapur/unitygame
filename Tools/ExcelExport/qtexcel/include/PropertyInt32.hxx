#ifndef _PROPERTYINT32_H
#define _PROPERTYINT32_H

#include "Property.hxx"

class PropertyInt32 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYINT32_H  