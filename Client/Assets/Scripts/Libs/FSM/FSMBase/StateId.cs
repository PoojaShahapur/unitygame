namespace FSM
{
    /**
     * Extend this class to define different states for your own state machine.
     * */
    public class StateId
    {
        private readonly int mId;

        public StateId(int id)
        {
            mId = id;
        }

        public int GetId()
        {
            return mId;
        }
    }
}