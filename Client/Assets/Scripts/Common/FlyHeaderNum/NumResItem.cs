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
        protected GameObject m_parentGo = new GameObject();        // 父节点
        protected List<GameObject> m_childList = new List<GameObject>();

        protected float m_modelWidth = 0.5f;
        protected float m_modelHeight = 0.5f;

        public void dispose()
        {
            m_parentGo.transform.parent = null;

            foreach (GameObject child in m_childList)
            {
                UtilApi.Destroy(child);
            }

            UtilApi.Destroy(m_parentGo);
        }

        public int num
        {
            set
            {
                m_num = value;

                int left = m_num;
                int mod = 0;
                List<int> numList = new List<int>();

                while(left > 0)
                {
                    mod = left % 10;
                    numList.Add(mod);
                    left /= 10;
                }

                mod = 0;
                ModelRes res;
                string path;
                string prefab;
                GameObject go;
                int idx = 0;
                int curNum = 0;
                while (idx < numList.Count)
                {
                    curNum = numList[numList.Count - 1 - idx];
                    path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + curNum.ToString();
                    prefab = curNum.ToString();
                    res = load(path, prefab);
                    if(res != null)
                    {
                        go = res.InstantiateObject(prefab);
                        go.transform.SetParent(m_parentGo.transform, true);
                        go.transform.localPosition = new Vector3(((float)-numList.Count / 2 + idx) * m_modelWidth, 0, 0);
                        m_childList.Add(go);
                    }
                
                    ++idx;
                }
            }
        }

        public void setPos(Vector3 pos)
        {
            m_parentGo.transform.localPosition = pos;
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

        public ModelRes load(string path, string prefab)
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = path;
            param.m_prefabName = prefab;

            // 这个需要立即加载
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            param.m_extName = "prefab";
            ModelRes res = Ctx.m_instance.m_modelMgr.load<ModelRes>(param) as ModelRes;
            Ctx.m_instance.m_poolSys.deleteObj(param);
            return res;
        }
    }
}