namespace SDK.Lib
{
    public class AnimFSM : FSM
    {
        protected BeingEntity mEntity;

        public BeingEntity getBeingEntity()
        {
            return mEntity;
        }

        public void setBeingEntity(BeingEntity entity)
        {
            mEntity = entity;
        }

        public AnimFSM(BeingEntity entity)
        {
            this.mEntity = entity;
        }

        public override void InitFSM()
        {
            base.InitFSM();
        }

        public override void UpdateFSM()
        {
            base.UpdateFSM();
        }

        protected override FSMState CreateState(StateId state)
        {
            FSMState retState = null;

            switch (state.GetId())
            {
                case CVAnimState.Idle:
                {
                    if(mNoUsedId2State.ContainsKey(state.GetId()))
                    {
                        retState = mNoUsedId2State[state.GetId()];
                    }
                    else
                    {
                        retState = new AnimIdleFS(this, mEntity);
                    }

                    return retState;
                }
                case CVAnimState.Walk:
                {
                    if (mNoUsedId2State.ContainsKey(state.GetId()))
                    {
                        retState = mNoUsedId2State[state.GetId()];
                    }
                    else
                    {
                        retState = new AnimWalkFS(this, mEntity);
                    }

                    return retState;
                }
                case CVAnimState.Run:
                {
                    if (mNoUsedId2State.ContainsKey(state.GetId()))
                    {
                        retState = mNoUsedId2State[state.GetId()];
                    }
                    else
                    {
                        retState = new AnimRunFS(this, mEntity);
                    }

                    return retState;
                }
                case CVAnimState.Attack:
                {
                    if (mNoUsedId2State.ContainsKey(state.GetId()))
                    {
                        retState = mNoUsedId2State[state.GetId()];
                    }
                    else
                    {
                        retState = new AnimAttackFS(this, mEntity);
                    }

                    return retState;
                }
                case CVAnimState.Split:
                {
                    if (mNoUsedId2State.ContainsKey(state.GetId()))
                    {
                        retState = mNoUsedId2State[state.GetId()];
                    }
                    else
                    {
                        retState = new AnimSplitFS(this, mEntity);
                    }

                    return retState;
                }
                default:
                {
                    return null;
                }
            }
        }

        public override void StopFSM()
        {
            base.StopFSM();
        }
    }
}