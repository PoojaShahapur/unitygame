using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class TextureMgr : ResMgrBase
    {
        public TextureMgr()
        {

        }

        public TextureRes getTexByCardID(int cardId)
        {
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathImage], "pig.prefab");

            return syncGet<TextureRes>(path) as TextureRes;
        }
    }
}