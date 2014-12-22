#include "PropertyUnsignedInt8.hxx"

void PropertyUnsignedInt8::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeUnsignedInt8(m_propData);
}