using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public Texture rankBackGroundImage;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        // 显示排行榜
        //GUI.BeginGroup(new Rect(0, 0, 1100, 662));
        //GUI.Box(new Rect(0, 0, 250, 105), "");
        GUI.Box(new Rect(0, 0, 420, 542), rankBackGroundImage);
        //GUI.Label(new Rect(10, 10, 200, 30), "<size=20><color=white>1. 陈奥宇</color></size>");
        //GUI.DrawTexture(new Rect(110, 15, 120 * 1, 20), FHealthGauge);
        //GUI.Label(new Rect(10, 40, 200, 30), "<size=20><color=white>2. 机器人10号</color></size>");
        //GUI.DrawTexture(new Rect(110, 45, 120 * 2, 20), FStaminaGauge);
        //GUI.Label(new Rect(10, 70, 200, 30), "<size=20><color=white>3. 机器人8号</color></size>");
        //GUI.DrawTexture(new Rect(110, 75, 120 * 3, 20), FStrengthGauge);
        //GUI.EndGroup();
    }
}
