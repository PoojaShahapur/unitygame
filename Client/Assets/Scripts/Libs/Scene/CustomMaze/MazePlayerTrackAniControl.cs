using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class MazePlayerTrackAniControl
    {
        protected MazePlayer m_mazePlayer;

        protected NumAniParallel m_numAniParal;
        protected MList<MazePt> m_ptList;

        public MazePlayerTrackAniControl(MazePlayer mazePlayer_)
        {
            m_mazePlayer = mazePlayer_;
            m_numAniParal = new NumAniParallel();
            m_numAniParal.setAniSeqEndDisp(onMoveEndHandle);
            m_ptList = new MList<MazePt>();
        }

        public MList<MazePt> ptList
        {
            get
            {
                return m_ptList;
            }
            set
            {
                m_ptList = value;
            }
        }

        public void setStartPos()
        {
            m_mazePlayer.selfGo.transform.localPosition = m_ptList[0].pos;
            // 删除第一个点
            m_ptList.RemoveAt(0);
        }

        // 简单直接移动移动动画
        public void moveToDestPos(MazePt pt_)
        {
            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_mazePlayer.selfGo);
            posAni.destPos = pt_.pos;
            posAni.setTime(0.5f);

            m_numAniParal.play();
        }

        // 移动到下一个开始点，需要跳跃
        public void moveToDestPos(MazeStartPt pt_)
        {
            Vector3 srcPos = m_mazePlayer.selfGo.transform.localPosition;
            Vector3 destPos = pt_.pos;
            float time = 1;

            Vector3 midPt;      // 中间点
            midPt = (srcPos + destPos) / 2;
            midPt.y = 2;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniParal.addOneNumAni(curveAni);
            curveAni.setGO(m_mazePlayer.selfGo);
            curveAni.setTime(time);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, srcPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);
            curveAni.setAniEndDisp(onMove2DestEnd);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniParal.play();
        }


        // 移动结束回调
        protected void onMoveEndHandle(NumAniSeqBase dispObj)
        {
            if(m_ptList.Count() > 0)  // 说明还有 WayPoint 可以走
            {
                moveToDestPos(m_ptList[0]);
                m_ptList.RemoveAt(0);
            }
            else    // 如果运行到终点位置
            {
                Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIMaze);
            }
        }

        public void onMove2DestEnd(NumAniBase ani)
        {
            onMoveEndHandle(null);
        }
    }
}