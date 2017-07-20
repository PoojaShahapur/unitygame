#ifndef COMMON_CSV_DETAIL_CSV_FIELD_VALUE_H
#define COMMON_CSV_DETAIL_CSV_FIELD_VALUE_H

#include <boost/variant.hpp>
#include <string>

using CsvFieldValue = boost::variant<int64_t, double, std::string>;

#endif  // COMMON_CSV_DETAIL_CSV_FIELD_VALUE_H