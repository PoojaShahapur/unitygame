using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class RotAni : ITweenAniBase
    {
        // 目标信息
        protected Vector3 m_destRot;       // 最终旋转

        public RotAni()
        {

        }

        public Vector3 destRot
        {
            set
            {
                m_destRot = value;
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

            args["rotation"] = m_destRot;
            args["time"] = m_time;
            args["islocal"] = true;
            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            incItweenCount();
            iTween.RotateTo(mGo, args);
        }
    }
}