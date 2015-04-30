using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
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
            item.m_moduleLayerPath = ModulePath.LOGINMN;
            item.m_path = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule], ModuleName.LOGINMN, ".prefab");
            m_type2ItemDic[item.m_moduleID] = item;

            item = new ModuleHandleItem();
            item.m_loaded = onGameLoaded;
            item.m_moduleID = ModuleID.GAMEMN;
            item.m_moduleLayerPath = ModulePath.GAMEMN;
            item.m_path = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule], ModuleName.GAMEMN, ".prefab");
            m_type2ItemDic[item.m_moduleID] = item;

            item = new ModuleHandleItem();
            item.m_loaded = onAutoUpdateLoaded;
            item.m_moduleID = ModuleID.AUTOUPDATEMN;
            item.m_moduleLayerPath = ModulePath.AUTOUPDATEMN;
            item.m_path = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModule], ModuleName.AUTOUPDATEMN, ".prefab");
            m_type2ItemDic[item.m_moduleID] = item;
        }

        // 加载游戏模块
        public void loadModule(ModuleID moduleID)
        {
            if (!m_type2ItemDic[moduleID].m_isLoaded)
            {
                // 初始化完成，开始加载自己的游戏场景
                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.m_path = Ctx.m_instance.m_pPakSys.getCurResPakPathByResPath(m_type2ItemDic[moduleID].m_path);
                param.m_loaded = m_type2ItemDic[moduleID].m_loaded;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("模块重复加载");
            }
        }

        public void unloadModule(ModuleID moduleID)
        {
            if (ModuleID.LOGINMN == moduleID)
            {
                if (Ctx.m_instance.m_loginSys != null)
                {
                    Ctx.m_instance.m_loginSys.unload();
                }
            }
            else if (ModuleID.AUTOUPDATEMN == moduleID)
            {
                if (Ctx.m_instance.m_autoUpdate!= null)
                {
                    Ctx.m_instance.m_autoUpdate.unload();
                }
            }
            if (Ctx.m_instance.m_layerMgr.m_path2Go.ContainsKey(m_type2ItemDic[moduleID].m_moduleLayerPath))
            {
                UtilApi.Destroy(Ctx.m_instance.m_layerMgr.m_path2Go[m_type2ItemDic[moduleID].m_moduleLayerPath]);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("没有找到卸载的模块的 object");
            }
            Ctx.m_instance.m_layerMgr.m_path2Go.Remove(m_type2ItemDic[moduleID].m_moduleLayerPath);
            UtilApi.UnloadUnusedAssets();
        }

        public void onLoginLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.LOGINMN] = res.InstantiateObject(m_type2ItemDic[ModuleID.LOGINMN].m_path);
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.LOGINMN].name = ModuleName.LOGINMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.LOGINMN].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].transform;

            // 立马卸载这个资源
            Ctx.m_instance.m_resLoadMgr.unload(m_type2ItemDic[ModuleID.LOGINMN].m_path);
        }

        public void onGameLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.GAMEMN] = res.InstantiateObject(m_type2ItemDic[ModuleID.GAMEMN].m_path);
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.GAMEMN].name = ModuleName.GAMEMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].transform;

            // 游戏模块也不释放
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Game]);

            // 立马卸载这个资源
            Ctx.m_instance.m_resLoadMgr.unload(m_type2ItemDic[ModuleID.GAMEMN].m_path);
        }

        public void onAutoUpdateLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.AUTOUPDATEMN] = res.InstantiateObject(m_type2ItemDic[ModuleID.AUTOUPDATEMN].m_path);
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.AUTOUPDATEMN].name = ModuleName.AUTOUPDATEMN;
            Ctx.m_instance.m_layerMgr.m_path2Go[ModulePath.AUTOUPDATEMN].transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root].transform;

            // 立马卸载这个资源
            Ctx.m_instance.m_resLoadMgr.unload(m_type2ItemDic[ModuleID.AUTOUPDATEMN].m_path);
        }
    }
}