using System;
using System.Collections.Generic;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 自动更新系统
     */
    public class AutoUpdateSys
    {
        public List<string> m_loadingPath;
        public List<string> m_loadedPath;
        public List<string> m_failedPath;
        public AddOnceAndCallOnceEventDispatch mOnUpdateEndDisp;

        public AutoUpdateSys()
        {
            m_loadingPath = new List<string>();
            m_loadedPath = new List<string>();
            m_failedPath = new List<string>();

            mOnUpdateEndDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public void startUpdate()
        {
            loadMiniVersion();
        }

        public void loadMiniVersion()
        {
            Ctx.m_instance.m_versionSys.mMiniLoadResultDisp.addEventHandle(null, miniVerLoadResult);
            Ctx.m_instance.m_versionSys.mLoadResultDisp.addEventHandle(null, verLoadResult);
            Ctx.m_instance.m_versionSys.loadMiniVerFile();
        }

        public void miniVerLoadResult(IDispatchObject dispObj)
        {
            // 本地文件版本必须要加载
            Ctx.m_instance.m_versionSys.loadVerFile();
        }

        public void verLoadResult(IDispatchObject idspObj)
        {
            if (Ctx.m_instance.m_versionSys.m_needUpdateVerFile) // 如果需要更新
            {
                // 开始正式加载文件
                loadAllUpdateFile();
            }
            else
            {
                onUpdateEnd();          // 更新结束
            }
        }

        public void loadAllUpdateFile()
        {
            foreach (KeyValuePair<string, FileVerInfo> kv in Ctx.m_instance.m_versionSys.m_serverVer.m_path2HashDic)
            {
                if(Ctx.m_instance.m_versionSys.m_localVer.m_path2Ver_P_Dic.ContainsKey(kv.Key))
                {
                    if(Ctx.m_instance.m_versionSys.m_localVer.m_path2Ver_P_Dic[kv.Key].m_fileMd5 != kv.Value.m_fileMd5)
                    {
                        loadOneUpdateFile(kv.Key, kv.Value);
                    }
                }
                else
                {
                    loadOneUpdateFile(kv.Key, kv.Value);
                }
            }
        }

        public void loadOneUpdateFile(string path, FileVerInfo fileInfo)
        {
            //string loadPath = UtilApi.combineVerPath(path, fileInfo.m_fileMd5);
            //m_loadingPath.Add(loadPath);
            m_loadingPath.Add(UtilLogic.webFullPath(path));
            if (Ctx.m_instance.m_versionSys.m_localVer.m_path2Ver_P_Dic.ContainsKey(path))
            {
                UtilPath.deleteFile(Path.Combine(MFileSys.getLocalWriteDir(), UtilLogic.combineVerPath(path, Ctx.m_instance.m_versionSys.m_localVer.m_path2Ver_P_Dic[path].m_fileMd5)));     // 删除当前目录下已经有的 old 文件
            }
            //UtilApi.delFileNoVer(path);     // 删除当前目录下已经有的 old 文件

            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(path);

            param.m_resLoadType = ResLoadType.eLoadWeb;
            param.m_version = fileInfo.m_fileMd5;

            param.m_loadEventHandle = onLoadEventHandle;

            Ctx.m_instance.m_resLoadMgr.loadData(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.log(string.Format("AutoUpdateSys::onLoadEventHandle, Success, Path is {0}", (dispObj as DataResItem).getLoadPath()));

                m_loadedPath.Add((dispObj as DataResItem).getResUniqueId());
                m_loadingPath.Remove((dispObj as DataResItem).getResUniqueId());

                if (m_loadingPath.Count == 0)
                {
                    onUpdateEnd();
                }
            }
            else if (res.hasFailed())
            {
                Ctx.m_instance.m_logSys.log(string.Format("AutoUpdateSys::onLoadEventHandle, Fail, Path is {0}", (dispObj as DataResItem).getLoadPath()));

                m_failedPath.Add((dispObj as DataResItem).getResUniqueId());
                m_loadingPath.Remove((dispObj as DataResItem).getResUniqueId());

                if (m_loadingPath.Count == 0)
                {
                    onUpdateEnd();
                }
            }
        }

        protected void onUpdateEnd()
        {
            // 进入游戏
            mOnUpdateEndDisp.dispatchEvent(null);
        }
    }
}