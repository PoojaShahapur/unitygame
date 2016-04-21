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
            mIsValid = true;
        }

        public void setIsValid(bool value)
        {
            mIsValid = value;
        }

        public bool getIsValid()
        {
            return mIsValid;
        }

        public void setInsGO(GameObject go)
        {
            mInsGO = go;
        }

        public GameObject getInsGO()
        {
            return mInsGO;
        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            if(mIsValid)
            {
                base.dispatchEvent(dispatchObject);
            }
            else
            {
                UtilApi.Destroy(mInsGO);
            }
        }
    }
}