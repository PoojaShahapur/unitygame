using UnityEngine;

namespace SDK.Lib
{
    public class ShurikenParticleSystem : AuxComponent
    {
        protected ParticleSystem m_particleSystem;

        public void play()
        {
            m_particleSystem.Play();
        }

        public void stop()
        {
            m_particleSystem.Stop();
        }
    }
}