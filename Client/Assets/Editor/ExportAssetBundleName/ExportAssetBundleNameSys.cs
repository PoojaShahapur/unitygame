using System.Collections.Generic;
using System.IO;

namespace EditorTool
{
    public class ExportAssetBundleNameSys
    {
        static public ExportAssetBundleNameSys m_instance;

        protected AssetBundleNameXmlData m_abNameXmlData;

        public static ExportAssetBundleNameSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new ExportAssetBundleNameSys();
            }
            return m_instance;
        }

        public ExportAssetBundleNameSys()
        {
            m_abNameXmlData = new AssetBundleNameXmlData();
        }

        public void clear()
        {
            m_abNameXmlData.clear();
        }

        public void parseXml()
        {
            m_abNameXmlData.parseXml();
        }

        public void setAssetBundleName()
        {
            m_abNameXmlData.setAssetBundleName();
        }

        // ���� Res Ŀ¼ AB ��Դ����ӳ��
        public void exportResABKV()
        {
            List<string> m_list = new List<string>();
            m_abNameXmlData.exportResABKV(m_list);

            writeFile(ExportUtil.getWorkPath("AssetBundlePath.txt"), m_list);
        }

        public void writeFile(string path, List<string> list)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            
            if (File.Exists(@path))                  // ����ļ�����
            {
                File.Delete(@path);
            }

            fileStream = new FileStream(path, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);

            foreach(string line in list)
            {
                streamWriter.WriteLine(line);
            }

            streamWriter.Close();
            fileStream.Close();
        }
    }
}