using SDK.Lib;
using System;
using UnityEngine;

/**
 * @brief 动画
 */

public class NumAniSeqBehaviour : MonoBehaviour
{
    public Action<NumAniBase> onAniEndDisp;

    // 一个动画播放完成
    public void onAniEnd(NumAniBase ani)
    {
        if (onAniEndDisp != null)
        {
            onAniEndDisp(ani);
        }
    }
}