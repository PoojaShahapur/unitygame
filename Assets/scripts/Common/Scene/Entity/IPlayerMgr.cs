namespace SDK.Common
{
    public interface IPlayerMgr
    {
        IPlayerMain createHero();
        void add(IBeingEntity being);
    }
}