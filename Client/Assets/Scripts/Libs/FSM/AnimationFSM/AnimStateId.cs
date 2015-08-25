using SDK.Lib;

namespace FSM
{
    public class AnimStateId : StateId
    {
        public static readonly AnimStateId ASIDLE = new AnimStateId(CVAnimState.Idle);
        public static readonly AnimStateId ASIWALK = new AnimStateId(CVAnimState.Walk);
        public static readonly AnimStateId ASRUN = new AnimStateId(CVAnimState.Run);

        public AnimStateId(int id)
            : base(id)
        {
        }

        static public StateId getStateIdByBeingState(BeingState beingState)
        {
            switch (beingState)
            {
                case BeingState.BSIdle:
                    {
                        return ASIDLE;
                    }
                case BeingState.BSWalk:
                    {
                        return ASIWALK;
                    }
                case BeingState.BSRun:
                    {
                        return ASRUN;
                    }
            }

            return ASIDLE;
        }
    }
}