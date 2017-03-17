using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIConnect : MonoBehaviour {

    public Text txtAnim;
    private string txt = "";
    private float startTime;
    public void Start()
    {
        startTime = Time.time;
    }
    public void Update()
    {
        if (Time.time - startTime >= 1.0f)
        {
            if (txt.Length == 3)
            {
                txt = ".";
            }
            else
            {
                txt = txt + ".";
            }
            txtAnim.text = txt;
            startTime = Time.time;
        }
    }
}
