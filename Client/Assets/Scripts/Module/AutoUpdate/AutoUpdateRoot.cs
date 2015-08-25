using Game.AutoUpdate;
using SDK.Lib;
using UnityEngine;

public class AutoUpdateRoot : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        Ctx.m_instance.m_autoUpdate = new Game.AutoUpdate.AutoUpdateSys();
        ((Ctx.m_instance.m_autoUpdate) as Game.AutoUpdate.AutoUpdateSys).Start();
	}
}