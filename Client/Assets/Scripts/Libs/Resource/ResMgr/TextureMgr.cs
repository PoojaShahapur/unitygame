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

        public override void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            (m_path2ResDic[path] as TextureRes).m_texture = res.getObject(res.getPrefabName()) as Texture;

            if (m_path2ListenItemDic[path].m_loaded != null)
            {
                m_path2ListenItemDic[path].m_loaded(m_path2ResDic[path]);
            }

            base.onLoaded(resEvt);
        }
    }
}