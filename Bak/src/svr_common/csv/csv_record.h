#ifndef COMMON_CSV_CSV_RECORD_H
#define COMMON_CSV_CSV_RECORD_H

#include "csv_record_sptr.h"
#include "detail/csv_field_value.h"  // for CsvFieldValue
#include "detail/csv_field2index.h"  // for CsvField2IndexSptr
#include "detail/csv_value_type.h"  // for CsvValueTypeVec
#include "str_vec.h"  // for StrVec

#include <string>
#include <type_traits>  // for is_integral
#include <unordered_map>

enum class CsvValueType : int;

// 对应一个csv表中的一行记录。
class CsvRecord
{
public:
	explicit CsvRecord(const CsvField2IndexSptr& pField2Index);
	virtual ~CsvRecord();

public:
	bool Parse(const CsvValueTypeVec& vType,
		const StrVec& vStr, size_t iRow);

public:
	int64_t GetInt64(const std::string& sField) const;

	template <class IntType>
	inline IntType GetInt(const std::string& sField) const;

	bool GetBool(const std::string& sField) const;

	double GetDouble(const std::string& sField) const;
	const std::string& GetString(const std::string& sField) const;

public:
	// 用于排序时读取键值
	const CsvFieldValue& GetValue(size_t index) const;

	const CsvFieldValue& GetValue(const std::string& sField) const;

private:
	bool SetValue(size_t index, const std::string& sValue, CsvValueType type);

private:
	static CsvFieldValue s_empty;

private:
	const CsvField2IndexSptr m_pField2Index;

	using VarVec = std::vector<CsvFieldValue>;
	VarVec m_vVar;
};  // class CsvRecord

template <class IntType>
inline IntType CsvRecord::GetInt(const std::string& sField) const
{
	static_assert(std::is_integral<IntType>::value, "Integer required.");
	return static_cast<IntType>(GetInt64(sField));
}

template <>
inline bool CsvRecord::GetInt<bool>(const std::string& sField) const
{
	static_assert(std::is_integral<bool>::value, "Integer required.");
	return 0 != GetInt64(sField);
}

#endif  // COMMON_CSV_CSV_RECORD_H
