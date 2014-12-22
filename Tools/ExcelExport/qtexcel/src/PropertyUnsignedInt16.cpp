#include "PropertyUnsignedInt16.hxx"

void PropertyUnsignedInt16::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeUnsignedInt16(m_propData);
}