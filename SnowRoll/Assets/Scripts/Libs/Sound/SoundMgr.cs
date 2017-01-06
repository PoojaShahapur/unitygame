using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 播放音乐和音效
     */
    public class SoundMgr 
    {
        protected MList<SoundItem> mAudioList;
        protected MDictionary<string, SoundItem> mPath2SoundDic;
        protected TimerItemBase mTimer;
        protected MList<SoundItem> mClearList;

        public SoundMgr()
        {
            this.mAudioList = new MList<SoundItem>();
            this.mPath2SoundDic = new MDictionary<string, SoundItem>();
            this.mClearList = new MList<SoundItem>();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void play(SoundParam soundParam)
        {
            if (!soundParam.mIsLoop)
            {
                addTimer();
            }

            soundParam.mPath = Ctx.mInstance.mPakSys.getCurResPakPathByResPath(soundParam.mPath, null);

            if (mPath2SoundDic.ContainsKey(soundParam.mPath))      // 如果已经有了直接返回
            {
                if (!mPath2SoundDic[soundParam.mPath].bInCurState(SoundPlayState.eSS_Play))
                {
                    mPath2SoundDic[soundParam.mPath].Play();
                }
            }
            else
            {
                // 创建
                if (isPrefab(soundParam.mPath))
                {
                    mPath2SoundDic[soundParam.mPath] = new SoundPrefabItem();
                    mPath2SoundDic[soundParam.mPath].mSoundResType = SoundResType.eSRT_Prefab;
                }
                else
                {
                    mPath2SoundDic[soundParam.mPath] = new SoundClipItem();
                    mPath2SoundDic[soundParam.mPath].mSoundResType = SoundResType.eSRT_Clip;
                }
                mAudioList.Add(mPath2SoundDic[soundParam.mPath]);
                mPath2SoundDic[soundParam.mPath].initParam(soundParam);

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
            if (mPath2SoundDic.ContainsKey(path))
            {
                //mPath2SoundDic[path].mIsLoop = loop_;
                mPath2SoundDic[path].Play();
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

                if (mPath2SoundDic.ContainsKey(res.getResUniqueId()))      // 如果有，说明还没被停止
                {
                    if (mPath2SoundDic[res.getResUniqueId()].mSoundResType == SoundResType.eSRT_Prefab)
                    {
                        mPath2SoundDic[res.getResUniqueId()].setResObj(res.InstantiateObject(res.getPrefabName()));
                    }
                    else
                    {
                        mPath2SoundDic[res.getResUniqueId()].setResObj(res.getObject(res.getPrefabName()));
                    }
                }
                // 播放音乐
                play(res.getResUniqueId());
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.getLoadPath());
                delSoundItem(mPath2SoundDic[res.getResUniqueId()]);
            }
            // 卸载数据
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);
        }

        protected void unload(string path)
        {
            if (mPath2SoundDic.ContainsKey(path))
            {
                mPath2SoundDic[path].unload();
                delSoundItem(mPath2SoundDic[path]);
            }
        }

        // 不要遍历中使用这个函数
        protected void delSoundItem(SoundItem item)
        {
            mPath2SoundDic.Remove(item.mPath);
            mAudioList.Remove(item);
        }

        // 定时释放资源
        public void onTimer(TimerItemBase time)
        {
            bool hasNoLoop = false;
            // 遍历看有没有播放完成的
            foreach(SoundItem sound in mAudioList.list())
            {
                if(sound.isEnd())
                {
                    mClearList.Add(sound);
                }
                else if (!sound.mIsLoop)
                {
                    hasNoLoop = true;
                }
            }

            foreach(SoundItem sound in mClearList.list())
            {
                unload(sound.mPath);
            }

            mClearList.Clear();

            if (!hasNoLoop)
            {
                this.mTimer.stopTimer();
            }
        }

        public void addTimer()
        {
            if (this.mTimer == null)
            {
                this.mTimer = new TimerItemBase();
                this.mTimer.mInternal = 3;        // 一分钟遍历一次
                this.mTimer.mIsInfineLoop = true;
                this.mTimer.mTimerDisp.setFuncObject(onTimer);
            }

            // 检查是否要加入定时器
            this.mTimer.startTimer();
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
            if (mTimer != null)
            {
                mTimer.stopTimer();
            }

            // 遍历看有没有播放完成的
            foreach (SoundItem sound in mAudioList.list())
            {
                sound.unload();
            }

            mAudioList.Clear();
            mPath2SoundDic.Clear();
        }
    }
}