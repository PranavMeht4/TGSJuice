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
        public string ColorPropertyName = "_Color";
        public Color FlashColor = Color.white;
        public float FlashDuration = 0.5f;
        public int RepeatCount = 0;
        public float DelayBetweenFlashes = 0.5f;

        private Color _originalColor;
        private Material _mat;

        private void Awake()
        {
            _mat = TargetRenderer.material;
            if (_mat.HasProperty(ColorPropertyName))
            {
                _originalColor = _mat.GetColor(ColorPropertyName);
            }
            else
            {
                Debug.LogError("The material doesn't have a color property with the name: " + ColorPropertyName);
            }
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

            // Reset color to original after all flashing done
            _mat.SetColor(ColorPropertyName, _originalColor);
        }

        private IEnumerator ChangeMaterialColor(Material material, Color targetColor, float duration)
        {
            Color initialColor = material.GetColor(ColorPropertyName);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                material.SetColor(ColorPropertyName, Color.Lerp(initialColor, targetColor, t));
                yield return null;
            }

            material.SetColor(ColorPropertyName, targetColor);
        }
    }
}