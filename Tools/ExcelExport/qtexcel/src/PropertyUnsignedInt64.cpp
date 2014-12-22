#include "PropertyUnsignedInt64.hxx"

void PropertyUnsignedInt64::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeUnsignedInt64(m_propData);
}