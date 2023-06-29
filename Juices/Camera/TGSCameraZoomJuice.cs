using System;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Zoom")]
    [JuiceDescription("Apply zoom effect to the camera. Ensure that the camera has a <color=#ADD8E6>TGSCameraZoomAction</color> component attached.")]
    public class TGSCameraZoomJuice : TGSJuiceBase
    {
        public float ZoomDuration = 0.5f;
        public float ZoomAmount = 0.1f;
        public int LoopCount = 0;
        public float DelayBetweenZooms = 0.5f;
        public override Type ActionType { get { return typeof(TGSCameraZoomAction); } }

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