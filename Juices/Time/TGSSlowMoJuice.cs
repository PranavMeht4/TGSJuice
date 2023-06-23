using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [System.Serializable]
    [AddComponentMenu("")]
    [JuiceLabel("Slow Motion")]
    public class TGSSlowMoJuice : TGSJuiceBase
    {
        public float timeSpeed = 0.2f;
        public float freezeDuration = 0.2f;

        public override void Play()
        {
            StartCoroutine(FreezeFrame());
        }

        private IEnumerator FreezeFrame()
        {
            Time.timeScale = timeSpeed;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSecondsRealtime(freezeDuration);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }
}