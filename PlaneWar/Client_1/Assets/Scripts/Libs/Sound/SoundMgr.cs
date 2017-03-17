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
                this.addTimer();
            }

            //soundParam.mPath = Ctx.mInstance.mPakSys.getCurResPakPathByResPath(soundParam.mPath, null);

            if (this.mPath2SoundDic.ContainsKey(soundParam.mPath))      // 如果已经有了直接返回
            {
                if (!this.mPath2SoundDic[soundParam.mPath].isInCurState(SoundPlayState.eSS_Play))
                {
                    this.mPath2SoundDic[soundParam.mPath].Play();
                }
            }
            else
            {
                // 创建
                if (this.isPrefab(soundParam.mPath))
                {
                    this.mPath2SoundDic[soundParam.mPath] = new SoundPrefabItem();
                    this.mPath2SoundDic[soundParam.mPath].mSoundResType = SoundResType.eSRT_Prefab;
                }
                else
                {
                    this.mPath2SoundDic[soundParam.mPath] = new SoundClipItem();
                    this.mPath2SoundDic[soundParam.mPath].mSoundResType = SoundResType.eSRT_Clip;
                }

                this.mAudioList.Add(mPath2SoundDic[soundParam.mPath]);
                this.mPath2SoundDic[soundParam.mPath].initParam(soundParam);

                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(soundParam.mPath);
                param.mLoadEventHandle = this.onLoadEventHandle;
                param.mLoadNeedCoroutine = false;
                param.mResNeedCoroutine = false;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        public void play(string path, bool loop_ = true)
        {
            if(MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("SoundMgr::play, path = {0}", path), LogTypeId.eLogMusicBug);
            }

            if (this.mPath2SoundDic.ContainsKey(path))
            {
                //mPath2SoundDic[path].mIsLoop = loop_;
                this.mPath2SoundDic[path].Play();
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
            //path = Ctx.mInstance.mPakSys.getCurResPakPathByResPath(path, null);
            this.unload(path);
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;

            string logicPath = res.getLogicPath();
            string uniqueId = res.getResUniqueId();

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("SoundMgr::onLoadEventHandle, Success, path = {0}", res.getLoadPath()), LogTypeId.eLogMusicBug);
                }

                if (this.mPath2SoundDic.ContainsKey(logicPath))      // 如果有，说明还没被停止
                {
                    this.mPath2SoundDic[logicPath].mUniqueId = uniqueId;

                    if (this.mPath2SoundDic[logicPath].mSoundResType == SoundResType.eSRT_Prefab)
                    {
                        this.mPath2SoundDic[logicPath].setResObj(res.InstantiateObject(res.getPrefabName()));
                    }
                    else
                    {
                        this.mPath2SoundDic[logicPath].setResObj(res.getObject(res.getPrefabName()));
                    }
                }

                // 播放音乐
                this.play(logicPath);
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("SoundMgr::onLoadEventHandle, Fail, path = {0}", res.getLoadPath()), LogTypeId.eLogMusicBug);
                }

                this.delSoundItem(this.mPath2SoundDic[logicPath]);

                Ctx.mInstance.mResLoadMgr.unload(uniqueId, this.onLoadEventHandle);
            }

            // 卸载数据，等到不用的时候在卸载，在台式机上没有问题，在手机上会导致 SoundClipItem::mClip 变成 null
            //Ctx.mInstance.mResLoadMgr.unload(uniqueId, this.onLoadEventHandle);
        }

        protected void unload(string path)
        {
            if (this.mPath2SoundDic.ContainsKey(path))
            {
                string uniqueId = this.mPath2SoundDic[path].mUniqueId;

                this.mPath2SoundDic[path].unload();
                this.delSoundItem(this.mPath2SoundDic[path]);

                Ctx.mInstance.mResLoadMgr.unload(uniqueId, this.onLoadEventHandle);
            }
        }

        // 不要遍历中使用这个函数
        protected void delSoundItem(SoundItem item)
        {
            this.mPath2SoundDic.Remove(item.mPath);
            this.mAudioList.Remove(item);
        }

        // 定时释放资源
        public void onTimer(TimerItemBase time)
        {
            bool hasNoLoop = false;
            // 遍历看有没有播放完成的
            foreach(SoundItem sound in this.mAudioList.list())
            {
                if(sound.isEnd())
                {
                    this.mClearList.Add(sound);
                }
                else if (!sound.mIsLoop)
                {
                    hasNoLoop = true;
                }
            }

            foreach(SoundItem sound in mClearList.list())
            {
                this.unload(sound.mPath);
            }

            this.mClearList.Clear();

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
            if (this.mTimer != null)
            {
                this.mTimer.stopTimer();
            }

            // 遍历看有没有播放完成的
            foreach (SoundItem sound in this.mAudioList.list())
            {
                sound.unload();
            }

            this.mAudioList.Clear();
            this.mPath2SoundDic.Clear();
        }
    }
}