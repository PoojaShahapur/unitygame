using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 同一时刻只能有一个场景存在
     */
    public class SceneSys
    {
        protected Action<Scene> onSceneLoaded;

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

        public void loadScene(string filename, Action<Scene> func)
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
                //Ctx.m_instance.m_resLoadMgr.unload(m_scene.file);
                UtilApi.UnloadUnusedAssets();           // 卸载共享资源
            }
        }

        public void loadSceneCfg(string filename)
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneXml] + filename;
            param.m_loadEventHandle = onSceneCfgLoadded;
            Ctx.m_instance.m_resLoadMgr.loadBundle(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void onSceneCfgLoadded(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            m_sceneParse.sceneCfg = m_scene.sceneCfg;
            string text = res.getText(m_scene.file);
            m_sceneParse.parse(text);
        }

        public void loadSceneRes(string filename)
        {
            Ctx.m_instance.m_netDispList.bStopNetHandle = true;        // 加载场景需要停止处理消息，因为很多资源都要等到场景加载完成才初始化

            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene], filename), param);
            param.m_loadEventHandle = onSceneResLoadded;
            param.m_resNeedCoroutine = true;
            param.m_loadNeedCoroutine = true;
            Ctx.m_instance.m_resLoadMgr.loadLevel(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public void onSceneResLoadded(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_uiMgr.findSceneUIRootGo();                 // 场景 UI 根节点

            //ResItem res = dispObj as ResItem;
            if(onSceneLoaded != null)
            {
                onSceneLoaded(m_scene);
            }

            onSceneLoaded = null;           // 清除所有的监听器
            Ctx.m_instance.m_netDispList.bStopNetHandle = false;        // 加载场景完成需要处理处理消息

            Ctx.m_instance.m_resLoadMgr.unload(m_scene.file, onSceneResLoadded);
        }

        // 卸载多有的场景
        public void unloadAll()
        {
            unloadScene();
        }

        // 创建当前场景对应的地图
        public void createTerrain()
        {
            if(m_scene != null)
            {
                m_scene.createTerrain();
            }
        }

        public float getHeightAt(float x, float z)
        {
            if (m_scene != null)
            {
                m_scene.getHeightAt(x, z);
            }

            return 0;
        }
    }
}