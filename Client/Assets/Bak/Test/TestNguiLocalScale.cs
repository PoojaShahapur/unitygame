using UnityEngine;
using System.Collections;

public class TestNguiLocalScale : MonoBehaviour 
{
    Transform localtran;
	// Use this for initialization
	void Start () 
    {
        localtran = transform;
        localtran.localScale = Vector3.one;
	}
	
	// Update is called once per frame
	void Update () 
    {
        localtran = transform;
	}
}