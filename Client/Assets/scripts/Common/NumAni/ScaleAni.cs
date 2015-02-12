using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    public class ScaleAni : NumAniBase
    {
        // 目标信息
        protected Vector3 m_destScale;     // 最终缩放

        public ScaleAni()
        {

        }

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
            buildAniParam();
        }

        public override void stop()
        {

        }

        public override void pause()
        {

        }

        protected void buildAniParam()
        {
            Hashtable args;
            args = new Hashtable();
            base.buildAniBasicParam(args);

            args["scale"] = m_destScale;
            args["time"] = m_time;
            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            incItweenCount();
            iTween.ScaleTo(m_go, args);
        }
    }
}