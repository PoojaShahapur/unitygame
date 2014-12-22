#ifndef _PROPERTYDOUBLE_H
#define _PROPERTYDOUBLE_H

#include "Property.hxx"

class PropertyDouble : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYDOUBLE_H  