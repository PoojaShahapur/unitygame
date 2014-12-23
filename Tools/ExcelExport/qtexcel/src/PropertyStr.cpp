#include "PropertyStr.hxx"

void PropertyStr::srz2BU(ByteBuffer& byteBuffer)
{
	byteBuffer.writeMultiByte(m_propData.c_str(), m_cfgLen);
}

void PropertyStr::srz2BUWeb(ByteBuffer& byteBuffer)
{
	if (m_cfgLen >= m_propData.size())
	{
		byteBuffer.writeUnsignedInt16(m_propData.size());	// 写入实际长度
		byteBuffer.writeMultiByte(m_propData.c_str(), m_propData.size());
	}
	else
	{
		byteBuffer.writeUnsignedInt16(m_cfgLen);	// 写入实际长度
		byteBuffer.writeMultiByte(m_propData.c_str(), m_cfgLen);
	}
}

void PropertyStr::srz2BUMobile(ByteBuffer& byteBuffer)
{
	srz2BUWeb(byteBuffer);
}