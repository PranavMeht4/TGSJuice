using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Shake")]
    [JuiceDescription("Shake camera")]
    public class TGSCameraShakeJuice : TGSJuiceBase
    {
        public float Magnitude = .1f;
        public float Duration = .1f;

        private Camera _mainCamera;
        private Vector3 _originalCameraPosition;
        private float _shakeTimer;

        public override void Play()
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            if (_mainCamera != null)
            {
                _originalCameraPosition = _mainCamera.transform.position;
                _shakeTimer = Duration;
            }
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;

                if (_shakeTimer <= 0)
                {
                    _mainCamera.transform.position = _originalCameraPosition;
                }
                else
                {
                    var shakeOffset = Random.insideUnitCircle * Magnitude;
                    _mainCamera.transform.position = _originalCameraPosition + new Vector3(shakeOffset.x, shakeOffset.y, 0);
                }
            }
        }
    }
}