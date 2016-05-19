using SDK.Lib;
using UnityEngine;

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
            //testQuadMeshRender();
        }

        protected void testArea()
        {
            TestTile tile = new TestTile();
            TextureRes m_selfTex = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>("Image/Scene/jieshu_zhanchang.tga");
            tile.setTexture(m_selfTex.getTexture());
            tile.render();
        }

        protected void testQuadMeshRender()
        {
            TestQuadMeshRender quadMeshRender = new TestQuadMeshRender(8);
            //GameObject go = UtilApi.GoFindChildByPObjAndName("GOStart");
            //quadMeshRender.selfGo = go;

            quadMeshRender.addVertex(new Vector3(0, 1, 0));
            quadMeshRender.addVertex(new Vector3(1, 1, 0));
            quadMeshRender.addVertex(new Vector3(0, 0, 0));
            quadMeshRender.addVertex(new Vector3(1, 0, 0));

            quadMeshRender.addVertex(new Vector3(1, 1, 0));
            quadMeshRender.addVertex(new Vector3(1, 1, 1));
            quadMeshRender.addVertex(new Vector3(1, 0, 0));
            quadMeshRender.addVertex(new Vector3(1, 0, 1));

            quadMeshRender.buildIndexA();
            quadMeshRender.uploadGeometry();
        }
    }
}