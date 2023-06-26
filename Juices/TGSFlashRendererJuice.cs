using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Flash Renderer")]
    [JuiceDescription("Flashes a renderer color over time.")]
    public class TGSFlashRendererJuice : TGSJuiceBase
    {
        public Renderer TargetRenderer;
        public Color FlashColor = Color.white;
        public float FlashDuration = 0.5f;
        public int RepeatCount = 0;
        public float DelayBetweenFlashes = 0.5f;

        private Color _originalColor;
        private Material _mat;

        private void Awake()
        {
            _mat = TargetRenderer.material;
            _originalColor = _mat.color;
        }

        public override void Play()
        {
            StartCoroutine(FlashMaterial());
        }

        private IEnumerator FlashMaterial()
        {
            int remainingFlashes = RepeatCount;

            while (remainingFlashes != 0 || RepeatCount == -1)
            {
                // Flash to color
                yield return ChangeMaterialColor(_mat, FlashColor, FlashDuration);

                if (RepeatCount != 0)
                {
                    // Delay between flashes
                    yield return new WaitForSeconds(DelayBetweenFlashes);

                    // Flash back to original color
                    yield return ChangeMaterialColor(_mat, _originalColor, FlashDuration);

                    if (remainingFlashes > 0)
                        remainingFlashes--;

                    if (remainingFlashes == 0 && RepeatCount != -1)
                        break;

                    yield return null;
                }
                else
                {
                    break;
                }
            }
        }

        private IEnumerator ChangeMaterialColor(Material material, Color targetColor, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                material.color = Color.Lerp(_originalColor, targetColor, t);
                yield return null;
            }

            _mat.color = _originalColor;
        }
    }
}