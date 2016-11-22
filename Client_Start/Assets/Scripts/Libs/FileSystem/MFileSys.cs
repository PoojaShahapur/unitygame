using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 本地文件系统
     */
    public class MFileSys
    {
        // 使用目录都直接使用这四个目录
        public static string msStreamingAssetsPath;
        public static string msPersistentDataPath;

        public static string msWWWStreamingAssetsPath;  // www 读取 StreamingAssets 文件系统时候不同平台的目录
        public static string msAssetBundlesStreamingAssetsPath;     // AssetBundles CreateFromFile 直接从 StreamingAssets 目录下读取 AssetBundles 资源的目录

        public static string msWWWPersistentDataPath;   // 使用 WWW 读取 PersistentDataPath 的路径
        public static string msAssetBundlesPersistentDataPath;    // AssetBundles CreateFromFile 直接从 StreamingAssets 目录下读取 AssetBundles 资源的目录

        public static string msDataStreamResourcesPath;     // 使用 MDataStream 读取 Resources 目录下的资源
        public static string msDataStreamStreamingAssetsPath;     // 使用 MDataStream 读取 StreamingAssetsPath 目录下的资源
        public static string msDataStreamPersistentDataPath;     // 使用 MDataStream 读取 PersistentDataPath 目录下的资源

        // 可读写目录
        //#if UNITY_EDITOR
        //public static string msRWDataPath = msStreamingAssetsPath;
        //#else
        //public static string msRWDataPath = msPersistentDataPath;
        //#endif

        // 初始化资源目录
        static public void init()
        {
            msStreamingAssetsPath = Application.streamingAssetsPath;
            msPersistentDataPath = Application.persistentDataPath;

            msWWWStreamingAssetsPath = getWWWStreamingAssetsPath();
            msAssetBundlesStreamingAssetsPath = getAssetBundlesStreamingAssetsPath();

            msWWWPersistentDataPath = getWWWPersistentDataPath();
            msAssetBundlesPersistentDataPath = getAssetBundlesPersistentDataPath();

            msDataStreamResourcesPath = ":";
            msDataStreamStreamingAssetsPath = getDataStreamStreamingAssetsPath();
            msDataStreamPersistentDataPath = getDataStreamPersistentDataPath();
        }

        // 获取本地 Data 目录
        static public string getLocalDataDir()
        {
            return Application.dataPath;
        }

        // 获取本地可以读取的 StreamingAssets 目录，不同平台下 StreamingAssets 是不同的，但是不能写，这个目录主要使用 WWW 读取资源。这个接口和接口 getWWWStreamingAssetsPath 是相同的功能，只不过是不同的写法，以后都是用 getWWWStreamingAssetsPath 这个接口，不要再使用这个接口了
        static public string getLocalReadDir()
        {
#if UNITY_EDITOR
            string filepath = "file://" + Application.dataPath + "/StreamingAssets";
#elif UNITY_IPHONE
            string filepath = "file://" + Application.dataPath + "/Raw";
#elif UNITY_ANDROID
            string filepath = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_STANDALONE_WIN
            string filepath = "file://" + Application.dataPath + "/StreamingAssets";
#elif UNITY_WEBPLAYER
            string filepath = "file://" + Application.dataPath + "/StreamingAssets";
#else
            string filepath = "file://" + Application.dataPath + "/StreamingAssets";
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

        // http://docs.unity3d.com/ScriptReference/WWW.html
        static protected string getWWWStreamingAssetsPath()
        {
#if UNITY_EDITOR
            // 实际测试 string filepath = "file://" + Application.streamingAssetsPath; 也是可以的
            string filepath = "file:///" + Application.streamingAssetsPath;
#elif UNITY_IPHONE
            // 实际测试 string filepath = "file://" + Application.streamingAssetsPath; 也是可以的，并且官方文档说使用 string filepath = "file://" + Application.streamingAssetsPath;
            string filepath = "file:///" + Application.streamingAssetsPath;
#elif UNITY_ANDROID
            // 千万不能使用 string filepath = "file:///" + Application.streamingAssetsPath; 或者 string filepath = "file://" + Application.streamingAssetsPath;
            string filepath = Application.streamingAssetsPath;
#elif UNITY_STANDALONE_WIN
            // 实际测试 string filepath = "file://" + Application.streamingAssetsPath; 也是可以的
            string filepath = "file:///" + Application.streamingAssetsPath;
#elif UNITY_WEBPLAYER
            string filepath = "file:///" + Application.streamingAssetsPath;
#else
            string filepath = "file:///" + Application.streamingAssetsPath;
#endif
            return filepath;
        }

        static protected string getAssetBundlesStreamingAssetsPath()
        {
#if UNITY_EDITOR
            string filepath = Application.streamingAssetsPath;
#elif UNITY_IPHONE
            string filepath = Application.streamingAssetsPath;
#elif UNITY_ANDROID
            // Android 一定要是这个，否则加载失败， 5.3.4 测试过，其它版本未知
            string filepath = Application.dataPath + "!assets";
#elif UNITY_STANDALONE_WIN
            string filepath = Application.streamingAssetsPath;
#elif UNITY_WEBPLAYER
            string filepath = Application.streamingAssetsPath;
#else
            string filepath = Application.streamingAssetsPath;
#endif
            return filepath;
        }

        static public string getWWWPersistentDataPath()
        {
            return msPersistentDataPath;
        }

        static public string getAssetBundlesPersistentDataPath()
        {
            return msPersistentDataPath;
        }

        static public string getDataStreamStreamingAssetsPath()
        {
            return msStreamingAssetsPath;
        }

        static public string getDataStreamPersistentDataPath()
        {
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
            string version = Ctx.mInstance.mVersionSys.getFileVer(relPath);
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

                if(ret == null)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MFileSys::readFileAllBytes, Path is {0}, Failed", fileName), LogTypeId.eLogResLoader);
                }
                else
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MFileSys::readFileAllBytes, Path is {0}, Success", fileName), LogTypeId.eLogResLoader);
                }

                auxBytesLoader.dispose();
            }
            catch
            {
                Ctx.mInstance.mLogSys.log("Not Find File " + fileName, LogTypeId.eLogCommon);
            }

            return ret;
        }

        //static public void writeBytesToFile(string fileName, byte[] buf)
        //{
        //    File.WriteAllBytes(msRWDataPath + "/" + fileName, buf);
        //}

        //static public string readFileAllText(string fileName)
        //{
        //    string ret = null;
        //    try
        //    {
        //        ret = File.ReadAllText(msRWDataPath + "/" + fileName);
        //    }
        //    catch
        //    {
        //        Debug.Log("Not Find File " + fileName);
        //    }

        //    return ret;
        //}

        //static public void writeTextToFile(string fileName, string text)
        //{
        //    File.WriteAllText(msRWDataPath + "/" + fileName, text);
        //}

        //static public LuaStringBuffer readLuaBufferToFile(string fileName)
        //{
        //    byte[] ret = readFileAllBytes(fileName.ToString());
        //    LuaStringBuffer buffer = new LuaStringBuffer(ret);
        //    return buffer;
        //}

        static public LuaInterface.LuaByteBuffer readLuaBufferToFile(string fileName)
        {
            byte[] ret = readFileAllBytes(fileName.ToString());
            LuaInterface.LuaByteBuffer buffer = new LuaInterface.LuaByteBuffer(ret);
            return buffer;
        }

        //static public void writeLuaBufferToFile(string fileName, LuaStringBuffer text)
        //{
        //    writeBytesToFile(fileName, text.buffer);
        //}
    }
}