#include "csv_cfg_impl.h"

#include "csv/csv_table.h"  // for CsvTable
#include "log.h"
#include "timer_queue/timer_queue_root.h"

#include <boost/filesystem.hpp>
#include <LuaIntf/LuaIntf.h>
#include "util.h"

namespace bfs = boost::filesystem;
const char LOG_NAME[] = "CsvCfgImpl";
const char CSV_DIR[] = "../csv";

CsvCfgImpl::CsvCfgImpl() : m_isInitLoad(true) {};
CsvCfgImpl::~CsvCfgImpl() {};

bool CsvCfgImpl::Init()
{
	// 仅初始化时判断Reload()返回值。定时重新加载时将忽略返回值。
	if (!Reload())
	{
		LOG_FATAL("Failed to load csv config.");
		return false;
	}

    m_isInitLoad = false;

	// 每5s检查更新
	TimerQueueRoot::Get().InsertRepeatFromNow(
		5000, 5000, [this]() { Reload(); });
	return true;
}

bool CsvCfgImpl::Reload()
{
	try
	{
		return Reload(path(CSV_DIR));
	}
	catch (const bfs::filesystem_error& ex)
	{
		LOG_ERROR("Reload error: " << ex.what());
		return false;
	}
}

CsvTable& CsvCfgImpl::GetTable(const std::string& sTablePath)
{
	auto itr = m_mapCsv.find(sTablePath);
	if (itr != m_mapCsv.end())
		return *(*itr).second.pTable;

	static CsvTable s_empty;
	LOG_ERROR("No such csv: " << sTablePath);
	return s_empty;
}

bool CsvCfgImpl::Reload(const path& p)
{
	if (is_regular_file(p))
		return ReloadFile(p);
	if (is_directory(p))
		return ReloadDir(p);
	return true;  // Skip other.
}

bool CsvCfgImpl::ReloadDir(const path& p)
{
	using namespace bfs;
	bool ok = true;
	for (directory_entry& x : directory_iterator(p))
		ok &= Reload(x);
	return ok;
}

bool CsvCfgImpl::ReloadFile(const path& p)
{
	if (p.extension() != ".csv")
		return true;  // skip non-csv
	time_t tFile = bfs::last_write_time(p);
	path sub = bfs::relative(p, CSV_DIR);
	std::string sKey = sub.generic_string();
	if (!NeedReload(sKey, tFile))
		return true;

	LOG_INFO("Reload csv: " << sKey);
	CsvTableSptr pTable(new CsvTable);
	bool ok = pTable->Load(p.string());
	m_mapCsv[sKey] = {tFile, pTable};
    // call lua loadconfig() function
    if (!m_isInitLoad && ok)
    {
	    using LuaIntf::LuaRef;
	    LuaRef require(Util::GetLuaState(), "require");
	    try
	    {
		    LuaRef handler = require.call<LuaRef>("plane.init");
		    handler.dispatchStatic("loadconfig");
	    }
	    catch (const LuaIntf::LuaException& e)
	    {
		    LOG_ERROR("Failed to loadconfig after csv " << sKey << "reloaded, "
			    << e.what());
	    }
    }
	return ok;
}

bool CsvCfgImpl::NeedReload(const std::string& sKey, time_t tFile) const
{
	auto itr = m_mapCsv.find(sKey);
	if (itr == m_mapCsv.end()) return true;
	return (*itr).second.tFile != tFile;
}

