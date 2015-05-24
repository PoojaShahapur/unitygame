using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Start
{
    public class StartUtil
    {
        // 获取本地可以读取的目录，但是不能写
        public static string getLocalReadDir()
        {
#if UNITY_EDITOR
            string filepath = Application.dataPath + "/StreamingAssets";
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

        // 加载文件，返回 byte
        public static byte[] LoadFileByte(string path)
        {
            byte[] bytes = null;
            try
            {
                bytes = File.ReadAllBytes(path);
            }
            catch (Exception /*e*/)
            {
                //路径与名称未找到文件则直接返回空
                return null;
            }

            return bytes;
        }

        public static AsyncOperation UnloadUnusedAssets()
        {
            AsyncOperation opt = Resources.UnloadUnusedAssets();
            return opt;
        }
    }
}