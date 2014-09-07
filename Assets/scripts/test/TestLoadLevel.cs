using UnityEngine;
using System.Collections;
using SDK.Lib;
using SDK.Common;

public class TestLoadLevel : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
    void OnGUI()
    {
        //设置按钮中文字的颜色  
        GUI.color = Color.green;
        //设置按钮的背景色  
        GUI.backgroundColor = Color.red;

        if(GUI.Button(new Rect(10,50,100,200),"aaa"))
        {
            //开始游戏按钮被按下
            //GameObject.Destroy(this);//销毁这个脚本
            LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
            param.m_path = "Level1.unity3d";
            param.m_type = ResType.eLevelType;
            param.m_lvlName = "Level1";
            Ctx.m_instance.m_resMgr.load(param);
        }
	}
}