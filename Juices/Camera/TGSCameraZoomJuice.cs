using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Zoom")]
    [JuiceDescription("Lerps camera's FOV to targetFOV. loopCount determines the behavior. 0: just zoom to targetFOV, 1: zoom in and zoom out once, 2: zoom in, zoom out, and zoom in again, -1: infinite loop.")]
    public class TGSCameraZoomJuice : TGSJuiceBase
    {
        public float zoomDuration = 0.5f;
        public float zoomAmount = 0.1f;
        public int loopCount = 0;
        public float delayBetweenZooms = 0.5f;

        private Camera mainCamera;
        private float originalFieldOfView;

        private void Awake()
        {
            mainCamera = Camera.main;
            originalFieldOfView = mainCamera.fieldOfView;
        }

        public override void Play()
        {
            StartCoroutine(ZoomCamera());
        }

        private IEnumerator ZoomCamera()
        {
            float targetFieldOfView = originalFieldOfView + zoomAmount;
            int remainingLoops = loopCount;

            while (remainingLoops != 0 || loopCount == -1)
            {
                // Zoom in
                yield return ChangeFieldOfView(mainCamera, targetFieldOfView, zoomDuration);

                if (loopCount != 0)
                {
                    // Delay between zooms
                    yield return new WaitForSeconds(delayBetweenZooms);

                    // Zoom out
                    yield return ChangeFieldOfView(mainCamera, originalFieldOfView, zoomDuration);

                    if (remainingLoops > 0)
                        remainingLoops--;

                    if (remainingLoops == 0 && loopCount != -1)
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