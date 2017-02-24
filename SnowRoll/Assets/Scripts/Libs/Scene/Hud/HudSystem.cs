namespace SDK.Lib
{
    /**
     * @brief HUD 系统
     */
    public class HudSystem
    {
        protected MList<HudItemBase> mList;

        public HudSystem()
        {
            this.mList = new MList<HudItemBase>();
        }

        public void init()
        {
            Ctx.mInstance.mGlobalDelegate.mCameraOrientChangedDispatch.addEventHandle(null, this.onCameraOrientChanged);
        }

        public void dispose()
        {
            this.clear();

            this.mList.Clear();
            //this.mList = null;
        }

        public void clear()
        {
            int idx = 0;
            int len = this.mList.length();

            HudItemBase item = null;

            while (idx < len)
            {
                item = this.mList[idx];
                item.dispose();

                ++idx;
            }
        }

        public HudItemBase createHud(BeingEntity being)
        {
            HudItemBase hud = null;

            if (EntityType.ePlayerMainChild == being.getEntityType())
            {
                hud = new PlayerMainChildHud();
            }
            else if (EntityType.ePlayerOtherChild == being.getEntityType())
            {
                hud = new PlayerOtherChildHud();
            }
            else if (EntityType.eComputerBall == being.getEntityType())
            {
                hud = new ComputerBallHud();
            }

            hud.setBeing(being);
            hud.init();

            this.addHud(hud);

            return hud;
        }

        public void addHud(HudItemBase hud)
        {
            this.mList.Add(hud);
        }

        public void removeHud(HudItemBase hud)
        {
            this.mList.Remove(hud);
        }

        // 摄像机方向位置发生改变
        public void onCameraOrientChanged(IDispatchObject dispObj)
        {
            int idx = 0;
            int len = this.mList.Count();

            while(idx < len)
            {
                this.mList[idx].onPosChanged();
                ++idx;
            }
        }
    }
}