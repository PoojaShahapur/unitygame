using System;
using UnityEngine;

namespace SDK.Lib
{
    public class MoveControl : ControlBase
    {
        protected NumAniSequence m_numAniSeq;       // 动画序列

        public MoveControl(SceneEntityBase entity) :
            base(entity)
        {
            m_numAniSeq = new NumAniSequence();
        }

        // 移动动画
        public void moveToDest(Vector3 srcPos, Vector3 destPos, float time, Action<NumAniBase> handle)
        {
            Vector3 midPt;      // 中间点
            midPt = (srcPos + destPos) / 2;
            midPt.y = 2;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(mEntity.gameObject());
            curveAni.setTime(time);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, srcPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);
            if (handle != null)
            {
                curveAni.setAniEndDisp(handle);
            }

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniSeq.play();
        }
    }
}