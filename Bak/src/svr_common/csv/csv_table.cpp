#include "csv_table.h"

#include "csv_record.h"  // for CsvRecord
#include "detail/csv_parser.h"  // for CCsvParser
#include "detail/csv_sorted_table.h"  // for SortKey
#include "log.h"
#include "str_vec.h"  // for StrVec

const char LOG_NAME[] = "CsvTable";

CsvTable::CsvTable() {};
CsvTable::~CsvTable() {};

bool CsvTable::Load(const std::string& sPath)
{
	Reset();

	CCsvParser parser;
	parser.Parse(sPath);  // 无返回
	size_t uRowCnt = parser.GetRowCount();
	if (3 > uRowCnt)
	{
		LOG_ERROR(Fmt("Illegal csv (row = %1%). %2% ") % uRowCnt % sPath);
		return false;
	}

	// 0行: 说明，忽略
	// 1行: 列名
	// 2行: 类型
	bool ok = true;
	ok &= ParseFieldNames(parser.GetRow(1));
	ok &= ParseTypeRow(parser.GetRow(2));
	for (size_t i = 3; i < uRowCnt; ++i)
		ok &= ParseRecordRow(parser.GetRow(i), i);
	return ok;
}

// 返回字段无次序
StrVec CsvTable::GetFields() const
{
	StrVec vRet;
	if (!m_pField2Index)  // 未加载
		return vRet;
	for (const auto& kv : *m_pField2Index)
		vRet.emplace_back(kv.first);
	return vRet;
}

// 初次查询会自动排序。
CsvRecordSptrVec CsvTable::GetRecords(
	const CsvQueryCondition& condition)
{
	if (!m_pField2Index)  // 未加载
		return CsvRecordSptrVec{};

	CsvSortType sortType;
	CsvSortedTable::SortKey sortKey;
	const StrVec& vKey = condition.GetKeys();
	for (const std::string& sKey : vKey)
	{
		auto itr = m_pField2Index->find(sKey);
		if (itr == m_pField2Index->end())
		{
			LOG_WARN("Illegal field name: " << sKey);
			return CsvRecordSptrVec{};
		}
		size_t idx = (*itr).second;  // 列索引号
		sortType.insert(idx);
		sortKey.emplace_back(condition.GetValue(sKey));
	}

	const CsvSortedTable& sortedTable = GetSortedTable(sortType);
	return sortedTable.GetRecords(sortKey);
}

const CsvRecord& CsvTable::GetRecord(const CsvQueryCondition& condition)
{
	CsvRecordSptrVec v = GetRecords(condition);
	if (!v.empty())
	{
		assert(v[0]);
		return *v[0];
	}

	static const CsvField2IndexSptr s_p(new CsvField2Index);
	static const CsvRecord s_rec{s_p};
	return s_rec;
}

bool CsvTable::ParseFieldNames(const StrVec& vField)
{
	bool ok = true;
	m_pField2Index.reset(new CsvField2Index);
	CsvField2Index& r = *m_pField2Index;
	for (size_t i = 0; i < vField.size(); i++)
	{
		const std::string& sField = vField[i];
		if (sField.empty())
		{
			ok = false;
			LOG_ERROR(Fmt("Field name of column %1% is blank!") % (i + 1));
		}
		else if (r.find(sField) != r.end())
		{
			ok = false;
			LOG_ERROR("Duplicated field name: " << sField);
		}
		r[sField] = i;
	}
	return ok;
}

bool CsvTable::ParseTypeRow(const StrVec& vTypeStr)
{
	assert(m_pField2Index);
	size_t uSize = m_pField2Index->size();
	bool ok = true;
	if (vTypeStr.size() > uSize)
	{
		ok = false;
		LOG_WARN("Csv type size is larger than field size.");
	}
	m_vType.resize(uSize);
	for (size_t i = 0; i < uSize; ++i)
		ok &= SetType(vTypeStr, i);
	return ok;
}

bool CsvTable::ParseRecordRow(const StrVec& vVal, size_t iRow)
{
	CsvRecordSptr pRecord(new CsvRecord(m_pField2Index));
	m_vRecord.push_back(pRecord);
	return pRecord->Parse(m_vType, vVal, iRow);
}

bool CsvTable::SetType(const StrVec& vTypeStr, size_t index)
{
	CsvValueType& rType = m_vType[index];
	rType = CsvValueType::String;  // 缺省值
	if (index >= vTypeStr.size())
		return true;  // 缺省

	const std::string& sType = vTypeStr[index];
	if (sType.empty()) return true;  // 缺省值
	if ("string" == sType) return true;
	if ("int" == sType)
	{
		rType = CsvValueType::Int64;
	}
	else if ("float" == sType)
	{
		rType = CsvValueType::Double;
	}
	else
	{
		LOG_ERROR("Illegal type: " << sType);
		return false;
	}
	return true;
}

// 未排序则先排序
const CsvSortedTable& CsvTable::GetSortedTable(const CsvSortType& sortType)
{
	Type2Sorted::const_iterator itr = m_type2Sorted.find(sortType);
	if (itr != m_type2Sorted.end())
	{
		assert((*itr).second);
		return *(*itr).second;
	}
	auto pSorted = std::make_shared<const CsvSortedTable>(sortType, m_vRecord);
	m_type2Sorted[sortType] = pSorted;
	return *pSorted;
}

void CsvTable::Reset()
{
	m_vRecord.clear();
	m_pField2Index.reset();
	m_vType.clear();
	m_type2Sorted.clear();
}

