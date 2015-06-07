using Game.UI;
using SDK.Common;
using SDK.Lib;

namespace FSM
{
    public class SceneStateId : StateId
    {
        public static readonly SceneStateId SSInplace = new SceneStateId((int)SceneState.eInplace);

        public static readonly SceneStateId SSInplace2DestStart = new SceneStateId((int)SceneState.eInplace2DestStart);
        public static readonly SceneStateId SSInplace2Desting = new SceneStateId((int)SceneState.eInplace2Desting);
        public static readonly SceneStateId SSInplace2Dested = new SceneStateId((int)SceneState.eInplace2Dested);

        public static readonly SceneStateId SSAttackStart = new SceneStateId((int)SceneState.eAttackStart);
        public static readonly SceneStateId SSAttacking = new SceneStateId((int)SceneState.eAttacking);
        public static readonly SceneStateId SSAttacked = new SceneStateId((int)SceneState.eAttacked);

        public static readonly SceneStateId SSDest2InplaceStart = new SceneStateId((int)SceneState.eDest2InplaceStart);
        public static readonly SceneStateId SSDest2Inplaceing = new SceneStateId((int)SceneState.eDest2Inplaceing);
        public static readonly SceneStateId SSDest2Inplaced = new SceneStateId((int)SceneState.eDest2Inplaced);

        public static readonly SceneStateId SSHurtStart = new SceneStateId((int)SceneState.eHurtStart);
        public static readonly SceneStateId SSHurting = new SceneStateId((int)SceneState.eHurting);
        public static readonly SceneStateId SSHurted = new SceneStateId((int)SceneState.eHurted);

        public SceneStateId(int id)
            : base(id)
        {
        }
    }
}