using System;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 蒙皮网格骨骼动画模型资源
     */
    public class SkinModelSkelAnim
    {
        protected SkeletonAnim m_skeletonAnim;
        protected SkinModel m_skinModel;
        protected AnimControl m_animControl;       // 动画控制器
        protected BoneSockets m_boneSockets;

        public SkinModelSkelAnim(int subModelNum)
        {
            m_skeletonAnim = new SkeletonAnim();
            m_skeletonAnim.skelLoadDisp.addEventHandle(onSkelAnimLoadedHandle);
            m_skinModel = new SkinModel(subModelNum);
            m_animControl = new AnimControl();
            m_boneSockets = new BoneSockets(1);
            m_boneSockets.addSocket(0, "Reference/Hips");
        }

        public AnimControl animControl
        {
            get
            {
                return m_animControl;
            }
        }

        public SkeletonAnim skeletonAnim
        {
            get
            {
                return m_skeletonAnim;
            }
            set
            {
                m_skeletonAnim = value;
            }
        }

        public SkinModel skinModel
        {
            get
            {
                return m_skinModel;
            }
            set
            {
                m_skinModel = value;
            }
        }

        public BoneSockets boneSockets
        {
            get
            {
                return m_boneSockets;
            }
            set
            {
                m_boneSockets = value;
            }
        }

        public void onSkelAnimLoadedHandle(IDispatchObject dispObj)
        {
            SkeletonAnim res = dispObj as SkeletonAnim;
            m_skinModel.setSkelAnim(res.selfGo);
            m_animControl.animator = res.selfGo.GetComponent<Animator>();
            m_boneSockets.setSkelAnim(res.selfGo);
        }
    }
}