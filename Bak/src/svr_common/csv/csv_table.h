#ifndef COMMON_CSV_CSV_TABLE_H
#define COMMON_CSV_CSV_TABLE_H

#include "csv_query_condition.h"  // for CsvQueryCondition
#include "csv_record_sptr_vec.h"  // for CsvRecordSptrVec
#include "detail/csv_field2index.h"  // for CsvField2IndexSptr
#include "detail/csv_sorted_table_sptr.h"  // for CsvSortedTableSptr
#include "detail/csv_value_type.h"  // for CsvValueTypeVec
#include "detail/csv_sort_type.h"  // for CsvSortType
#include "str_vec.h"  // for StrVec

#include <boost/core/noncopyable.hpp>

#include <cassert>
#include <map>
#include <memory>  // for shared_ptr<>
#include <set>
#include <string>
#include <vector>

class CsvRecord;

// 对应一个csv表。
class CsvTable : boost::noncopyable
{
public:
	CsvTable();
	virtual ~CsvTable();

public:
	bool Load(const std::string& sCsvPath);

public:
	size_t GetRecordCount() const { return m_vRecord.size(); }
	size_t GetFieldCount() const { return m_vType.size(); }
	StrVec GetFields() const;
	const CsvRecordSptrVec& GetRecords() const { return m_vRecord; }

	const CsvRecord& GetRecord(size_t index) const
	{
		assert(index <= m_vRecord.size());
		return *m_vRecord[index];
	}

public:
	// 查询接口。首次查询会自动建立索引。注意double字段不能作为键值, 不然编译错误：
	// error C2280: “CsvQueryCondition::CsvQueryCondition(const std::string &,double)”: 尝试引用已删除的函数
	template<class ValType>
	CsvRecordSptrVec GetRecords(
		const std::string& sKey, const ValType& val);
	template<class ValType1, class ValType2>
	CsvRecordSptrVec GetRecords(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2);
	template<class ValType1, class ValType2, class ValType3>
	CsvRecordSptrVec GetRecords(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2,
		const std::string& sKey3, const ValType3& val3);
	template<class ValType1, class ValType2, class ValType3, class ValType4>
	CsvRecordSptrVec GetRecords(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2,
		const std::string& sKey3, const ValType3& val3,
		const std::string& sKey4, const ValType4& val4);
	template<class ValType1, class ValType2, class ValType3, class ValType4, class ValType5>
	CsvRecordSptrVec GetRecords(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2,
		const std::string& sKey3, const ValType3& val3,
		const std::string& sKey4, const ValType4& val4,
		const std::string& sKey5, const ValType5& val5);

	// 更多的条件可使用 CsvQueryCondition.
	// 只能查询相等条件，其他条件只能GetRecords()后遍历。
	CsvRecordSptrVec GetRecords(const CsvQueryCondition& condition);

	// 返回第一条记录。如不存在则返回空记录。判断是否存在记录请用 GetRecords().empty()
	template<class ValType>
	const CsvRecord& GetRecord(
		const std::string& sKey, const ValType& val);
	template<class ValType1, class ValType2>
	const CsvRecord& GetRecord(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2);
	template<class ValType1, class ValType2, class ValType3>
	const CsvRecord& GetRecord(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2,
		const std::string& sKey3, const ValType3& val3);
	template<class ValType1, class ValType2, class ValType3, class ValType4>
	const CsvRecord& GetRecord(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2,
		const std::string& sKey3, const ValType3& val3,
		const std::string& sKey4, const ValType4& val4);
	template<class ValType1, class ValType2, class ValType3, class ValType4, class ValType5>
	const CsvRecord& GetRecord(
		const std::string& sKey1, const ValType1& val1,
		const std::string& sKey2, const ValType2& val2,
		const std::string& sKey3, const ValType3& val3,
		const std::string& sKey4, const ValType4& val4,
		const std::string& sKey5, const ValType5& val5);

	// 更多的条件可使用 CsvQueryCondition.
	// 只能查询相等条件，其他条件只能GetRecords()后遍历。
	const CsvRecord& GetRecord(const CsvQueryCondition& condition);

private:
	bool ParseFieldNames(const StrVec& vField);
	bool ParseTypeRow(const StrVec& vTypeStr);
	bool ParseRecordRow(const StrVec& vVal, size_t iRow);
	bool SetType(const StrVec& vTypeStr, size_t index);
	const CsvSortedTable& GetSortedTable(const CsvSortType& sortType);
	void Reset();

private:
	CsvRecordSptrVec m_vRecord;

	CsvField2IndexSptr m_pField2Index;  // 列名转列索引
	CsvValueTypeVec m_vType;  // 列类型

	using Type2Sorted = std::map<CsvSortType, CsvSortedTableSptr>;
	Type2Sorted m_type2Sorted;  // 动态生成的各种排序表，用于快速查询。
};  // class CsvTable

template<class ValType>
CsvRecordSptrVec CsvTable::GetRecords(
	const std::string& sKey, const ValType& val)
{
	CsvQueryCondition c;
	c.Add(sKey, val);
	return GetRecords(c);
}

template<class ValType1, class ValType2>
CsvRecordSptrVec CsvTable::GetRecords(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	return GetRecords(c);
}

template<class ValType1, class ValType2, class ValType3>
CsvRecordSptrVec CsvTable::GetRecords(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2,
	const std::string& sKey3, const ValType3& val3)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	c.Add(sKey3, val3);
	return GetRecords(c);
}

template<class ValType1, class ValType2, class ValType3, class ValType4>
CsvRecordSptrVec CsvTable::GetRecords(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2,
	const std::string& sKey3, const ValType3& val3,
	const std::string& sKey4, const ValType4& val4)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	c.Add(sKey3, val3);
	c.Add(sKey4, val4);
	return GetRecords(c);
}

template<class ValType1, class ValType2, class ValType3, class ValType4, class ValType5>
CsvRecordSptrVec CsvTable::GetRecords(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2,
	const std::string& sKey3, const ValType3& val3,
	const std::string& sKey4, const ValType4& val4,
	const std::string& sKey5, const ValType5& val5)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	c.Add(sKey3, val3);
	c.Add(sKey4, val4);
	c.Add(sKey5, val5);
	return GetRecords(c);
}

template<class ValType>
const CsvRecord& CsvTable::GetRecord(
	const std::string& sKey, const ValType& val)
{
	CsvQueryCondition c;
	c.Add(sKey, val);
	return GetRecord(c);
}

template<class ValType1, class ValType2>
const CsvRecord& CsvTable::GetRecord(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	return GetRecord(c);
}

template<class ValType1, class ValType2, class ValType3>
const CsvRecord& CsvTable::GetRecord(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2,
	const std::string& sKey3, const ValType3& val3)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	c.Add(sKey3, val3);
	return GetRecord(c);
}

template<class ValType1, class ValType2, class ValType3, class ValType4>
const CsvRecord& CsvTable::GetRecord(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2,
	const std::string& sKey3, const ValType3& val3,
	const std::string& sKey4, const ValType4& val4)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	c.Add(sKey3, val3);
	c.Add(sKey4, val4);
	return GetRecord(c);
}

template<class ValType1, class ValType2, class ValType3, class ValType4, class ValType5>
const CsvRecord& CsvTable::GetRecord(
	const std::string& sKey1, const ValType1& val1,
	const std::string& sKey2, const ValType2& val2,
	const std::string& sKey3, const ValType3& val3,
	const std::string& sKey4, const ValType4& val4,
	const std::string& sKey5, const ValType5& val5)
{
	CsvQueryCondition c;
	c.Add(sKey1, val1);
	c.Add(sKey2, val2);
	c.Add(sKey3, val3);
	c.Add(sKey4, val4);
	c.Add(sKey5, val5);
	return GetRecord(c);
}

#endif  // COMMON_CSV_CSV_TABLE_H
