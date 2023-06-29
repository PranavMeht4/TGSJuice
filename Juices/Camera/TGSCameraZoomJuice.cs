using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Zoom")]
    [JuiceDescription("Apply zoom effect to the camera. Ensure that the camera has a TGSCameraZoomAction component attached.")]
    public class TGSCameraZoomJuice : TGSJuiceBase
    {
        public float ZoomDuration = 0.5f;
        public float ZoomAmount = 0.1f;
        public int LoopCount = 0;
        public float DelayBetweenZooms = 0.5f;

        public override void Play()
        {
            TGSCameraZoomAction.InvokeAction(new TGSCameraZoomActionParam()
            {
                ZoomAmount = ZoomAmount,
                ZoomDuration = ZoomDuration,
                LoopCount = LoopCount,
                DelayBetweenZooms = DelayBetweenZooms
            });
        }
    }
}