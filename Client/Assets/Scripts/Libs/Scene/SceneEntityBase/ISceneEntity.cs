using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 场景中的实体接口
     */
    public interface ISceneEntity : IDelayHandleItem
    {
        void onTick(float delta);
    }
}