using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReliveEvent : MonoBehaviour {
    //返回大厅
    public void OnClickRoomBtn()
    {
        SceneManager.LoadScene("StartGame");
    }

    //立刻复活
    public void OnClickReliveBtn()
    {
        GlobalUI._Instance.ShowReliveArea(false);
        CreatePlayer._Instace.SetLeftSeconds(GlobalUI._Instance.GetLeftReliveSeconds());
    }
}
