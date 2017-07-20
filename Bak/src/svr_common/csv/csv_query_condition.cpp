#include "csv_query_condition.h"

CsvQueryCondition::CsvQueryCondition()
{
}

StrVec CsvQueryCondition::GetKeys() const
{
	StrVec v;
	for (const auto& kv : m_kv)
		v.emplace_back(kv.first);
	return v;
}

const CsvFieldValue& CsvQueryCondition::GetValue(const std::string& sKey) const
{
	auto itr = m_kv.find(sKey);
	if (itr != m_kv.end())
		return (*itr).second;

	assert(false);
	static CsvFieldValue s_v;
	return s_v;
}

