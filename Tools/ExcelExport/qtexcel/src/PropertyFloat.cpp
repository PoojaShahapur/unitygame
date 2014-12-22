#include "PropertyFloat.hxx"

void PropertyFloat::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeFloat(m_propData);
}