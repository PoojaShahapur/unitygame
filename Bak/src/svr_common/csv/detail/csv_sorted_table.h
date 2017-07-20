#ifndef SVR_COMMON_CSV_DETAIL_CSV_SORTED_TABLE_H
#define SVR_COMMON_CSV_DETAIL_CSV_SORTED_TABLE_H

#include "csv_field_value.h"  // for CsvFieldValue
#include "csv/csv_record_sptr_vec.h"  // for CsvRecordSptrVec
#include "csv_sort_type.h"  // for CsvSortType

#include <vector>
#include <map>

// 表索引，用于快速查询
class CsvSortedTable final
{
public:
	CsvSortedTable(const CsvSortType& sortType,
		const CsvRecordSptrVec vRecord);

public:
	// Csv表排序的键.
	using SortKey = std::vector<CsvFieldValue>;
	const CsvRecordSptrVec& GetRecords(const SortKey& sortKey) const;

private:
	using KeyToRecords = std::map<SortKey, CsvRecordSptrVec>;
	KeyToRecords m_keyToRecords;
};  // class CsvSortedTable

#endif  // SVR_COMMON_CSV_DETAIL_CSV_SORTED_TABLE_H
