using UnityEngine;
using System.Collections;

namespace TGSJuice
{
    public class TGSWobbleAction : MonoBehaviour
    {
        public delegate void WobbleDelegate(int channelId, float wobbleAmount, float wobbleSpeed, float wobbleFrequency, Vector3 wobbleDirection);
        public static event WobbleDelegate WobbleInvoked;

        public int ChannelId;

        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        private void OnEnable()
        {
            WobbleInvoked += Wobble;
        }

        private void OnDisable()
        {
            WobbleInvoked -= Wobble;
        }

        public static void InvokeWobble(int channelId, float wobbleAmount, float wobbleSpeed, float wobbleFrequency, Vector3 wobbleDirection)
        {
            WobbleInvoked?.Invoke(channelId, wobbleAmount, wobbleSpeed, wobbleFrequency, wobbleDirection);
        }

        public void Wobble(int channelId, float wobbleAmount, float wobbleSpeed, float wobbleFrequency, Vector3 wobbleDirection)
        {
            if (channelId == ChannelId)
            {
                StartCoroutine(WobbleRoutine(wobbleAmount, wobbleSpeed, wobbleFrequency, wobbleDirection));
            }
        }

        private IEnumerator WobbleRoutine(float wobbleAmount, float wobbleSpeed, float wobbleFrequency, Vector3 wobbleDirection)
        {
            float elapsed = 0f;
            while (elapsed < wobbleSpeed)
            {
                float wobbleFactor = wobbleAmount * Mathf.Sin(2 * Mathf.PI * elapsed / wobbleFrequency);
                transform.localScale = _initialScale + Vector3.Scale(wobbleDirection, new Vector3(wobbleFactor, wobbleFactor, wobbleFactor));

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localScale = _initialScale;
        }
    }
}