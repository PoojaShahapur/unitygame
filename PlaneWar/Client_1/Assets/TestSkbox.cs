using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkbox : MonoBehaviour {

	public float addnum = 0.1f;
	private Material mt ;
	// Use this for initialization
	void Start () {

		mt = GetComponent<Skybox> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		mt.SetFloat ("_Rotation", mt.GetFloat ("_Rotation") + addnum);
	}
}
