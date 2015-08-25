using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItem> m_dicAttr = new Dictionary<UIFormID, UIAttrItem>();
        protected LuaCSBridgeUICore m_luaCSBridgeUICore;

        public UIAttrs()
        {
            m_dicAttr = new Dictionary<UIFormID, UIAttrItem>();

            // ****************** Canvas_50 开始**********************

            // ****************** 第二层开始 ***********************
            m_dicAttr[UIFormID.eUITuJianTop] = new UIAttrItem();
            m_dicAttr[UIFormID.eUITuJianTop].m_canvasID = UICanvasID.eCanvas_50;
            m_dicAttr[UIFormID.eUITuJianTop].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUITuJianTop].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUITuJianTop].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITuJian", "UITuJianTop", ".prefab");
            m_dicAttr[UIFormID.eUITuJianTop].m_scriptTypeName = "Game.UI.UITuJianTop";

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************
            m_dicAttr[UIFormID.eUIInfo] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIInfo].m_canvasID = UICanvasID.eCanvas_50;
            m_dicAttr[UIFormID.eUIInfo].m_LayerID = UILayerID.eForthLayer;
            m_dicAttr[UIFormID.eUIInfo].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIInfo].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIInfo", "UIInfo", ".prefab");
            m_dicAttr[UIFormID.eUIInfo].m_scriptTypeName = "Game.UI.UIInfo";
            // ****************** 第四层结束 ***********************
            // ****************** Canvas_50 结束 **********************

            // ****************** Canvas_100 开始 **********************
            // ****************** 第二层开始 ***********************
            m_dicAttr[UIFormID.eUIPack] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIPack].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIPack].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIPack].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIPack].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIPack", "UIPack", ".prefab");
            m_dicAttr[UIFormID.eUIPack].m_scriptTypeName = "Game.UI.UIPack";

            m_dicAttr[UIFormID.eUIPack].m_bNeedLua = false;
            m_dicAttr[UIFormID.eUIPack].m_luaScriptPath = "LuaScript/UI/UIPack/UIPack.lua";
            m_dicAttr[UIFormID.eUIPack].m_luaScriptTableName = "UIPack";

            m_dicAttr[UIFormID.eUILogin] = new UIAttrItem();
            m_dicAttr[UIFormID.eUILogin].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUILogin].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUILogin].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUILogin].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");
            m_dicAttr[UIFormID.eUILogin].m_scriptTypeName = "Game.UI.UILogin";

            m_dicAttr[UIFormID.eUIHeroSelect] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIHeroSelect].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIHeroSelect].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIHeroSelect].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIHeroSelect].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHeroSelect", "UIHeroSelect", ".prefab");
            m_dicAttr[UIFormID.eUIHeroSelect].m_scriptTypeName = "Game.UI.UIHeroSelect";

            m_dicAttr[UIFormID.eUIBlurBg] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIBlurBg].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIBlurBg].m_LayerID = UILayerID.eBtmLayer;
            m_dicAttr[UIFormID.eUIBlurBg].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIBlurBg].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIBlurBg", "UIBlurBg", ".prefab");
            m_dicAttr[UIFormID.eUIBlurBg].m_scriptTypeName = "Game.UI.UIBlurBg";

            m_dicAttr[UIFormID.eUITest] = new UIAttrItem();
            m_dicAttr[UIFormID.eUITest].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUITest].m_LayerID = UILayerID.eTopLayer;
            m_dicAttr[UIFormID.eUITest].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUITest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITest", "UITest", ".prefab");
            m_dicAttr[UIFormID.eUITest].m_scriptTypeName = "Game.UI.UITest";

            m_dicAttr[UIFormID.eUIDZ] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIDZ].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIDZ].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIDZ].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUIDZ].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIDZ", "UIDZ", ".prefab");
            m_dicAttr[UIFormID.eUIDZ].m_scriptTypeName = "Game.UI.UIDZ";

            m_dicAttr[UIFormID.eUIExtraOp] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIExtraOp].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIExtraOp].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIExtraOp].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUIExtraOp].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIExtraOp", "UIExtraOp", ".prefab");
            m_dicAttr[UIFormID.eUIExtraOp].m_scriptTypeName = "Game.UI.UIExtraOp";

            m_dicAttr[UIFormID.eUIChat] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIChat].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIChat].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIChat].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUIChat].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIChat", "UIChat", ".prefab");
            m_dicAttr[UIFormID.eUIChat].m_scriptTypeName = "Game.UI.UIChat";

            m_dicAttr[UIFormID.eUIJobSelect] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIJobSelect].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIJobSelect].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIJobSelect].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIJobSelect].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIJobSelect", "UIJobSelect", ".prefab");
            m_dicAttr[UIFormID.eUIJobSelect].m_scriptTypeName = "Game.UI.UIJobSelect";

            m_dicAttr[UIFormID.eUITuJian] = new UIAttrItem();
            m_dicAttr[UIFormID.eUITuJian].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUITuJian].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUITuJian].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUITuJian].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITuJian", "UITuJian", ".prefab");
            m_dicAttr[UIFormID.eUITuJian].m_scriptTypeName = "Game.UI.UITuJian";

            m_dicAttr[UIFormID.eUIMain] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIMain].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIMain].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIMain].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIMain].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIMain", "UIMain", ".prefab");
            m_dicAttr[UIFormID.eUIMain].m_scriptTypeName = "Game.UI.UIMain";

            m_dicAttr[UIFormID.eUIHero] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIHero].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIHero].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIHero].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIHero].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHero", "UIHero", ".prefab");
            m_dicAttr[UIFormID.eUIHero].m_scriptTypeName = "Game.UI.UIHero";

            m_dicAttr[UIFormID.eUIOpenPack] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIOpenPack].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIOpenPack].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIOpenPack].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIOpenPack].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIOpenPack", "UIOpenPack", ".prefab");
            m_dicAttr[UIFormID.eUIOpenPack].m_scriptTypeName = "Game.UI.UIOpenPack";

            m_dicAttr[UIFormID.eUIShop] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIShop].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIShop].m_LayerID = UILayerID.eThirdLayer;
            m_dicAttr[UIFormID.eUIShop].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIShop].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIShop", "UIShop", ".prefab");
            m_dicAttr[UIFormID.eUIShop].m_scriptTypeName = "Game.UI.UIShop";

            m_dicAttr[UIFormID.eUITask] = new UIAttrItem();
            m_dicAttr[UIFormID.eUITask].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUITask].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUITask].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUITask].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITask", "UITask", ".prefab");
            m_dicAttr[UIFormID.eUITask].m_scriptTypeName = "Game.UI.UITask";

            m_dicAttr[UIFormID.eUIMaze] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIMaze].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIMaze].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIMaze].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIMaze].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIMaze", "UIMaze", ".prefab");
            m_dicAttr[UIFormID.eUIMaze].m_scriptTypeName = "Game.UI.UIMaze";

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************

            // ****************** 顶层开始 ***********************
            m_dicAttr[UIFormID.eUILogicTest] = new UIAttrItem();
            m_dicAttr[UIFormID.eUILogicTest].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUILogicTest].m_LayerID = UILayerID.eTopLayer;
            m_dicAttr[UIFormID.eUILogicTest].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUILogicTest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogicTest", "UILogicTest", ".prefab");
            m_dicAttr[UIFormID.eUILogicTest].m_scriptTypeName = "Game.UI.UILogicTest";

            m_dicAttr[UIFormID.eUIGM] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIGM].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIGM].m_LayerID = UILayerID.eTopLayer;
            m_dicAttr[UIFormID.eUIGM].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIGM].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIGM", "UIGM", ".prefab");
            m_dicAttr[UIFormID.eUIGM].m_scriptTypeName = "Game.UI.UIGM";
            // ****************** 顶层结束 ***********************
            // ****************** Canvas_100 结束 **********************

            m_luaCSBridgeUICore = new LuaCSBridgeUICore(this);
            m_luaCSBridgeUICore.DoFile("script/UI/UICore/UIAttrs.lua");
            m_luaCSBridgeUICore.loadLuaCfg();
        }

        public string getPath(UIFormID id)
        {
            if (m_dicAttr.ContainsKey(id))
            {
                return m_dicAttr[id].m_widgetPath;
                //ret = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + ret;
                //return ret;
            }

            return null;
        }

        // 通过路径获取
        public UIFormID GetFormIDByPath(string resPath, ResPathType pathType)
        {
            foreach(UIFormID id in m_dicAttr.Keys)
            {
                if (ResPathType.ePathComUI == pathType)
                {
                    if (m_dicAttr[id].m_widgetPath == resPath)
                    {
                        return id;
                    }
                }
                else if (ResPathType.ePathCodePath == pathType)
                {
                    if (m_dicAttr[id].m_codePath == resPath)
                    {
                        return id;
                    }
                }
            }

            return (UIFormID)0;       // 默认返回最大值
        }
    }
}