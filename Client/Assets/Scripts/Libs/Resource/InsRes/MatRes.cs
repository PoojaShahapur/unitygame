using UnityEngine;

namespace SDK.Lib
{
    public class MatRes : InsResBase
    {
        public Material m_mat;

        public MatRes()
        {

        }

        public Material getMat()
        {
            return m_mat;
        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            m_mat = res.getObject(res.getPrefabName()) as Material;
            base.initImpl(res);
        }

        public override void unload()
        {
            if (m_mat != null)
            {
                UtilApi.UnloadAsset(m_mat);
                m_mat = null;
            }
            base.unload();
        }
    }
}