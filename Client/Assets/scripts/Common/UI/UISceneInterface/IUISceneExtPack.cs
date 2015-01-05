namespace SDK.Common
{
    /**
     * @brief 场景商店
     */
    public interface IUISceneExtPack : ISceneForm
    {
        void showUI();
        void psstRetGiftBagCardsDataUserCmd(params uint[] idList);
    }
}