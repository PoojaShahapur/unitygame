using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EditorTool
{
    /**
    * @brief 导出的一项资源列表
    */
    public class ResListItem
    {
        public string m_srcName;
        public string m_destName;
    }

    /**
     * @brief 导出打包的资源列表
     */
    class ExportResList
    {
        protected List<ResListItem> m_list = new List<ResListItem>();

        public void addItem(ResListItem item)
        {
            m_list.Add(item);
        }

        // 导出资源列表文件
        public void exportResList()
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                if (File.Exists(@ResExportSys.m_instance.m_pResourcesCfgPackData.m_resListOutpath))                  // 如果文件存在
                {
                    File.Delete(@ResExportSys.m_instance.m_pResourcesCfgPackData.m_resListOutpath);
                }

                fileStream = new FileStream(ResExportSys.m_instance.m_pResourcesCfgPackData.m_resListOutpath, FileMode.Create);
                streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

                bool bFirstLine = true;
                foreach(var item in m_list)
                {
                    if (bFirstLine)
                    {
                        bFirstLine = false;
                    }
                    else
                    {
                        streamWriter.Write("\r\n");
                    }
                    streamWriter.Write(string.Format("{0}={1}", item.m_srcName, item.m_destName));
                }
            }
            finally
            {
                streamWriter.Dispose();
                streamWriter.Close();

                fileStream.Dispose();
                fileStream.Close();
            }
        }
    }
}