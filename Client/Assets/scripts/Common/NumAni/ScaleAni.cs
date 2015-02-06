using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    public class ScaleAni : NumAniBase
    {
        // 目标信息
        protected Vector3 m_destScale;     // 最终缩放

        public Vector3 destScale
        {
            set
            {
                m_destScale = value;
            }
        }

        public override void play()
        {
            base.play();
            Hashtable args = new Hashtable();
            buildAniParam(args);
        }

        public override void stop()
        {

        }

        public override void pause()
        {

        }

        protected override void buildAniParam(Hashtable args)
        {
            base.buildAniParam(args);

            args = new Hashtable();
            args["scale"] = m_destScale;
            args["time"] = m_time;
            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            iTween.ScaleTo(m_go, args);
        }
    }
}