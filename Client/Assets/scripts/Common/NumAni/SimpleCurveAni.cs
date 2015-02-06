using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 简单的曲线
     */
    public class SimpleCurveAni : NumAniBase
    {
        protected Vector3[] m_plotPtList;           // 控制点数组

        public SimpleCurveAni()
        {
            m_plotPtList = new Vector3[3];
            m_plotPtList[0] = new Vector3(1, 1, 1);
            m_plotPtList[1] = new Vector3(2, 2, 2);
            m_plotPtList[2] = new Vector3(3, 3, 3);
        }

        public override void play()
        {
            base.play();

            Hashtable args = new Hashtable();
            buildAniParam(args);
        }

        protected override void buildAniParam(Hashtable args)
        {
            base.buildAniParam(args);

            args["path"] = m_plotPtList;
            args["time"] = 5;
            args["islocal"] = true;

            //args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            iTween.MoveTo(m_go, args);
        }
    }
}