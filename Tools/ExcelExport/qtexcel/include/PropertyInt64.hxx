#ifndef _PROPERTYINT64_H
#define _PROPERTYINT64_H

#include "Property.hxx"

class PropertyInt64 : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYINT64_H  