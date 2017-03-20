namespace SDK.Lib
{
    /**
     * @brief 相机目标实体
     */
    public interface ICamTargetEntiry
    {
        UnityEngine.GameObject getNativeTarget();
        void addTargetCreatedHandle(MAction<IDispatchObject> dispObj);
        void removeTargetCreatedHandle(MAction<IDispatchObject> dispObj);
    }
}