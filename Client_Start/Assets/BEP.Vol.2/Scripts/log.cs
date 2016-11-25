using UnityEngine;
using System.Collections;
using Assets.BEP.Vol._2.Scripts;

public class log : MonoBehaviour {
    public static Logging logHelper = new Logging();
	// Use this for initialization
	void Awake() {        
        logHelper.Init(UnityEngine.Application.persistentDataPath + "/snowball_161028.log");
	}	
}
