using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SDK.Lib
{
    /**
     * @brief 视频剪辑播放管理器
     */
    public class MovieMgr
    {
        protected bool m_bPlaying;
#if UNITY_STANDALONE || UNITY_EDITOR
        protected MovieTexture mMovieTexture;
#endif
        protected TimerItemBase mTimer;
        protected Action mHandle;

        public MovieMgr()
        {
            m_bPlaying = false;
        }

        public void PlayMovie(string movieName, int time, Action acton)
        {
            this.mHandle = acton;
#if UNITY_EDITOR
            m_bPlaying = true;
            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath("Assets/res/Movie/" + movieName + ".mp4", typeof(MovieTexture));
            if(o != null)
            {
                this.mMovieTexture = o as MovieTexture;
                this.mMovieTexture.Play();
            }
#elif UNITY_STANDALONE
            //UnityEngine.Handheld.PlayFullScreenMovie("Movie/" + movieName + ".mp4", Color.black, FullScreenMovieControlMode.Full | FullScreenMovieControlMode.Hidden);
#endif

            if (this.mTimer == null)
            {
                this.mTimer = new TimerItemBase();
            }
            else
            {
                this.mTimer.reset();
            }
            this.mTimer.mInternal = time;        // 一分钟遍历一次
            this.mTimer.mTimerDisp.setFuncObject(OnPlayEnd);
            Ctx.mInstance.mTimerMgr.addTimer(this.mTimer);
        }

        // 视频结束，定时器回调
        protected void OnPlayEnd(TimerItemBase time)
        {
            if (this.mHandle != null)
            {
                this.mHandle();
            }
#if UNITY_STANDALONE || UNITY_EDITOR
            if (this.mMovieTexture != null)
            {
                this.mMovieTexture.Stop();
                this.mMovieTexture = null;
            }
#endif
        }

        // 绘制视频纹理到窗口上
        void OnGUI()
        {
#if UNITY_EDITOR
            if(m_bPlaying && this.mMovieTexture != null)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.mMovieTexture, ScaleMode.ScaleToFit);
            }
#endif
        }
    }
}