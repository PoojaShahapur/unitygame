#include "csv/csv_cfg.h"
#include "csv/csv_table.h"

#include <cassert>

void TestCsv()
{
	bool ok = CsvCfg::Init();
	CsvTable& t = CsvCfg::GetTable("server.csv");
	CsvQueryCondition c;
	c.Add("abc", "dbe");
	t.GetRecords(c);
	t.GetRecords("id", 123);
	t.GetRecords("id", 123, "adsf", "9");
	t.GetRecords("a", 1, "b", 2, "c", 3);
	t.GetRecords("a", 1, "b", 2, "c", 3, "d", 4);
	t.GetRecords("a", 1, "b", 2, "c", 3, "d", 4, "e", 5);
	t.GetRecords("a", "1", "b", "2", "c", "3", "d", "4", "e", "5");
	t.GetRecords("a", 1, "b", "2", "c", 3, "d", "4", "e", 5);
	assert(ok);
}

