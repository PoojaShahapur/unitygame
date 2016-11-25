using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {
    public InputField myName;	

    public void OnStartGame()
    {
        PlayerPrefs.SetString("myname", myName.text);
        SceneManager.LoadScene("NewWorld");
    }
}
