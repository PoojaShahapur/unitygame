#ifndef _PROPERTYSTR_H
#define _PROPERTYSTR_H

#include "Property.hxx"

class PropertyStr : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYSTR_H  