namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统
     */
    public class ABMemUnPakLevelFileResItem : ABUnPakLevelFileResItem
    {
        override public void initByPakRes()
        {
            base.initByPakRes();
            initByBytes(m_resItem.getBytes(mLoadPath), SCENE_PRE_PATH);
        }
    }
}