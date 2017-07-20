#ifndef COMMON_CSV_CSV_CFG_H
#define COMMON_CSV_CSV_CFG_H

#include <string>

class CsvTable;

namespace CsvCfg {

// 初始化加载Csv, 并设置定时热更新。Csv有错则返回false.
bool Init();

// 获取已加载的Csv表。
// 返回引用，使用者不应该保存该引用，因为热更新后就会失效。
// sTablePath为csv文件路径，相对于 ../csv 根目录，如： test/test.csv.
// 注意大小写，目录分隔符必须为'/'.
// 无此表则返回空表。
// 因为查询时会有排序，所以不能是const.
CsvTable& GetTable(const std::string& sTablePath);

}  // namespace CsvCfg

#endif  // COMMON_CSV_CSV_CFG_H
