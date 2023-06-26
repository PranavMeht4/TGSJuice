using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [System.Serializable]
    [AddComponentMenu("")]
    [JuiceLabel("Time/Slow Motion")]
    [JuiceDescription("Trigger slow motion effect /n" + "set timeSpeed to 0 to enable freeze frame effect")]
    public class TGSSlowMoJuice : TGSJuiceBase
    {
        public float TimeSpeed = 0.2f;
        public float FreezeDuration = 0.2f;

        public override void Play()
        {
            StartCoroutine(FreezeFrame());
        }

        private IEnumerator FreezeFrame()
        {
            Time.timeScale = TimeSpeed;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSecondsRealtime(FreezeDuration);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }
}