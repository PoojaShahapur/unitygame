using System;

namespace SDK.Lib
{
    /**
     * @brief 版本系统，文件格式   path=value
     */
    public class VersionSys
    {
        public ServerVer mServerVer;
        public LocalVer mLocalVer;

        public AddOnceAndCallOnceEventDispatch mMiniLoadResultDisp;
        public AddOnceAndCallOnceEventDispatch mLoadResultDisp;
        public bool mNeedUpdateVerFile;

        public string mMiniVer;    // mini 版本文件版本号

        public VersionSys()
        {
            this.mMiniVer = UtilApi.Range(0, int.MaxValue).ToString();
            this.mMiniLoadResultDisp = new AddOnceAndCallOnceEventDispatch();
            this.mLoadResultDisp = new AddOnceAndCallOnceEventDispatch();

            this.mServerVer = new ServerVer();
            this.mLocalVer = new LocalVer();
        }

        public void loadMiniVerFile()
        {
            this.mLocalVer.mMiniLoadedDisp.addEventHandle(null, onLocalMiniLoaded);
            this.mLocalVer.loadMiniVerFile();
        }

        public void loadVerFile()
        {
            this.mLocalVer.mLoadedDisp.addEventHandle(null, onVerLoaded);

            this.mLocalVer.loadVerFile();
        }

        public void onLocalMiniLoaded(IDispatchObject dispObj)
        {
            if (this.mLocalVer.mIsMiniLoadSuccess)
            {
                this.mServerVer.mMiniLoadedDisp.addEventHandle(null, onWebMiniLoaded);
            }
            else
            {
                this.mServerVer.mMiniLoadedDisp.addEventHandle(null, onWebMiniLoaded);
            }

            this.mServerVer.loadMiniVerFile(this.mMiniVer);
        }

        public void onWebMiniLoaded(IDispatchObject dispObj)
        {
            if (this.mServerVer.mIsMiniLoadSuccess)
            {
                // 删除旧 mini 版本，修改新版本文件名字
                //UtilPath.deleteFile(Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_P));
                // 修改新的版本文件名字
                //UtilPath.renameFile(UtilLogic.combineVerPath(Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI), m_miniVer), Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI));

                this.mNeedUpdateVerFile = (this.mLocalVer.mFileVerInfo.mFileMd5 != this.mServerVer.mFileVerInfo.mFileMd5);      // 如果版本不一致，需要重新加载
                                                                                                             //m_needUpdateVerFile = true;         // 测试强制更新
                mMiniLoadResultDisp.dispatchEvent(null);
            }
            else
            {

            }
        }

        public void onVerLoaded(IDispatchObject dispObj)
        {
            if (this.mLocalVer.mIsVerLoadSuccess)
            {
                if (this.mNeedUpdateVerFile)
                {
                    this.mServerVer.mLoadedDisp.addEventHandle(null, onWebVerLoaded);
                    string ver = this.mServerVer.mFileVerInfo.mFileMd5;
                    this.mServerVer.loadVerFile(ver);
                }
                else
                {
                    this.mLoadResultDisp.dispatchEvent(null);
                }
            }
            else
            {
                if (this.mNeedUpdateVerFile)
                {
                    this.mServerVer.mLoadedDisp.addEventHandle(null, onWebVerLoaded);
                    string ver = this.mServerVer.mFileVerInfo.mFileMd5;
                    this.mServerVer.loadVerFile(ver);
                }
                else
                {
                    mLoadResultDisp.dispatchEvent(null);
                }
            }
        }

        public void onWebVerLoaded(IDispatchObject dispObj)
        {
            mLoadResultDisp.dispatchEvent(null);
        }

        public string getFileVer(string path)
        {
            if(this.mNeedUpdateVerFile)
            {
                if (this.mServerVer.mPath2HashDic.ContainsKey(path))
                {
                    return this.mServerVer.mPath2HashDic[path].mFileMd5;
                }
            }
            else
            {
                if (this.mLocalVer.mPath2Ver_P_Dic.ContainsKey(path))
                {
                    return this.mLocalVer.mPath2Ver_P_Dic[path].mFileMd5;
                }
            }

            return "";
        }

        public void loadLocalVer()
        {
            this.mLocalVer.load();
        }
    }
}