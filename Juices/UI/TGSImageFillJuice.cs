using System;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("UI/Image Fill", "Image Icon")]
    [JuiceDescription("Update image fill amount. Ensure that the image has a <color=#ADD8E6>TGSImageFillAction</color> component attached.")]
    public class TGSImageFillJuice : TGSJuiceBase
    {
        [SerializeField] private float _fillAmount = 1f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private FillMode _fillMode = FillMode.Replace;
        [SerializeField] private EaseType _easeType = EaseType.Linear;
        [SerializeField] private float _delayBeforeStart = 0f;

        public override Type ActionType { get { return typeof(TGSImageFillAction); } }

        public override void Play()
        {
            TGSImageFillAction.InvokeAction(new TGSImageFillActionParam()
            {
                Value = _fillAmount,
                Duration = _duration,
                Mode = _fillMode,
                Ease = _easeType,
                DelayBeforeStart = _delayBeforeStart,
            });
        }
    }
}