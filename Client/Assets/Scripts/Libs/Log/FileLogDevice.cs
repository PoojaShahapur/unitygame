using SDK.Common;
using System;
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
        protected string m_fileSuffix;      // 文件后缀。例如 log_suffix.txt ，suffix 就是后缀
        protected string m_filePrefix;      // 文件前缀。例如 prefix_suffix.txt ，prefix 就是前缀
        protected FileStream m_fileStream;
        protected StreamWriter m_streamWriter;
        //protected StackTrace m_stackTrace;
        //protected string m_traceStr;

        public FileLogDevice()
        {
            m_filePrefix = "log";
        }

        public string fileSuffix
        {
            get
            {
                return m_fileSuffix;
            }
            set
            {
                m_fileSuffix = value;
            }
        }

        public string filePrefix
        {
            get
            {
                return m_filePrefix; 
            }
            set
            {
                m_filePrefix = value;
            }
        }

        public override void initDevice()
        {
#if UNITY_EDITOR
            //string path = string.Format("{0}{1}", Application.dataPath, "/Debug");
            string path = string.Format("{0}{1}", Ctx.m_instance.m_localFileSys.getWorkPath(), "/Debug");
#else
            string path = string.Format("{0}{1}", Application.persistentDataPath,"/Debug");
#endif
            checkDirSize(path); // 检查目录大小

            string file;
            if(string.IsNullOrEmpty(m_fileSuffix))
            {
                file = string.Format("{0}/{1}_{2}{3}", path, m_filePrefix, UtilApi.getUTCFormatText(), ".txt");
            }
            else
            {
                file = string.Format("{0}/{1}_{2}{3}{4}{5}", path, m_filePrefix, m_fileSuffix, "_", UtilApi.getUTCFormatText(), ".txt");
            }
            
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

        // 检测日志目录大小，如果太大，就删除
        protected void checkDirSize(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                long Size = 0;
                // 所有文件大小.
                FileInfo[] fis = dirInfo.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    Size += fi.Length;
                }

                // 如果超过限制就删除
                if (Size > 10 * 1024 * 1024)
                {
                    foreach (FileInfo fi in fis)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch (Exception /*err*/)
                        {
                            Ctx.m_instance.m_logSys.log(string.Format("删除文件 {0} 出错", fi.FullName));
                        }
                    }
                }

                //{
                //    string[] strFiles = Directory.GetFiles(path);

                //    foreach (string strFile in strFiles)
                //    {
                //        FileInfo fileInfo = new FileInfo(strFile);
                //        Size += fileInfo.Length;
                //    }

                //    if (Size > 10 * 1024 * 1024)
                //    {
                //        foreach (string strFile in strFiles)
                //        {
                //            File.Delete(strFile);
                //        }
                //    }
                //}
            }
        }
    }
}