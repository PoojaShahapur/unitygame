using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 模型定义，目前玩家分这几种， npc 怪物只有一个模型 
     */
    public enum ePlayerModelType
    {
        eModelHead = 0,         // 头
        eModelChest = 1,        // 胸
        eModelWaist = 2,        // 腰
        eModelLeg = 3,          // 腿

        eModelFoot = 4,         // 脚
        eModelArm = 5,          // 胳膊
        eModelHand = 6,         // 手
        eModelTotal
    }

    public enum eNpcModelType
    {
        eModelBody = 0,
        eModelTotal
    }

    public enum eMonstersModelType
    {
        eModelBody = 0,
        eModelTotal
    }

    /**
     * @brief 蒙皮子模型
     */
    public class SkinSubModel
    {
        public ResItem m_res;           // 加载的资源
        public string m_bundleName;     // 资源包 assetBundle 的名字
        public string m_partName;       // 资源的名字
        public GameObject m_partGo;     // 实例化的资源
    }
}