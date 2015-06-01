﻿using SDK.Common;
using SDK.Lib;

namespace FSM
{
    public class AnimStateId : StateId
    {
        public static readonly AnimStateId ASIDLE = new AnimStateId("ASIDLE");
        public static readonly AnimStateId ASIWALK = new AnimStateId("ASIWALK");
        public static readonly AnimStateId ASRUN = new AnimStateId("ASRUN");

        public AnimStateId(string id)
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