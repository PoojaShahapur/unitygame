using SDK.Lib;

namespace UnitTest
{
    public class TestTable
    {
        public void run()
        {
            //testCardTable();
        }

        protected void testCardTable()
        {
            //Ctx.mInstance.mTableSys.loadOneTable(TableID.TABLE_SKILL);
            //Ctx.mInstance.mTableSys.getItem(TableID.TABLE_SKILL, 2);
            Ctx.mInstance.mTableSys.getItem(TableID.TABLE_CARD, 12000);
        }
    }
}