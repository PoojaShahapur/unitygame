#include "csv_record.h"

#include "log.h"

#include <boost/algorithm/string.hpp>  // for trim_copy()
#include <boost/lexical_cast.hpp>

const char LOG_NAME[] = "CsvRecord";

CsvFieldValue CsvRecord::s_empty;

CsvRecord::CsvRecord(const CsvField2IndexSptr& pField2Index)
	: m_pField2Index(pField2Index)
{
	assert(pField2Index);
	m_vVar.resize(pField2Index->size());
}

CsvRecord::~CsvRecord()
{
}

static std::string GetTypeStr(CsvValueType type)
{
	if (CsvValueType::Int64 == type)
		return "int";
	if (CsvValueType::Double == type)
		return "float";
	return "string";
}

// iRow是行索引，仅用于日志。
bool CsvRecord::Parse(const CsvValueTypeVec& vType,
	const StrVec& vStr, size_t iRow)
{
	bool ok = true;
	size_t uSize = m_pField2Index->size();
	assert(uSize == m_vVar.size());
	assert(uSize == vType.size());
	StrVec vStr2 = vStr;
	vStr2.resize(uSize);
	for (size_t i = 0; i < uSize; ++i)
	{
		const std::string& sValue = vStr[i];
		CsvValueType type = vType[i];
		if (SetValue(i, sValue, type))
			continue;

		LOG_ERROR(Fmt("Illegal grid[%1%][%2%] value '%3%' (type: %4%)")
			% (iRow + 1) % (i + 1) % sValue % GetTypeStr(type));
		ok = false;
	}
	return ok;
}

int64_t CsvRecord::GetInt64(const std::string& sField) const
{
	const CsvFieldValue& v = GetValue(sField);
	const int64_t* p = boost::get<int64_t>(&v);
	if (p) return *p;
	return 0;
}

bool CsvRecord::GetBool(const std::string& sField) const
{
	return GetInt<bool>(sField);
}

double CsvRecord::GetDouble(const std::string& sField) const
{
	const CsvFieldValue& v = GetValue(sField);
	const double* p = boost::get<double>(&v);
	if (p) return *p;
	return 0.0;
}

const std::string& CsvRecord::GetString(const std::string& sField) const
{
	const CsvFieldValue& v = GetValue(sField);
	const std::string* p = boost::get<std::string>(&v);
	if (p) return *p;
	static std::string s_empty;
	return s_empty;
}

const CsvFieldValue& CsvRecord::GetValue(size_t index) const
{
	if (index < m_vVar.size())
		return m_vVar[index];
	static CsvFieldValue s_v;
	return s_v;
}

bool CsvRecord::SetValue(size_t index,
	const std::string& sValue, CsvValueType type)
{
	CsvFieldValue& rV = m_vVar[index];
	if (CsvValueType::Int64 != type &&
		CsvValueType::Double != type)
	{
		rV = sValue;
		return true;
	}

	using boost::algorithm::trim_copy;
	std::string sValue2 = trim_copy(sValue);
	bool isInt = (CsvValueType::Int64 == type);

	// 设置缺省值
	if (isInt)
		rV = int64_t(0);
	else
		rV = double(0.0);

	if (sValue2.empty())
		return true;  // 允许缺省

	using boost::lexical_cast;
	try
	{
		if (CsvValueType::Int64 == type)
			rV = lexical_cast<int64_t>(sValue2);
		else
			rV = lexical_cast<double>(sValue2);
	}
	catch(const boost::bad_lexical_cast &)
	{
		return false;  // And use default value 0.
	}
	return true;
}

const CsvFieldValue& CsvRecord::GetValue(const std::string& sField) const
{
	if (!m_pField2Index) return s_empty;
	const CsvField2Index& mapFld2Idx = *m_pField2Index;
	auto itr = mapFld2Idx.find(sField);
	if (itr == mapFld2Idx.end()) return s_empty;
	size_t idx = (*itr).second;
	return m_vVar[idx];
}

