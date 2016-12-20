using SDK.Lib;
using UnityEngine;

public class AutoUpdateRoot : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        Ctx.mInstance.mAutoUpdate = new Game.AutoUpdate.AutoUpdateSys();
        ((Ctx.mInstance.mAutoUpdate) as Game.AutoUpdate.AutoUpdateSys).Start();
	}
}