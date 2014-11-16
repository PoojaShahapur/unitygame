using SDK.Common;
using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 蒙皮网格骨骼动画模型资源
     */
    public class SkinAniModel
    {
        protected GameObject m_rootGo;                  // 跟 GO
        public PartInfo[] m_modelList;                  // 一个数组
        public string m_skeletonName;                   // 骨骼名字
        protected Transform m_transform;                // 位置信息
        protected Action m_handleCB;

        public GameObject rootGo
        {
            get
            {
                return m_rootGo;
            }
            set
            {
                m_rootGo = value;
                m_transform = m_rootGo.transform;
            }
        }

        public Transform transform
        {
            get
            {
                return m_transform;
            }
            set
            {
                m_transform = value;
            }
        }

        public Action handleCB
        {
            set
            {
                m_handleCB = value;
            }
        }

        public void loadSkeleton()
        {
            LoadParam param = Ctx.m_instance.m_resMgr.getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + m_skeletonName + ".unity3d"; ;
            param.m_loadedcb = onloaded;
            Ctx.m_instance.m_resMgr.loadBundle(param);
        }

        // 资源加载成功，通过事件回调
        public void onloaded(SDK.Common.Event resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            m_rootGo = res.InstantiateObject(m_skeletonName);
            m_transform = m_rootGo.transform;

            if(m_handleCB != null)
            {
                m_handleCB();
            }
        }
    }
}
