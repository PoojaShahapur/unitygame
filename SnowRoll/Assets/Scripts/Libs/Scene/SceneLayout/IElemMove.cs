using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 实现元素移动接口
     */
    public interface IElemMove
    {
        // 设置位置信息
        void setNormalPos(Vector3 pos);
        void setExpandPos(Vector3 pos);
    }
}