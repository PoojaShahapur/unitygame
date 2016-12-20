using SDK.Lib;
using System.IO;
using UnityEngine;

namespace UnitTest
{
    public class TestResources
    {
        public void run()
        {
           //testResLoad();
           //testDestroyImmediate();
        }

        protected void testResLoad()
        {
            string path;
            //UnityEngine.Object mPrefabObj = Resources.Load("Table/Test");
            //UnityEngine.Object mPrefabObj = Resources.Load("Table/CardBase_client", typeof(TextAsset)) as TextAsset;
            TextAsset mPrefabObj = Resources.Load("Table/CardBase_client", typeof(TextAsset)) as TextAsset;
            if(mPrefabObj != null && ((mPrefabObj as TextAsset) != null))
            {
                byte[] bytes = (mPrefabObj as TextAsset).bytes;

                // 输出文件
                path = Path.Combine(MFileSys.getLocalDataDir(), "Resources/Table/CardBase_client_bak.txt");

                MDataStream mDataStream = new MDataStream(path, null, FileMode.CreateNew, FileAccess.Write);
                mDataStream.writeByte(bytes);
                mDataStream.dispose();
                mDataStream = null;
            }
        }

        protected void testDestroyImmediate()
        {
            GameObject mPrefabObj = Resources.Load<GameObject>("Model/CardModel");
            //UnityEngine.Object.DestroyImmediate(mPrefabObj, true);   // Destroy 磁盘上的 Assets 资源
            UnityEngine.Object.DestroyObject(mPrefabObj);

            Debug.Log("Test Result");
        }
    }
}