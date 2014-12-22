#include "PropertyInt64.hxx"

void PropertyInt64::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeInt64(m_propData);
}