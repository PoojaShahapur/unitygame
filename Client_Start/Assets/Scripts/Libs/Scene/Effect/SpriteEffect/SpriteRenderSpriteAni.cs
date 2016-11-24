using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 这个是完整的显示流程，场景中不能直接使用这个，需要使用 Effect 对象
     */
    public class SpriteRenderSpriteAni : SpriteAni
    {
        protected SceneEntityBase mEntity;
        protected SpriteRenderer mSpriteRender;    // 精灵渲染器
        protected ModelRes mEffectPrefab;          // 特效 Prefab

        public SpriteRenderSpriteAni(SceneEntityBase entity_)
        {
            this.mEntity = entity_;

            // 创建自己的场景 GameObject
            //selfGo = UtilApi.createSpriteGameObject();
        }

        public override void dispose()
        {
            clearEffectRes();

            base.dispose();
        }

        protected void clearEffectRes()
        {
            if (this.mSelfGo != null)        // 场景中的特效需要直接释放这个 GameObject
            {
                UtilApi.Destroy(this.mSelfGo);
                this.mSelfGo = null;
                this.mSpriteRender = null;
            }

            if (this.mEffectPrefab != null)
            {
                Ctx.mInstance.mModelMgr.unload(this.mEffectPrefab.getResUniqueId(), null);
                this.mEffectPrefab = null;
            }
        }

        override public void stop()
        {
            base.stop();
            if (this.mSpriteRender != null)
            {
                if (!this.mIsKeepLastFrame)
                {
                    this.mSpriteRender.sprite = null;
                    //m_spriteRender = null;
                }
            }
            else
            {
                Ctx.mInstance.mLogSys.log("spriteRender delete is already null");
            }
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (this.mSpriteRender == null)
            {
                if (string.IsNullOrEmpty(this.mGoName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
                {
                    this.mSpriteRender = UtilApi.getComByP<SpriteRenderer>(this.mSelfGo);
                }
                else
                {
                    this.mSpriteRender = UtilApi.getComByP<SpriteRenderer>(this.mPntGo, this.mGoName);
                }

                if(this.mSpriteRender == null)
                {
                    Ctx.mInstance.mLogSys.log("m_spriteRender is null");
                }
            }
        }
        
        override public void updateImage()
        {
            if (this.mSpriteRender != null)
            {
                this.mSpriteRender.sprite = this.mAtlasScriptRes.getImage(mCurFrame).image;
                if(this.mSpriteRender.sprite == null)
                {
                    Ctx.mInstance.mLogSys.log("updateImage m_spriteRender is null");
                }
            }
            else
            {
                Ctx.mInstance.mLogSys.log("updateImage m_spriteRender is null");
            }
        }

        override protected void dispEndEvent()
        {
            this.mPlayEndEventDispatch.dispatchEvent(this.mEntity);
        }

        override public bool checkRender()
        {
            return this.mSpriteRender != null;
        }

        // 特效对应的精灵 Prefab 改变
        override public void onSpritePrefabChanged()
        {
            clearEffectRes();

            string path = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathSpriteAni], this.mTableBody.m_aniPrefabName);
            this.mEffectPrefab = Ctx.mInstance.mModelMgr.getAndSyncLoad<ModelRes>(path);
            selfGo = this.mEffectPrefab.InstantiateObject(path);

            if(this.mSelfGo == null)
            {
                Ctx.mInstance.mLogSys.log(string.Format("Load SpritePrefab Path = {0} Failed", path));
            }
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();
            linkSelf2Parent();
        }
    }
}