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
            GameObject nodestroy = GameObject.FindGameObjectWithTag("App");
            AppRoot approot = nodestroy.GetComponent<AppRoot>();
            GameSys.m_instance.m_ctx = approot.getCtx();
        }

        public void Start()
        {
            initGVar();
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.M))  // 加载场景资源
            {

            }
            else if(Input.GetKeyUp(KeyCode.K))  // 加载 UI 资源
            {
                LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
                param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathUI] + "UIScrollForm.assetbundle";
                param.m_type = ResPackType.eBundleType;
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
