using Fight;
using FightCore;
using Game.Msg;
using SDK.Lib;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 滑动控件的元素
     */
    public class SlideListItem : AuxResComponent
    {
        protected float m_width = 0.3f;      // 宽度
        protected float m_height = 0.3f;     // 高度

        protected stRetBattleHistoryInfoUserCmd m_data;

        protected string m_texPath;                          // 目录
        protected TextureRes m_texRes;

        public stRetBattleHistoryInfoUserCmd data
        {
            get
            {
                return m_data;
            }
            set
            {
                m_data = value;
            }
        }

        public float height
        {
            get
            {
                return m_height;
            }
            set
            {
                m_height = value;
            }
        }

        public string texPath
        {
            get
            {
                return m_texPath;
            }
            set
            {
                m_texPath = value;
            }
        }


        public void loadRes()
        {
            // 加载模型
            load();
        }

        public override void onLoadEventHandle(IDispatchObject dispObj)
        {
            base.onLoadEventHandle(dispObj);
            UtilApi.addHoverHandle(m_selfGo, OnMouseHover);

            // 加载贴图，换贴图
            loadTex();
        }

        // 显示基本信息
        public void OnMouseHover(GameObject go, bool state)
        {
            if (state)
            {
                UISceneTips tips = Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneTips>(UISceneFormID.eUISceneTips) as UISceneTips;
                tips.showTips(Ctx.m_instance.m_coordConv.getCurTouchScenePos(), m_data);
            }
            else
            {
                Ctx.m_instance.m_uiSceneMgr.hideSceneForm(UISceneFormID.eUISceneTips);
            }
        }

        public void onTexLoadEventHandle(IDispatchObject dispObj)
        {
            m_texRes = dispObj as TextureRes;
            GameObject go_ = UtilApi.TransFindChildByPObjAndPath(m_selfGo, "25e9d638.obj");
#if UNITY_5
		    go_.GetComponent<Renderer>().material.mainTexture = m_texRes.getTexture();
#elif UNITY_4_6 || UNITY_4_5
            go_.renderer.material.mainTexture = m_texRes.getTexture();
#endif
        }

        public void loadTex()
        {
            bool needLoad = true;

            if (m_texRes != null)
            {
                if (m_texRes.GetPath() != m_texPath)
                {
                    unloadTex();
                }
                else
                {
                    needLoad = false;
                }
            }
            if (needLoad)
            {
                if (!string.IsNullOrEmpty(m_texPath))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.m_path = m_texPath;
                    param.m_loadEventHandle = onTexLoadEventHandle;
                    Ctx.m_instance.m_texMgr.load<TextureRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }

        public void unloadTex()
        {
            if (m_selfGo != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_texPath, onTexLoadEventHandle);
                m_texRes = null;
            }
        }
    }
}