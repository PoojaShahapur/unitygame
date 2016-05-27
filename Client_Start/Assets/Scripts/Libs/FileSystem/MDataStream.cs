using System;
using System.IO;
using System.Text;

namespace SDK.Lib
{
    public class MDataStream : GObject
    {
        protected string mFilePath;
        protected FileMode mMode;
        protected FileAccess mAccess;
        protected bool mIsInValid;
        public FileStream mFileStream;

        public MDataStream(string filePath, FileMode mode = FileMode.CreateNew, FileAccess access = FileAccess.ReadWrite)
        {
            this.mTypeId = "MDataStream";
            mFilePath = filePath;
            mMode = mode;
            mAccess = access;
            mIsInValid = true;

            open();
        }

        public void dispose()
        {
            close();
        }

        protected void open()
        {
            if (mIsInValid)
            {
                mIsInValid = false;
                mFileStream = new FileStream(mFilePath, mMode, mAccess);
            }
        }

        protected void close()
        {
            if(!mIsInValid && mFileStream != null)
            {
                mFileStream.Close();
                mFileStream.Dispose();
                mFileStream = null;
            }
        }

        public void writeText(string text, Encoding encode = null)
        {
            open();

            if (mFileStream.CanWrite)
            {
                Encoding encodeOrig = GkEncode.UTF8;
                if (encode != null)
                {
                    encodeOrig = encode;
                }

                byte[] bytes = encode.GetBytes(text);
                if (bytes != null)
                {
                    try
                    {
                        mFileStream.Write(bytes, 0, bytes.Length);
                    }
                    catch(Exception err)
                    {
                        Ctx.m_instance.m_logSys.log(err.Message);
                    }
                }
            }
        }

        public void writeByte(byte[] bytes, int offset = 0, int count = 0)
        {
            open();

            if (mFileStream.CanWrite)
            {
                if (bytes != null)
                {
                    if(count == 0)
                    {
                        count = bytes.Length;
                    }

                    if (count != 0)
                    {
                        try
                        {
                            mFileStream.Write(bytes, offset, count);
                        }
                        catch (Exception err)
                        {
                            Ctx.m_instance.m_logSys.log(err.Message);
                        }
                    }
                }
            }
        }

        public byte[] readByte(int offset = 0, int count = 0)
        {
            open();

            if(count == 0)
            {
                count = (int)mFileStream.Length;
            }
            byte[] bytes = null;

            if (mFileStream.CanRead)
            {
                try
                {
                    bytes = new byte[count];
                    mFileStream.Read(bytes, 0, count);
                }
                catch (Exception err)
                {
                    Ctx.m_instance.m_logSys.log(err.Message);
                }
            }

            return bytes;
        }

        //public MFileSys()
        //{
        //    m_persistentDataPath = Application.persistentDataPath;
        //}

        ///**
        //* @param path：文件创建目录
        //* @param name：文件的名称
        //* @param info：写入的内容
        //*/
        //void createFile(string filePath, string fileName, string info)
        //{
        //    //文件流信息
        //    StreamWriter streamWriter = null;
        //    FileInfo fileInfo = new FileInfo(filePath + "/" + fileName);
        //    if (!fileInfo.Exists)
        //    {
        //        //如果此文件不存在则创建
        //        streamWriter = fileInfo.CreateText();
        //    }
        //    else
        //    {
        //        //如果此文件存在则打开
        //        streamWriter = fileInfo.AppendText();
        //    }
        //    //以行的形式写入信息
        //    streamWriter.WriteLine(info);
        //    //关闭流
        //    streamWriter.Close();
        //    //销毁流
        //    streamWriter.Dispose();
        //}

        ///**
        // * @param path：读取文件的路径
        // * @param name：读取文件的名称
        // */
        //public ArrayList LoadFile(string path, string name)
        //{
        //    //使用流的形式读取
        //    StreamReader sr = null;
        //    try
        //    {
        //        sr = File.OpenText(path + "//" + name);
        //    }
        //    catch (Exception e)
        //    {
        //        //路径与名称未找到文件则直接返回空
        //        Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
        //        return null;
        //    }
        //    string line;
        //    ArrayList arrlist = new ArrayList();
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        //一行一行的读取
        //        //将每一行的内容存入数组链表容器中
        //        arrlist.Add(line);
        //    }
        //    //关闭流
        //    sr.Close();
        //    //销毁流
        //    sr.Dispose();
        //    //将数组链表容器返回
        //    return arrlist;
        //}

        //public string LoadFileText(string path, string name)
        //{
        //    //使用流的形式读取
        //    StreamReader sr = null;
        //    try
        //    {
        //        sr = File.OpenText(path + "//" + name);
        //    }
        //    catch (Exception e)
        //    {
        //        //路径与名称未找到文件则直接返回空
        //        Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
        //        return null;
        //    }
        //    string text = sr.ReadToEnd();

        //    //关闭流
        //    sr.Close();
        //    //销毁流
        //    sr.Dispose();
        //    //将数组链表容器返回
        //    return text;
        //}

        //// 加载文件，返回 byte
        //public byte[] LoadFileByte(string path)
        //{
        //    byte[] bytes = null;
        //    try
        //    {
        //        bytes = File.ReadAllBytes(path);
        //    }
        //    catch (Exception e)
        //    {
        //        Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
        //        //路径与名称未找到文件则直接返回空
        //        return null;
        //    }

        //    return bytes;
        //}

        //// 写二进制文件
        //public void writeFileByte(string path, params byte[] bytes)
        //{
        //    FileStream fileStream;
        //    try
        //    {
        //        if (File.Exists(@path))                  // 如果文件存在
        //        {
        //            File.Delete(@path);
        //        }

        //        fileStream = new FileStream(path, FileMode.Create);
        //        fileStream.Write(bytes, 0, bytes.Length);
        //    }
        //    catch (Exception e)
        //    {
        //        Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
        //    }
        //}

        //public FileStream openFile(string path)
        //{
        //    // 判断文件是否存在
        //    if (File.Exists(path))
        //    {
        //        FileStream fs = null;
        //        //fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        //        try
        //        {
        //            fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);//读取文件设定
        //        }
        //        catch (Exception /*ex*/)
        //        {
        //            return null;
        //        }
        //        return fs;
        //    }

        //    return null;
        //}



        //public void saveTex2Disc(Texture2D tex, string fileName)
        //{
        //    //将图片信息编码为字节信息
        //    byte[] bytes = tex.EncodeToPNG();
        //    //保存
        //    string path = string.Format("{0}/{1}.png", getDebugWorkPath(), fileName);
        //    writeFileByte(path, bytes);
        //}

        //public void writeStr2File(string fileName, Vector2[] datas)
        //{
        //    FileStream fileStream;
        //    StreamWriter streamWriter;
        //    string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);

        //    fileStream = new FileStream(path, FileMode.Create);
        //    streamWriter = new StreamWriter(fileStream);
        //    int idx = 0;
        //    for (idx = 0; idx < datas.Length; idx += 1)
        //    {
        //        streamWriter.Write(datas[idx].x);
        //        streamWriter.Write(" -- ");
        //        streamWriter.Write(datas[idx].y);
        //        streamWriter.Write("\n");
        //    }

        //    streamWriter.Flush();
        //    fileStream.Flush();
        //    streamWriter.Close();
        //    fileStream.Close();
        //    streamWriter.Dispose();
        //    fileStream.Dispose();
        //}

        //public void writeStr2File(string fileName, Vector3[] datas)
        //{
        //    FileStream fileStream;
        //    StreamWriter streamWriter;
        //    string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);

        //    fileStream = new FileStream(path, FileMode.Create);
        //    streamWriter = new StreamWriter(fileStream);
        //    int idx = 0;
        //    for (idx = 0; idx < datas.Length; idx += 1)
        //    {
        //        streamWriter.Write(datas[idx].x);
        //        streamWriter.Write(" -- ");
        //        streamWriter.Write(datas[idx].y);
        //        streamWriter.Write(" -- ");
        //        streamWriter.Write(datas[idx].z);
        //        streamWriter.Write("\n");
        //    }

        //    streamWriter.Flush();
        //    fileStream.Flush();
        //    streamWriter.Close();
        //    fileStream.Close();
        //    streamWriter.Dispose();
        //    fileStream.Dispose();
        //}

        //public void writeStr2File(string fileName, Vector4[] datas)
        //{
        //    FileStream fileStream;
        //    StreamWriter streamWriter;
        //    string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);

        //    fileStream = new FileStream(path, FileMode.Create);
        //    streamWriter = new StreamWriter(fileStream);
        //    int idx = 0;
        //    for (idx = 0; idx < datas.Length; idx += 1)
        //    {
        //        streamWriter.Write(datas[idx].x);
        //        streamWriter.Write(" -- ");
        //        streamWriter.Write(datas[idx].y);
        //        streamWriter.Write(" -- ");
        //        streamWriter.Write(datas[idx].z);
        //        streamWriter.Write("\n");
        //    }

        //    streamWriter.Flush();
        //    fileStream.Flush();
        //    streamWriter.Close();
        //    fileStream.Close();
        //    streamWriter.Dispose();
        //    fileStream.Dispose();
        //}

        //public void serializeArray<T>(string fileName, T[] datas, int stride)
        //{
        //    FileStream fileStream;
        //    StreamWriter streamWriter;
        //    string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);
        //    try
        //    {
        //        if (File.Exists(@path))                  // 如果文件存在
        //        {
        //            File.Delete(@path);
        //        }

        //        fileStream = new FileStream(path, FileMode.Create);
        //        streamWriter = new StreamWriter(fileStream);
        //        int idx = 0;
        //        int strideIdx = 0;
        //        for (idx = 0; idx < datas.Length; idx += stride)
        //        {
        //            for (strideIdx = 0; strideIdx < stride; ++strideIdx)
        //            {
        //                streamWriter.Write(datas[idx + strideIdx].ToString());
        //                if (strideIdx < stride - 1)
        //                {
        //                    streamWriter.Write(" -- ");
        //                }
        //            }

        //            streamWriter.Write("\n");
        //        }

        //        streamWriter.Flush();
        //        streamWriter.Close();
        //        streamWriter.Dispose();
        //        fileStream.Flush();
        //        fileStream.Close();
        //        fileStream.Dispose();
        //    }
        //    catch (Exception e)
        //    {
        //        Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
        //    }
        //    //}
        //}
    }
}