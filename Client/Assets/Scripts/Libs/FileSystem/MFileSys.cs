using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 本地文件系统
     */
    public class MFileSys
    {
        public const string kAssetBundlesPath = "/AssetBundles/";
        static public string BaseDownloadingURL;
        static public string AssetBundlesPrefixPath = "Assets/Resources/";

        protected string m_persistentDataPath;

        public MFileSys()
        {
            m_persistentDataPath = Application.persistentDataPath;
        }

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
        public ArrayList LoadFile(string path, string name)
        {
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
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

        public string LoadFileText(string path, string name)
        {
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
                return null;
            }
            string text = sr.ReadToEnd();
            
            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            //将数组链表容器返回
            return text;
        }

        // 加载文件，返回 byte
        public byte[] LoadFileByte(string path)
        {
            byte[] bytes= null;
            try
            {
                bytes = File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
                //路径与名称未找到文件则直接返回空
                return null;
            }

            return bytes;
        }

        // 写二进制文件
        public void writeFileByte(string path, params byte[] bytes)
        {
            FileStream fileStream;
            try
            {
                if (File.Exists(@path))                  // 如果文件存在
                {
                    File.Delete(@path);
                }

                fileStream = new FileStream(path, FileMode.Create);
                fileStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
            }
        }

        /**
         * @param path：删除文件的路径
         * @param name：删除文件的名称
         */
        public void DeleteFile(string path, string name)
        {
            File.Delete(path + "//" + name);
        }

        // 文件是否存在
        public bool isFileExist(string path)
        {
            return File.Exists(path);
        }

        // 获取本地 Data 目录
        public string getLocalDataDir()
        {
            return Application.dataPath;
        }

        // 获取本地可以读取的目录，但是不能写
        public string getLocalReadDir()
        {
            #if UNITY_EDITOR
            string filepath = Application.dataPath +"/StreamingAssets";
            #elif UNITY_IPHONE
              string filepath = Application.dataPath +"/Raw";
            #elif UNITY_ANDROID
              string filepath = "jar:file://" + Application.dataPath + "!/assets/";
            #elif UNITY_STANDALONE_WIN
            string filepath = Application.dataPath +"/StreamingAssets";
            #elif UNITY_WEBPLAYER
            string filepath = Application.dataPath +"/StreamingAssets";
            #else
            string filepath = Application.dataPath +"/StreamingAssets";
            #endif

            return filepath;
        }

        // 获取本地可以写的目录
        public string getLocalWriteDir()
        {
            // get_persistentDataPath can only be called from the main thread
            //return Application.persistentDataPath;      // 这个目录是可读写的
            return m_persistentDataPath;
        }

        // 获取编辑器工作目录
        public string getWorkPath()
        {
            return System.Environment.CurrentDirectory;
        }

        // 获取编辑器工作目录
        public string getDebugWorkPath()
        {
            string path = string.Format("{0}{1}", getWorkPath(), "/Debug");
            return path;
        }

        public FileStream openFile(string path)
        {
            // 判断文件是否存在
            if(File.Exists(path))
            {
                FileStream fs = null;
                //fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                try
                {
                    fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);//读取文件设定
                }
                catch(Exception /*ex*/)
                {
                    return null;
                }
                return fs;
            }

            return null;
        }

        public string getAbsPathByRelPath(ref string relPath, ref ResLoadType loadType)
        {
            // 获取版本
            string version = Ctx.m_instance.m_versionSys.getFileVer(relPath);
            string absPath = relPath;
            if (!string.IsNullOrEmpty(version))
            {
                absPath = UtilLogic.combineVerPath(Path.Combine(Ctx.m_instance.m_fileSys.getLocalWriteDir(), relPath), version);
                if (!File.Exists(absPath))
                {
                    absPath = Path.Combine(Ctx.m_instance.m_fileSys.getLocalReadDir(), relPath);
                    if (!File.Exists(absPath))
                    {
                        absPath = "";
                    }
                    else
                    {
                        loadType = ResLoadType.eStreamingAssets;
                    }
                }
                else
                {
                    relPath = UtilLogic.combineVerPath(relPath, version);         // 在可写目录下，文件名字是有版本号的
                    loadType = ResLoadType.ePersistentData;
                }
            }
            else
            {
                loadType = ResLoadType.eStreamingAssets;
            }

            return absPath;
        }

        public static void modifyLoadParam(string resPath, LoadParam param)
        {
            if (MacroDef.PKG_RES_LOAD)
            {
                param.m_origPath = resPath;             // 记录原始的资源名字

                string retPath = resPath;

                if ("Module/AutoUpdate.prefab" == resPath)       // 自动更新模块更新还没有实现
                {
                    param.m_resLoadType = ResLoadType.eStreamingAssets;
                }
                else
                {
                    // 获取包的名字
                    if (Ctx.m_instance.m_pPakSys.path2PakDic.ContainsKey(resPath))
                    {
                        retPath = Ctx.m_instance.m_pPakSys.path2PakDic[resPath].m_pakName;
                    }

                    if (param != null)
                    {
                        Ctx.m_instance.m_fileSys.getAbsPathByRelPath(ref retPath, ref param.m_resLoadType);
                    }
                    else
                    {
                        ResLoadType tmp = ResLoadType.eStreamingAssets;
                        Ctx.m_instance.m_fileSys.getAbsPathByRelPath(ref retPath, ref tmp);
                    }
                }
                param.m_path = retPath;
                param.m_pakPath = param.m_path;
            }
            else if (MacroDef.UNPKG_RES_LOAD)
            {
                if (param != null)
                {
                    param.m_resLoadType = ResLoadType.eStreamingAssets;
                }
                param.m_path = resPath;
            }
            else if (MacroDef.ASSETBUNDLES_LOAD)
            {
                param.m_path = AssetBundlesPrefixPath + resPath;
                param.m_path = param.m_path.ToLower();
            }
            else
            {
                param.m_path = resPath;
            }
        }

        public void saveTex2Disc(Texture2D tex, string fileName)
        {
            //将图片信息编码为字节信息
            byte[] bytes = tex.EncodeToPNG();
            //保存
            string path = string.Format("{0}/{1}.png", getDebugWorkPath(), fileName);
            writeFileByte(path, bytes);
        }

        public void writeStr2File(string fileName, Vector2[] datas)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);

            fileStream = new FileStream(path, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);
            int idx = 0;
            for (idx = 0; idx < datas.Length; idx += 1)
            {
                streamWriter.Write(datas[idx].x);
                streamWriter.Write(" -- ");
                streamWriter.Write(datas[idx].y);
                streamWriter.Write("\n");
            }

            streamWriter.Flush();
            fileStream.Flush();
            streamWriter.Close();
            fileStream.Close();
            streamWriter.Dispose();
            fileStream.Dispose();
        }

        public void writeStr2File(string fileName, Vector3[] datas)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);

            fileStream = new FileStream(path, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);
            int idx = 0;
            for (idx = 0; idx < datas.Length; idx += 1)
            {
                streamWriter.Write(datas[idx].x);
                streamWriter.Write(" -- ");
                streamWriter.Write(datas[idx].y);
                streamWriter.Write(" -- ");
                streamWriter.Write(datas[idx].z);
                streamWriter.Write("\n");
            }

            streamWriter.Flush();
            fileStream.Flush();
            streamWriter.Close();
            fileStream.Close();
            streamWriter.Dispose();
            fileStream.Dispose();
        }

        public void writeStr2File(string fileName, Vector4[] datas)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);

            fileStream = new FileStream(path, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);
            int idx = 0;
            for (idx = 0; idx < datas.Length; idx += 1)
            {
                streamWriter.Write(datas[idx].x);
                streamWriter.Write(" -- ");
                streamWriter.Write(datas[idx].y);
                streamWriter.Write(" -- ");
                streamWriter.Write(datas[idx].z);
                streamWriter.Write("\n");
            }

            streamWriter.Flush();
            fileStream.Flush();
            streamWriter.Close();
            fileStream.Close();
            streamWriter.Dispose();
            fileStream.Dispose();
        }

        public void serializeArray<T>(string fileName, T[] datas, int stride)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string path = string.Format("{0}/{1}", m_persistentDataPath, fileName);
            try
            {
                if (File.Exists(@path))                  // 如果文件存在
                {
                    File.Delete(@path);
                }

                fileStream = new FileStream(path, FileMode.Create);
                streamWriter = new StreamWriter(fileStream);
                int idx = 0;
                int strideIdx = 0;
                for (idx = 0; idx < datas.Length; idx += stride)
                {
                    for (strideIdx = 0; strideIdx < stride; ++strideIdx)
                    {
                        streamWriter.Write(datas[idx + strideIdx].ToString());
                        if (strideIdx < stride - 1)
                        {
                            streamWriter.Write(" -- ");
                        }
                    }

                    streamWriter.Write("\n");
                }

                streamWriter.Flush();
                streamWriter.Close();
                streamWriter.Dispose();
                fileStream.Flush();
                fileStream.Close();
                fileStream.Dispose();
            }
            catch (Exception e)
            {
                Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
            }
        }

        static public void initABRootPath()
        {
            string relativePath = "";
            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                relativePath = Application.streamingAssetsPath;
            }
            else
            {
                relativePath = Application.streamingAssetsPath;
            }
            string platformFolderForAssetBundles = UtilApi.GetPlatformFolderForAssetBundles(Application.platform);
            BaseDownloadingURL = relativePath + kAssetBundlesPath + platformFolderForAssetBundles;
        }

        static public string convResourcesPath2AssetBundlesPath(string resPath)
        {
            if(MacroDef.ASSETBUNDLES_LOAD)
            {
                return "Assets/Resources/" + resPath;
            }

            return resPath;
        }

        static public string convAssetBundlesPath2ResourcesPath(string assetBundlesPath)
        {
            if(MacroDef.ASSETBUNDLES_LOAD)
            {
                int idx = assetBundlesPath.IndexOf(AssetBundlesPrefixPath);
                // 如果有前缀
                if(-1 != idx)
                {
                    return assetBundlesPath.Substring(AssetBundlesPrefixPath.Length);
                }
            }

            return assetBundlesPath;
        }
    }
}