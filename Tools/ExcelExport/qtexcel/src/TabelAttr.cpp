#include "TabelAttr.hxx"

// 解析数据，结构是 "1-100;130-500"
void TableAttr::parseInRange(std::string idrange)
{
	// 清理之前的内容
	m_idList.clear();

	if(idrange.length() == 0)
	{
		return;
	}

	int idxpre = 0;	// 前一个索引
	int idxcur = 0;	// 当前索引
	int idxFenGeFu = 0;	// "-" 分隔符索引
	IDRange* pidrange;
	std::string substring;
	idxcur = idrange.find_first_of(";", 0);

	while(idxcur != -1)
	{
		substring = idrange.substr(idxpre, idxcur - idxpre);
		// 解析子字符串
		idxFenGeFu = substring.find_first_of("-", 0);
		m_idList.push_back(IDRange());
		pidrange = &m_idList.back();
		pidrange->m_startId = atoi(substring.substr(0, idxFenGeFu).c_str());
		pidrange->m_endId = atoi(substring.substr(idxFenGeFu + 1, substring.length()).c_str());

		idxpre = idxcur + 1;
		idxcur = idrange.find_first_of(";", idxcur + 1);
	}

	// 把最后一个解析了
	substring = idrange.substr(idxpre, idrange.length());

	idxFenGeFu = substring.find_first_of("-", 0);
	m_idList.push_back(IDRange());
	pidrange = &m_idList.back();
	pidrange->m_startId = atoi(substring.substr(0, idxFenGeFu).c_str());
	pidrange->m_endId = atoi(substring.substr(idxFenGeFu + 1, substring.length()).c_str());
}

bool TableAttr::bIdInRange(unsigned int id)
{
	std::vector<IDRange>::iterator itBeg = m_idList.begin();
	std::vector<IDRange>::iterator itEnd = m_idList.end();
	if(itBeg != itEnd)	// 如果配置 id 段
	{
		while(itBeg != itEnd)
		{
			if(itBeg->m_startId <= id && id <= itBeg->m_endId)
			{
				return true;
			}
			++itBeg;
		}

		return false;		// 如果配置了 id 段，并且没有在 id 段内，就返回 false
	}
	else	// 如果没有配置 id 段，就肯定打包进入
	{
		return true;
	}
}