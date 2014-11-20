using SDK.Common;

namespace SDK.Lib
{
    public class MonsterMgr : BeingMgr, IMonsterMgr
    {
        public IMonster createMonster()
        {
            return new Monster();
        }
    }
}