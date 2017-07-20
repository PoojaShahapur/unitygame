#ifndef COMMON_CSV_CSV_QUERY_CONDITION_H
#define COMMON_CSV_CSV_QUERY_CONDITION_H

#include "detail/csv_field_value.h"  // for CsvFieldValue
#include "str_vec.h"  // for StrVec

#include <map>
#include <string>

// 查询的键值条件。注意double不能用作键值来查询。
class CsvQueryCondition final
{
public:
	CsvQueryCondition();

public:
	using string = std::string;

	void Add(const string& sKey, const CsvFieldValue& val)
	{
		m_kv[sKey] = val;
	}

	void Add(const string& sKey, int8_t nVal)
	{
		Add(sKey, int64_t(nVal));
	}
	void Add(const string& sKey, uint8_t nVal)
	{
		Add(sKey, int64_t(nVal));
	}
	void Add(const string& sKey, int16_t nVal)
	{
		Add(sKey, int64_t(nVal));
	}
	void Add(const string& sKey, uint16_t nVal)
	{
		Add(sKey, int64_t(nVal));
	}
	void Add(const string& sKey, int32_t nVal)
	{
		Add(sKey, int64_t(nVal));
	}
	void Add(const string& sKey, uint32_t nVal)
	{
		Add(sKey, int64_t(nVal));
	}
	void Add(const string& sKey, int64_t qwVal)
	{
		Add(sKey, CsvFieldValue(qwVal));
	}

	// 禁止double作为键值
	void Add(const string& sKey, float dVal) = delete;
	void Add(const string& sKey, double dVal) = delete;

	void Add(const string& sKey, const char* pVal)
	{
		assert(pVal);
		Add(sKey, string(pVal));
	}

	void Add(const string& sKey, const string& sVal)
	{
		Add(sKey, CsvFieldValue(sVal));
	}

	StrVec GetKeys() const;
	const CsvFieldValue& GetValue(const string& sKey) const;

private:
	// 因个数较少，不需要 unordered_map
	using KeyValMap = std::map<string, CsvFieldValue>;
	KeyValMap m_kv;
};  // class CsvQueryCondition

#endif  // COMMON_CSV_CSV_QUERY_CONDITION_H
