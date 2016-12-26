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
            switch (state.GetId())
            {
                case CVAnimState.Idle:
                {
                    return new AnimIdleFS(this, mEntity);
                }
                case CVAnimState.Walk:
                {
                    return new AnimWalkFS(this, mEntity);
                }
                case CVAnimState.Run:
                {
                    return new AnimRunFS(this, mEntity);
                }
                case CVAnimState.Attack:
                {
                    return new AnimAttackFS(this, mEntity);
                }
                case CVAnimState.Split:
                {
                    return new AnimSplitFS(this, mEntity);
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