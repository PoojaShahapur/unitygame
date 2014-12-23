#ifndef _DATAITEM_H
#define _DATAITEM_H

#include <vector>
#include "ByteBuffer.hxx"
#include "PropertyBase.hxx"

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

/**
 * @brief 数据的一行，存储字节流    
 */

class DataItem
{
protected:
	unsigned long int m_id;		// 唯一 ID
	ByteBuffer m_data;			// 字节缓冲区
	std::vector<PropertyBase*> m_propVec;		// 属性向量

public:
	DataItem();
	~DataItem();

	unsigned long int getID();
	void setID(unsigned long int id);
	ByteBuffer& getByteBuffer();
	void writeFileServer(FILE* file);
	void writeFileDesktop(FILE* file);
	void writeFileWeb(FILE* file);
	void writeFileMobile(FILE* file);
	std::vector<PropertyBase*>& getPropVec();
};

struct lessCmp 
{
	bool operator() (DataItem* a, DataItem* b);
};

ENDNAMESPACE(NSExcelExport)

#endif	// DATAITEM_H