#ifndef COMMON_CSV_CSV_VALUE_TYPE_H
#define COMMON_CSV_CSV_VALUE_TYPE_H

#include <vector>

enum class CsvValueType : int
{
	Illegal,
	Int64,
	Double,
	String,
	Max,
};

using CsvValueTypeVec = std::vector<CsvValueType>;

#endif  // COMMON_CSV_CSV_VALUE_TYPE_H
