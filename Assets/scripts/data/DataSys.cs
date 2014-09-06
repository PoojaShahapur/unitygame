using UnityEngine;
using System.Collections;
using San.Guo;
/**
 * @brief 数据系统
 */
public class DataSys : MonoBehaviour 
{
    public DB m_db;

    void Awark()
    {
        m_db.Awark();
    }

	// Use this for initialization
	void Start () 
    {
        m_db.Start();
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_db.Update();
	}

    void OnApplicationQuit()
    {
        
    }
}
