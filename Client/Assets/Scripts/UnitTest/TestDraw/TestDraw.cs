using SDK.Lib;

namespace UnitTest
{
    /**
     * @brief 测试自己绘制顶点和模型
     */
    public class TestDraw
    {
        public void run()
        {
            testArea();
        }

        protected void testArea()
        {
            Area area = new Area();
            TextureRes m_selfTex = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>("Image/Scene/jieshu_zhanchang.tga");
            area.setTexture(m_selfTex.getTexture());
            area.render();
        }
    }
}