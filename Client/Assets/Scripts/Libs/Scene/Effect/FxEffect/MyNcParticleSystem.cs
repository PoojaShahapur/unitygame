namespace SDK.Lib
{
    public class MyNcParticleSystem : AuxComponent
    {
        protected NcParticleSystem m_ncParticleSystem;

        public void play()
        {
            m_ncParticleSystem.enabled = true;
        }

        public void stop()
        {
            m_ncParticleSystem.enabled = false;
        }
    }
}