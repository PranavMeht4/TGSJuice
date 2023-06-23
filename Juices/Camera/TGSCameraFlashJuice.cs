using UnityEngine;

namespace TGSJuice
{
    [System.Serializable]
    [AddComponentMenu("")]
    [JuiceLabel("Camera Flash")]
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