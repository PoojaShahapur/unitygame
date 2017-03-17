﻿using SDK.Lib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 选择角色界面
     */
    public class UISelectRole : Form
    {
        private UInt64 selAvatarDBID = 0;
        private string stringAvatarName = "";
        private bool startCreateAvatar = false;
        private Dictionary<UInt64, Dictionary<string, object>> ui_avatarList = null;

        protected AuxLabel mInfoLabel;
        protected AuxGo mMainGroup;
        protected AuxGo mCreateGroup;
        protected AuxButton mPlayerBtn;            // 一个玩家
        protected AuxInputField mNameInputField;// 创建名字输入

        public override void onInit()
        {
            base.onInit();

            this.mInfoLabel = new AuxLabel();
            this.mMainGroup = new AuxGo();
            this.mCreateGroup = new AuxGo();
            this.mPlayerBtn = new AuxButton();
            this.mNameInputField = new AuxInputField();
        }

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

            this.updateRoleList();
        }

        override public void onExit()
        {
            base.onExit();
        }

        // 关联窗口
        protected void findWidget()
        {
            this.mInfoLabel.setSelfGo(mGuiWin.mUiRoot, SelectRoleComPath.PathLabelInfo);
            this.mMainGroup.setSelfGo(mGuiWin.mUiRoot, SelectRoleComPath.PathMainGroup);
            this.mCreateGroup.setSelfGo(mGuiWin.mUiRoot, SelectRoleComPath.PathCreateGroup);

            this.mPlayerBtn.setSelfGo(mGuiWin.mUiRoot, SelectRoleComPath.PathBtnRoleOne);
            this.mNameInputField.setSelfGo(
                mGuiWin.mUiRoot, 
                SelectRoleComPath.PathBtnInputFieldName
                );

            this.mCreateGroup.setVisible(false);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(mGuiWin.mUiRoot, SelectRoleComPath.PathBtnEnter, onEnterBtnClk);
            UtilApi.addEventHandle(mGuiWin.mUiRoot, SelectRoleComPath.PathBtnCreateRole, onCreateRoleBtnClk);
            UtilApi.addEventHandle(mGuiWin.mUiRoot, SelectRoleComPath.PathBtnDeleteRole, onDeleteRoleBtnClk);

            UtilApi.addEventHandle(mGuiWin.mUiRoot, SelectRoleComPath.PathBtnRoleOne, onSelectRoleBtnClick);
            UtilApi.addEventHandle(mGuiWin.mUiRoot, SelectRoleComPath.PathBtnCreateOk, onCreateOkBtnClk);
        }

        public void onEnterBtnClk()
        {
            if(startCreateAvatar == false)
            {
                if (selAvatarDBID == 0)
                {
                    err("Please select a Avatar!(请选择角色!)");
                }
                else
                {
                    info("Please wait...(请稍后...)");

                    // 先进场景，再发送选择具体角色的消息
                    //KBEngine.Event.fireIn("selectAvatarGame", selAvatarDBID);
                    //Application.LoadLevel("world");

                    Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUILogin);
                    Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUISelectRole);

                    Ctx.mInstance.mModuleSys.unloadModule(ModuleId.LOGINMN);
                    Ctx.mInstance.mModuleSys.loadModule(ModuleId.GAMEMN);
                }
            }
        }

        public void onCreateRoleBtnClk()
        {
            this.toggleCreateRole();
        }

        public void onDeleteRoleBtnClk()
        {
            if (startCreateAvatar == false)
            {
                if (selAvatarDBID == 0)
                {
                    err("Please select a Avatar!(请选择角色!)");
                }
                else
                {
                    info("Please wait...(请稍后...)");

                    if (ui_avatarList != null && ui_avatarList.Count > 0)
                    {
                        Dictionary<string, object> avatarinfo = ui_avatarList[selAvatarDBID];
                    }
                }
            }
        }

        public void onCreateOkBtnClk()
        {
            if (startCreateAvatar)
            {
                stringAvatarName = this.mNameInputField.getText();

                if (stringAvatarName.Length > 1)
                {
                    this.toggleCreateRole();
                }
                else
                {
                    err("avatar name is null(角色名称为空)!");
                }
            }
        }

        public void onSelectRoleBtnClick()
        {
            
        }

        protected void toggleCreateRole()
        {
            startCreateAvatar = !startCreateAvatar;

            if (startCreateAvatar)
            {
                this.mMainGroup.setVisible(false);
                this.mCreateGroup.setVisible(true);
            }
            else
            {
                this.mMainGroup.setVisible(true);
                this.mCreateGroup.setVisible(false);
            }
        }

        public void err(string s)
        {
            this.mInfoLabel.setColor(Color.red);
            this.mInfoLabel.setText(s);
        }

        public void info(string s)
        {
            this.mInfoLabel.setColor(Color.green);
            this.mInfoLabel.setText(s);
        }

        public void updateRoleList()
        {
            if (this.ui_avatarList != null && ui_avatarList.Count > 0)
            {
                this.mPlayerBtn.show();

                int idx = 0;
                foreach (UInt64 dbid in ui_avatarList.Keys)
                {
                    Dictionary<string, object> info = ui_avatarList[dbid];
                    //	Byte roleType = (Byte)info["roleType"];
                    string name = (string)info["name"];
                    //	UInt16 level = (UInt16)info["level"];
                    UInt64 idbid = (UInt64)info["dbid"];

                    idx++;

                    if (selAvatarDBID == idbid)
                    {
                        this.mPlayerBtn.setColor(Color.red);
                    }

                    this.mPlayerBtn.setText(name);

                    selAvatarDBID = idbid;
                }
            }
            else
            {
                this.mPlayerBtn.hide();
            }
        }

        public void setAvatarList(Dictionary<UInt64, Dictionary<string, object>> avatarList)
        {
            this.ui_avatarList = avatarList;
            this.updateRoleList();
        }
    }
}