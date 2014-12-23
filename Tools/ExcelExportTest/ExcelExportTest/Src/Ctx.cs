namespace SDK.Common
{
    public class Ctx
    {
        public static Ctx m_instance = new Ctx();
        public LocalFileSys m_localFileSys = new LocalFileSys();
        public TableSys m_tableSys = new TableSys();
        public Config m_cfg = new Config();
    }
}
