using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [RequireComponent(typeof(Camera))]
    public class TGSCameraShakeAction : TGSActionBase<TGSCameraShakeActionParam>
    {
        private Camera _mainCamera;
        private Vector3 _originalCameraPosition;
        private float _shakeTimer;
        private float _randomStart;

        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
            _originalCameraPosition = _mainCamera.transform.position;
            _randomStart = Random.Range(0, 100);
        }

        private IEnumerator ShakeCoroutine(float duration, float magnitude, float speed, bool shakeVertical, bool shakeHorizontal)
        {
            _shakeTimer = duration;

            while (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;

                if (_shakeTimer <= 0)
                {
                    _mainCamera.transform.position = _originalCameraPosition;
                }
                else
                {
                    float x = shakeHorizontal ? magnitude * (Mathf.PerlinNoise(_randomStart, Time.time * speed) * 2 - 1) : 0;
                    float y = shakeVertical ? magnitude * (Mathf.PerlinNoise(Time.time * speed, _randomStart) * 2 - 1) : 0;

                    Vector3 shakeVector = new Vector3(x, y, 0);
                    _mainCamera.transform.position = _originalCameraPosition + shakeVector;
                }

                yield return null;
            }
        }

        protected override void PerformAction(TGSCameraShakeActionParam actionParams)
        {
            StartCoroutine(ShakeCoroutine(actionParams.Duration, actionParams.Magnitude, actionParams.Speed, actionParams.ShakeVertical, actionParams.ShakeHorizontal));
        }
    }

    public class TGSCameraShakeActionParam
    {
        public float Magnitude;
        public float Duration;
        public float Speed;
        public bool ShakeVertical;
        public bool ShakeHorizontal;
    }
}