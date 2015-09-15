﻿using UnityEngine;
using Game.Login;
using SDK.Lib;

public class LoginRoot : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        Ctx.m_instance.m_loginSys = new LoginSys();
        ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle = new LoginFlowHandle();
        ((Ctx.m_instance.m_loginSys) as LoginSys).Start();
	}

    //void OnGUI()
    //{
        //设置按钮中文字的颜色  
        //GUI.color = Color.green;
        //设置按钮的背景色  
        //GUI.backgroundColor = Color.red;

        //if (GUI.Button(new Rect(200, 200, 300, 40), "你已经在游戏场景了"))
        //{
            //开始游戏按钮被按下
        //}
    //}
}