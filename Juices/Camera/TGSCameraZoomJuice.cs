using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [System.Serializable]
    [AddComponentMenu("")]
    [JuiceLabel("Camera Zoom")]
    public class TGSCameraZoomJuice : TGSJuiceBase
    {
        public float TargetFOV = 60f;
        public float Duration = 1f;

        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        public override void Play()
        {
            StartCoroutine(Zoom());
        }

        private IEnumerator Zoom()
        {
            float initialFOV = _cam.fieldOfView;
            float elapsed = 0f;

            while (elapsed < Duration)
            {
                float t = elapsed / Duration;

                _cam.fieldOfView = Mathf.Lerp(initialFOV, TargetFOV, t);

                elapsed += Time.deltaTime;

                yield return null;
            }

            _cam.fieldOfView = TargetFOV;
        }
    }
}
