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
            //loadSceneCfg(filename);
            loadSceneRes(filename);
        }

        public void loadSceneCfg(string filename)
        {
            LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneXml] + filename + ".unity3d";
            //param.m_resPackType = ResPackType.eBundleType;
            param.m_loadedcb = onSceneCfgLoadded;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //param.m_resNeedCoroutine = false;
            //param.m_loadNeedCoroutine = false;
            //Ctx.m_instance.m_resMgr.load(param);
            Ctx.m_instance.m_resMgr.loadBundle(param);
        }

        protected void onSceneCfgLoadded(SDK.Common.Event resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            m_sceneParse.sceneCfg = m_scene.sceneCfg;
            byte[] bytes = (res.getObject(m_scene.file) as TextAsset).bytes;
            Stream stream = new MemoryStream(bytes);
            m_sceneParse.parse(stream);
        }

        public void loadSceneRes(string filename)
        {
            LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene] + filename + ".unity3d";
            //param.m_resPackType = ResPackType.eLevelType;
            param.m_loadedcb = onSceneResLoadded;
            //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            //param.m_resNeedCoroutine = true;
            //param.m_loadNeedCoroutine = true;
            param.m_lvlName = filename;
            //Ctx.m_instance.m_resMgr.load(param);
            Ctx.m_instance.m_resMgr.loadLevel(param);
        }

        public void onSceneResLoadded(SDK.Common.Event resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            if(onSceneLoaded != null)
            {
                onSceneLoaded(m_scene);
            }
        }
    }
}