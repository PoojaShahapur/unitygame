#include "DataItem.hxx"

DataItem::DataItem()
{
	m_id = 0;
}

DataItem::~DataItem()
{

}

ByteBuffer& DataItem::getByteBuffer()
{
	return m_data;
}

unsigned long int DataItem::getID()
{
	return m_id;
}

void DataItem::setID(unsigned long int id)
{
	m_id = id;
}

void DataItem::writeFile(FILE* file)
{
	m_data.writeFile(file);
}

std::vector<PropertyBase*>& DataItem::getPropVec()
{
	return m_propVec;
}

bool lessCmp::operator() (DataItem* a, DataItem* b) 
{ 
	return (a->getID() < b->getID());
}