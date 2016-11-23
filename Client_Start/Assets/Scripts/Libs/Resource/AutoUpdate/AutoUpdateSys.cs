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
        public List<string> mLoadingPath;
        public List<string> mLoadedPath;
        public List<string> mFailedPath;
        public AddOnceAndCallOnceEventDispatch mOnUpdateEndDisp;

        public AutoUpdateSys()
        {
            this.mLoadingPath = new List<string>();
            this.mLoadedPath = new List<string>();
            this.mFailedPath = new List<string>();

            this.mOnUpdateEndDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public void startUpdate()
        {
            loadMiniVersion();
        }

        public void loadMiniVersion()
        {
            Ctx.mInstance.mVersionSys.mMiniLoadResultDisp.addEventHandle(null, miniVerLoadResult);
            Ctx.mInstance.mVersionSys.mLoadResultDisp.addEventHandle(null, verLoadResult);
            Ctx.mInstance.mVersionSys.loadMiniVerFile();
        }

        public void miniVerLoadResult(IDispatchObject dispObj)
        {
            // 本地文件版本必须要加载
            Ctx.mInstance.mVersionSys.loadVerFile();
        }

        public void verLoadResult(IDispatchObject idspObj)
        {
            if (Ctx.mInstance.mVersionSys.mNeedUpdateVerFile) // 如果需要更新
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
            foreach (KeyValuePair<string, FileVerInfo> kv in Ctx.mInstance.mVersionSys.mServerVer.mPath2HashDic)
            {
                if(Ctx.mInstance.mVersionSys.mLocalVer.mPath2Ver_P_Dic.ContainsKey(kv.Key))
                {
                    if(Ctx.mInstance.mVersionSys.mLocalVer.mPath2Ver_P_Dic[kv.Key].mFileMd5 != kv.Value.mFileMd5)
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
            this.mLoadingPath.Add(UtilLogic.webFullPath(path));
            if (Ctx.mInstance.mVersionSys.mLocalVer.mPath2Ver_P_Dic.ContainsKey(path))
            {
                UtilPath.deleteFile(Path.Combine(MFileSys.getLocalWriteDir(), UtilLogic.combineVerPath(path, Ctx.mInstance.mVersionSys.mLocalVer.mPath2Ver_P_Dic[path].mFileMd5)));     // 删除当前目录下已经有的 old 文件
            }
            //UtilApi.delFileNoVer(path);     // 删除当前目录下已经有的 old 文件

            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(path);

            param.mResLoadType = ResLoadType.eLoadWeb;
            param.mVersion = fileInfo.mFileMd5;

            param.mLoadEventHandle = onLoadEventHandle;

            Ctx.mInstance.mResLoadMgr.loadData(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.log(string.Format("AutoUpdateSys::onLoadEventHandle, Success, Path is {0}", (dispObj as DataResItem).getLoadPath()));

                this.mLoadedPath.Add((dispObj as DataResItem).getResUniqueId());
                this.mLoadingPath.Remove((dispObj as DataResItem).getResUniqueId());

                if (this.mLoadingPath.Count == 0)
                {
                    onUpdateEnd();
                }
            }
            else if (res.hasFailed())
            {
                Ctx.mInstance.mLogSys.log(string.Format("AutoUpdateSys::onLoadEventHandle, Fail, Path is {0}", (dispObj as DataResItem).getLoadPath()));

                this.mFailedPath.Add((dispObj as DataResItem).getResUniqueId());
                this.mLoadingPath.Remove((dispObj as DataResItem).getResUniqueId());

                if (this.mLoadingPath.Count == 0)
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