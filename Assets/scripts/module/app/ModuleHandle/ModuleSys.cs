using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.App
{
    /**
     * @brief 主要是各个模块的管理
     */
    public class ModuleSys : IModuleSys
    {
        protected Dictionary<string, ModuleHandleItem> m_type2ItemDic = new Dictionary<string, ModuleHandleItem>();

        public ModuleSys()
        {
            registerHandler();
        }

        protected void registerHandler()
        {
            ModuleHandleItem item;

            item = new ModuleHandleItem();
            item.m_loadedcb = onLoginLoaded;
            item.m_key = DestroyPath.CV_Login;
            m_type2ItemDic[ModuleName.LOGINMN] = item;

            item = new ModuleHandleItem();
            item.m_loadedcb = onGameLoaded;
            item.m_key = NotDestroyPath.ND_CV_Game;
            m_type2ItemDic[ModuleName.GAMEMN] = item;
        }

        // 加载游戏模块
        public void loadModule(string name)
        {
            // 初始化完成，开始加载自己的游戏场景
            LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule] + name;
            //param.m_resPackType = ResPackType.eBundleType;
            param.m_loadedcb = m_type2ItemDic[name].m_loadedcb;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //Ctx.m_instance.m_resMgr.load(param);
            //Ctx.m_instance.m_resMgr.loadBundle(param);
            Ctx.m_instance.m_resMgr.loadResources(param);
        }

        public void unloadModule(string name)
        {
            UtilApi.Destroy(Ctx.m_instance.m_layerMgr.m_path2Go[m_type2ItemDic[name].m_key]);
            Ctx.m_instance.m_layerMgr.m_path2Go.Remove(m_type2ItemDic[name].m_key);
        }

        public void onLoginLoaded(EventDisp resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[DestroyPath.CV_Login] = res.InstantiateObject(ModuleName.LOGINMN);
            Ctx.m_instance.m_layerMgr.m_path2Go[DestroyPath.CV_Login].name = ModuleName.LOGINMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[DestroyPath.CV_Login].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer].transform;
        }

        public void onGameLoaded(EventDisp resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game] = res.InstantiateObject(ModuleName.GAMEMN);
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game].name = ModuleName.GAMEMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer].transform;

            // 游戏模块也不释放
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game]);
        }
    }
}