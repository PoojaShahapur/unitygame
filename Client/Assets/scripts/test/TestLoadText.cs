using UnityEngine;
using System.Collections;

public class TestLoadText : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Table/base");
        string text = textAsset.text;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}
}