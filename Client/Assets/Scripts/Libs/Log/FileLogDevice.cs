using SDK.Common;
using System.Diagnostics;
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
        //protected StackTrace m_stackTrace;
        //protected string m_traceStr;

        public override void initDevice()
        {
#if UNITY_EDITOR
            string path = Application.dataPath + "/Debug";
#else
            string path = Application.persistentDataPath + "/Debug";
#endif
            string file = path + "/log.txt";
            if (!Directory.Exists(path))                    // 判断是否存在
            {
                Directory.CreateDirectory(path);            // 创建新路径
            }

            //if (File.Exists(@file))                  // 判断文件是否存在
            //{
            //    FileInfo fileinfo = new FileInfo(file);
            //    if (fileinfo.Length > 1 * 1024 * 1024)           // 如果大于 1 M ，就删除
            //    {
            //        File.Delete(@file);
            //    }
            //}

            if (File.Exists(@file))                  // 如果文件存在
            {
                //m_fileStream = new FileStream(file, FileMode.Append);
                File.Delete(@file);
                m_fileStream = new FileStream(file, FileMode.Create);
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
                //if (type == LogColor.WARN || type == LogColor.ERROR)
                //{
                //    m_stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
                //    m_traceStr = m_stackTrace.ToString();
                //    m_streamWriter.Write(m_traceStr);
                //    m_streamWriter.Write("\n");
                //}
                m_streamWriter.Flush();             // 立马输出
            }
        }
    }
}