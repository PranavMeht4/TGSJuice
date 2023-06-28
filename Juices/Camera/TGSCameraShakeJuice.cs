using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Shake")]
    [JuiceDescription("Apply a customizable shake effect to the camera.")]
    public class TGSCameraShakeJuice : TGSJuiceBase
    {
        public float Magnitude = 0.1f;
        public float Duration = 0.1f;
        public float Speed = 1.0f;
        public bool ShakeVertical = true;
        public bool ShakeHorizontal = true;

        private Camera _mainCamera;
        private Vector3 _originalCameraPosition;
        private float _shakeTimer;
        private float _randomStart;

        private void Start()
        {
            _randomStart = Random.Range(0, 100);
        }

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
                    float x = ShakeHorizontal ? Magnitude * (Mathf.PerlinNoise(_randomStart, Time.time * Speed) * 2 - 1) : 0;
                    float y = ShakeVertical ? Magnitude * (Mathf.PerlinNoise(Time.time * Speed, _randomStart) * 2 - 1) : 0;

                    Vector3 shakeVector = new Vector3(x, y, 0);
                    _mainCamera.transform.position = _originalCameraPosition + shakeVector;
                }
            }
        }
    }
}