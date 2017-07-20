#ifndef SVR_COMMON_CSV_DETAIL_CSV_SORT_TYPE_H
#define SVR_COMMON_CSV_DETAIL_CSV_SORT_TYPE_H

#include <set>

// 排序类型。如(1,3,5)表示1,3,5列组合成一个排序。
// 排序即数据库中的索引概念，为避免与列索引混淆而改称排序
using CsvSortType = std::set<size_t>;

#endif  // SVR_COMMON_CSV_DETAIL_CSV_SORT_TYPE_H
