using System;

namespace SDK.Lib
{
    /**
     * @brief 版本系统，文件格式   path=value
     */
    public class VersionSys
    {
        public ServerVer m_serverVer;
        public LocalVer m_localVer;

        public AddOnceAndCallOnceEventDispatch mMiniLoadResultDisp;
        public AddOnceAndCallOnceEventDispatch mLoadResultDisp;
        public bool m_needUpdateVerFile;

        public string m_miniVer;    // mini 版本文件版本号

        public VersionSys()
        {
            m_miniVer = UtilApi.Range(0, int.MaxValue).ToString();
            mMiniLoadResultDisp = new AddOnceAndCallOnceEventDispatch();
            mLoadResultDisp = new AddOnceAndCallOnceEventDispatch();

            m_serverVer = new ServerVer();
            m_localVer = new LocalVer();
        }

        public void loadMiniVerFile()
        {
            m_localVer.mMiniLoadedDisp.addEventHandle(null, onLocalMiniLoaded);
            m_localVer.loadMiniVerFile();
        }

        public void loadVerFile()
        {
            m_localVer.mLoadedDisp.addEventHandle(null, onVerLoaded);

            m_localVer.loadVerFile();
        }

        public void onLocalMiniLoaded(IDispatchObject dispObj)
        {
            if (m_localVer.mIsMiniLoadSuccess)
            {
                m_serverVer.mMiniLoadedDisp.addEventHandle(null, onWebMiniLoaded);
            }
            else
            {
                m_serverVer.mMiniLoadedDisp.addEventHandle(null, onWebMiniLoaded);
            }

            m_serverVer.loadMiniVerFile(m_miniVer);
        }

        public void onWebMiniLoaded(IDispatchObject dispObj)
        {
            if (m_serverVer.mIsMiniLoadSuccess)
            {
                // 删除旧 mini 版本，修改新版本文件名字
                //UtilPath.deleteFile(Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_P));
                // 修改新的版本文件名字
                //UtilPath.renameFile(UtilLogic.combineVerPath(Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI), m_miniVer), Path.Combine(MFileSys.getLocalWriteDir(), VerFileName.VER_MINI));

                m_needUpdateVerFile = (m_localVer.mFileVerInfo.m_fileMd5 != m_serverVer.mFileVerInfo.m_fileMd5);      // 如果版本不一致，需要重新加载
                                                                                                             //m_needUpdateVerFile = true;         // 测试强制更新
                mMiniLoadResultDisp.dispatchEvent(null);
            }
            else
            {

            }
        }

        public void onVerLoaded(IDispatchObject dispObj)
        {
            if (m_localVer.mIsVerLoadSuccess)
            {
                if (m_needUpdateVerFile)
                {
                    m_serverVer.mLoadedDisp.addEventHandle(null, onWebVerLoaded);
                    string ver = m_serverVer.mFileVerInfo.m_fileMd5;
                    m_serverVer.loadVerFile(ver);
                }
                else
                {
                    mLoadResultDisp.dispatchEvent(null);
                }
            }
            else
            {
                if (m_needUpdateVerFile)
                {
                    m_serverVer.mLoadedDisp.addEventHandle(null, onWebVerLoaded);
                    string ver = m_serverVer.mFileVerInfo.m_fileMd5;
                    m_serverVer.loadVerFile(ver);
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
            if(m_needUpdateVerFile)
            {
                if (m_serverVer.m_path2HashDic.ContainsKey(path))
                {
                    return m_serverVer.m_path2HashDic[path].m_fileMd5;
                }
            }
            else
            {
                if (m_localVer.m_path2Ver_P_Dic.ContainsKey(path))
                {
                    return m_localVer.m_path2Ver_P_Dic[path].m_fileMd5;
                }
            }

            return "";
        }

        public void loadLocalVer()
        {
            m_localVer.load();
        }
    }
}