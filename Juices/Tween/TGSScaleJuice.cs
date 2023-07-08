using UnityEngine;
using DG.Tweening;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Scale", "Transform Icon")]
    [JuiceDescription("Scale gameobject smoothly")]
    public class TGSScaleJuice : TGSJuiceBase
    {
        public Transform TransformToScale;
        public Vector3 ScaleVector = Vector3.one;
        public float ScaleDuration = .2f;
        public Ease ScaleEase = Ease.Linear;
        public bool Loop = false;
        [HideIfFalse("Loop")]
        public int LoopCount = 0;
        [HideIfFalse("Loop")]
        public LoopType LoopType = LoopType.Restart;
        public bool AutoKillOnComplete = true;
        public System.Action OnComplete;

        private Tween scaleTween;

        public override void Play()
        {
            if (scaleTween == null)
            {
                scaleTween = TransformToScale.DOScale(ScaleVector, ScaleDuration)
                                        .SetEase(ScaleEase)
                                        .SetAutoKill(AutoKillOnComplete)
                                        .OnComplete(() => OnComplete?.Invoke())
                                        .Pause();

                if (Loop) scaleTween.SetLoops(LoopCount, LoopType);
            }

            scaleTween.Restart();
        }
    }
}