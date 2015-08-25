using SDK.Lib;

namespace UnitTestSrc
{
    public class TestTable
    {
        public void run()
        {
            //testCardTable();
        }

        protected void testCardTable()
        {
            //Ctx.m_instance.m_tableSys.loadOneTable(TableID.TABLE_SKILL);
            //Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, 2);
            Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, 12000);
        }
    }
}