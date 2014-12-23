#include "DataItem.hxx"

DataItem::DataItem()
{
	m_id = 0;
}

DataItem::~DataItem()
{
	std::vector<PropertyBase*>::iterator iteVecBeginProp;
	std::vector<PropertyBase*>::iterator iteVecEndProp;

	iteVecBeginProp = m_propVec.begin();
	iteVecEndProp = m_propVec.end();
	for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	{
		delete (*iteVecBeginProp);
	}
	m_propVec.clear();
}

ByteBuffer& DataItem::getByteBuffer()
{
	return m_byteBuffer;
}

unsigned int DataItem::getID()
{
	return m_id;
}

void DataItem::setID(unsigned long int id)
{
	m_id = id;
}

void DataItem::writeFileServer(ByteBuffer& byteBuffer)
{
	// 写 Property 到 ByteBuffer 
	std::vector<PropertyBase*>::iterator iteVecBeginProp;
	std::vector<PropertyBase*>::iterator iteVecEndProp;

	iteVecBeginProp = m_propVec.begin();
	iteVecEndProp = m_propVec.end();
	for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	{
		(*iteVecBeginProp)->srz2BUServer(m_byteBuffer);
	}

	byteBuffer.append(m_byteBuffer);
}

void DataItem::writeFileDesktop(ByteBuffer& byteBuffer)
{
	// 写 Property 到 ByteBuffer 
	std::vector<PropertyBase*>::iterator iteVecBeginProp;
	std::vector<PropertyBase*>::iterator iteVecEndProp;

	iteVecBeginProp = m_propVec.begin();
	iteVecEndProp = m_propVec.end();
	for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	{
		(*iteVecBeginProp)->srz2BUDesktop(m_byteBuffer);
	}

	byteBuffer.append(m_byteBuffer);
}

void DataItem::writeFileWeb(ByteBuffer& byteBuffer)
{
	// 写 Property 到 ByteBuffer 
	std::vector<PropertyBase*>::iterator iteVecBeginProp;
	std::vector<PropertyBase*>::iterator iteVecEndProp;

	iteVecBeginProp = m_propVec.begin();
	iteVecEndProp = m_propVec.end();
	for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	{
		(*iteVecBeginProp)->srz2BUWeb(m_byteBuffer);
	}

	byteBuffer.append(m_byteBuffer);
}

void DataItem::writeFileMobile(ByteBuffer& byteBuffer)
{
	// 写 Property 到 ByteBuffer 
	//std::vector<PropertyBase*>::iterator iteVecBeginProp;
	//std::vector<PropertyBase*>::iterator iteVecEndProp;

	//iteVecBeginProp = m_propVec.begin();
	//iteVecEndProp = m_propVec.end();
	//for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	//{
	//	(*iteVecBeginProp)->srz2BUMobile(m_data);
	//}

	byteBuffer.append(m_byteBuffer);
}

// 将所有的内容添加到 ByteBuffer 中
// ignoreID 是否写入 id 字段
void DataItem::writeByteBuffer(bool ignoreID)
{
	// 写 Property 到 ByteBuffer 
	std::vector<PropertyBase*>::iterator iteVecBeginProp;
	std::vector<PropertyBase*>::iterator iteVecEndProp;

	iteVecBeginProp = m_propVec.begin();
	iteVecEndProp = m_propVec.end();
	for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	{
		if (ignoreID)
		{
			if (!(*iteVecBeginProp)->m_isIDField)
			{
				(*iteVecBeginProp)->srz2BUMobile(m_byteBuffer);
			}
		}
		else
		{
			(*iteVecBeginProp)->srz2BUMobile(m_byteBuffer);
		}
	}
}

std::vector<PropertyBase*>& DataItem::getPropVec()
{
	return m_propVec;
}

bool lessCmp::operator() (DataItem* a, DataItem* b) 
{ 
	return (a->getID() < b->getID());
}