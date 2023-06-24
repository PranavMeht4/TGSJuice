using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Screen Flash")]
    [JuiceDescription("Applies a flash effect to the camera.\n" + "- Add TGSFlashAction component to the empty UI. It will add all the necessary components.\n")]
    public class TGSCameraFlashJuice : TGSJuiceBase
    {
        public float FlashDuration = 0.2f;
        public float MaxOpacity = 1f;
        public Color FlashColor = Color.white;

        public override void Play()
        {
            TGSFlashAction.CameraFlashInvoked?.Invoke(FlashDuration, MaxOpacity, FlashColor);
        }
    }
}