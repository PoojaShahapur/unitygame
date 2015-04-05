using System.Collections.Generic;
using System.Reflection;

namespace SDK.Common
{
    /**
     * @brief 对象池
     */
    public class PoolSys
    {
        //protected List<object> m_poolList = new List<object>();
        protected List<IRecycle> m_poolList = new List<IRecycle>();

        public T newObject<T>() where T : IRecycle, new()
        {
            T retObj = default(T);
            // 查找
            int idx = 0;
            for(idx = 0; idx < m_poolList.Count; ++idx)
            {
                if (typeof(T) == m_poolList[idx].GetType())
                {
                    retObj = (T)m_poolList[idx];
                    m_poolList.RemoveAt(idx);
                    MethodInfo myMethodInfo = retObj.GetType().GetMethod("resetDefault");
                    if (myMethodInfo != null)
                    {
                        myMethodInfo.Invoke(retObj, null);
                    }
                    return retObj;
                }
            }

            retObj = new T();
            return retObj;
        }

        public void deleteObj(IRecycle obj)
        {
            foreach (IRecycle item in m_poolList)
            {
                if(item.Equals(obj))        // 如果已经加入
                {
                    return;
                }
            }
            m_poolList.Add(obj);
        }
    }
}