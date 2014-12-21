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
        protected Dictionary<ModuleID, ModuleHandleItem> m_type2ItemDic = new Dictionary<ModuleID, ModuleHandleItem>();

        public ModuleSys()
        {
            registerHandler();
        }

        protected void registerHandler()
        {
            ModuleHandleItem item;

            item = new ModuleHandleItem();
            item.m_loaded = onLoginLoaded;
            item.m_moduleID = ModuleID.LOGINMN;
            item.m_modulePath = ModulePath.LOGINMN;
            item.m_moduleName = ModuleName.LOGINMN;
            m_type2ItemDic[item.m_moduleID] = item;

            item = new ModuleHandleItem();
            item.m_loaded = onGameLoaded;
            item.m_moduleID = ModuleID.GAMEMN;
            item.m_modulePath = ModulePath.GAMEMN;
            item.m_moduleName = ModuleName.GAMEMN;
            m_type2ItemDic[item.m_moduleID] = item;
        }

        // 加载游戏模块
        public void loadModule(ModuleID moduleID)
        {
            // 初始化完成，开始加载自己的游戏场景
            LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule] + m_type2ItemDic[moduleID].m_moduleName;
            //param.m_resPackType = ResPackType.eBundleType;
            param.m_loaded = m_type2ItemDic[moduleID].m_loaded;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //Ctx.m_instance.m_resMgr.load(param);
            //Ctx.m_instance.m_resMgr.loadBundle(param);
            Ctx.m_instance.m_resMgr.loadResources(param);
        }

        public void unloadModule(ModuleID moduleID)
        {
            UtilApi.Destroy(Ctx.m_instance.m_layerMgr.m_path2Go[m_type2ItemDic[moduleID].m_modulePath]);
            Ctx.m_instance.m_layerMgr.m_path2Go.Remove(m_type2ItemDic[moduleID].m_modulePath);
        }

        public void onLoginLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.LOGINMN] = res.InstantiateObject(ModuleName.LOGINMN);
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.LOGINMN].name = ModuleName.LOGINMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.LOGINMN].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_RootLayer].transform;
        }

        public void onGameLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.GAMEMN] = res.InstantiateObject(ModuleName.GAMEMN);
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.GAMEMN].name = ModuleName.GAMEMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_GameLayer].transform;

            // 游戏模块也不释放
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game]);
        }
    }
}