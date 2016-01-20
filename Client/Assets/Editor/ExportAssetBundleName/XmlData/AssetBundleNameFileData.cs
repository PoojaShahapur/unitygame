namespace EditorTool
{
    public class AssetBundleNameFileData
    {
        protected string m_fullPath;
        protected string m_subPathNoExt;
        protected AssetBundleNameDirData m_dirData;

        protected string[] m_pathArr;
        protected string m_resPath;     // 就是放在 Resources 中的目录
        protected string m_abPath;      // 就是打包进 AB 中的目录

        public AssetBundleNameFileData(string path, AssetBundleNameDirData dir)
        {
            m_fullPath = path;
            m_dirData = dir;
        }

        public void buildData()
        {
            int dotIdx = m_fullPath.IndexOf(".");
            string pathNoExt = m_fullPath.Substring(0, dotIdx);
            m_subPathNoExt = pathNoExt.Substring(m_dirData.fullDirPath.Length + 1);

            pathNoExt = pathNoExt.Substring(m_dirData.fullDirPath.Length + 1);
            char[] separator = new char[1];
            separator[0] = '/';
            m_pathArr = pathNoExt.Split(separator);

            int resIndex = m_fullPath.IndexOf(ExportUtil.RESOURCES);
            if (resIndex != -1)
            {
                m_resPath = m_fullPath.Substring(resIndex + ExportUtil.RESOURCES.Length + 1);

                dotIdx = m_resPath.IndexOf(".");
                if(dotIdx != -1)
                {
                    m_resPath = m_resPath.Substring(0, dotIdx);
                }
            }

            int assetIndex = m_fullPath.IndexOf(ExportUtil.ASSETS);
            if (resIndex != -1)
            {
                m_abPath = m_fullPath.Substring(assetIndex);

                dotIdx = m_abPath.IndexOf(".");
                if (dotIdx != -1)
                {
                    m_abPath = m_abPath.Substring(0, dotIdx);
                }
            }
        }
    }
}