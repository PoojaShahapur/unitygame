#ifndef _PROPERTYBOOL_H
#define _PROPERTYBOOL_H

#include "Property.hxx"

class PropertyBool : public Property
{
public:
	// ���л��� ByteBuffer 
	virtual void srz2BU(ByteBuffer& byteBuffer);
};

#endif	// _PROPERTYBOOL_H  