namespace SDK.Lib
{
    /**
     * @brief 节点代理，主要用来存放于节点没有关系的操作数据
     */
    public class MTestNodeProxy
    {
        /**
         * @brief 显示一些东西
         */
        virtual public void show()
        {

        }

        /**
         * @brief 隐藏一些东西
         */
        virtual public void hide()
        {

        }

        /**
         * @brief 更新 Mesh 数据
         */
        virtual public void updateMesh(MTestTerrain terrain, int tileIndex)
        {

        }

        virtual public MAxisAlignedBox getAABox()
        {
            return MAxisAlignedBox.BOX_NULL;
        }
    }
}