using System;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Object Wobble")]
    [JuiceDescription("Applies a wobble effect to an object. Ensure that the target has a <color=#ADD8E6>TGSWobbleAction</color> component attached.")]
    public class TGSWobbleJuice : TGSJuiceBase
    {
        public int ChannelId;
        public float WobbleAmount = 0.5f;
        public float WobbleSpeed = 1f;
        public float WobbleFrequency = 1f;
        public Vector3 WobbleDirection = Vector3.one;
        public override Type ActionType { get { return typeof(TGSWobbleAction); } }

        public override void Play()
        {
            TGSWobbleAction.InvokeAction(new TGSWobbleParams
            {
                ChannelId = ChannelId,
                WobbleAmount = WobbleAmount,
                WobbleSpeed = WobbleSpeed,
                WobbleFrequency = WobbleFrequency,
                WobbleDirection = WobbleDirection
            });
        }
    }
}