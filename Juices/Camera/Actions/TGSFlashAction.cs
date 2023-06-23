using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TGSJuice
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class TGSFlashAction : MonoBehaviour
    {
        public static UnityAction<float, float, Color> CameraFlashInvoked;

        private CanvasGroup _flashOverlay;
        private Image _flashImage;

        private float _flashDuration;
        private float _maxOpacity;

        private void Awake()
        {
            _flashOverlay = GetComponent<CanvasGroup>();
            _flashImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            CameraFlashInvoked += Flash;
        }

        private void OnDisable()
        {
            CameraFlashInvoked -= Flash;
        }

        public void Flash(float flashDuration, float maxOpacity, Color flashColor)
        {
            _flashDuration = flashDuration;
            _maxOpacity = maxOpacity;
            _flashImage.color = flashColor;

            StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            var halfDuration = _flashDuration / 2f;

            // Fade in
            for (var t = 0f; t < halfDuration; t += Time.deltaTime)
            {
                _flashOverlay.alpha = Mathf.Lerp(0f, _maxOpacity, t / halfDuration);
                yield return null;
            }

            // Fade out
            for (var t = 0f; t < halfDuration; t += Time.deltaTime)
            {
                _flashOverlay.alpha = Mathf.Lerp(_maxOpacity, 0f, t / halfDuration);
                yield return null;
            }

            _flashOverlay.alpha = 0f;
        }
    }
}