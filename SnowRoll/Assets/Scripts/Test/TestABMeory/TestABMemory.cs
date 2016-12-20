using UnityEngine;

namespace SDK.Lib
{
    public class TestABMemory : MonoBehaviour
    {
        void Start()
        {
            load();
            //load_1();
        }

        protected void load()
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/Windows/aaa/bbb.unity3d");
            Object bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            GameObject go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
        }

        protected void load_1()
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/Windows/aaa/bbb.unity3d");
            Object bt = null;
            GameObject go = null;
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
            bt = assetBundle.LoadAsset("Assets/Resources/TestPrefab/ABTest.prefab");
            go = UnityEngine.Object.Instantiate(bt) as GameObject;
            assetBundle.Unload(false);
        }
    }
}