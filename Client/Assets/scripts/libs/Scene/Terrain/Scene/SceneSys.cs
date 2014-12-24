using System;
using System.Collections.Generic;
using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 同一时刻只能有一个场景存在
     */
    public class SceneSys : ISceneSys
    {
        protected Action<IScene> onSceneLoaded;

        protected SceneParse m_sceneParse;
        protected Scene m_scene;

        public SceneSys()
        {
            m_sceneParse = new SceneParse();
        }

        public Scene scene
        {
            get
            {
                return m_scene;
            }
            set
            {
                m_scene = value;
            }
        }

        public void loadScene(string filename, Action<IScene> func)
        {
            // 卸载之前的场景
            unloadScene();

            // 加载新的场景
            m_scene = new Scene();
            m_scene.file = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene] + filename;
            if(func != null)
            {
                onSceneLoaded += func;
            }
            //loadSceneCfg(filename);
            loadSceneRes(filename);
        }

        protected void unloadScene()
        {
            if(null != m_scene)
            {
                Ctx.m_instance.m_resMgr.unload(m_scene.file);
            }
        }

        public void loadSceneCfg(string filename)
        {
            LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneXml] + filename;
            //param.m_resPackType = ResPackType.eBundleType;
            param.m_loaded = onSceneCfgLoadded;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //param.m_resNeedCoroutine = false;
            //param.m_loadNeedCoroutine = false;
            //Ctx.m_instance.m_resMgr.load(param);
            Ctx.m_instance.m_resMgr.loadBundle(param);
        }

        protected void onSceneCfgLoadded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            m_sceneParse.sceneCfg = m_scene.sceneCfg;
            byte[] bytes = (res.getObject(m_scene.file) as TextAsset).bytes;
            Stream stream = new MemoryStream(bytes);
            m_sceneParse.parse(stream);
        }

        public void loadSceneRes(string filename)
        {
            LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene] + filename;
            //param.m_resPackType = ResPackType.eLevelType;
            param.m_loaded = onSceneResLoadded;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //param.m_resNeedCoroutine = true;
            //param.m_loadNeedCoroutine = true;
            param.m_lvlName = filename;
            //Ctx.m_instance.m_resMgr.load(param);
            Ctx.m_instance.m_resMgr.loadLevel(param);
        }

        public void onSceneResLoadded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            if(onSceneLoaded != null)
            {
                onSceneLoaded(m_scene);
            }

            onSceneLoaded = null;           // 清除所有的监听器
        }
    }
}