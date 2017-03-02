using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 直接加载声音文件
     */
    public class SoundClipItem : SoundItem
    {
        public AudioClip mClip;            // 声音资源放在 prefab 中国
        protected bool mIsLoaded;          // 资源是否加载完成
        protected bool mIsDontDestroy;     // 是否不销毁

        public SoundClipItem()
        {
            this.mIsLoaded = false;
            this.mIsDontDestroy = true;
        }

        public override void setResObj(UnityEngine.Object go_)
        {
            if (MacroDef.ENABLE_LOG)
            {
                if (go_)
                {
                    Ctx.mInstance.mLogSys.log("SoundClipItem::setResObj, go_ no null", LogTypeId.eLogMusicBug);
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("SoundClipItem::setResObj, go_ is null", LogTypeId.eLogMusicBug);
                }
            }

            this.mClip = go_ as AudioClip;
            this.mGo = UtilApi.createGameObject("SoundGO");

            if(this.mIsDontDestroy)
            {
                UtilApi.DontDestroyOnLoad(this.mGo);
            }

            if (this.mClip == null)
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("SoundClipItem::setResObj, Clip is null", LogTypeId.eLogMusicBug);
                }

                return;
            }

            this.mAudio = mGo.GetComponent<AudioSource>();

            if (this.mAudio == null)
            {
                this.mAudio = (AudioSource)mGo.AddComponent<AudioSource>();
            }

            this.mAudio.clip = mClip;

            this.updateParam();
        }

        public override void unload()
        {
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log("SoundClipItem::unload", LogTypeId.eLogMusicBug);
            }

            Ctx.mInstance.mSoundLoadStateCheckMgr.removeSound(this);

            if (this.isInCurState(SoundPlayState.eSS_Play))
            {
                this.Stop();
            }

            if (this.mGo != null)
            {
                //mClip.UnloadAudioData();
                UtilApi.Destroy(this.mGo);
                //UtilApi.UnloadUnusedAssets();
                this.mGo = null;
            }

            this.mClip = null;

            base.unload();
        }

        override public void Play()
        {
            //if(AudioDataLoadState.Loaded != this.mClip.loadState)
            if(!this.mIsLoaded)
            {
                if (MacroDef.ENABLE_LOG)
                {
                    if (null != this.mClip)
                    {
                        Ctx.mInstance.mLogSys.log("SoundClipItem::Play, Clip not null", LogTypeId.eLogMusicBug);
                    }
                    else
                    {
                        Ctx.mInstance.mLogSys.log("SoundClipItem::Play, Clip is null", LogTypeId.eLogMusicBug);
                    }
                }

                Ctx.mInstance.mSoundLoadStateCheckMgr.addSound(this);
            }

            base.Play();
        }

        // 检查加载状态
        override protected void checkLoadState()
        {
            if (null != this.mClip && AudioDataLoadState.Loaded == this.mClip.loadState)
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("SoundClipItem::checkLoadState, set", LogTypeId.eLogMusicBug);
                }

                this.mIsLoaded = true;
                this.Play();
                Ctx.mInstance.mSoundLoadStateCheckMgr.removeSound(this);
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    if (null == this.mClip)
                    {
                        Ctx.mInstance.mLogSys.log("SoundClipItem::checkLoadState, Clip is null", LogTypeId.eLogMusicBug);

                        GameObject go = UtilApi.GoFindChildByName("SoundGO");

                        if(null == go)
                        {
                            Ctx.mInstance.mLogSys.log("SoundClipItem::checkLoadState, GameObject is destroy", LogTypeId.eLogMusicBug);
                        }
                    }
                }

                Ctx.mInstance.mSoundLoadStateCheckMgr.removeSound(this);
            }
        }
    }
}