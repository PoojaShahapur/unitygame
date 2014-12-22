#include "PropertyInt32.hxx"

void PropertyInt32::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeInt32(m_propData);
}