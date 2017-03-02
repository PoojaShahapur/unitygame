using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 模型定义，目前玩家分这几种， npc 怪物只有一个模型 
     */
    public enum ePlayerModelType
    {
        eModelHead = 0,         // 头
        eModelChest = 1,        // 胸
        eModelWaist = 2,        // 腰
        eModelLeg = 3,          // 腿

        eModelFoot = 4,         // 脚
        eModelArm = 5,          // 胳膊
        eModelHand = 6,         // 手
        eModelTotal
    }

    public enum eNpcModelType
    {
        eModelBody = 0,
        eModelTotal
    }

    public enum eMonstersModelType
    {
        eModelBody = 0,
        eModelTotal
    }

    /**
     * @brief 蒙皮子模型
     */
    public class SkinSubModel
    {
        protected ModelRes m_modelRes;
        protected string m_modelPath;
        protected bool m_bNeedReloadModel;

        protected SkinRes m_skinRes;
        protected string m_skinPath;
        protected bool m_bNeedReloadSkin;

        protected GameObject m_skelRootGo;  // 骨骼根节点
        protected GameObject m_modelGo;     // 模型实例化

        public SkinSubModel()
        {
            m_skelRootGo = null;
            m_modelGo = null;

            m_bNeedReloadModel = false;
            m_bNeedReloadSkin = false;
        }

        public void dispose()
        {
            if (m_modelRes != null)
            {
                Ctx.mInstance.mModelMgr.unload(m_modelRes.getResUniqueId(), onSubModelLoaded);
                m_modelRes = null;
            }
            if (m_skinRes != null)
            {
                Ctx.mInstance.mModelMgr.unload(m_skinRes.getResUniqueId(), onSubModelLoaded);
                m_skinRes = null;
            }
        }

        public GameObject skelRootGo
        {
            get
            {
                return m_skelRootGo;
            }
            set
            {
                m_skelRootGo = value;

                if(m_skelRootGo != null)
                {
                    if (m_modelRes.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
                    {
                        linkSkelModel();
                    }
                }
            }
        }

        public string modelPath
        {
            get
            {
                return m_modelPath;
            }
            set
            {
                if (m_modelPath != value)
                {
                    m_bNeedReloadModel = true;
                }
                m_modelPath = value;
            }
        }

        public string skinPath
        {
            get
            {
                return m_skinPath;
            }
            set
            {
                if (m_skinPath != value)
                {
                    m_bNeedReloadSkin = true;
                }
                m_skinPath = value;
            }
        }

        public void loadSubModel()
        {
            if (m_bNeedReloadModel)
            {
                if(m_modelRes != null)
                {
                    Ctx.mInstance.mModelMgr.unload(m_modelRes.getResUniqueId(), onSubModelLoaded);
                    m_modelRes = null;
                }

                m_modelRes = Ctx.mInstance.mModelMgr.getAndAsyncLoad<ModelRes>(m_modelPath, onSubModelLoaded) as ModelRes;
                m_bNeedReloadModel = false;
            }
        }

        public void onSubModelLoaded(IDispatchObject dispObj)
        {
            ModelRes res = dispObj as ModelRes;
            Ctx.mInstance.mLogSys.logLoad(res);

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                if (m_skelRootGo != null)
                {
                    linkSkelModel();
                }
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mModelMgr.unload(res.getResUniqueId(), onSubModelLoaded);
                m_modelRes = null;
            }
        }

        public void loadSkin()
        {
            if (m_bNeedReloadSkin)
            {
                if (m_skinRes != null)
                {
                    Ctx.mInstance.mModelMgr.unload(m_skinRes.getResUniqueId(), onSubModelLoaded);
                    m_skinRes = null;
                }

                m_skinRes = Ctx.mInstance.mModelMgr.getAndAsyncLoad<SkinRes>(m_skinPath, onSkinLoaded) as SkinRes;
                m_bNeedReloadSkin = false;
            }
        }

        public void onSkinLoaded(IDispatchObject dispObj)
        {
            SkinRes res = dispObj as SkinRes;
            Ctx.mInstance.mLogSys.logLoad(res);

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                if (m_skelRootGo != null)
                {
                    if (m_modelGo != null)
                    {
                        UtilSkin.skinSkel(m_modelGo, m_skelRootGo, m_skinRes.boneArr);
                    }
                }
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mModelMgr.unload(res.getResUniqueId(), onSkinLoaded);
                m_skinRes = null;
            }
        }

        public void linkSkelModel()
        {
            m_modelGo = m_modelRes.InstantiateObject(m_modelRes.getPrefabName(), false, UtilMath.ZeroVec3, UtilMath.UnitQuat);
            UtilApi.SetParent(m_modelGo, m_skelRootGo);
            if (m_skinRes.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                UtilSkin.skinSkel(m_modelGo, m_skelRootGo, m_skinRes.boneArr);
            }
        }
    }

    public class SkinModel
    {
        protected SkinSubModel[] m_skinSubModelArr;

        public SkinModel(int subModelNum)
        {
            m_skinSubModelArr = new SkinSubModel[subModelNum];

            for (int idx = 0; idx < subModelNum; ++idx)
            {
                m_skinSubModelArr[idx] = new SkinSubModel();
            }
        }

        public SkinSubModel[] skinSubModelArr
        {
            get
            {
                return m_skinSubModelArr;
            }
            set
            {
                m_skinSubModelArr = value;
            }
        }

        public void setSkelAnim(GameObject go_)
        {
            foreach(var model in m_skinSubModelArr)
            {
                model.skelRootGo = go_;
            }
        }
    }
}