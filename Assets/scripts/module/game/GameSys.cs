using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;

namespace Game.Game
{
    public class GameSys
    {
        static public GameSys m_instance;
        public Ctx m_ctx;

        public void initGVar()
        {
            // 获取全局变量
            GameObject nodestroy = GameObject.FindGameObjectWithTag("GoNoDestroy");
            AppRoot approot = nodestroy.GetComponent<AppRoot>();
            GameSys.m_instance.m_ctx = approot.getCtx();
        }

        public void Update()
        {
            if(Input.GetKeyUp(KeyCode.K))
            {
                LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
                param.m_path = "UIScrollForm.assetbundle";
                param.m_type = ResType.eBundleType;
                param.m_prefabName = "UIScrollForm";
                param.m_cb = onResLoad;
                Ctx.m_instance.m_resMgr.load(param);
            }
        }

        public void onResLoad(IRes res)
        {
            GameObject go = (res as IBundleRes).InstantiateObject("UIScrollForm");
            GameObject nodestroy = GameObject.FindGameObjectWithTag("UIFirstLayer");
            go.transform.parent = nodestroy.transform;
        }
    }
}
