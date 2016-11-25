using UnityEngine;

namespace SDK.Lib
{
    public enum SoundPlayState
    {
        eSS_None,           // 默认状态
        eSS_Play,           // 播放状态
        eSS_Stop,           // 暂停状态
        eSS_Pause,           // 停止状态
    }

    public enum SoundResType
    {
        eSRT_Prefab,
        eSRT_Clip,
    }

    /**
     * @brief 音乐和音效都是这个类
     */
    public class SoundItem
    {
        public string mPath;           // 资源目录
        public SoundResType mSoundResType = SoundResType.eSRT_Prefab;
        protected SoundPlayState mPlayState = SoundPlayState.eSS_None;      // 音乐音效播放状态
        public Transform mTrans;       // 位置信息
        public GameObject mGo;         // audio 组件 GameObject 对象
        public AudioSource mAudio;             // 音源
        public bool mPlayOnStart = true;

        public ulong mDelay = 0;
        public bool mBypassEffects = false;        // 是否开启音频特效
        public bool mMute = false;         // 是否静音
        public bool mIsLoop = false;        // 是否循环播放
        public float mVolume = 1.0f;
        public float mPitch = 1.0f;
        public bool mScaleOutputVolume = true;

        public SoundItem()
        {

        }

        public bool bInCurState(SoundPlayState state)
        {
            return mPlayState == state;
        }

        public virtual void setResObj(UnityEngine.Object go_)
        {

        }

        public void initParam(SoundParam soundParam)
        {
            mTrans = soundParam.mTrans;
            mIsLoop = soundParam.mIsLoop;
            mPath = soundParam.mPath;
        }

        protected void updateParam()
        {
            if (mTrans != null)
            {
                mGo.transform.position = mTrans.position;
            }
            mAudio = mGo.GetComponent<AudioSource>();
            //mAudio.rolloffMode = AudioRolloffMode.Logarithmic;
            mAudio.loop = mIsLoop;
            //mAudio.dopplerLevel = 0f;
            //mAudio.spatialBlend = 0f;
            volume = mVolume;

            //mAudio.minDistance = 1.0f;
            //mAudio.maxDistance = 50;
        }

        public float volume
        {
            get 
            { 
                return mVolume; 
            }
            set
            {
                if (mScaleOutputVolume)
                {
                    mAudio.volume = ScaleVolume(value);
                }
                else
                {
                    mAudio.volume = value;
                }
                mVolume = value;
            }
        }

        public float pitch
        {
            get 
            {
                return mPitch; 
            }
            set
            {
                mAudio.pitch = value;
                mPitch = value;
            }
        }

        void Start()
        {
            if (mPlayOnStart)
            {
                Play();
            }
        }

        public void Pause()
        {
            mPlayState = SoundPlayState.eSS_Pause;
            mAudio.Pause();
        }

        public void Play()
        {
            if (SoundPlayState.eSS_Pause == mPlayState)
            {
                mAudio.UnPause();
            }
            else
            {
                mAudio.Play(mDelay);
            }

            mPlayState = SoundPlayState.eSS_Play;
        }

        public void Stop()
        {
            mPlayState = SoundPlayState.eSS_Stop;
            mAudio.Stop();
        }

        public void SetPitch(float p)
        {
            pitch = p;
        }

        // TODO: we should consider using this dB scale as an option when porting these changes
        // over to unity-bowerbird: http://wiki.unity3d.com/index.php?title=Loudness
        /*
        * Quadratic scaling of actual volume used by AudioSource. Approximates the proper exponential.
        */
        public float ScaleVolume(float v)
        {
            v = Mathf.Pow(v, 4);
            return Mathf.Clamp(v, 0f, 1f);
        }

        // 卸载
        public virtual void unload()
        {
            
        }

        public bool isEnd()
        {
            if (SoundPlayState.eSS_Play == mPlayState)     // 如果处于播放状态的
            {
                return !mAudio.isPlaying;
            }

            return false;
        }
    }
}