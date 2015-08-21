using Fight;
using FightCore;
using Game.UI;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.Game
{
    /**
     * @brief 处理各种场景切换
     */
    public class GotoScene
    {
        public void addSceneHandle()
        {
            Ctx.m_instance.m_gameRunStage.addQuitingAndEnteringDisp(quitingAndEnteringStageHandle);
            Ctx.m_instance.m_gameRunStage.addQuitedAndEnteredDisp(quitedAndEnteredStageHandle);
        }

        public void loadGameScene()
        {
            if (!Ctx.m_instance.m_gameRunStage.isCurInStage(EGameStage.eStage_Game))
            {
                Ctx.m_instance.m_gameRunStage.toggleGameStage(EGameStage.eStage_Game);
                Ctx.m_instance.m_sceneSys.loadScene("Game.unity", onGameResLoadScene);
            }
        }

        public void loadDZScene(uint sceneNumber)
        {
            // 查找场景配置文件
            MapXmlItem xmlItem = Ctx.m_instance.m_mapCfg.getXmlItem(sceneNumber);

            if (xmlItem != null)
            {
                if (!Ctx.m_instance.m_gameRunStage.isCurInStage(EGameStage.eStage_DZ))
                {
                    Ctx.m_instance.m_gameRunStage.toggleGameStage(EGameStage.eStage_DZ);
                    Ctx.m_instance.m_sceneSys.loadScene(xmlItem.m_levelName, onDZResLoadScene);
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("xml 中没有匹配的场景 id {0} ", sceneNumber));
            }
        }

        // 这个是操作场景资源加载完成回调
        public void onGameResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_gameRunStage.quitedAndEnteredCurStage();
        }

        // 这个是对战场景资源加载完成回调
        public void onDZResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_gameRunStage.quitedAndEnteredCurStage();
        }

        public void loadScene(string sceneName)
        {
            Ctx.m_instance.m_sceneSys.loadScene(sceneName, onLoadScene);
        }

        public void onLoadScene(Scene scene)
        {
            testLoadModel();
        }

        protected void testLoadModel()
        {
            /*
            // 加载骨骼动画
            string path = "Scene/Man/Skeleton/DefaultAvatar.prefab";
            SkelAnimRes skelAnim = Ctx.m_instance.m_skelAniMgr.getAndSyncLoad<SkelAnimRes>(path) as SkelAnimRes;
            // 加载模型
            path = "Scene/Man/SubMesh/DefaultAvatar_Lw_Teeth_Mesh.prefab";
            ModelRes lowTeethModel = Ctx.m_instance.m_skelAniMgr.getAndSyncLoad<ModelRes>(path) as ModelRes;
            path = "Scene/Man/SubMesh/DefaultAvatar_Tounge_Mesh.prefab";
            ModelRes toungeModel = Ctx.m_instance.m_skelAniMgr.getAndSyncLoad<ModelRes>(path) as ModelRes;
            path = "Scene/Man/SubMesh/DefaultAvatar_Unity_Body_Mesh.prefab";
            ModelRes bodyModel = Ctx.m_instance.m_skelAniMgr.getAndSyncLoad<ModelRes>(path) as ModelRes;
            path = "Scene/Man/SubMesh/DefaultAvatar_Up_Teeth_Mesh.prefab";
            ModelRes upTeethModel = Ctx.m_instance.m_skelAniMgr.getAndSyncLoad<ModelRes>(path) as ModelRes;
            // 加载蒙皮
            path = "Scene/Man/Skin/lwteeth.xml";
            SkinRes lowTeethSkinRes = Ctx.m_instance.m_skinResMgr.getAndSyncLoad<SkinRes>(path) as SkinRes;
            path = "Scene/Man/Skin/tounge.xml";
            SkinRes toungeSkinRes = Ctx.m_instance.m_skinResMgr.getAndSyncLoad<SkinRes>(path) as SkinRes;
            path = "Scene/Man/Skin/body.xml";
            SkinRes bodySkinRes = Ctx.m_instance.m_skinResMgr.getAndSyncLoad<SkinRes>(path) as SkinRes;
            path = "Scene/Man/Skin/upteeth.xml";
            SkinRes upTeethSkinRes = Ctx.m_instance.m_skinResMgr.getAndSyncLoad<SkinRes>(path) as SkinRes;

            GameObject skelAnimGo = skelAnim.InstantiateObject(skelAnim.GetPath());
            GameObject lowTeethGo = lowTeethModel.InstantiateObject(lowTeethModel.GetPath());
            GameObject toungeGo = toungeModel.InstantiateObject(skelAnim.GetPath());
            GameObject bodyGo = bodyModel.InstantiateObject(skelAnim.GetPath());
            GameObject upTeethGo = upTeethModel.InstantiateObject(skelAnim.GetPath());

            UtilApi.SetParent(lowTeethGo, skelAnimGo);
            UtilApi.SetParent(toungeGo, skelAnimGo);
            UtilApi.SetParent(bodyGo, skelAnimGo);
            UtilApi.SetParent(upTeethGo, skelAnimGo);

            UtilSkin.skinSkel(lowTeethGo, skelAnimGo, lowTeethSkinRes.boneArr);
            UtilSkin.skinSkel(toungeGo, skelAnimGo, toungeSkinRes.boneArr);
            UtilSkin.skinSkel(bodyGo, skelAnimGo, bodySkinRes.boneArr);
            UtilSkin.skinSkel(upTeethGo, skelAnimGo, upTeethSkinRes.boneArr);

            // 挂在相机跟随
            Transform hips = UtilApi.TransFindChildByPObjAndPath(skelAnimGo, "Reference/Hips").transform;
            SmoothFollow sm = Camera.main.GetComponent<SmoothFollow>();
            sm.target = hips;
            */

            SkinModelSkelAnim skinModelSkelAnim = new SkinModelSkelAnim(4);
            skinModelSkelAnim.skeletonAnim.skelAnimPath = "Scene/Man/Skeleton/DefaultAvatar.prefab";
            skinModelSkelAnim.skeletonAnim.loadSkelAnim();

            skinModelSkelAnim.skinModel.skinSubModelArr[0].modelPath = "Scene/Man/SubMesh/DefaultAvatar_Lw_Teeth_Mesh.prefab";
            skinModelSkelAnim.skinModel.skinSubModelArr[0].skinPath = "Scene/Man/Skin/lwteeth.xml";
            skinModelSkelAnim.skinModel.skinSubModelArr[0].loadSubModel();
            skinModelSkelAnim.skinModel.skinSubModelArr[0].loadSkin();

            skinModelSkelAnim.skinModel.skinSubModelArr[1].modelPath = "Scene/Man/SubMesh/DefaultAvatar_Tounge_Mesh.prefab";
            skinModelSkelAnim.skinModel.skinSubModelArr[1].skinPath = "Scene/Man/Skin/tounge.xml";
            skinModelSkelAnim.skinModel.skinSubModelArr[1].loadSubModel();
            skinModelSkelAnim.skinModel.skinSubModelArr[1].loadSkin();

            skinModelSkelAnim.skinModel.skinSubModelArr[2].modelPath = "Scene/Man/SubMesh/DefaultAvatar_Unity_Body_Mesh.prefab";
            skinModelSkelAnim.skinModel.skinSubModelArr[2].skinPath = "Scene/Man/Skin/body.xml";
            skinModelSkelAnim.skinModel.skinSubModelArr[2].loadSubModel();
            skinModelSkelAnim.skinModel.skinSubModelArr[2].loadSkin();

            skinModelSkelAnim.skinModel.skinSubModelArr[3].modelPath = "Scene/Man/SubMesh/DefaultAvatar_Up_Teeth_Mesh.prefab";
            skinModelSkelAnim.skinModel.skinSubModelArr[3].skinPath = "Scene/Man/Skin/upteeth.xml";
            skinModelSkelAnim.skinModel.skinSubModelArr[3].loadSubModel();
            skinModelSkelAnim.skinModel.skinSubModelArr[3].loadSkin();

            Transform hips = skinModelSkelAnim.boneSockets.getSocket(0).placeHolderGo.transform;
            SmoothFollow sm = Camera.main.GetComponent<SmoothFollow>();
            sm.target = hips;
        }

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIMain);

            Ctx.m_instance.m_uiMgr.m_UIAttrs.m_dicAttr[UIFormID.eUIGM].addUISceneType(UISceneType.eUIScene_Game);
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIGM);
        }

        protected void loadAllDZUIScene()
        {
            //Ctx.m_instance.m_uiMgr.loadForm<UITest>(UIFormID.eUITest);
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.eUIDZ);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.eUIChat);      // 显示聊天
            Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneDZ>(UISceneFormID.eUISceneDZ);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.m_UIAttrs.m_dicAttr[UIFormID.eUIGM].addUISceneType(UISceneType.eUIScene_DZ);
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIGM);
        }

        // 第一次进入游戏场景初始化
        protected void initOnFirstEnterGameScene()
        {
            if (Ctx.m_instance.m_gameRunStage.ePreGameStage == EGameStage.eStage_Login)
            {
                Ctx.m_instance.m_camSys.m_boxCam = new SDK.Lib.BoxCam();

                // 卸载登陆模块，关闭登陆界面
                Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.LOGINMN);
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUILogin);
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIHeroSelect);
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIChat);      // 退出聊天

                // 请求主角基本数据
                //Ctx.m_instance.m_dataPlayer.reqMainData();
            }
        }

        // 进入场景，但是场景还没有加载完成
        public void quitingAndEnteringStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            //Ctx.m_instance.m_soundMgr.unloadAll();          // 卸载所有的音频

            if (EGameStage.eStage_Game == srcGameState)
            {
                // 必然是从游戏场景进入战斗场景
                Ctx.m_instance.m_uiMgr.unloadUIBySceneType(UISceneType.eUIScene_Game, UISceneType.eUIScene_DZ);
            }
            else if (EGameStage.eStage_DZ == srcGameState)
            {
                Ctx.m_instance.m_uiMgr.unloadUIBySceneType(UISceneType.eUIScene_DZ, UISceneType.eUIScene_Game);        // 退出测试
                Ctx.m_instance.m_uiSceneMgr.unloadAll();
            }
        }

        // 进入场景，场景资源加载成功
        public void quitedAndEnteredStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            // 播放音乐
            //SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
            //param.m_path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "ZuiZhenDeMeng.mp3");
            //Ctx.m_instance.m_soundMgr.play(param);
            //Ctx.m_instance.m_poolSys.deleteObj(param);

            if (EGameStage.eStage_Login == srcGameState)
            {
                initOnFirstEnterGameScene();
            }

            if (EGameStage.eStage_Game == destGameState)
            {
                Ctx.m_instance.m_logSys.log("场景加载成功");
                loadAllUIScene();
                Ctx.m_instance.m_camSys.m_boxCam.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam"));
                Ctx.m_instance.m_sceneEventCB.onLevelLoaded();
                Ctx.m_instance.m_camSys.setSceneCamera2UICamera();
            }
            else if (EGameStage.eStage_DZ == destGameState)
            {
                Ctx.m_instance.m_camSys.setSceneCamera2MainCamera();

                Ctx.m_instance.m_dataPlayer.m_dzData.clear();
                Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = true;         // 进入对战就设置这个标示位为可以继续战斗
                Ctx.m_instance.m_camSys.m_dzCam = new DzCam();
                loadAllDZUIScene();
            }
        }
    }
}