using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 数字资源
     */
    public class NumResItem
    {
        protected int mNum;        // 数字
        protected bool mIsPositive;     // 负数还是正数
        protected GameObject mParentGo;        // 父节点
        protected List<AuxDynModel> mChildList;

        protected float mModelWidth = 0.5f;
        protected float mModelHeight = 0.5f;

        public NumResItem()
        {
            mParentGo = UtilApi.createGameObject("NumResParentGO");
            mChildList = new List<AuxDynModel>();
        }

        public void dispose()
        {
            this.mParentGo.transform.parent = null;
            disposeNum();
            UtilApi.Destroy(this.mParentGo);
        }

        public void setNum(int value)
        {
            if (this.mNum != value)
            {
                disposeNum();

                this.mNum = value;
                if(value > 0)
                {
                    this.mIsPositive = true;
                }
                else
                {
                    this.mIsPositive = false;
                    this.mNum = -this.mNum;
                }

                int left = this.mNum;
                int mod = 0;
                List<int> numList = new List<int>();

                while (left > 0)
                {
                    mod = left % 10;
                    numList.Add(mod);
                    left /= 10;
                }

                mod = 0;
                AuxDynModel modelItem;
                int idx = 0;
                int curNum = 0;

                // 添加 + - 号
                modelItem = new AuxDynModel();
                modelItem.pntGo = this.mParentGo;
                if (this.mIsPositive)
                {
                    modelItem.modelResPath = string.Format("{0}Num/+{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathModel], ".prefab");
                }
                else
                {
                    modelItem.modelResPath = string.Format("{0}Num/-{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathModel], ".prefab");
                }
                modelItem.syncUpdateModel();
                UtilApi.setPos(modelItem.selfGo.transform, new Vector3(((float)-(numList.Count + 1) / 2) * this.mModelWidth, 0, 0));
                this.mChildList.Add(modelItem);

                while (idx < numList.Count)
                {
                    curNum = numList[numList.Count - 1 - idx];
                    modelItem = new AuxDynModel();
                    modelItem.pntGo = this.mParentGo;
                    if (this.mIsPositive)
                    {
                        modelItem.modelResPath = string.Format("{0}Num/+{1}{2}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathModel], curNum, ".prefab");
                    }
                    else
                    {
                        modelItem.modelResPath = string.Format("{0}Num/-{1}{2}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathModel], curNum, ".prefab");
                    }
                    modelItem.syncUpdateModel();
                    UtilApi.setPos(modelItem.selfGo.transform, new Vector3(((float)-(numList.Count + 1) / 2 + (idx + 1)) * this.mModelWidth, 0, 0));
                    this.mChildList.Add(modelItem);

                    ++idx;
                }
            }
        }

        public void disposeNum()
        {
            foreach (AuxDynModel child in this.mChildList)
            {
                child.dispose();
            }

            this.mChildList.Clear();
        }

        public void setPos(Vector3 pos)
        {
            UtilApi.setPos(this.mParentGo.transform, pos);
        }

        public Vector3 getPos()
        {
            return this.mParentGo.transform.localPosition;
        }

        public GameObject getParentGo()
        {
            return this.mParentGo;
        }

        public void setParent(GameObject pntGo)
        {
            this.mParentGo.transform.SetParent(pntGo.transform, true);
        }
    }
}