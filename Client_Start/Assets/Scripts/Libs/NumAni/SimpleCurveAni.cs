using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 简单的曲线
     */
    public class SimpleCurveAni : ITweenAniBase
    {
        protected Vector3[] mPlotPtList;           // 控制点数组

        public SimpleCurveAni()
        {
            //mPlotPtList = new Vector3[3];
            //mPlotPtList[0] = new Vector3(1, 1, 1);
            //mPlotPtList[1] = new Vector3(2, 2, 2);
            //mPlotPtList[2] = new Vector3(3, 3, 3);
        }

        public void setPlotCount(int cnt)
        {
            mPlotPtList = new Vector3[cnt];
        }

        public void addPlotPt(int idx, Vector3 pt)
        {
            mPlotPtList[idx] = pt;
        }

        public override void play()
        {
            base.play();
            buildAniParam();
        }

        protected void buildAniParam()
        {
            Hashtable args;
            args = new Hashtable();
            base.buildAniBasicParam(args);

            args["path"] = mPlotPtList;
            args["time"] = m_time;
            args["islocal"] = true;

            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            incItweenCount();
            iTween.MoveTo(mGo, args);
        }
    }
}