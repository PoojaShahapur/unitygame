#include "PropertyDouble.hxx"

void PropertyDouble::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeDouble(m_propData);
}