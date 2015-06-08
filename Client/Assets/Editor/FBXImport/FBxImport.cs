namespace NSFBXImport
{
    /**
     * @brief 导入 FBX 文件
     */
    public class FBXImportSys
    {
        static public FBXImportSys m_instance;

        public static FBXImportSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new FBXImportSys();
            }
            return m_instance;
        }
    }
}