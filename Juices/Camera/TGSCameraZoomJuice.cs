using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Camera/Camera Zoom")]
    [JuiceDescription("Lerp camera's FOV to targetFOV")]
    public class TGSCameraZoomJuice : TGSJuiceBase
    {
        public float zoomDuration = 0.5f;
        public float zoomAmount = 0.1f;
        public int repeatCount = 0;
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
            int remainingRepeats = repeatCount;

            while (remainingRepeats != 0)
            {
                // Zoom in
                yield return ChangeFieldOfView(mainCamera, targetFieldOfView, zoomDuration);

                // Delay between zooms
                yield return new WaitForSeconds(delayBetweenZooms);

                // Zoom out
                yield return ChangeFieldOfView(mainCamera, originalFieldOfView, zoomDuration);

                if (remainingRepeats > 0)
                    remainingRepeats--;

                if (remainingRepeats == 0)
                    break;

                yield return null;
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