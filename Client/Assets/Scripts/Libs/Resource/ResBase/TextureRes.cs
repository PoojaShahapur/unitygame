using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class TextureRes : InsResBase
    {
        public Texture m_texture;

        public TextureRes()
        {

        }

        public Texture getTexture()
        {
            return m_texture;
        }

        override public void init(ResItem res)
        {
            m_texture = res.getObject(res.getPrefabName()) as Texture;
            base.init(res);
        }

        public override void unload()
        {
            UtilApi.Destroy(m_texture);
            m_texture = null;
        }
    }
}