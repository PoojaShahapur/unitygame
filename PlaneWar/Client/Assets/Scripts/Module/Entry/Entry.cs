using Game.Main;
using System.Collections;
using UnityEngine;

/**
 * @brief ���ģ����ԶҲ����̬���£�һ���޸ı����������ؿͻ��ˣ��������������������
 */
public class Entry : MonoBehaviour
{
    void Start()
    {
        // ��Ȼ�ӷ��������ص�һ��ģ��
        //StartCoroutine(downloadStart());
        execute();
    }

    // ���� Start ģ��
    IEnumerator downloadStart()
    {
        long curTime = System.DateTime.Now.Ticks;
        System.TimeSpan timeSpan = new System.TimeSpan(curTime);

        WWW www = WWW.LoadFromCacheOrDownload("http://127.0.0.1/UnityServer/Main.unity3d", (int)timeSpan.TotalSeconds);
        yield return www;

        // ʹ��Ԥ�����
        AssetBundle bundle = www.assetBundle;
#if UNITY_5
        // Unity5
        UnityEngine.Object bt = bundle.LoadAsset("Assets/Resources/Module/Start");
#elif UNITY_4_6 || UNITY_4_5
        // Unity4
        UnityEngine.Object bt = bundle.Load("Assets/Resources/Module/Start");
#endif
        UnityEngine.GameObject go = Instantiate(bt) as GameObject;
        bundle.Unload(false);
        bundle = null;
        www.Dispose();
        www = null;
    }

    // ���� Main ģ��
    protected void loadMainModule()
    {

    }

    // ִ��ģ��
    protected void execute()
    {
        MainModule mainModule = new MainModule();
        mainModule.mEntry = this;
        mainModule.run();
    }
}