using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 数字
     */
    public class RSTAni : NumAniBase
    {
        // 目标信息
        protected Vector3 m_destPos;       // 最终位置
        protected Vector3 m_destRot;       // 最终旋转
        protected Vector3 m_destScale;     // 最终缩放

        public RSTAni()
        {

        }

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

        public Vector3 destRot
        {
            set
            {
                m_destRot = value;
            }
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

            args["position"] = m_destPos;
            args["time"] = m_time;
            args["islocal"] = true;

            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            //args["method"] = "to";
            //args["type"] = "color";
            iTween.MoveTo(m_go, args);

            args = new Hashtable();
            args["rotation"] = m_destRot;
            args["time"] = m_time;
            args["islocal"] = true;
            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            iTween.RotateTo(m_go, args);

            args = new Hashtable();
            args["scale"] = m_destScale;
            args["time"] = m_time;
            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            iTween.ScaleTo(m_go, args);
        }
    }
}