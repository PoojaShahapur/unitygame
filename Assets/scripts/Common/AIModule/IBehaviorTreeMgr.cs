namespace SDK.Common
{
    public interface IBehaviorTreeMgr
    {
        void loadBT();
        IBehaviorTree getBTByID(string id);
    }
}
