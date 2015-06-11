using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 这个是完整的显示流程，场景中不能直接使用这个，需要使用 Effect 对象
     */
    public class SpriteRenderSpriteAni : SpriteAni
    {
        protected SceneEntityBase m_entity;
        protected SpriteRenderer m_spriteRender;    // 精灵渲染器
        protected ModelRes m_effectPrefab;          // 特效 Prefab

        public SpriteRenderSpriteAni(SceneEntityBase entity_)
        {
            m_entity = entity_;

            // 创建自己的场景 GameObject
            //selfGo = UtilApi.createSpriteGameObject();
        }

        public override void dispose()
        {
            if(m_selfGo != null)        // 场景中的特效需要直接释放这个 GameObject
            {
                UtilApi.Destroy(m_selfGo);
            }

            if (m_effectPrefab != null)
            {
                Ctx.m_instance.m_modelMgr.unload(m_effectPrefab.GetPath(), null);
                m_effectPrefab = null;
            }

            base.dispose();
        }

        override public void stop()
        {
            base.stop();
            m_spriteRender.sprite = null;
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (m_spriteRender == null)
            {
                if (string.IsNullOrEmpty(m_goName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
                {
                    m_spriteRender = UtilApi.getComByP<SpriteRenderer>(m_selfGo);
                }
                else
                {
                    m_spriteRender = UtilApi.getComByP<SpriteRenderer>(m_pntGo, m_goName);
                }

                if(m_spriteRender == null)
                {
                    Ctx.m_instance.m_logSys.log("m_spriteRender is null");
                }
            }
        }
        
        override public void updateImage()
        {
            if (m_spriteRender != null)
            {
                m_spriteRender.sprite = m_atlasScriptRes.getImage(m_curFrame).image;
            }
            else
            {
                Ctx.m_instance.m_logSys.log("updateImage m_spriteRender is null");
            }
        }

        override protected void dispEndEvent()
        {
            m_playEndEventDispatch.dispatchEvent(m_entity);
        }

        override public bool checkRender()
        {
            return m_spriteRender != null;
        }

        // 特效对应的精灵 Prefab 改变
        override public void onSpritePrefabChanged()
        {
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSpriteAni], m_tableBody.m_aniPrefabName);
            m_effectPrefab = Ctx.m_instance.m_modelMgr.getAndSyncLoad<ModelRes>(path);
            selfGo = m_effectPrefab.InstantiateObject(path);
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();
            linkSelf2Parent();
        }
    }
}