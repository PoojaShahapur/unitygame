#ifndef COMMON_CSV_CSV_FIELD2INDEX_H
#define COMMON_CSV_CSV_FIELD2INDEX_H

#include <memory>
#include <unordered_map>

// 将字段名映射为列索引
using CsvField2Index = std::unordered_map<std::string, size_t>;
using CsvField2IndexSptr = std::shared_ptr<CsvField2Index>;

#endif  // COMMON_CSV_CSV_FIELD2INDEX_H