#include "csv_sorted_table.h"

#include "csv/csv_record.h"  // for CsvRecord

void GetSortKey(const CsvRecord& rec,
	const CsvSortType& sortType,
	CsvSortedTable::SortKey& rSortKey)
{
	assert(rSortKey.empty());
	for (const auto idx : sortType)  // idx 是从小到大排列的
	{
		const CsvFieldValue& v = rec.GetValue(idx);
		rSortKey.emplace_back(v);
	}
}

CsvSortedTable::CsvSortedTable(const CsvSortType& sortType,
	const CsvRecordSptrVec vRecord)
{
	for (auto pRec : vRecord)
	{
		if (!pRec) continue;
		SortKey sortKey;
		GetSortKey(*pRec, sortType, sortKey);
		m_keyToRecords[sortKey].emplace_back(pRec);
	}
}

const CsvRecordSptrVec& CsvSortedTable::GetRecords(
	const SortKey& sortKey) const
{
	auto itr = m_keyToRecords.find(sortKey);
	if (itr != m_keyToRecords.end())
		return (*itr).second;

	static CsvRecordSptrVec s_v{};
	return s_v;
}

