#ifndef COMMON_CSV_CSV_CFG_IMPL_H
#define COMMON_CSV_CSV_CFG_IMPL_H

#include "csv/csv_table_sptr.h"  // for CsvTableSptr 

#include <unordered_map>
#include <memory>

namespace boost {
namespace filesystem {
class path;
}}  // namespace boost::filesystem

class CsvCfgImpl
{
public:
	CsvCfgImpl();
	virtual ~CsvCfgImpl();

public:
	// 初始化加载Csv, 并设置定时热更新。Csv有错则返回false.
	bool Init();

	// 检查是否有更新，有则重新加载. 加载错误则返回false.
	bool Reload();

public:
	// 因为Table查询时会排序，所以不是const
	CsvTable& GetTable(const std::string& sTablePath);

private:
	using path = boost::filesystem::path;
	// 重新加载文件，或重新加载子目录，其下所有子目录会递归加载
	bool Reload(const path& p);
	bool ReloadDir(const path& p);
	bool ReloadFile(const path& p);
	bool NeedReload(const std::string& sKey, time_t tFile) const;

private:
	struct TimeAndCsv
	{
		time_t tFile;
		CsvTableSptr pTable;
	};
	// 以相对于"../csv"的文件名为键，如 "test/test.csv"。
	using CsvMap = std::unordered_map<std::string, TimeAndCsv>;
	CsvMap m_mapCsv;
    bool m_isInitLoad;//是否是初始化启动
};  // class CsvCfgImpl

#endif  // COMMON_CSV_CSV_CFG_IMPL_H
