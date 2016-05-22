using SDK.Lib;

namespace UnitTest
{
    public class TestIsometric
    {
        public void run()
        {
            test();
        }

        protected void test()
        {
            MMatrix3 matric = new MMatrix3(0);
            MPointF pt1 = UtilMath.convOrthoTile2IsoPt(new MPointF(0, 0));
            MPointF pt2 = UtilMath.convOrthoTile2IsoPt(new MPointF(2.5f, 0));
            MPointF pt3 = UtilMath.convOrthoTile2IsoPt(new MPointF(0, 2.5f));
            MPointF pt4 = UtilMath.convOrthoTile2IsoPt(new MPointF(2.5f, 2.5f));
            MPointF pt5 = UtilMath.convOrthoTile2IsoPt(new MPointF(2.5f / 2, 2.5f / 2));

            MPointF pt1_2 = UtilMath.convOrthoTile2IsoPt(new MPointF(0, 0));
            MPointF pt2_2 = UtilMath.convOrthoTile2IsoPt(new MPointF(UtilMath.MAP_WIDTH, 0));
            MPointF pt3_2 = UtilMath.convOrthoTile2IsoPt(new MPointF(0, UtilMath.MAP_HEIGHT));
            MPointF pt4_2 = UtilMath.convOrthoTile2IsoPt(new MPointF(UtilMath.MAP_WIDTH, UtilMath.MAP_HEIGHT));
            MPointF pt5_2 = UtilMath.convOrthoTile2IsoPt(new MPointF(UtilMath.MAP_WIDTH / 2, UtilMath.MAP_HEIGHT / 2));

            MPointF pt1_1 = UtilMath.convOrthoPt2IsoPt(new MPointF(0, 0));
            MPointF pt2_1 = UtilMath.convOrthoPt2IsoPt(new MPointF(2.5f, 0));
            MPointF pt3_1 = UtilMath.convOrthoPt2IsoPt(new MPointF(0, 2.5f));
            MPointF pt4_1 = UtilMath.convOrthoPt2IsoPt(new MPointF(2.5f, 2.5f));
            MPointF pt5_1 = UtilMath.convOrthoPt2IsoPt(new MPointF(2.5f / 2, 2.5f / 2));

            int bbb = 0;
        }
    }
}