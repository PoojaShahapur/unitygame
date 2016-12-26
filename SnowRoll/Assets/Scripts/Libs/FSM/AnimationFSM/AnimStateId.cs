namespace SDK.Lib
{
    public class AnimStateId : StateId
    {
        public static readonly AnimStateId ASIDLE = new AnimStateId(CVAnimState.Idle);
        public static readonly AnimStateId ASIWALK = new AnimStateId(CVAnimState.Walk);
        public static readonly AnimStateId ASRUN = new AnimStateId(CVAnimState.Run);
        public static readonly AnimStateId ASATTACK = new AnimStateId(CVAnimState.Attack);
        public static readonly AnimStateId ASSPLIT = new AnimStateId(CVAnimState.Split);

        public AnimStateId(int id)
            : base(id)
        {
        }

        static public StateId getStateIdByBeingState(BeingState beingState)
        {
            switch (beingState)
            {
                case BeingState.eBSIdle:
                {
                    return ASIDLE;
                }
                case BeingState.eBSWalk:
                {
                    return ASIWALK;
                }
                case BeingState.eBSRun:
                {
                    return ASRUN;
                }
                case BeingState.eBSAttack:
                {
                    return ASATTACK;
                }
                case BeingState.eBSSeparation:
                {
                    return ASIWALK;
                }
                case BeingState.eBSBirth:
                {
                    return ASIWALK;
                }
                case BeingState.eBSIOControlWalk:
                {
                    return ASIWALK;
                }
                case BeingState.eBSSplit:
                {
                    return ASSPLIT;
                }
            }

            return ASIDLE;
        }
    }
}