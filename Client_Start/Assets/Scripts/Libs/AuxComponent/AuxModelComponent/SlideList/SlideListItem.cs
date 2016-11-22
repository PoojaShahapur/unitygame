using Game.Msg;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 滑动控件的元素
     */
    public class SlideListItem : AuxResComponent
    {
        protected float mWidth = 0.3f;      // 宽度
        protected float mHeight = 0.3f;     // 高度

        protected string mTexPath;                          // 目录
        protected TextureRes mTexRes;

        public float height
        {
            get
            {
                return this.mHeight;
            }
            set
            {
                this.mHeight = value;
            }
        }

        public string texPath
        {
            get
            {
                return this.mTexPath;
            }
            set
            {
                this.mTexPath = value;
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
            UtilApi.addHoverHandle(this.mSelfGo, OnMouseHover);

            // 加载贴图，换贴图
            loadTex();
        }

        // 显示基本信息
        public void OnMouseHover(GameObject go, bool state)
        {

        }

        public void onTexLoadEventHandle(IDispatchObject dispObj)
        {
            this.mTexRes = dispObj as TextureRes;
            GameObject go_ = UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, "25e9d638.obj");
#if UNITY_5
		    go_.GetComponent<Renderer>().material.mainTexture = this.mTexRes.getTexture();
#elif UNITY_4_6 || UNITY_4_5
            go_.renderer.material.mainTexture = m_texRes.getTexture();
#endif
        }

        public void loadTex()
        {
            bool needLoad = true;

            if (this.mTexRes != null)
            {
                if (this.mTexRes.getLogicPath() != this.mTexPath)
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
                if (!string.IsNullOrEmpty(this.mTexPath))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.setPath(this.mTexPath);
                    param.m_loadEventHandle = onTexLoadEventHandle;
                    Ctx.m_instance.m_texMgr.load<TextureRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }

        public void unloadTex()
        {
            if (this.mSelfGo != null)
            {
                Ctx.m_instance.m_texMgr.unload(this.mTexPath, onTexLoadEventHandle);
                this.mTexRes = null;
            }
        }
    }
}