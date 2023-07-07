using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TGSJuice
{
    [RequireComponent(typeof(Image))]
    public class TGSImageFillAction : TGSActionBase<TGSImageFillActionParam>
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        protected override void PerformAction(TGSImageFillActionParam actionParams)
        {
            StartCoroutine(PerformActionRoutine(actionParams));
        }

        protected IEnumerator PerformActionRoutine(TGSImageFillActionParam actionParams)
        {
            yield return new WaitForSeconds(actionParams.DelayBeforeStart);

            float initialValue = _image.fillAmount;
            float targetValue = 0;
            switch (actionParams.Mode)
            {
                case FillMode.Add:
                    targetValue = _image.fillAmount + actionParams.Value;
                    break;
                case FillMode.Replace:
                    targetValue = actionParams.Value;
                    break;
            }
            float time = 0;
            while (time < actionParams.Duration)
            {
                time += Time.deltaTime;
                float t = time / actionParams.Duration;
                switch (actionParams.Ease)
                {
                    case EaseType.Linear:
                        break;
                    case EaseType.EaseIn:
                        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                        break;
                    case EaseType.EaseOut:
                        t = Mathf.Sin(t * Mathf.PI * 0.5f);
                        break;
                }
                _image.fillAmount = Mathf.Lerp(initialValue, targetValue, t);
                yield return null;
            }
            _image.fillAmount = targetValue;
        }
    }

    public enum FillMode
    {
        Add,
        Replace
    }

    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
    }

    public class TGSImageFillActionParam
    {
        public float Value;
        public float Duration;
        public FillMode Mode;
        public EaseType Ease;
        public float DelayBeforeStart;
    }
}
