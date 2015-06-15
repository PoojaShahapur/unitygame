using Game.Login;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UILogin : Form
    {
        protected SpriteAni m_spriteAni;        //登陆界面中间的动画
        protected GameObject m_imageEffect;

        override public void onShow()
        {
            base.onShow();
            // 加载测试界面
            //Ctx.m_instance.m_uiMgr.loadForm<UILogicTest>(UIFormID.eUILogicTest);
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        override public void onExit()
        {
            base.onExit();
            if (m_spriteAni != null)
            {
                m_spriteAni.dispose();
                m_spriteAni = null;
            }
        }

        // 关联窗口
        protected void findWidget()
        {
            AuxInputField lblName = new AuxInputField(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
            lblName.text = "zhanghao06";      //zhanghao01---zhanghao09

            if(Ctx.m_instance.m_systemSetting.getString(SystemSetting.USERNAME) != default(string))
            {
                lblName.text = Ctx.m_instance.m_systemSetting.getString(SystemSetting.USERNAME);
            }

            AuxInputField lblPassWord = new AuxInputField(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);
            lblPassWord.text = "1";

            if (Ctx.m_instance.m_systemSetting.getString(SystemSetting.PASSWORD) != default(string))
            {
                lblPassWord.text = Ctx.m_instance.m_systemSetting.getString(SystemSetting.PASSWORD);
            }

            // 忽略鼠标事件
            UtilApi.getComByP<Image>(m_GUIWin.m_uiRoot, "ImageName").maskable = false;

            m_imageEffect = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, LoginComPath.PathImageEffect);
            UtilApi.SetActive(m_imageEffect, false);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LoginComPath.PathBtnLogin, onBtnClkLogin);
        }

        // 点击登陆处理
        protected void onBtnClkLogin()
        {
            if (Ctx.m_instance.m_loginSys.get_LoginState() != LoginState.eLoginingLoginServer && Ctx.m_instance.m_loginSys.get_LoginState() != LoginState.eLoginingGateServer)    // 如果没有正在登陆登陆服务器和网关服务器
            {
                AuxInputField lblName = new AuxInputField(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
                AuxInputField lblPassWord = new AuxInputField(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);

                if (validStr(lblName.text, lblPassWord.text))
                {
                    Ctx.m_instance.m_systemSetting.setString(SystemSetting.USERNAME, lblName.text);
                    Ctx.m_instance.m_systemSetting.setString(SystemSetting.PASSWORD, lblPassWord.text);

#if !DEBUG_NOTNET
                    if (Ctx.m_instance.m_loginSys.get_LoginState() != LoginState.eLoginNone)        // 先关闭之前的 socket
                    {
                        Ctx.m_instance.m_netMgr.closeSocket(Ctx.m_instance.m_cfg.m_ip, Ctx.m_instance.m_cfg.m_port);
                    }
                    Ctx.m_instance.m_loginSys.connectLoginServer(lblName.text, lblPassWord.text);
#else
                    Ctx.m_instance.m_moduleSys.loadModule(ModuleID.GAMEMN);
#endif

                    UtilApi.SetActive(m_imageEffect, true);
                    m_spriteAni = Ctx.m_instance.m_spriteAniMgr.createAndAdd();
                    if (m_spriteAni != null)
                    {
                        m_spriteAni.selfGo = m_imageEffect;
                        m_spriteAni.tableID = 12;
                        m_spriteAni.bLoop = true;
                        m_spriteAni.play();
                    }
                }
            }
        }

        // 验证字符串
        protected bool validStr(string name, string passwd)
        {
            if(name.Length == 0)
            {
                InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin3, LangItemID.eItem2); ;
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem22);
                UIInfo.showMsg(param);
                return false;
            }
            else if (UtilLogic.IsIncludeChinese(name))
            {
                InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin3, LangItemID.eItem0);
                UIInfo.showMsg(param);
                return false;
            }

            if (name.Length == 0)
            {
                InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin3, LangItemID.eItem3);
                UIInfo.showMsg(param);
                return false;
            }
            else if (UtilLogic.IsIncludeChinese(passwd))
            {
                InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin3, LangItemID.eItem1);
                UIInfo.showMsg(param);
                return false;
            }

            return true;
        }
    }
}