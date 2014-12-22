#ifndef _DATAITEM_H
#define _DATAITEM_H

#include <vector>
#include "ByteBuffer.hxx"

/**
 * @brief 数据的一行，存储字节流    
 */

class DataItem
{
protected:
	unsigned long int m_id;		// 唯一 ID
	ByteBuffer m_data;			// 字节缓冲区

public:
	DataItem();
	~DataItem();

	unsigned long int getID();
	void setID(unsigned long int id);
	ByteBuffer& getByteBuffer();
	void writeFile(FILE* file);
};

struct lessCmp 
{
	bool operator() (DataItem* a, DataItem* b);
};

#endif	// DATAITEM_H