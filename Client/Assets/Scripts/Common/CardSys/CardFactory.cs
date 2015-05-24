using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 卡牌生成
     */
    public class CardFactory
    {
        // 创建一个卡牌
        public GameObject createCard(CardItemBase cardItem)
        {
            GameObject tmpGO = null;
            GameObject go = null;
            tmpGO = Ctx.m_instance.m_modelMgr.getSceneCardModel((CardType)cardItem.m_tableItemCard.m_type).getObject() as GameObject;   // 加载卡牌模型
            if (tmpGO != null)
            {
                go = UtilApi.Instantiate(tmpGO) as GameObject;
                UtilApi.normalPos(go.transform);

                UtilApi.updateCardDataNoChange(cardItem.m_tableItemCard, go);
                UtilApi.updateCardDataChange(cardItem.m_tableItemCard, go);
            }

            return go;
        }

        // 修改卡牌纹理
        protected void modifyTex(GameObject go_, CardItemBase cardItem)
        {
            // 
        }

        public void destroyCard(CardItemBase cardItem)
        {
            Ctx.m_instance.m_modelMgr.unload("", null);
        }
    }
}