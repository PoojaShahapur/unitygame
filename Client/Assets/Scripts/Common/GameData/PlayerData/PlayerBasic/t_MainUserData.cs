using SDK.Lib;
using Game.UI;
namespace SDK.Common
{
    public class t_MainUserData
    {
        public uint m_dwUserTempID;             // 用户临时 id
        public string m_name = "";                   //玩家名字
        public uint m_gold;                     //玩家的金币

        public void derialize(ByteBuffer ba)
        {
            ba.readMultiByte(ref m_name, CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            ba.readUnsignedInt32(ref m_gold);

            //UISceneShop uiShop = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneShop>(UISceneFormID.eUISceneShop);
            //if (uiShop == null)
            //{
            //    Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneShop>(UISceneFormID.eUISceneShop);
            //}
            //uiShop = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneShop) as UISceneShop;
            //uiShop.updateGoldNum(m_gold);

            if(!Ctx.m_instance.m_uiMgr.hasForm(UIFormID.eUIShop))
            {
                Ctx.m_instance.m_uiMgr.loadForm<UIShop>(UIFormID.eUIShop);
            }

            UIShop shop = Ctx.m_instance.m_uiMgr.getForm<UIShop>(UIFormID.eUIShop);
            if(shop != null)
            {
                shop.UpdateGoldNum(m_gold);
            }
        }
    }
}