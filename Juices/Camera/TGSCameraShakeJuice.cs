using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Shake")]
    [JuiceDescription("Apply shake effect to the camera. Ensure that the camera has a <color=#ADD8E6>TGSCameraShakeAction</color> component attached.")]
    public class TGSCameraShakeJuice : TGSJuiceBase
    {
        public float Magnitude = 0.1f;
        public float Duration = 0.1f;
        public float Speed = 1.0f;
        public bool ShakeVertical = true;
        public bool ShakeHorizontal = true;

        public TGSCameraShakeAction TGSCameraShakeAction;

        public override void Play()
        {
            TGSCameraShakeAction.InvokeAction(new TGSCameraShakeActionParam
            {
                Magnitude = this.Magnitude,
                Duration = this.Duration,
                Speed = this.Speed,
                ShakeVertical = this.ShakeVertical,
                ShakeHorizontal = this.ShakeHorizontal
            });
        }
    }
}