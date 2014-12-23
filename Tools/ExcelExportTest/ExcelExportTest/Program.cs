using SDK.Common;
namespace ExcelExportTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Ctx.m_instance.m_tableSys.loadOneTable(TableID.TABLE_OBJECT);
        }
    }
}