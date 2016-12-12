﻿using UnityEngine;
using System.IO;
using LuaInterface;

namespace SDK.Lib
{
    /// <summary>
    /// 集成自LuaFileUtils，重写里面的ReadFile，
    /// </summary>
    public class MyLuaLoader : LuaFileUtils
    {
        public MyLuaLoader()
        {
            instance = this;
            beZip = LuaFramework.AppConst.LuaBundleMode;
        }

        /// <summary>
        /// 添加打入Lua代码的AssetBundle
        /// </summary>
        /// <param name="bundle"></param>
        public void AddBundle(string bundleName)
        {
            //string url = LuaFramework.Util.DataPath + bundleName.ToLower();
            //if (File.Exists(url))
            //{
            //    AssetBundle bundle = AssetBundle.LoadFromFile(url);
            //    if (bundle != null)
            //    {
            //        bundleName = bundleName.Replace("lua/", "").Replace(".unity3d", "");
            //        base.AddSearchBundle(bundleName.ToLower(), bundle);
            //    }
            //}

            string url = string.Format("{0}/{1}", MFileSys.msAssetBundlesStreamingAssetsPath, bundleName.ToLower());
            Ctx.mInstance.mLogSys.log(string.Format("MyLuaLoader::AddBundle url is {0} .", url));

            AssetBundle bundle = AssetBundle.LoadFromFile(url);
            if (bundle != null)
            {
                Ctx.mInstance.mLogSys.log(string.Format("MyLuaLoader::AddBundle loaded, url is {0} .", url));

                bundleName = bundleName.Replace("lua/", "").Replace(".unity3d", "");
                base.AddSearchBundle(bundleName.ToLower(), bundle);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("MyLuaLoader::AddBundle failed, url is {0} .", url));
            }
        }

        /// <summary>
        /// 当LuaVM加载Lua文件的时候，这里就会被调用，
        /// 用户可以自定义加载行为，只要返回byte[]即可。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override byte[] ReadFile(string fileName)
        {
            return base.ReadFile(fileName);
        }
    }
}