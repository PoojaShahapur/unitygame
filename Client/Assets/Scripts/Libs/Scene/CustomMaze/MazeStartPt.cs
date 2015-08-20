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

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }
}