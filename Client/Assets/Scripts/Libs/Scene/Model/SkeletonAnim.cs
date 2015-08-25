using SDK.Lib;

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

        public void dispose()
        {
            if (m_skelAnim != null)
            {
                Ctx.m_instance.m_skelAniMgr.unload(m_skelAnim.GetPath(), onLoaded);
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
                    Ctx.m_instance.m_skelAniMgr.unload(m_skelAnim.GetPath(), onLoaded);
                    m_skelAnim = null;
                }

                m_skelAnim = Ctx.m_instance.m_skelAniMgr.getAndAsyncLoad<SkelAnimRes>(m_skelAnimPath, onLoaded) as SkelAnimRes;
                m_bNeedReloadSkel = false;
            }
        }

        public void onLoaded(IDispatchObject dispObj)
        {
            SkelAnimRes res = dispObj as SkelAnimRes;
            Ctx.m_instance.m_logSys.logLoad(res);

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                this.selfGo = res.InstantiateObject(res.GetPath());
                m_skelLoadDisp.dispatchEvent(this);
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_skelAniMgr.unload(res.GetPath(), onLoaded);
                m_skelAnim = null;
            }
        }
    }
}