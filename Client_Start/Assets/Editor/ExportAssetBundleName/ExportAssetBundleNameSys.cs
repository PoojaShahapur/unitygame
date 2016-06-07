using System.Collections.Generic;
using System.IO;

namespace EditorTool
{
    public class ExportAssetBundleNameSys
    {
        protected AssetBundleNameXmlData m_abNameXmlData;

        public ExportAssetBundleNameSys()
        {
            m_abNameXmlData = new AssetBundleNameXmlData();
        }

        public void init()
        {

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

        // 导出 Res 目录 AB 资源名字映射
        public void exportResABKV()
        {
            List<string> m_list = new List<string>();
            m_abNameXmlData.exportResABKV(m_list);

            writeFile(UtilEditor.getWorkPath("AssetBundlePath.txt"), m_list);
        }

        public void writeFile(string path, List<string> list)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            
            if (File.Exists(@path))                  // 如果文件存在
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