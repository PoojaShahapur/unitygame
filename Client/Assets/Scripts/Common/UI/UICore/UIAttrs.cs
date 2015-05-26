using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class UIAttrs
    {
        public Dictionary<UIFormID, UIAttrItem> m_dicAttr = new Dictionary<UIFormID, UIAttrItem>();

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

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************
            m_dicAttr[UIFormID.eUIInfo] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIInfo].m_canvasID = UICanvasID.eCanvas_50;
            m_dicAttr[UIFormID.eUIInfo].m_LayerID = UILayerID.eForthLayer;
            m_dicAttr[UIFormID.eUIInfo].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIInfo].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIInfo", "UIInfo", ".prefab");
            // ****************** 第四层结束 ***********************
            // ****************** Canvas_50 结束 **********************

            // ****************** Canvas_100 开始 **********************
            // ****************** 第二层开始 ***********************
            m_dicAttr[UIFormID.eUIPack] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIPack].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIPack].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIPack].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIPack].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIPack", "UIPack", ".prefab");

            m_dicAttr[UIFormID.eUILogin] = new UIAttrItem();
            m_dicAttr[UIFormID.eUILogin].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUILogin].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUILogin].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUILogin].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");

            m_dicAttr[UIFormID.eUIHeroSelect] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIHeroSelect].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIHeroSelect].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIHeroSelect].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIHeroSelect].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHeroSelect", "UIHeroSelect", ".prefab");

            m_dicAttr[UIFormID.eUIBlurBg] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIBlurBg].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIBlurBg].m_LayerID = UILayerID.eBtmLayer;
            m_dicAttr[UIFormID.eUIBlurBg].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIBlurBg].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIBlurBg", "UIBlurBg", ".prefab");

            m_dicAttr[UIFormID.eUITest] = new UIAttrItem();
            m_dicAttr[UIFormID.eUITest].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUITest].m_LayerID = UILayerID.eTopLayer;
            m_dicAttr[UIFormID.eUITest].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUITest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITest", "UITest", ".prefab");

            m_dicAttr[UIFormID.eUIDZ] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIDZ].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIDZ].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIDZ].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUIDZ].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIDZ", "UIDZ", ".prefab");

            m_dicAttr[UIFormID.eUIExtraOp] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIExtraOp].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIExtraOp].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIExtraOp].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUIExtraOp].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIExtraOp", "UIExtraOp", ".prefab");

            m_dicAttr[UIFormID.eUIChat] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIChat].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIChat].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIChat].addUISceneType(UISceneType.eUIScene_DZ);
            m_dicAttr[UIFormID.eUIChat].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIChat", "UIChat", ".prefab");

            m_dicAttr[UIFormID.eUIJobSelect] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIJobSelect].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIJobSelect].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIJobSelect].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIJobSelect].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIJobSelect", "UIJobSelect", ".prefab");

            m_dicAttr[UIFormID.eUITuJian] = new UIAttrItem();
            m_dicAttr[UIFormID.eUITuJian].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUITuJian].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUITuJian].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUITuJian].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UITuJian", "UITuJian", ".prefab");

            m_dicAttr[UIFormID.eUIMain] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIMain].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIMain].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIMain].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIMain].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIMain", "UIMain", ".prefab");

            m_dicAttr[UIFormID.eUIHero] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIHero].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIHero].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIHero].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIHero].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIHero", "UIHero", ".prefab");

            m_dicAttr[UIFormID.eUIOpenPack] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIOpenPack].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIOpenPack].m_LayerID = UILayerID.eSecondLayer;
            m_dicAttr[UIFormID.eUIOpenPack].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIOpenPack].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIOpenPack", "UIOpenPack", ".prefab");

            m_dicAttr[UIFormID.eUIShop] = new UIAttrItem();
            m_dicAttr[UIFormID.eUIShop].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUIShop].m_LayerID = UILayerID.eThirdLayer;
            m_dicAttr[UIFormID.eUIShop].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUIShop].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UIShop", "UIShop", ".prefab");

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************

            // ****************** 顶层开始 ***********************
            m_dicAttr[UIFormID.eUILogicTest] = new UIAttrItem();
            m_dicAttr[UIFormID.eUILogicTest].m_canvasID = UICanvasID.eCanvas_100;
            m_dicAttr[UIFormID.eUILogicTest].m_LayerID = UILayerID.eTopLayer;
            m_dicAttr[UIFormID.eUILogicTest].addUISceneType(UISceneType.eUIScene_Game);
            m_dicAttr[UIFormID.eUILogicTest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI], "UILogicTest", "UILogicTest", ".prefab");
            // ****************** 顶层结束 ***********************
            // ****************** Canvas_100 结束 **********************
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