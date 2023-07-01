using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Play Particle", "ParticleSystem Icon")]
    [JuiceDescription("Play assigned particle effect")]
    public class TGSPlayParticleJuice : TGSJuiceBase
    {
        public ParticleSystem ParticleEffect;
        public bool Loop = false;

        private float _effectTimer;

        public override void Play()
        {
            if (ParticleEffect == null) return;
            ParticleSystem.MainModule mainModule = ParticleEffect.main;
            mainModule.loop = Loop;

            ParticleEffect.Play();
        }
    }
}