using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 辅助基类
     */
    public class AuxComponent : IDispatchObject
    {
        protected GameObject mSelfGo;      // 自己节点
        protected GameObject mPntGo;       // 指向父节点
        protected GameObject mPlaceHolderGo;      // 自己节点，资源挂在 m_placeHolderGo 上， m_placeHolderGo 挂在 m_pntGo 上
        protected bool mIsNeedPlaceHolderGo;    // 是否需要占位 GameObject
        protected LuaCSBridge mLuaCSBridge;

        public AuxComponent(LuaCSBridge luaCSBridge_ = null)
        {
            this.mLuaCSBridge = luaCSBridge_;
            this.mIsNeedPlaceHolderGo = false;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {
            if (this.mIsNeedPlaceHolderGo && this.mPlaceHolderGo != null)
            {
                UtilApi.Destroy(this.mPlaceHolderGo);
            }
        }

        public void setSelfName(string name_)
        {
            this.selfGo.name = name_;
        }

        public GameObject selfGo
        {
            get
            {
                return this.mSelfGo;
            }
            set
            {
                bool isPntChange = isChange(this.mSelfGo, value);
                this.mSelfGo = value;
                if (isPntChange)
                {
                    onSelfChanged();
                }
            }
        }

        public GameObject pntGo
        {
            get
            {
                return this.mPntGo;
            }
            set
            {
                bool isPntChange = isChange(this.mPntGo, value);
                this.mPntGo = value;
                if (isPntChange)
                {
                    onPntChanged();
                }
            }
        }

        public virtual void setPntGo(GameObject go)
        {
            this.mPntGo = go;
        }

        public bool bNeedPlaceHolderGo
        {
            get
            {
                return this.mIsNeedPlaceHolderGo;
            }
            set
            {
                this.mIsNeedPlaceHolderGo = value;
                if(this.mIsNeedPlaceHolderGo)
                {
                    if (this.mPlaceHolderGo == null)
                    {
                        this.mPlaceHolderGo = UtilApi.createGameObject("PlaceHolderGO");
                    }
                }
            }
        }

        public GameObject placeHolderGo
        {
            get
            {
                return this.mPlaceHolderGo;
            }
            set
            {
                this.mPlaceHolderGo = value;
            }
        }

        public bool isSelfValid()
        {
            return this.mSelfGo != null;
        }

        protected bool isChange(GameObject srcGO, GameObject destGO)
        {
            if (srcGO == null || !srcGO.Equals(destGO))
            {
                return true;
            }

            return false;
        }

        // 父节点发生改变
        virtual protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        // 自己发生改变
        virtual protected void onSelfChanged()
        {
            linkSelf2Parent();
        }

        public void linkPlaceHolder2Parent()
        {
            if (this.mPlaceHolderGo == null)
            {
                this.mPlaceHolderGo = UtilApi.createGameObject("PlaceHolderGO");
            }
            UtilApi.SetParent(this.mPlaceHolderGo, this.mPntGo, false);
        }

        public void linkSelf2Parent()
        {
            if (this.mSelfGo != null && this.mPntGo != null)   // 现在可能还没有创建
            {
                UtilApi.SetParent(this.mSelfGo, mPntGo, false);
            }
        }

        virtual public void show()
        {
            if (this.mSelfGo != null)
            {
                UtilApi.SetActive(this.mSelfGo, true);
            }
        }

        public void hide()
        {
            if (this.mSelfGo != null && this.IsVisible())
            {
                UtilApi.SetActive(this.mSelfGo, false);
            }
        }

        public bool IsVisible()
        {
            return UtilApi.IsActive(this.mSelfGo);
        }
    }
}