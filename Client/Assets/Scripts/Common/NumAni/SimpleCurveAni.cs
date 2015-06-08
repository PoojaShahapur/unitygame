using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 简单的曲线
     */
    public class SimpleCurveAni : ITweenAniBase
    {
        protected Vector3[] m_plotPtList;           // 控制点数组

        public SimpleCurveAni()
        {
            //m_plotPtList = new Vector3[3];
            //m_plotPtList[0] = new Vector3(1, 1, 1);
            //m_plotPtList[1] = new Vector3(2, 2, 2);
            //m_plotPtList[2] = new Vector3(3, 3, 3);
        }

        public void setPlotCount(int cnt)
        {
            m_plotPtList = new Vector3[cnt];
        }

        public void addPlotPt(int idx, Vector3 pt)
        {
            m_plotPtList[idx] = pt;
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

            args["path"] = m_plotPtList;
            args["time"] = m_time;
            args["islocal"] = true;

            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            incItweenCount();
            iTween.MoveTo(m_go, args);
        }
    }
}