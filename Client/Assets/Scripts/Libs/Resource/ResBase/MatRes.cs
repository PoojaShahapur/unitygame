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
            m_mat = null;
        }
    }
}