using UnityEngine;
using System.Collections;

namespace TGSJuice
{
    public class TGSWobbleAction : TGSActionBase<TGSWobbleParams>
    {
        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        protected override void PerformAction(TGSWobbleParams actionParams)
        {
            if (actionParams.ChannelId == ChannelId)
            {
                StartCoroutine(WobbleRoutine(actionParams.WobbleAmount, actionParams.WobbleSpeed, actionParams.WobbleFrequency, actionParams.WobbleDirection));
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

    public class TGSWobbleParams
    {
        public int ChannelId;
        public float WobbleAmount;
        public float WobbleSpeed;
        public float WobbleFrequency;
        public Vector3 WobbleDirection;
    }
}