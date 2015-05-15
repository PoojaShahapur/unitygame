using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 自己回合提示界面
     */
    public class SelfTurnTip : SceneComponent
    {
        // 显示自己回合提示
        public void turnBegin()
        {
            //出现你的回合
            iTween.ScaleTo(gameObject, Vector3.one, 0.5f);
            iTween.ScaleTo(gameObject, iTween.Hash(
                "scale", Vector3.one * 0.00001f,
                "time", 0.5f,
                "delay", 1f
                ));
        }
    }
}