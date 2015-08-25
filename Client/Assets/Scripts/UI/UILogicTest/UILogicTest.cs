using Game.Msg;
using SDK.Lib;
using SDK.Lib;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILogicTest : Form, IUILogicTest
    {
        protected GameObject m_spriteGo;
        protected GameObject m_imageGo;
        protected SpriteAni m_spriteAni;
        protected LinkEffect m_linkEffect;

        override public void onShow()
        {
            base.onShow();
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        // 关联窗口
        protected void findWidget()
        {
            m_spriteGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, LogicTestComPath.PathSprite);
            m_imageGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, LogicTestComPath.PathImage);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LogicTestComPath.PathButton, onBtnClkOk);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LogicTestComPath.PathButton2, onBtnClkOk2);
        }

        // 点击登陆处理
        protected void onBtnClkOk()
        {
            //testUIInfo();
            //testAudio();
            //testLoadMapCfg();
            testShareSpriteAtlas();
            //testScriptSprite();
        }

        protected void onBtnClkOk2()
        {
            //testClostAudio();
            //sendMsg();
            //testLoadSceneUI();
            testSpriteEffect();
            //testImageEffect();
        }

        protected void testUIInfo()
        {
            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
            param.m_midDesc = "aaaaaa";
            UIInfo.showMsg(param);
        }

        // 测试音效
        protected void testAudio()
        {
            SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
            //param.m_path = "TestSound.prefab";
            param.m_path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "ZuiZhenDeMeng.mp3");
            Ctx.m_instance.m_soundMgr.play(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void testClostAudio()
        {
            //Ctx.m_instance.m_soundMgr.stop("TestSound.prefab");
            string path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "ZuiZhenDeMeng.mp3");
            Ctx.m_instance.m_soundMgr.stop(path);
        }

        protected void testLoadMapCfg()
        {
            Ctx.m_instance.m_mapCfg.getXmlItem(1);
        }

        protected void testLoadSceneUI()
        {
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.eUIJobSelect);
        }

        protected void sendMsg()
        {
            stPasswdLogonUserCmd cmd = new stPasswdLogonUserCmd();
            cmd.dwUserID = Ctx.m_instance.m_loginSys.getUserID();
            cmd.loginTempID = Ctx.m_instance.m_pTimerMsgHandle.m_loginTempID;
            UtilMsg.sendMsg(cmd);
        }

        protected void shareSprite()
        {
            GameObject srcBtnGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "ShopBtn");
            GameObject destBtnGo_1 = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "Button_1");
            GameObject destBtnGo_2 = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "Button_2");

            Image srcImage = UtilApi.getComByP<Image>(srcBtnGo);
            Image destImage_1 = UtilApi.getComByP<Image>(destBtnGo_1);
            Image destImage_2 = UtilApi.getComByP<Image>(destBtnGo_2);

            destImage_1.sprite = srcImage.sprite;
            //destImage_1.fillMethod = Image.FillMethod.Horizontal;
            destImage_1.type = Image.Type.Simple;
            destImage_1.SetNativeSize();

            destImage_2.sprite = srcImage.sprite;
            destImage_2.type = Image.Type.Simple;
            destImage_2.SetNativeSize();
        }

        protected void testShareSpriteAtlas()
        {
            GameObject srcBtnGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "BtnTest1");
            Image srcImage = UtilApi.getComByP<Image>(srcBtnGo);

            ImageItem imageItem = Ctx.m_instance.m_atlasMgr.getAndSyncLoadImage(CVAtlasName.Common, "denglu_srk");
            srcImage.sprite = imageItem.image;

            UtilApi.setImageType(srcImage, Image.Type.Simple);
            UtilApi.SetNativeSize(srcImage);
        }

        protected void testScriptSprite()
        {
            GameObject srcBtnGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "BtnTest1");
            Image srcImage = UtilApi.getComByP<Image>(srcBtnGo);

            SOSpriteList spriteList = Resources.Load<SOSpriteList>("Atlas/TuJian");
            //srcImage.sprite = sprite.m_path2SpriteDic["aaa"].m_sprite;
            srcImage.sprite = spriteList.m_objList[0].m_sprite;

            UtilApi.setImageType(srcImage, Image.Type.Simple);
            UtilApi.SetNativeSize(srcImage);
        }

        protected void testSpriteEffect()
        {
            m_linkEffect = Ctx.m_instance.m_sceneEffectMgr.createAndAdd(EffectType.eLinkEffect, EffectRenderType.eSpriteEffectRender) as LinkEffect;
            m_linkEffect.setGameObject(m_spriteGo);
            m_linkEffect.setTableID(1);
            m_linkEffect.setLoop(true);
            m_linkEffect.play();
        }

        protected void testImageEffect()
        {
            m_spriteAni = Ctx.m_instance.m_spriteAniMgr.createAndAdd();
            m_spriteAni.selfGo = m_imageGo;
            m_spriteAni.tableID = 1;
            m_spriteAni.bLoop = true;
            m_spriteAni.play();
        }
    }
}