using System.Collections.Generic;

namespace SDK.Lib
{
    public class UIAttrSystem
    {
        public Dictionary<UIFormID, UIAttrItem> m_id2AttrDic;
        protected LuaCSBridgeUICore m_luaCSBridgeUICore;

        public UIAttrSystem()
        {
            m_id2AttrDic = new Dictionary<UIFormID, UIAttrItem>();

            // ****************** Canvas_50 开始**********************

            // ****************** 第二层开始 ***********************

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************
            // ****************** Canvas_50 结束 **********************

            // ****************** Canvas_100 开始 **********************
            // ****************** 第二层开始 ***********************
            m_id2AttrDic[UIFormID.eUILogin] = new UIAttrItem();
            m_id2AttrDic[UIFormID.eUILogin].m_canvasID = UICanvasID.eSecondCanvas;
            m_id2AttrDic[UIFormID.eUILogin].m_LayerID = UILayerID.eSecondLayer;
            m_id2AttrDic[UIFormID.eUILogin].addUISceneType(UISceneType.eUIScene_Game);
            m_id2AttrDic[UIFormID.eUILogin].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");
            m_id2AttrDic[UIFormID.eUILogin].m_scriptTypeName = "Game.UI.UILogin";

            m_id2AttrDic[UIFormID.eUITest] = new UIAttrItem();
            m_id2AttrDic[UIFormID.eUITest].m_canvasID = UICanvasID.eSecondCanvas;
            m_id2AttrDic[UIFormID.eUITest].m_LayerID = UILayerID.eTopLayer;
            m_id2AttrDic[UIFormID.eUITest].addUISceneType(UISceneType.eUIScene_Game);
            m_id2AttrDic[UIFormID.eUITest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");
            m_id2AttrDic[UIFormID.eUITest].m_scriptTypeName = "Game.UI.UILogin";


            m_id2AttrDic[UIFormID.eUITerrainEdit] = new UIAttrItem();
            m_id2AttrDic[UIFormID.eUITerrainEdit].m_canvasID = UICanvasID.eSecondCanvas;
            m_id2AttrDic[UIFormID.eUITerrainEdit].m_LayerID = UILayerID.eSecondLayer;
            m_id2AttrDic[UIFormID.eUITerrainEdit].addUISceneType(UISceneType.eUIScene_Game);
            m_id2AttrDic[UIFormID.eUITerrainEdit].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UITerrainEdit", "UITerrainEdit", ".prefab");
            m_id2AttrDic[UIFormID.eUITerrainEdit].m_scriptTypeName = "Game.UI.UITerrainEdit";

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************

            // ****************** 顶层开始 ***********************

            // ****************** 顶层结束 ***********************
            // ****************** Canvas_100 结束 **********************
        }

        public void init()
        {
            m_luaCSBridgeUICore = new LuaCSBridgeUICore(this);
            Ctx.mInstance.mLuaSystem.doFile("MyLua/Libs/UI/UICore/UIAttrSystem.lua");
            m_luaCSBridgeUICore.loadLuaCfg();
        }

        public string getPath(UIFormID id)
        {
            if (m_id2AttrDic.ContainsKey(id))
            {
                return m_id2AttrDic[id].m_widgetPath;
                //ret = Ctx.mInstance.mCfg.m_pathLst[(int)ResPathType.ePathComUI] + ret;
                //return ret;
            }

            return null;
        }

        // 通过路径获取
        public UIFormID GetFormIDByPath(string resPath, ResPathType pathType)
        {
            foreach(UIFormID id in m_id2AttrDic.Keys)
            {
                if (ResPathType.ePathComUI == pathType)
                {
                    if (m_id2AttrDic[id].m_widgetPath == resPath)
                    {
                        return id;
                    }
                }
                else if (ResPathType.ePathCodePath == pathType)
                {
                    if (m_id2AttrDic[id].m_codePath == resPath)
                    {
                        return id;
                    }
                }
            }

            return (UIFormID)0;       // 默认返回最大值
        }
    }
}