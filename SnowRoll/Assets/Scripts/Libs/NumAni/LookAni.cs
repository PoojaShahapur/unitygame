using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class LookAni : ITweenAniBase
    {
        protected Vector3 m_looktarget;         // 观察的位置
        protected string m_axis;                // 轴
        protected float m_speed;                // 旋转速度 iTween.cs , if(tweenArguments.Contains("speed")){ speed 暂时不用

        public LookAni()
        {
            m_axis = "y";
        }

        public Vector3 looktarget
        {
            get
            {
                return m_looktarget;
            }
            set
            {
                m_looktarget = value;
            }
        }

        public string axis
        {
            get
            {
                return m_axis;
            }
            set
            {
                m_axis = value;
            }
        }

        public float speed
        {
            get
            {
                return m_speed;
            }
            set
            {
                m_speed = value;
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

            args["looktarget"] = looktarget;
            args["time"] = m_time;
            args["islocal"] = true;

            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;

            args["axis"] = m_axis;
            //args["speed"] = "speed";

            incItweenCount();
            iTween.MoveTo(mGo, args);
        }
    }
}