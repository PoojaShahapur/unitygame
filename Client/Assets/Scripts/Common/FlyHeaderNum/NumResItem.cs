using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 数字资源
     */
    public class NumResItem
    {
        protected int m_num;        // 数字
        protected bool m_bPositive;     // 负数还是正数
        protected GameObject m_parentGo = UtilApi.createGameObject("NumResParentGO");        // 父节点
        protected List<AuxDynModel> m_childList = new List<AuxDynModel>();

        protected float m_modelWidth = 0.5f;
        protected float m_modelHeight = 0.5f;

        public void dispose()
        {
            m_parentGo.transform.parent = null;
            disposeNum();
            UtilApi.Destroy(m_parentGo);
        }

        public void setNum(int value)
        {
            if (m_num != value)
            {
                disposeNum();

                m_num = value;
                if(value > 0)
                {
                    m_bPositive = true;
                }
                else
                {
                    m_bPositive = false;
                    m_num = -m_num;
                }

                int left = m_num;
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
                modelItem.pntGo = m_parentGo;
                if (m_bPositive)
                {
                    modelItem.modelResPath = string.Format("{0}Num/+{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], ".prefab");
                }
                else
                {
                    modelItem.modelResPath = string.Format("{0}Num/-{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], ".prefab");
                }
                modelItem.syncUpdateModel();
                UtilApi.setPos(modelItem.selfGo.transform, new Vector3(((float)-(numList.Count + 1) / 2) * m_modelWidth, 0, 0));
                m_childList.Add(modelItem);

                while (idx < numList.Count)
                {
                    curNum = numList[numList.Count - 1 - idx];
                    modelItem = new AuxDynModel();
                    modelItem.pntGo = m_parentGo;
                    if (m_bPositive)
                    {
                        modelItem.modelResPath = string.Format("{0}Num/+{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], curNum, ".prefab");
                    }
                    else
                    {
                        modelItem.modelResPath = string.Format("{0}Num/-{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], curNum, ".prefab");
                    }
                    modelItem.syncUpdateModel();
                    UtilApi.setPos(modelItem.selfGo.transform, new Vector3(((float)-(numList.Count + 1) / 2 + (idx + 1)) * m_modelWidth, 0, 0));
                    m_childList.Add(modelItem);

                    ++idx;
                }
            }
        }

        public void disposeNum()
        {
            foreach (AuxDynModel child in m_childList)
            {
                child.dispose();
            }

            m_childList.Clear();
        }

        public void setPos(Vector3 pos)
        {
            UtilApi.setPos(m_parentGo.transform, pos);
        }

        public Vector3 getPos()
        {
            return m_parentGo.transform.localPosition;
        }

        public GameObject getParentGo()
        {
            return m_parentGo;
        }

        public void setParent(GameObject pntGo)
        {
            m_parentGo.transform.SetParent(pntGo.transform, true);
        }
    }
}