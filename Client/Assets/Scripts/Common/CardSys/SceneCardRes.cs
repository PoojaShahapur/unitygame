using SDK.Lib;
using UnityEngine;
namespace SDK.Common
{
    /**
     * @brief 场景卡牌资源，主要是显示卡牌使用的各种资源
     */
    public class SceneCardRes : SceneComponent
    {
        protected GameObject m_rootGo;
        protected CardModelItem m_cardModelItem;        // 异步加载的时候，使用 path 字段
        // 这些是为了卸载资源使用
        protected ModelRes m_modelRes;
        protected TextureRes m_headerTex;
        protected TextureRes m_frameTex;
        protected TextureRes m_yaoDaiTex;
        protected TextureRes m_pinZhiTex;

        public GameObject rootGo
        {
            get
            {
                return m_rootGo;
            }
        }

        virtual public void createCard(CardItemBase cardItem)
        {
            m_rootGo = new GameObject();
            UtilApi.setGOName(m_rootGo, "SceneCard");

            m_cardModelItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)cardItem.m_tableItemCard.m_type];
            m_modelRes = Ctx.m_instance.m_modelMgr.getSceneCardModel((CardType)cardItem.m_tableItemCard.m_type);
            gameObject = m_modelRes.InstantiateObject(m_cardModelItem.m_path);   // 加载卡牌模型
            if (gameObject != null)
            {
                UtilApi.SetParent(gameObject, m_rootGo);
                UtilApi.normalPos(gameObject.transform);

                UtilApi.updateCardDataNoChange(cardItem.m_tableItemCard, gameObject);
                UtilApi.updateCardDataChange(cardItem.m_tableItemCard, gameObject);

                //modifyTex(gameObject, cardItem);
            }
        }

        // 修改卡牌纹理
        protected void modifyTex(GameObject go_, CardItemBase cardItem)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathImage], cardItem.m_tableItemCard.m_cardHeader);
            m_headerTex = Ctx.m_instance.m_texMgr.syncGet<TextureRes>(path);

            Material mat = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_headerSubModel).GetComponent<Renderer>().material;
            mat.mainTexture = m_headerTex.getTexture();

            TableJobItemBody jobTable;
            // 边框
            jobTable = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)(cardItem.m_tableItemCard.m_career)).m_itemBody as TableJobItemBody;
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathImage], jobTable.m_frameImage);
            m_frameTex = Ctx.m_instance.m_texMgr.syncGet<TextureRes>(path);

            mat = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_frameSubModel).GetComponent<Renderer>().material;
            mat.mainTexture = m_frameTex.getTexture();

            // 腰带
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathImage], jobTable.m_yaoDaiImage);
            m_yaoDaiTex = Ctx.m_instance.m_texMgr.syncGet<TextureRes>(path);

            mat = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_yaoDaiSubModel).GetComponent<Renderer>().material;
            mat.mainTexture = m_yaoDaiTex.getTexture();

            // 品质
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathImage], UtilApi.getImageByPinZhi(cardItem.m_tableItemCard.m_quality));
            m_pinZhiTex = Ctx.m_instance.m_texMgr.syncGet<TextureRes>(path);

            mat = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_pinZhiSubModel).GetComponent<Renderer>().material;
            mat.mainTexture = m_pinZhiTex.getTexture();
        }

        public void dispose()
        {
            if (m_modelRes != null)
            {
                Ctx.m_instance.m_modelMgr.unload(m_modelRes.GetPath());
            }
            if (m_headerTex != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_headerTex.GetPath());
            }
            if (m_frameTex != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_frameTex.GetPath());
            }
            if (m_yaoDaiTex != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_yaoDaiTex.GetPath());
            }
            if (m_pinZhiTex != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_pinZhiTex.GetPath());
            }

            UtilApi.Destroy(m_rootGo);
        }
    }
}