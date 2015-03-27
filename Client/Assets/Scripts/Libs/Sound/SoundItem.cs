using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 音乐和音效都是这个类
     */
    public class SoundItem
    {
        public GameObject m_go;
        public AudioClip tracks;
        public AudioSource audio;
        public bool playOnStart = true;

        public int defaultTrack = 0;
        public string preferenceName = "music_volume";
        public bool initialVolumeFromPreference = true;
        public float _volume = 1.0f;
        public float _pitch = 1.0f;
        public bool ScaleOutputVolume = true;

        public SoundItem()
        {
            volume = _volume;
            AudioSource audioSrc = m_go.GetComponent<AudioSource>();
            audioSrc.rolloffMode = AudioRolloffMode.Linear;
            audioSrc.loop = true;
            audioSrc.dopplerLevel = 0f;
            //audioSrc.spatialBlend = 0f;
        }

        public float volume
        {
            get { return _volume; }
            set
            {
                if (ScaleOutputVolume)
                {
                    audio.volume = ScaleVolume(value);
                }
                else
                {
                    audio.volume = value;
                }
                _volume = value;
            }
        }

        public float pitch
        {
            get { return _pitch; }
            set
            {
                audio.pitch = value;
                _pitch = value;
            }
        }

        void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        public void Pause()
        {
            audio.Pause();
        }

        public void Play()
        {
            audio.Play();
        }

        public void Stop()
        {
            audio.Stop();
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
    }
}