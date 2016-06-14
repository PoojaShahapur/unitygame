using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 本地文件系统
     */
    public class MFileSys
    {
        public static string msStreamingAssetsPath = Application.streamingAssetsPath;
        public static string msPersistentDataPath = Application.persistentDataPath;
        // 可读写目录
#if UNITY_EDITOR
        public static string msRWDataPath = msStreamingAssetsPath;
#else
        public static string msRWDataPath = msPersistentDataPath;
#endif
        // 获取本地 Data 目录
        static public string getLocalDataDir()
        {
            return Application.dataPath;
        }

        // 获取本地可以读取的 StreamingAssets 目录，不同平台下 StreamingAssets 是不同的，但是不能写
        static public string getLocalReadDir()
        {
#if UNITY_EDITOR
            string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#elif UNITY_IPHONE
              string filepath = Application.dataPath +"/Raw/";
#elif UNITY_ANDROID
              string filepath = "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_STANDALONE_WIN
            string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#elif UNITY_WEBPLAYER
            string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#else
            string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#endif

            return filepath;
        }

        // 获取本地可以写的目录
        static public string getLocalWriteDir()
        {
            // get_persistentDataPath can only be called from the main thread
            //return Application.persistentDataPath;      // 这个目录是可读写的
            return msPersistentDataPath;
        }

        // 获取编辑器工作目录
        static public string getWorkPath()
        {
            return System.Environment.CurrentDirectory;
        }

        // 获取编辑器工作目录
        static public string getDebugWorkPath()
        {
            string path = string.Format("{0}{1}", getWorkPath(), "/Debug");
            return path;
        }

        static public string getAbsPathByRelPath(ref string relPath, ref ResLoadType loadType)
        {
            // 获取版本
            string version = Ctx.m_instance.m_versionSys.getFileVer(relPath);
            string absPath = relPath;
            if (!string.IsNullOrEmpty(version))
            {
                absPath = UtilLogic.combineVerPath(Path.Combine(MFileSys.getLocalWriteDir(), relPath), version);
                if (!File.Exists(absPath))
                {
                    absPath = Path.Combine(MFileSys.getLocalReadDir(), relPath);
                    if (!File.Exists(absPath))
                    {
                        absPath = "";
                    }
                    else
                    {
                        loadType = ResLoadType.eLoadStreamingAssets;
                    }
                }
                else
                {
                    relPath = UtilLogic.combineVerPath(relPath, version);         // 在可写目录下，文件名字是有版本号的
                    loadType = ResLoadType.eLoadLocalPersistentData;
                }
            }
            else
            {
                loadType = ResLoadType.eLoadStreamingAssets;
            }

            return absPath;
        }

        // 下面这几个接口尽量不要使用，Lua 中加载 PB 文件有使用
        static public byte[] readFileAllBytes(string fileName)
        {
            byte[] ret = null;
            try
            {
                //ret = File.ReadAllBytes(msRWDataPath + "/" + fileName);
                AuxBytesLoader auxBytesLoader = new AuxBytesLoader();
                auxBytesLoader.syncLoad(fileName);
                ret = auxBytesLoader.getBytes();
                auxBytesLoader.dispose();
            }
            catch
            {
                Debug.Log("Not Find File " + fileName);
            }

            return ret;
        }

        static public void writeBytesToFile(string fileName, byte[] buf)
        {
            File.WriteAllBytes(msRWDataPath + "/" + fileName, buf);
        }

        static public string readFileAllText(string fileName)
        {
            string ret = null;
            try
            {
                ret = File.ReadAllText(msRWDataPath + "/" + fileName);
            }
            catch
            {
                Debug.Log("Not Find File " + fileName);
            }

            return ret;
        }

        static public void writeTextToFile(string fileName, string text)
        {
            File.WriteAllText(msRWDataPath + "/" + fileName, text);
        }

        static public LuaStringBuffer readLuaBufferToFile(string fileName)
        {
            byte[] ret = readFileAllBytes(fileName.ToString());
            LuaStringBuffer buffer = new LuaStringBuffer(ret);
            return buffer;
        }

        static public void writeLuaBufferToFile(string fileName, LuaStringBuffer text)
        {
            writeBytesToFile(fileName, text.buffer);
        }
    }
}