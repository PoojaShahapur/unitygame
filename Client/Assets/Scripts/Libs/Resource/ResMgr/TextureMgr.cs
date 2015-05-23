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

        public override void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            (m_path2ResDic[path] as TextureRes).m_texture = res.getObject(res.getPrefabName()) as Texture;
            m_path2ResDic[path].refCountResLoadResultNotify.loadEventDispatch.dispatchEvent(m_path2ResDic[path]);

            base.onLoadEventHandle(dispObj);
        }
    }
}