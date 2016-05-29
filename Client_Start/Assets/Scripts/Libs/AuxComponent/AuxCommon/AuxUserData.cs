using UnityEngine;

namespace SDK.Lib
{
    public class AuxUserData : MonoBehaviour
    {
        protected object mData = null;

        public bool isValid()
        {
            return mData != null;
        }

        public void setData(object value)
        {
            mData = value;
        }

        public object getData()
        {
            return mData;
        }

        public AuxBasicButton getButtonData()
        {
            return this.getUserData<AuxBasicButton>();
        }

        public AuxBasicButton addButtonData()
        {
            if (mData == null)
            {
                mData = new AuxBasicButton(this.gameObject);
            }

            return mData as AuxBasicButton;
        }

        protected T getUserData<T>() where T : class
        {
            return mData as T;
        }

        protected T AddUserData<T>() where T : class, new()
        {
            if (mData == null)
            {
                mData = new T();
            }

            return mData as T;
        }
    }
}