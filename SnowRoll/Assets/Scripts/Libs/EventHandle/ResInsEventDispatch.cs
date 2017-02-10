using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 资源实例化事件分发器
     */
    public class ResInsEventDispatch : EventDispatch, IDispatchObject
    {
        protected bool mIsValid;
        protected GameObject mInsGO;

        public ResInsEventDispatch()
        {
            this.mIsValid = true;
        }

        public void setIsValid(bool value)
        {
            this.mIsValid = value;
        }

        public bool getIsValid()
        {
            return this.mIsValid;
        }

        public void setInsGO(GameObject go)
        {
            this.mInsGO = go;
        }

        public GameObject getInsGO()
        {
            return this.mInsGO;
        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            if(this.mIsValid)
            {
                base.dispatchEvent(dispatchObject);
            }
            else
            {
                UtilApi.Destroy(this.mInsGO);
                this.mInsGO = null;
            }
        }
    }
}