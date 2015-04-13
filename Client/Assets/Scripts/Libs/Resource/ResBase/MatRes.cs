using UnityEngine;

namespace SDK.Lib
{
    public class MatRes : InsResBase
    {
        public Material m_mat;

        public override void unload()
        {
            m_mat = null;
        }
    }
}