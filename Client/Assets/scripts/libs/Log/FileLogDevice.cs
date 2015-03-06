using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 文件日志
     */
    public class FileLogDevice : LogDeviceBase
    {
        protected FileStream m_fileStream;
        protected StreamWriter m_streamWriter;

        public override void initDevice()
        {
            string path = Application.dataPath + "/Debug";
            string file = path + "/log.txt";
            if (!Directory.Exists(path))                    // 判断是否存在
            {
                Directory.CreateDirectory(path);            // 创建新路径
            }

            if (File.Exists(@file))                  // 判断文件是否存在
            {
                FileInfo fileinfo = new FileInfo(file);
                if (fileinfo.Length > 1 * 1024 * 1024)           // 如果大于 1 M ，就删除
                {
                    File.Delete(@file);
                }
            }

            if (File.Exists(@file))                  // 如果文件存在
            {
                m_fileStream = new FileStream(file, FileMode.Append);
            }
            else
            {
                m_fileStream = new FileStream(file, FileMode.Create);
            }

            m_streamWriter = new StreamWriter(m_fileStream);
        }

        public override void closeDevice()
        {
            m_streamWriter.Flush();
            //关闭流
            m_streamWriter.Close();
            m_fileStream.Close();
        }

        // 写文件
        public override void logout(string message, LogColor type = LogColor.LOG)
        {
            if (m_streamWriter != null)
            {
                m_streamWriter.Write(message);
                m_streamWriter.Write("\n");
                m_streamWriter.Flush();             // 立马输出
                m_fileStream.Flush();
            }
        }
    }
}