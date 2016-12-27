using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要处理场景中鼠标事件处理
     */
    public class IOSceneHandle
    {
        protected MDictionary<string, Action<GameObject>> m_str2HandleDic;

        public IOSceneHandle()
        {
            m_str2HandleDic = new MDictionary<string, Action<GameObject>>();
            registerHandle();
        }

        protected void registerHandle()
        {
            //m_str2HandleDic[SceneEntityName.BTN] = psBtnIO;
        }

        public void OnMouseUp(string name, GameObject go)
        {
            if(m_str2HandleDic.ContainsKey(name))
            {
                m_str2HandleDic[name](go);
            }
        }

        // 处理场中按钮点击处理
        protected void psBtnIO(GameObject go)
        {

        }
    }
}