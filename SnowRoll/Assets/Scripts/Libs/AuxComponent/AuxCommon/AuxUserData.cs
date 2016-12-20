using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 定义 UserData 基本接口
     */
    public class AuxUserData : MonoBehaviour
    {
        //protected object mData;

        virtual public bool isValid()
        {
            //return mData != null;
            return false;
        }

        //public void setData(object value)
        //{
        //    mData = value;
        //}

        //public object getData()
        //{
        //    return mData;
        //}

        //protected T getUserData<T>() where T : class
        //{
        //    return mData as T;
        //}

        //protected T AddUserData<T>() where T : class, new()
        //{
        //    if (mData == null)
        //    {
        //        mData = new T();
        //    }

        //    return mData as T;
        //}
    }
}