#include "lua_csv.h"

#include "csv/csv.h"  // for GetTable()
#include "log.h"

#include <LuaIntf/LuaIntf.h>

const char LOG_NAME[] = "LuaCsv";

using LuaIntf::LuaRef;

namespace {

// 单个Csv记录转为lua table
LuaRef CsvRecordToLua(lua_State* L, const StrVec& vField, const CsvRecord& rec)
{
	auto luaRec = LuaRef::createTable(L);
	for (const std::string& sFld : vField)
	{
		using Value = boost::variant<int64_t, double, std::string>;
		const Value& val = rec.GetValue(sFld);
		switch (val.which())
		{
		case 0:
			luaRec[sFld] = boost::get<int64_t>(val);
			break;
		case 1:
			luaRec[sFld] = boost::get<double>(val);
			break;
		case 2:
			luaRec[sFld] = boost::get<std::string>(val);
			break;
		}
	}
	return luaRec;
}

// Csv 记录组转为lua table
LuaRef CsvRecordsToLua(lua_State* L, const StrVec& vField,
	const CsvRecordSptrVec& vRec)
{
	assert(L);
	auto luaRet = LuaRef::createTable(L);
	for (size_t uRec = 0; uRec < vRec.size(); uRec++)
	{
		CsvRecordSptr pRec = vRec[uRec];
		assert(pRec);
		luaRet[uRec + 1] = CsvRecordToLua(L, vField, *pRec);
	}
	return luaRet;
}

// 返回所有记录，转化成lua table
LuaRef GetAllRecords(lua_State* L, const CsvTable& tbl)
{
	StrVec vField = tbl.GetFields();
	const CsvRecordSptrVec& vRec = tbl.GetRecords();
	return CsvRecordsToLua(L, vField, vRec);
}

void LuaToCondition(const LuaRef& luaCondition, CsvQueryCondition& rCondition)
{
	if (!luaCondition.isTable())
	{
		LOG_ERROR("Illegal condition. Expect table but got "
			<< luaCondition.typeName());
		return;
	}
	for (auto& e : luaCondition)
	{
		std::string sKey = e.key<std::string>();
		LuaRef value = e.value<LuaRef>();
		std::string sType = value.typeName();
		if ("string" == sType)
			rCondition.Add(sKey, value.toValue<std::string>());
		else if ("number" == sType)
			rCondition.Add(sKey, value.toValue<int64_t>());
		else
			LOG_ERROR(Fmt("Illegel condition type: %s=%s") % sKey % sType);
	}
}

// 返回符合条件的记录，转化成lua table
LuaRef GetRecords(CsvTable* pTbl,
	const LuaRef& luaCondition)
{
	assert(pTbl);
	lua_State* L = luaCondition.state();
	if (!L) return LuaRef();

	CsvQueryCondition condition;
	LuaToCondition(luaCondition, condition);
	const CsvRecordSptrVec& vRec = pTbl->GetRecords(condition);
	return CsvRecordsToLua(L, pTbl->GetFields(), vRec);
}

// 返回符合条件的首个记录，转化成lua table
LuaRef GetRecord(CsvTable* pTbl, const LuaRef& luaCondition)
{
	assert(pTbl);
	lua_State* L = luaCondition.state();
	if (!L) return LuaRef();

	CsvQueryCondition condition;
	LuaToCondition(luaCondition, condition);
	const CsvRecord& rec = pTbl->GetRecord(condition);
	return CsvRecordToLua(L, pTbl->GetFields(), rec);
}

}  // namespace

namespace LuaCsv {

void Bind(lua_State* L)
{
	assert(L);
	using namespace CsvCfg;
	// 所有导出模块带前缀"c_"
	LuaIntf::LuaBinding(L).beginModule("c_csv")
		.beginClass<CsvTable>("CsvTable")
			.addFactory(&CsvCfg::GetTable)
			.addPropertyReadOnly("record_count", &CsvTable::GetRecordCount)
			.addPropertyReadOnly("field_count", &CsvTable::GetFieldCount)
			.addFunction("get_all_records", [L](const CsvTable* pTbl) {
					assert(pTbl);
					return GetAllRecords(L, *pTbl);
				})
			.addFunction("get_records", &GetRecords)
			.addFunction("get_record", &GetRecord)
		.endClass()
	.endModule();
}

}  // namespace LuaRpc
