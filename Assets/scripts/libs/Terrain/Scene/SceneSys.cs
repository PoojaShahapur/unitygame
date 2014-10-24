using System;
using System.Collections.Generic;
using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    public class SceneSys : ISceneSys
    {
        protected Action<IScene> onSceneLoaded;

        protected SceneParse m_sceneParse;
        protected Scene m_scene;

        public SceneSys()
        {
            m_sceneParse = new SceneParse();
        }

        public void loadScene(string filename, Action<IScene> func)
        {
            m_scene = new Scene();
            m_scene.file = filename;
            if(func != null)
            {
                onSceneLoaded += func;
            }
            loadSceneCfg(filename);
            loadSceneRes(filename);
        }

        public void loadSceneCfg(string filename)
        {
            LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneXml] + filename + ".unity3d";
            param.m_type = ResPackType.eBundleType;
            param.m_cb = onSceneCfgLoadded;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            param.m_resNeedCoroutine = false;
            param.m_loadNeedCoroutine = false;
            Ctx.m_instance.m_resMgr.load(param);
        }

        protected void onSceneCfgLoadded(IRes res)
        {
            m_sceneParse.sceneCfg = m_scene.sceneCfg;
            byte[] bytes = ((res as IBundleRes).getObject(m_scene.file) as TextAsset).bytes;
            Stream stream = new MemoryStream(bytes);
            m_sceneParse.parse(stream);
        }

        public void loadSceneRes(string filename)
        {
            LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene] + filename + ".unity3d";
            param.m_type = ResPackType.eLevelType;
            param.m_cb = onSceneResLoadded;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            param.m_resNeedCoroutine = true;
            param.m_loadNeedCoroutine = true;
            param.m_lvlName = filename;
            Ctx.m_instance.m_resMgr.load(param);
        }

        public void onSceneResLoadded(IRes res)
        {
            if(onSceneLoaded != null)
            {
                onSceneLoaded(m_scene);
            }
        }
    }
}
