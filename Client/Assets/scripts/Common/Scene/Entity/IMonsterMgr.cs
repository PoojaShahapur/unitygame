namespace SDK.Common
{
    public interface IMonsterMgr : IBeingMgr
    {
        IMonster createMonster();
    }
}