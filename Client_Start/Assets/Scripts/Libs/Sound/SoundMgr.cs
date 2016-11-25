using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 播放音乐和音效
     */
    public class SoundMgr 
    {
        protected List<SoundItem> m_audioList = new List<SoundItem>();
        protected Dictionary<string, SoundItem> m_path2SoundDic = new Dictionary<string,SoundItem>();
        protected TimerItemBase m_timer;
        protected List<SoundItem> m_clearList = new List<SoundItem>();

        public SoundMgr()
        {

        }

        public void play(SoundParam soundParam)
        {
            if (!soundParam.mIsLoop)
            {
                addTimer();
            }

            soundParam.mPath = Ctx.mInstance.mPakSys.getCurResPakPathByResPath(soundParam.mPath, null);

            if (m_path2SoundDic.ContainsKey(soundParam.mPath))      // 如果已经有了直接返回
            {
                if (!m_path2SoundDic[soundParam.mPath].bInCurState(SoundPlayState.eSS_Play))
                {
                    m_path2SoundDic[soundParam.mPath].Play();
                }
            }
            else
            {
                // 创建
                if (isPrefab(soundParam.mPath))
                {
                    m_path2SoundDic[soundParam.mPath] = new SoundPrefabItem();
                    m_path2SoundDic[soundParam.mPath].mSoundResType = SoundResType.eSRT_Prefab;
                }
                else
                {
                    m_path2SoundDic[soundParam.mPath] = new SoundClipItem();
                    m_path2SoundDic[soundParam.mPath].mSoundResType = SoundResType.eSRT_Clip;
                }
                m_audioList.Add(m_path2SoundDic[soundParam.mPath]);
                m_path2SoundDic[soundParam.mPath].initParam(soundParam);

                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(soundParam.mPath);
                param.mLoadEventHandle = onLoadEventHandle;
                param.mLoadNeedCoroutine = false;
                param.mResNeedCoroutine = false;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        public void play(string path, bool loop_ = true)
        {
            if (m_path2SoundDic.ContainsKey(path))
            {
                //m_path2SoundDic[path].mIsLoop = loop_;
                m_path2SoundDic[path].Play();
            }
            else
            {
                SoundParam param = Ctx.mInstance.mPoolSys.newObject<SoundParam>();
                param.mPath = path;
                param.mIsLoop = loop_;
                play(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        public void stop(string path)
        {
            path = Ctx.mInstance.mPakSys.getCurResPakPathByResPath(path, null);
            unload(path);
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.getLoadPath());

                if (m_path2SoundDic.ContainsKey(res.getResUniqueId()))      // 如果有，说明还没被停止
                {
                    if (m_path2SoundDic[res.getResUniqueId()].mSoundResType == SoundResType.eSRT_Prefab)
                    {
                        m_path2SoundDic[res.getResUniqueId()].setResObj(res.InstantiateObject(res.getPrefabName()));
                    }
                    else
                    {
                        m_path2SoundDic[res.getResUniqueId()].setResObj(res.getObject(res.getPrefabName()));
                    }
                }
                // 播放音乐
                play(res.getResUniqueId());
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.getLoadPath());
                delSoundItem(m_path2SoundDic[res.getResUniqueId()]);
            }
            // 卸载数据
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);
        }

        protected void unload(string path)
        {
            if (m_path2SoundDic.ContainsKey(path))
            {
                m_path2SoundDic[path].unload();
                delSoundItem(m_path2SoundDic[path]);
            }
        }

        // 不要遍历中使用这个函数
        protected void delSoundItem(SoundItem item)
        {
            m_path2SoundDic.Remove(item.mPath);
            m_audioList.Remove(item);
        }

        // 定时释放资源
        public void onTimer(TimerItemBase time)
        {
            bool hasNoLoop = false;
            // 遍历看有没有播放完成的
            foreach(SoundItem sound in m_audioList)
            {
                if(sound.isEnd())
                {
                    m_clearList.Add(sound);
                }
                else if (!sound.mIsLoop)
                {
                    hasNoLoop = true;
                }
            }

            foreach(SoundItem sound in m_clearList)
            {
                unload(sound.mPath);
            }

            m_clearList.Clear();

            if (!hasNoLoop)
            {
                Ctx.mInstance.mTimerMgr.removeTimer(m_timer);
            }
        }

        public void addTimer()
        {
            if (this.m_timer == null)
            {
                this.m_timer = new TimerItemBase();
                this.m_timer.mInternal = 3;        // 一分钟遍历一次
                this.m_timer.mIsInfineLoop = true;
                this.m_timer.mTimerDisp.setFuncObject(onTimer);
            }

            // 检查是否要加入定时器
            Ctx.mInstance.mTimerMgr.addTimer(m_timer);
        }

        protected bool isPrefab(string path)
        {
            if (path.Substring(path.IndexOf(".") + 1) == "prefab")
            {
                return true;
            }

            return false;
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            if (m_timer != null)
            {
                Ctx.mInstance.mTimerMgr.removeTimer(m_timer);
            }

            // 遍历看有没有播放完成的
            foreach (SoundItem sound in m_audioList)
            {
                sound.unload();
            }

            m_audioList.Clear();
            m_path2SoundDic.Clear();
        }
    }
}