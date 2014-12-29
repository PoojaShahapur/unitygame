using SDK.Common;
using System;

namespace ExcelExportTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ctx.m_instance.m_tableSys.loadOneTable(TableID.TABLE_OBJECT);

            string str = string.Format("网关信息:  网关IP: {0}, 网关端口: {1}, 用户ID: {2}, 用户临时 ID: {3}", "192.168.125.252", 20004, 10000010u, 66u);
            Console.WriteLine(str);
        }
    }
}