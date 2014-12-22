#ifndef _TABLEATTR_H
#define _TABLEATTR_H

#include <string>
#include <vector>

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class IDRange
{
public:
	unsigned int m_startId;		// 开始范围
	unsigned int m_endId;		// 结束范围
};

/**
 * @brief xml 配置表中表的属性
 */
class TableAttr
{
public:
	std::string m_inrange;		// id 范围
	std::vector<IDRange> m_idList;	// 存放整个 id 列表

public:
	void parseInRange(std::string idrange);
	bool bIdInRange(unsigned int id);
};

ENDNAMESPACE(NSExcelExport)

#endif