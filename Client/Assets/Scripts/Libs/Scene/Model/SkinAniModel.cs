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
        protected GameObject m_rootGo;                  // 根 GO ，骨骼同步加载，不会异步加载
        public PartInfo[] m_modelList;                  // 一个数组
        public string m_skeletonName;                   // 骨骼名字
        protected Transform m_transform;                // 位置信息
        protected Action m_handleCB;

        protected AnimSys m_animSys = new AnimSys();       // 动画数据

        public SkinAniModel()
        {
            
        }

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

        public AnimSys animSys
        {
            get
            {
                return m_animSys;
            }
        }

        public void loadSkeleton()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + m_skeletonName;
            param.m_loaded = onSkeletonloaded;
            param.m_extName = "prefab";
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 资源加载成功，通过事件回调
        public void onSkeletonloaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            m_rootGo = res.InstantiateObject(m_skeletonName);
            m_transform = m_rootGo.transform;
            m_animSys.animator = m_rootGo.GetComponent<Animator>();

            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath());

            int idx = 0;
            foreach (PartInfo partInfo in m_modelList)
            {
                if (partInfo.m_res != null)
                {
                    partInfo.m_partGo = partInfo.m_res.InstantiateObject(m_modelList[idx].m_partName);
                    partInfo.m_partGo.transform.parent = m_rootGo.transform;
                    skinSubMesh(idx);

                    Ctx.m_instance.m_resLoadMgr.unload(partInfo.m_res.GetPath());
                    partInfo.m_res = null;
                }

                ++idx;
            }

            if(m_handleCB != null)
            {
                m_handleCB();
            }
        }

        public void loadPartModel(int modelDef)
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + m_modelList[modelDef].m_bundleName;
            param.m_loaded = onPartModelloaded;
            param.m_extName = "prefab";
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 资源加载成功，通过事件回调
        public void onPartModelloaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            int idx = getModelIdx(res.GetPath());
            
            if (m_rootGo != null)
            {
                m_modelList[idx].m_partGo = res.InstantiateObject(m_modelList[idx].m_partName);
                m_modelList[idx].m_partGo.transform.parent = m_rootGo.transform;
                skinSubMesh(idx);
            }
            else
            {
                m_modelList[idx].m_res = res;
            }
        }

        protected int getModelIdx(string path)
        {
            string modelPath = "";
            int ret = 0;
            foreach(PartInfo partInfo in m_modelList)
            {
                modelPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + partInfo.m_bundleName;
                if(modelPath == path)
                {
                    break;
                }

                ++ret;
            }

            return ret;
        }

        protected void skinSubMesh(int subMeshIdx)
        {
            string submeshName = UtilSkin.convID2PartName(subMeshIdx);
            string[] bonesList = Ctx.m_instance.m_modelMgr.getBonesListByName(submeshName);
            UtilSkin.skinSkel(m_modelList[subMeshIdx].m_partGo, m_rootGo, bonesList);
        }
    }
}