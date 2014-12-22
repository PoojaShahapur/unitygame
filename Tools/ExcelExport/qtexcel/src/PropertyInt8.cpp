#include "PropertyInt8.hxx"

void PropertyInt8::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeInt8(m_propData);
}