#include "csv_cfg.h"

#include "detail/csv_cfg_impl.h"

namespace {

CsvCfgImpl& GetImpl()
{
	static CsvCfgImpl s_impl;
	return s_impl;
}

}

namespace CsvCfg
{

bool Init()
{
	return GetImpl().Init();
}

CsvTable& GetTable(const std::string& sTablePath)
{
	return GetImpl().GetTable(sTablePath);
}

}
