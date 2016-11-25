using UnityEngine;

namespace SDK.Lib
{
    public class SoundParam : IRecycle
    {
        public string mPath = "";      // 音乐路径
        public bool mIsLoop = true;    // 是否循环播放
        public Transform mTrans;       // 播放音乐的对象的位置信息

        public void resetDefault()
        {
            mPath = "";
            mIsLoop = true;
            mTrans = null;
        }
    }
}