using System;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("UI/Screen Flash", "Image Icon")]
    [JuiceDescription("Applies a flash effect to the screen. Ensure that the image you're applying the flash to has a <color=#ADD8E6>TGSFlashAction</color> component attached.")]
    public class TGSImageFlashJuice : TGSJuiceBase
    {
        public float FlashDuration = 0.2f;
        public float MaxOpacity = 1f;
        public Color FlashColor = Color.white;
        public override Type ActionType { get { return typeof(TGSUIFlashAction); } }

        public override void Play()
        {
            TGSUIFlashAction.InvokeAction(new TGSFlashActionParam()
            {
                FlashDuration = this.FlashDuration,
                MaxOpacity = this.MaxOpacity,
                FlashColor = this.FlashColor
            });
        }
    }
}