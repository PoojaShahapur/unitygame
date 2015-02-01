using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class SceneAniCard : SceneCardEntityBase
    {
        // 做动画使用
        // 原始信息
        protected Vector3 m_origPos;
        protected Vector3 m_origRot;
        protected Vector3 m_origScale;

        // 开始信息
        protected Vector3 m_startPos;       // 最终位置
        protected Vector3 m_startRot;       // 最终选择
        protected Vector3 m_startScale;       // 最终选择

        // 目标信息
        protected Vector3 m_destPos;       // 最终位置
        protected Vector3 m_destRot;       // 最终选择
        protected Vector3 m_destScale;       // 最终选择

        protected float m_time = 0.5f;      // 动画时间

        public override void Start()
        {
            //初始化都是最初的
            m_origPos = transform.localPosition;
            m_origRot = transform.localRotation.eulerAngles;
            m_origScale = transform.localScale;

            m_startPos = transform.localPosition;
            m_startRot = transform.localRotation.eulerAngles;
            m_startScale = transform.localScale;

            m_destPos = transform.localPosition;
            m_destRot = transform.localRotation.eulerAngles;
            m_destScale = transform.localScale;
        }

        public Vector3 startPos
        {
            set
            {
                m_startPos = value;
            }
        }

        public Vector3 startRot
        {
            set
            {
                m_startRot = value;
            }
        }

        public Vector3 startScale
        {
            set
            {
                m_startScale = value;
            }
        }

        public Vector3 destPos
        {
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

        public void moveToStart()
        {
            transform.localPosition = m_startPos;
            transform.localRotation = Quaternion.Euler(m_startRot);
            transform.localScale = m_startScale;
        }

        // 到目标位置
        public void moveToDest()
        {
            m_startPos = transform.localPosition;
            m_startRot = transform.localRotation.eulerAngles;
            m_startScale = transform.localScale;

            // 非动画
            //transform.localPosition = m_destPos;
            //transform.localRotation = Quaternion.Euler(m_destRot);
            //transform.localScale = m_destScale;

            // 动画
            //iTween.MoveTo(gameObject, m_destPos, m_time);
            //iTween.RotateTo(gameObject, m_destRot, m_time);
            //iTween.ScaleTo(gameObject, m_destScale, m_time);

            Hashtable args;

            args = new Hashtable();
            args["position"] = m_destPos;
            args["time"] = m_time;
            args["islocal"] = true;
            iTween.MoveTo(gameObject, args);

            args = new Hashtable();
            args["rotation"] = m_destRot;
            args["time"] = m_time;
            args["islocal"] = true;
            iTween.RotateTo(gameObject, args);

            args = new Hashtable();
            args["scale"] = m_destScale;
            args["time"] = m_time;
            iTween.ScaleTo(gameObject, args);
        }
    }
}