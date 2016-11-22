using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 数字管理器
     */
    public class FlyNumMgr
    {
        protected List<FlyNumItem> mNumList;

        public FlyNumMgr()
        {
            this.mNumList = new List<FlyNumItem>();
        }

        public void addFlyNum(int num, Vector3 pos, GameObject parentGo)
        {
            FlyNumItem item = new FlyNumItem();
            this.mNumList.Add(item);
            item.setNum(num);
            item.setParent(parentGo);
            item.setPos(pos);
            item.setDisp(onEndFlyNum);
            item.play();
        }

        protected void onEndFlyNum(FlyNumItem item)
        {
            this.mNumList.Remove(item);
        }
    }
}