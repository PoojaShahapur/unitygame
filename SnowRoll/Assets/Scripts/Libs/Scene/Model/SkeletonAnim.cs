namespace SDK.Lib
{
    /**
     * @brief 骨骼动画，假设骨骼是不会换的，如果换骨骼就会有问题
     */
    public class SkeletonAnim : AuxComponent
    {
        protected string m_skelAnimPath;
        protected SkelAnimRes m_skelAnim;
        protected bool m_bNeedReloadSkel;
        protected EventDispatch m_skelLoadDisp;

        public SkeletonAnim()
        {
            this.bNeedPlaceHolderGo = true;
            m_bNeedReloadSkel = false;
            m_skelLoadDisp = new EventDispatch();
        }

        public string skelAnimPath
        {
            get
            {
                return m_skelAnimPath;
            }
            set
            {
                if (m_skelAnimPath != value)
                {
                    m_bNeedReloadSkel = true;
                }
                m_skelAnimPath = value;
            }
        }

        public EventDispatch skelLoadDisp
        {
            get
            {
                return m_skelLoadDisp;
            }
            set
            {
                m_skelLoadDisp = value;
            }
        }

        override public void dispose()
        {
            if (m_skelAnim != null)
            {
                Ctx.mInstance.mSkelAniMgr.unload(m_skelAnim.getResUniqueId(), onLoaded);
                m_skelAnim = null;
            }

            m_skelLoadDisp.clearEventHandle();
        }

        public void loadSkelAnim()
        {
            if (m_bNeedReloadSkel)
            {
                if (m_skelAnim != null)
                {
                    Ctx.mInstance.mSkelAniMgr.unload(m_skelAnim.getResUniqueId(), onLoaded);
                    m_skelAnim = null;
                }

                m_skelAnim = Ctx.mInstance.mSkelAniMgr.getAndAsyncLoad<SkelAnimRes>(m_skelAnimPath, onLoaded) as SkelAnimRes;
                m_bNeedReloadSkel = false;
            }
        }

        public void onLoaded(IDispatchObject dispObj)
        {
            SkelAnimRes res = dispObj as SkelAnimRes;
            Ctx.mInstance.mLogSys.logLoad(res);

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                this.selfGo = res.InstantiateObject(res.getPrefabName());
                m_skelLoadDisp.dispatchEvent(this);
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mSkelAniMgr.unload(res.getResUniqueId(), onLoaded);
                m_skelAnim = null;
            }
        }
    }
}