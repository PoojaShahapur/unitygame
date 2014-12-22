#include "PropertyUnsignedInt32.hxx"

void PropertyUnsignedInt32::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeUnsignedInt32(m_propData);
}