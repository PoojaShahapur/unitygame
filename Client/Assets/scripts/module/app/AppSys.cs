using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using BehaviorLibrary;

namespace Game.App
{
    /**
     * @brief 数据系统
     */
    public class AppSys : Object
    {
        public void constructAll(Transform transform)
        {
            ByteUtil.checkEndian();     // 检查系统大端小端

            Ctx.m_instance = new Ctx();

            Ctx.m_instance.m_cfg = new Config();
            Ctx.m_instance.m_factoryBuild = new FactoryBuild();

            Ctx.m_instance.m_netMgr = new NetworkMgr();
            Ctx.m_instance.m_log = new Logger();
            Ctx.m_instance.m_resMgr = new ResMgr();
            Ctx.m_instance.m_inputMgr = new InputMgr();
            Ctx.m_instance.m_dataTrans = transform;

            Ctx.m_instance.m_processSys = new ProcessSys();
            Ctx.m_instance.m_tickMgr = new TickMgr();
            Ctx.m_instance.m_timerMgr = new TimerMgr();
            Ctx.m_instance.m_coroutineMgr = new CoroutineMgr();
            Ctx.m_instance.m_shareMgr = new ShareMgr();
            Ctx.m_instance.m_sceneSys = new SceneSys();
            Ctx.m_instance.m_layerMgr = new LayerMgr();

            Ctx.m_instance.m_uiMgr = new UIMgr();
            Ctx.m_instance.m_engineLoop = new EngineLoop();
            Ctx.m_instance.m_resizeMgr = new ResizeMgr();

            Ctx.m_instance.m_playerMgr = new PlayerMgr();
            Ctx.m_instance.m_monsterMgr = new MonsterMgr();
            Ctx.m_instance.m_fObjectMgr = new FObjectMgr();
            Ctx.m_instance.m_npcMgr = new NpcMgr();

            Ctx.m_instance.m_camSys = new CamSys();
            Ctx.m_instance.m_meshMgr = new MeshMgr();
            Ctx.m_instance.m_aiSystem = new AISystem();
            Ctx.m_instance.m_sysMsgRoute = new SysMsgRoute();
            Ctx.m_instance.m_moduleSys = new ModuleSys();
            Ctx.m_instance.m_tableSys = new TableSys();
            Ctx.m_instance.m_localFileSys = new LocalFileSys();
            Ctx.m_instance.m_langMgr = new LangMgr();

            Ctx.m_instance.m_interActiveEntityMgr = new InterActiveEntityMgr();
        }

        public void PostInit()
        {
            Ctx.m_instance.m_resizeMgr.addResizeObject(Ctx.m_instance.m_uiMgr as IResizeObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_inputMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_playerMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_monsterMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_fObjectMgr as ITickedObject);
            Ctx.m_instance.m_tickMgr.AddTickObj(Ctx.m_instance.m_npcMgr as ITickedObject);

            Ctx.m_instance.m_uiMgr.getLayerGameObject();

            //Ctx.m_instance.m_tableSys.loadOneTable(TableID.TABLE_OBJECT);
        }

        public void setNoDestroyObject()
        {
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root] = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_Root);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_App);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Canvas] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_Canvas);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Canvas]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UICamera);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_EventSystem] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_EventSystem);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_EventSystem]);

            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIBtmLayer);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIFirstLayer);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UISecondLayer);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIThirdLayer);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UIForthLayer);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer]);

            Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer] = UtilApi.TransFindChildByPObjAndPath(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_Root], NotDestroyPath.ND_CV_UITopLayer);
            UtilApi.DontDestroyOnLoad(Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer]);
        }
    }
}