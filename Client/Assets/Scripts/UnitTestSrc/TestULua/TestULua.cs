using UnityEngine;
using System.Collections;

public class TestULua : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LuaScriptMgr lua = new LuaScriptMgr();
        lua.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}