using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    public class UITerrainEdit : Form
    {
        protected GameObject m_btnExportScene;

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
            m_btnExportScene = UtilApi.TransFindChildByPObjAndPath(mGuiWin.mUiRoot, TerrainEditPath.BtnExportScene);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(mGuiWin.mUiRoot, TerrainEditPath.BtnExportScene, onBtnExportSceneClk);
            UtilApi.addEventHandle(mGuiWin.mUiRoot, TerrainEditPath.BtnEnableLog, onBtnEnableLogClk);
        }

        protected void onBtnExportSceneClk()
        {
            exportScene();
        }

        protected void exportScene()
        {
            Ctx.mInstance.mTerrainGroup.serializeTerrain(0, 0);
        }

        protected void onBtnEnableLogClk()
        {
            Ctx.mInstance.mLogSys.setEnableLog(true);
        }
    }
}