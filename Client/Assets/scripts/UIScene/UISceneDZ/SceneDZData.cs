using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    public class SceneDZData
    {
        public dzturn m_dzturn = new dzturn();          // 翻转按钮，结束当前一局
        public luckycoin m_luckycoin = new luckycoin(); // 对战场景中的幸运币

        public GameObject m_centerGO;                   // 中心 GO
        public GameObject m_startGO;                   // 开始
    }
}