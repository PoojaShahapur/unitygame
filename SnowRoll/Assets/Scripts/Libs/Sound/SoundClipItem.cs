using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 直接加载声音文件
     */
    public class SoundClipItem : SoundItem
    {
        public AudioClip mClip;            // 声音资源放在 prefab 中国
        protected bool mIsLoaded;           // 资源是否加载完成

        public SoundClipItem()
        {
            this.mIsLoaded = false;
        }

        public override void setResObj(UnityEngine.Object go_)
        {
            this.mClip = go_ as AudioClip;
            this.mGo = UtilApi.createGameObject("SoundGO");

            if (this.mClip == null)
            {
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

            base.unload();
        }

        override public void Play()
        {
            //if(AudioDataLoadState.Loaded != this.mClip.loadState)
            if(!this.mIsLoaded)
            {
                Ctx.mInstance.mSoundLoadStateCheckMgr.addSound(this);
            }

            base.Play();
        }

        // 检查加载状态
        override protected void checkLoadState()
        {
            if (AudioDataLoadState.Loaded == this.mClip.loadState)
            {
                this.mIsLoaded = true;
                this.Play();
                Ctx.mInstance.mSoundLoadStateCheckMgr.removeSound(this);
            }
        }
    }
}