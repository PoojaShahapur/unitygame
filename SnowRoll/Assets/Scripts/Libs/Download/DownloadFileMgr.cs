using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 下载文件
     */
    public class DownloadFileMgr
    {
        private XmlCfgBase xml = null;
        private XmlSnowBallCfg snowballconfigxml = null;
        public DownloadFileMgr()
        {
        }

        public void init()
        {
            xml = new XmlCfgBase();
            snowballconfigxml = new XmlSnowBallCfg();
        }

        public void downloadFile()
        {
            this.downloadServerList();
            this.downloadConfigXml();
        }

        private void downloadServerList()
        {
            AuxDownloader auxDownload = new AuxDownloader();
            //auxDownload.download("https://gif5.club/server.xml", onDownLoad, null, 0);
            auxDownload.download("server.xml", onDownLoad, null, 0, true, 0);
        }

        private void downloadConfigXml()
        {
            AuxDownloader auxDownload = new AuxDownloader();
            auxDownload.download("SnowBall.xml", onConfigDownLoad, null, 0, true, 0);
        }

        private void onDownLoad(IDispatchObject dispObj)
        {
            AuxDownloader res = dispObj as AuxDownloader;
            //默认配置
            string retStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Config>\n\t<server ip=\"106.14.32.169\" port=\"20013\" />\n</Config>\n";
            if (res.isSuccessLoaded())
            {
                retStr = System.Text.Encoding.UTF8.GetString(res.getBytes());
            }
            getIpAndPort(retStr);
            getNoticeMessage(retStr);
        }

        private void getIpAndPort(string addr)
        {
            xml.parseXml(addr);
            System.Security.SecurityElement serElem = null;
            UtilXml.getXmlChild(xml.mXmlConfig, "server", ref serElem);
            string ip = "";
            UtilXml.getXmlAttrStr(serElem, "ip", ref ip);
            string port = "";
            UtilXml.getXmlAttrStr(serElem, "port", ref port);

            Ctx.mInstance.mSystemSetting.setString("ip", ip);
            Ctx.mInstance.mSystemSetting.setInt("port", Convert.ToInt32(port));
            Ctx.mInstance.mSystemSetting.setInt("ServerAddr", 10086);
        }

        private void getNoticeMessage(string msg)
        {
            xml.parseXml(msg);
            System.Security.SecurityElement msgElem = null;
            UtilXml.getXmlChild(xml.mXmlConfig, "notice", ref msgElem);
            int id = 0;
            UtilXml.getXmlAttrInt(msgElem, "id", ref id);
            Ctx.mInstance.mShareData.noticeId = id;
            int times = 0;
            UtilXml.getXmlAttrInt(msgElem, "showtimes", ref times);
            Ctx.mInstance.mShareData.noticeTimes = times;
            string msgtext = "";
            UtilXml.getXmlAttrStr(msgElem, "content", ref msgtext);
            Ctx.mInstance.mShareData.noticeMsg = msgtext;

            if (Ctx.mInstance.mSystemSetting.hasKey("NoticeId"))
            {
                int oldid = Ctx.mInstance.mSystemSetting.getInt("NoticeId");
                if(oldid != id)
                {
                    Ctx.mInstance.mSystemSetting.setInt("NoticeId", id);
                    Ctx.mInstance.mSystemSetting.setInt("NoticeTimes", 0);
                }
            }
            else
            {
                Ctx.mInstance.mSystemSetting.setInt("NoticeId", id);
                Ctx.mInstance.mSystemSetting.setInt("NoticeTimes", 0);
            }

            Ctx.mInstance.mLuaSystem.receiveToLua_KBE("ShowNoticeMsg", null);
        }

        private void onConfigDownLoad(IDispatchObject dispObj)
        {
            AuxDownloader res = dispObj as AuxDownloader;
            //默认配置
            string retStr = null;
            if (res.isSuccessLoaded())
            {
                retStr = System.Text.Encoding.UTF8.GetString(res.getBytes());
            }
            snowballconfigxml.parseXml(retStr);
           
            Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg = snowballconfigxml;
            Ctx.mInstance.mSnowBallCfg.preInit();
        }

        public void dispose()
        {
         
        }
    }
}