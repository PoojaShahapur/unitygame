using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class MatRes : InsResBase
    {
        public Material m_mat;

        public MatRes()
        {

        }

        override public void init(ResItem res)
        {
            // 获取资源单独保存
            m_mat = res.getObject(res.getPrefabName()) as Material;
            base.init(res);
        }

        public override void unload()
        {
            UtilApi.Destroy(m_mat);
            m_mat = null;
        }
    }
}