using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    public class PosAni : ITweenAniBase
    {
        // 目标信息
        protected Vector3 m_destPos;       // 最终位置

        public Vector3 destPos
        {
            get
            {
                return m_destPos;
            }
            set
            {
                m_destPos = value;
            }
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

            args["position"] = m_destPos;
            args["time"] = m_time;
            args["islocal"] = true;

            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            incItweenCount();
            iTween.MoveTo(m_go, args);
        }
    }
}