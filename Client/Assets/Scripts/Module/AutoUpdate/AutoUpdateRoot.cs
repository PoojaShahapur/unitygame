using Game.AutoUpdate;
using SDK.Common;
using UnityEngine;

public class AutoUpdateRoot : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        Ctx.m_instance.m_autoUpdate = new AutoUpdateSys();
        ((Ctx.m_instance.m_autoUpdate) as AutoUpdateSys).Start();
	}
}