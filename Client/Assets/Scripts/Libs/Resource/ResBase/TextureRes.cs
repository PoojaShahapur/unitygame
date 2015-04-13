using UnityEngine;

namespace SDK.Lib
{
    public class TextureRes : InsResBase
    {
        public Texture m_texture;

        public Texture getTexture()
        {
            return m_texture;
        }

        public override void unload()
        {
            m_texture = null;
        }
    }
}