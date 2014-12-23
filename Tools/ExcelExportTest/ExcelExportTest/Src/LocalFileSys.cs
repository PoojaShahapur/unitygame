using SDK.Common;
using System;
using System.Collections;
using System.IO;

namespace SDK.Common
{
    /**
     * @brief 本地文件系统，参考 http://www.xuanyusong.com/archives/1069
     */
    public class LocalFileSys
    {
        /**
        * @param path：文件创建目录
        * @param name：文件的名称
        * @param info：写入的内容
        */
        void CreateFile(string path, string name, string info)
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
            }
            else
            {
                //如果此文件存在则打开
                sw = t.AppendText();
            }
            //以行的形式写入信息
            sw.WriteLine(info);
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
        }

        /**
         * @param path：读取文件的路径
         * @param name：读取文件的名称
         */
        ArrayList LoadFile(string path, string name)
        {
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception error)
            {
                //路径与名称未找到文件则直接返回空
                return null;
            }
            string line;
            ArrayList arrlist = new ArrayList();
            while ((line = sr.ReadLine()) != null)
            {
                //一行一行的读取
                //将每一行的内容存入数组链表容器中
                arrlist.Add(line);
            }
            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            //将数组链表容器返回
            return arrlist;
        }

        // 加载文件，返回 byte
        public byte[] LoadFileByte(string path, string name)
        {
            byte[] bytes= null;
            try
            {
                bytes = File.ReadAllBytes(path + "//" + name);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                return null;
            }

            return bytes;
        }

        /**
         * @param path：删除文件的路径
         * @param name：删除文件的名称
         */
        public void DeleteFile(string path, string name)
        {
            File.Delete(path + "//" + name);
        }
    }
}