namespace SDK.Common
{
    public interface IPlayerMgr : IBeingMgr
    {
        IPlayerMain createHero();
        IPlayerMain getHero();
        void addHero(IPlayerMain hero);
    }
}