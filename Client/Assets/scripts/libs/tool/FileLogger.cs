using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 输出日志到文件
     */
    public class FileLogger : ILogger
    {
        protected FileStream m_fileStream;
        protected StreamWriter m_streamWriter;

        public void openFile()
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
                if(fileinfo.Length > 1 * 1024 * 1024)           // 如果大于 1 M ，就删除
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

        public void closeFile()
        {
            m_streamWriter.Flush();
            //关闭流
            m_streamWriter.Close();
            m_fileStream.Close();
        }

        public void log(string message)
        {
            logout(message, LogColor.LOG);
        }

        public void warn(string message)
        {
            logout(message, LogColor.WARN);
        }

        public void error(string message)
        {
            logout(message, LogColor.ERROR);
        }

        // 写文件
        public void logout(string message, LogColor type = LogColor.LOG)
        {
            if(m_streamWriter != null)
            {
                m_streamWriter.Write(message);
            }
        }

        public void updateLog()
        {

        }

        public void asynclog(string message)
        {

        }
    }
}