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

unsigned long int DataItem::getID()
{
	return m_id;
}

void DataItem::setID(unsigned long int id)
{
	m_id = id;
}

void DataItem::writeFileServer(FILE* file)
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

	m_byteBuffer.writeFile(file);
}

void DataItem::writeFileDesktop(FILE* file)
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

	m_byteBuffer.writeFile(file);
}

void DataItem::writeFileWeb(FILE* file)
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

	m_byteBuffer.writeFile(file);
}

void DataItem::writeFileMobile(FILE* file)
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

	m_byteBuffer.writeFile(file);
}

// 将所有的内容添加到 ByteBuffer 中
void DataItem::writeByteBuffer()
{
	// 写 Property 到 ByteBuffer 
	std::vector<PropertyBase*>::iterator iteVecBeginProp;
	std::vector<PropertyBase*>::iterator iteVecEndProp;

	iteVecBeginProp = m_propVec.begin();
	iteVecEndProp = m_propVec.end();
	for (; iteVecBeginProp != iteVecEndProp; ++iteVecBeginProp)
	{
		(*iteVecBeginProp)->srz2BUMobile(m_byteBuffer);
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