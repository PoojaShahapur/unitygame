#include "PropertyStr.hxx"

void PropertyStr::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeMultiByte(m_propData.c_str(), m_cfgLen);
}