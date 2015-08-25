using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 数字管理器
     */
    public class FlyNumMgr
    {
        protected List<FlyNumItem> m_numList = new List<FlyNumItem>();

        public void addFlyNum(int num, Vector3 pos, GameObject parentGo)
        {
            FlyNumItem item = new FlyNumItem();
            m_numList.Add(item);
            item.setNum(num);
            item.setParent(parentGo);
            item.setPos(pos);
            item.setDisp(onEndFlyNum);
            item.play();
        }

        protected void onEndFlyNum(FlyNumItem item)
        {
            m_numList.Remove(item);
        }
    }
}