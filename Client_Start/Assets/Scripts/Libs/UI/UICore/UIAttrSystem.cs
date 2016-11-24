using System.Collections.Generic;

namespace SDK.Lib
{
    public class UIAttrSystem
    {
        public Dictionary<UIFormID, UIAttrItem> mId2AttrDic;
        protected LuaCSBridgeUICore m_luaCSBridgeUICore;

        public UIAttrSystem()
        {
            mId2AttrDic = new Dictionary<UIFormID, UIAttrItem>();

            // ****************** Canvas_50 开始**********************

            // ****************** 第二层开始 ***********************

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************
            // ****************** Canvas_50 结束 **********************

            // ****************** Canvas_100 开始 **********************
            // ****************** 第二层开始 ***********************
            mId2AttrDic[UIFormID.eUILogin] = new UIAttrItem();
            mId2AttrDic[UIFormID.eUILogin].m_canvasID = UICanvasID.eSecondCanvas;
            mId2AttrDic[UIFormID.eUILogin].m_LayerID = UILayerID.eSecondLayer;
            mId2AttrDic[UIFormID.eUILogin].addUISceneType(UISceneType.eUIScene_Game);
            mId2AttrDic[UIFormID.eUILogin].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");
            mId2AttrDic[UIFormID.eUILogin].m_scriptTypeName = "Game.UI.UILogin";

            mId2AttrDic[UIFormID.eUISelectRole] = new UIAttrItem();
            mId2AttrDic[UIFormID.eUISelectRole].m_canvasID = UICanvasID.eSecondCanvas;
            mId2AttrDic[UIFormID.eUISelectRole].m_LayerID = UILayerID.eSecondLayer;
            mId2AttrDic[UIFormID.eUISelectRole].addUISceneType(UISceneType.eUIScene_Game);
            mId2AttrDic[UIFormID.eUISelectRole].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UISelectRole", "UISelectRole", ".prefab");
            mId2AttrDic[UIFormID.eUISelectRole].m_scriptTypeName = "Game.UI.UISelectRole";

            mId2AttrDic[UIFormID.eUITest] = new UIAttrItem();
            mId2AttrDic[UIFormID.eUITest].m_canvasID = UICanvasID.eSecondCanvas;
            mId2AttrDic[UIFormID.eUITest].m_LayerID = UILayerID.eTopLayer;
            mId2AttrDic[UIFormID.eUITest].addUISceneType(UISceneType.eUIScene_Game);
            mId2AttrDic[UIFormID.eUITest].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UILogin", "UILogin", ".prefab");
            mId2AttrDic[UIFormID.eUITest].m_scriptTypeName = "Game.UI.UILogin";


            mId2AttrDic[UIFormID.eUITerrainEdit] = new UIAttrItem();
            mId2AttrDic[UIFormID.eUITerrainEdit].m_canvasID = UICanvasID.eSecondCanvas;
            mId2AttrDic[UIFormID.eUITerrainEdit].m_LayerID = UILayerID.eSecondLayer;
            mId2AttrDic[UIFormID.eUITerrainEdit].addUISceneType(UISceneType.eUIScene_Game);
            mId2AttrDic[UIFormID.eUITerrainEdit].m_widgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], "UITerrainEdit", "UITerrainEdit", ".prefab");
            mId2AttrDic[UIFormID.eUITerrainEdit].m_scriptTypeName = "Game.UI.UITerrainEdit";

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
            if (mId2AttrDic.ContainsKey(id))
            {
                return mId2AttrDic[id].m_widgetPath;
                //ret = Ctx.mInstance.mCfg.m_pathLst[(int)ResPathType.ePathComUI] + ret;
                //return ret;
            }

            return null;
        }

        // 通过路径获取
        public UIFormID GetFormIDByPath(string resPath, ResPathType pathType)
        {
            foreach(UIFormID id in mId2AttrDic.Keys)
            {
                if (ResPathType.ePathComUI == pathType)
                {
                    if (mId2AttrDic[id].m_widgetPath == resPath)
                    {
                        return id;
                    }
                }
                else if (ResPathType.ePathCodePath == pathType)
                {
                    if (mId2AttrDic[id].m_codePath == resPath)
                    {
                        return id;
                    }
                }
            }

            return (UIFormID)0;       // 默认返回最大值
        }
    }
}