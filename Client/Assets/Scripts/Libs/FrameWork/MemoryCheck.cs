using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 查找内存数据
     */
    public class MemoryCheck
    {
        // 根据名字获取对象
        public int getNumByName(string name)
        {
            int iNum = 0;
            UnityEngine.Object[] objs = UtilApi.FindObjectsOfTypeAll<UnityEngine.Object>();
            foreach(UnityEngine.Object obj in objs)
            {
                if(obj.name == name)
                {
                    ++iNum;
                }
            }

            GameObject[] gameObjs = UtilApi.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject gameObj in gameObjs)
            {
                if (gameObj.name == name)
                {
                    ++iNum;
                }
            }

            return iNum;
        }
    }
}