#ifndef _PROPERTYFLOAT_H
#define _PROPERTYFLOAT_H

#include "Property.hxx"

class PropertyFloat : public Property
{
public:
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYFLOAT_H  