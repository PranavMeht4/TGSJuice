using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [RequireComponent(typeof(Camera))]
    public class TGSCameraZoomAction : TGSActionBase<TGSCameraZoomActionParam>
    {
        private Camera _mainCamera;
        private float _originalFieldOfView;

        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
            _originalFieldOfView = _mainCamera.fieldOfView;
        }

        private IEnumerator ChangeFieldOfView(float targetFieldOfView, float duration)
        {
            float initialFieldOfView = _mainCamera.fieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                _mainCamera.fieldOfView = Mathf.Lerp(initialFieldOfView, targetFieldOfView, t);
                yield return null;
            }

            _mainCamera.fieldOfView = targetFieldOfView;
        }

        private IEnumerator ZoomCamera(float zoomAmount, int loopCount, float delayBetweenZooms, float zoomDuration)
        {
            float targetFieldOfView = _originalFieldOfView + zoomAmount;
            int remainingLoops = loopCount;

            while (remainingLoops != 0 || loopCount == -1)
            {
                yield return ChangeFieldOfView(targetFieldOfView, zoomDuration);

                if (loopCount != 0)
                {
                    yield return new WaitForSeconds(delayBetweenZooms);

                    yield return ChangeFieldOfView(_originalFieldOfView, zoomDuration);

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

        protected override void PerformAction(TGSCameraZoomActionParam actionParams)
        {
            StartCoroutine(ZoomCamera(actionParams.ZoomAmount, actionParams.LoopCount, actionParams.DelayBetweenZooms, actionParams.ZoomDuration));
        }
    }

    public class TGSCameraZoomActionParam
    {
        public float ZoomDuration;
        public float ZoomAmount;
        public int LoopCount;
        public float DelayBetweenZooms;
    }
}