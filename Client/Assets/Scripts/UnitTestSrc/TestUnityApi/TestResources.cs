using SDK.Common;
using System.IO;
using UnityEngine;

namespace UnitTestSrc
{
    public class TestResources
    {
        public void run()
        {
            //testResLoad();
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
                path = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalDataDir(), "Prefabs/Resources/Table/CardBase_client_bak.txt");
                Ctx.m_instance.m_localFileSys.writeFileByte(path, bytes);
            }
        }
    }
}