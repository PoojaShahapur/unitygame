using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 对战区域接口
     */
    public interface ISceneDZArea
    {
        SceneCardEntityBase getUnderSceneCard(GameObject underGo);
        void addCardToOutList(SceneDragCard card, int idx = 0);
        SceneCardEntityBase getSceneCardByThisID(uint thisID);
    }
}