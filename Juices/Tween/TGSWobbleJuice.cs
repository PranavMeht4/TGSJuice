using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Object Wobble")]
    [JuiceDescription("Applies a wobble effect to an object.")]
    public class TGSWobbleJuice : TGSJuiceBase
    {
        public int ChannelId;
        public float WobbleAmount = 0.5f;
        public float WobbleSpeed = 1f;
        public float WobbleFrequency = 1f;
        public Vector3 WobbleDirection = Vector3.one;

        public override void Play()
        {
            TGSWobbleAction.InvokeWobble(ChannelId, WobbleAmount, WobbleSpeed, WobbleFrequency, WobbleDirection);
        }
    }
}