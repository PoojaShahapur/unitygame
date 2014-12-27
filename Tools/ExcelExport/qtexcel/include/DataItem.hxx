#ifndef _DATAITEM_H
#define _DATAITEM_H

#include <vector>
#include "ByteBuffer.hxx"
#include "PropertyBase.hxx"

#include "Platform.hxx"
BEGIN_NAMESPACE

/**
 * @brief 数据的一行，存储字节流    
 */

class DataItem
{
protected:
	unsigned int m_id;		// 唯一 ID
	ByteBuffer m_byteBuffer;			// 字节缓冲区
	std::vector<PropertyBase*> m_propVec;		// 属性向量

public:
	DataItem();
	~DataItem();

	unsigned int getID();
	void setID(unsigned long int id);
	ByteBuffer& getByteBuffer();
	void writeByteBuffer(bool ignoreID);
	void writeFileServer(ByteBuffer& byteBuffer);
	void writeFileDesktop(ByteBuffer& byteBuffer);
	void writeFileWeb(ByteBuffer& byteBuffer);
	void writeFileMobile(ByteBuffer& byteBuffer);
	std::vector<PropertyBase*>& getPropVec();
};

struct lessCmp 
{
	bool operator() (DataItem* a, DataItem* b);
};

END_NAMESPACE

#endif	// DATAITEM_H