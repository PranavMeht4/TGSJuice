using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Zoom")]
    [JuiceDescription("Lerps camera's FOV to targetFOV. loopCount determines the behavior. 0: just zoom to targetFOV, 1: zoom in and zoom out once, 2: zoom in, zoom out, and zoom in again, -1: infinite loop.")]
    public class TGSCameraZoomJuice : TGSJuiceBase
    {
        public float ZoomDuration = 0.5f;
        public float ZoomAmount = 0.1f;
        public int LoopCount = 0;
        public float DelayBetweenZooms = 0.5f;

        private Camera _mainCamera;
        private float _originalFieldOfView;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _originalFieldOfView = _mainCamera.fieldOfView;
        }

        public override void Play()
        {
            StartCoroutine(ZoomCamera());
        }

        private IEnumerator ZoomCamera()
        {
            float targetFieldOfView = _originalFieldOfView + ZoomAmount;
            int remainingLoops = LoopCount;

            while (remainingLoops != 0 || LoopCount == -1)
            {
                // Zoom in
                yield return ChangeFieldOfView(_mainCamera, targetFieldOfView, ZoomDuration);

                if (LoopCount != 0)
                {
                    // Delay between zooms
                    yield return new WaitForSeconds(DelayBetweenZooms);

                    // Zoom out
                    yield return ChangeFieldOfView(_mainCamera, _originalFieldOfView, ZoomDuration);

                    if (remainingLoops > 0)
                        remainingLoops--;

                    if (remainingLoops == 0 && LoopCount != -1)
                        break;

                    yield return null;
                }
                else
                {
                    break;
                }
            }
        }

        private IEnumerator ChangeFieldOfView(Camera camera, float targetFieldOfView, float duration)
        {
            float initialFieldOfView = camera.fieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                camera.fieldOfView = Mathf.Lerp(initialFieldOfView, targetFieldOfView, t);
                yield return null;
            }

            camera.fieldOfView = targetFieldOfView;
        }
    }
}