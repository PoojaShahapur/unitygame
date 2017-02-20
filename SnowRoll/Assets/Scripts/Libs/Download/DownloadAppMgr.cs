using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 下载文件
     */
    public class DownloadAppMgr
    {
        public DownloadAppMgr()
        {
        }

        public void init()
        {
            
        }

        public void downloadApp()
        {
            AuxDownloader auxDownload = new AuxDownloader();
            //auxDownload.download("https://gif5.club/fish.ipa", onDownLoad, null, 0);
            auxDownload.download("fish.ipa", onDownLoad, null, 0, true, 0);
        }

        public void onDownLoad(IDispatchObject dispObj)
        {
            AuxDownloader res = dispObj as AuxDownloader;
            if (res.isSuccessLoaded())
            {
                
            }
        }

        public void dispose()
        {
         
        }
    }
}