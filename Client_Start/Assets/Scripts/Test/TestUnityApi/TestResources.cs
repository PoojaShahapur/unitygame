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
            //UnityEngine.Object m_prefabObj = Resources.Load("Table/Test");
            //UnityEngine.Object m_prefabObj = Resources.Load("Table/CardBase_client", typeof(TextAsset)) as TextAsset;
            TextAsset m_prefabObj = Resources.Load("Table/CardBase_client", typeof(TextAsset)) as TextAsset;
            if(m_prefabObj != null && ((m_prefabObj as TextAsset) != null))
            {
                byte[] bytes = (m_prefabObj as TextAsset).bytes;

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
            GameObject m_prefabObj = Resources.Load<GameObject>("Model/CardModel");
            //UnityEngine.Object.DestroyImmediate(m_prefabObj, true);   // Destroy 磁盘上的 Assets 资源
            UnityEngine.Object.DestroyObject(m_prefabObj);

            Debug.Log("Test Result");
        }
    }
}