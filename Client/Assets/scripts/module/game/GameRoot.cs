using UnityEngine;
using System.Collections;
using Game.Game;

public class GameRoot : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        GameSys.m_instance = new GameSys();
        GameSys.m_instance.Start();
	}

    void OnGUI()
    {
        //设置按钮中文字的颜色  
        GUI.color = Color.green;
        //设置按钮的背景色  
        GUI.backgroundColor = Color.red;

        //if (GUI.Button(new Rect(200, 200, 300, 40), "你已经在游戏场景了"))
        //{
            //开始游戏按钮被按下
        //}
    }
}