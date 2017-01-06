using System.Collections.Generic;

namespace SDK.Lib
{
    public class UIAttrSystem
    {
        public MDictionary<UIFormId, UIAttrItem> mId2AttrDic;
        protected LuaCSBridgeUICore mLuaCSBridgeUICore;

        public UIAttrSystem()
        {
            mId2AttrDic = new MDictionary<UIFormId, UIAttrItem>();

            // ****************** Canvas_50 开始**********************

            // ****************** 第二层开始 ***********************

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************
            // ****************** Canvas_50 结束 **********************

            // ****************** Canvas_100 开始 **********************
            // ****************** 第二层开始 ***********************
            addAttrItem(UIFormId.eUILogin, UICanvasID.eSecondCanvas, UILayerId.eSecondLayer, "UILogin");
            addAttrItem(UIFormId.eUISelectRole, UICanvasID.eSecondCanvas, UILayerId.eSecondLayer, "UISelectRole");
            addAttrItem(UIFormId.eUITest, UICanvasID.eSecondCanvas, UILayerId.eSecondLayer, "UITest");
            addAttrItem(UIFormId.eUITerrainEdit, UICanvasID.eSecondCanvas, UILayerId.eSecondLayer, "UITerrainEdit");
            addAttrItem(UIFormId.eUIPack, UICanvasID.eSecondCanvas, UILayerId.eSecondLayer, "UIPack");
            addAttrItem(UIFormId.eUIJoyStick, UICanvasID.eFirstCanvas, UILayerId.eSecondLayer, "UIJoyStick");
            addAttrItem(UIFormId.eUIForwardForce, UICanvasID.eFirstCanvas, UILayerId.eSecondLayer, "UIForwardForce");

            // ****************** 第二层结束 ***********************

            // ****************** 第四层开始 ***********************

            // ****************** 第四层结束 ***********************

            // ****************** 顶层开始 ***********************

            // ****************** 顶层结束 ***********************
            // ****************** Canvas_100 结束 **********************
        }

        protected void addAttrItem(UIFormId formId, UICanvasID canvasId, UILayerId layerId, string formName)
        {
            if (!mId2AttrDic.ContainsKey(formId))
            {
                mId2AttrDic[formId] = new UIAttrItem();
                mId2AttrDic[formId].mCanvasID = canvasId > UICanvasID.eCanvas_Total ? UICanvasID.eSecondCanvas : canvasId;
                mId2AttrDic[formId].mLayerID = layerId > UILayerId.eMaxLayer ? UILayerId.eSecondLayer : layerId;
                mId2AttrDic[formId].addUISceneType(UISceneType.eUIScene_Game);
                mId2AttrDic[formId].mWidgetPath = string.Format("{0}{1}/{2}{3}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathComUI], formName, formName, ".prefab");
                mId2AttrDic[formId].mScriptTypeName = string.Format("Game.UI.{0}", formName);
            }
        }

        public void init()
        {
            mLuaCSBridgeUICore = new LuaCSBridgeUICore(this);
            // doFile 会重复执行文件中的内容，可能会覆盖之前表中的内容
            //Ctx.mInstance.mLuaSystem.doFile("MyLua/Libs/UI/UICore/UIAttrSystem.lua");
            //Ctx.mInstance.mLuaSystem.requireFile("MyLua.Libs.UI.UICore.UIAttrSystem");
            //Ctx.mInstance.mLuaSystem.requireFile("MyLua/Libs/UI/UICore/UIAttrSystem.lua");
            mLuaCSBridgeUICore.loadLuaCfg();
        }

        public string getPath(UIFormId id)
        {
            if (mId2AttrDic.ContainsKey(id))
            {
                return mId2AttrDic[id].mWidgetPath;
                //ret = Ctx.mInstance.mCfg.m_pathLst[(int)ResPathType.ePathComUI] + ret;
                //return ret;
            }

            return null;
        }

        // 通过路径获取
        public UIFormId GetFormIDByPath(string resPath, ResPathType pathType)
        {
            foreach(UIFormId id in mId2AttrDic.Keys)
            {
                if (ResPathType.ePathComUI == pathType)
                {
                    if (mId2AttrDic[id].mWidgetPath == resPath)
                    {
                        return id;
                    }
                }
                else if (ResPathType.ePathCodePath == pathType)
                {
                    if (mId2AttrDic[id].mCodePath == resPath)
                    {
                        return id;
                    }
                }
            }

            return (UIFormId)0;       // 默认返回最大值
        }
    }
}