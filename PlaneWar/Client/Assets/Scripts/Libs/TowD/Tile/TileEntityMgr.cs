namespace SDK.Lib
{
    /**
     * @brief Tile 中 Entity 管理器
     */
    public class TileEntityMgr : EntityMgrBase
    {
        public TileEntityMgr()
        {

        }

        override public void dispose()
        {
            this.mSceneEntityList.Clear();
            this.mId2EntityDic.Clear();
            this.mThisId2EntityDic.Clear();
        }

        override public void removeEntity(SceneEntityBase entity, bool isDispose = true)
        {
            this.removeObject(entity);

            if (this.mId2EntityDic.ContainsKey(entity.getEntityUniqueId()))
            {
                this.mId2EntityDic.Remove(entity.getEntityUniqueId());
            }

            if (this.mThisId2EntityDic.ContainsKey(entity.getThisId()))
            {
                this.mThisId2EntityDic.Remove(entity.getThisId());
            }
        }
    }
}