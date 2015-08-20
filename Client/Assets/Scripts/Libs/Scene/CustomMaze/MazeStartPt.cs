namespace SDK.Lib
{
    public class MazeStartPt : MazePtBase
    {
        public MazeStartPt()
            : base(eMazePtType.eStart)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }
    }
}