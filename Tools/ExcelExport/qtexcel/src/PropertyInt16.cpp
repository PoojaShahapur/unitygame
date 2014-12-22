#include "PropertyInt16.hxx"

void PropertyInt16::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeInt16(m_propData);
}