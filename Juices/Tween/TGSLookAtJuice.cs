using UnityEngine;
using DG.Tweening;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Look At", "Transform Icon")]
    [JuiceDescription("Rotate a transform to look towards a target")]
    public class TGSLookAtJuice : TGSJuiceBase
    {
        public Transform TransformToTween;
        public Transform Target;
        public float TweenDuration = 1f;
        public Ease RotationEase = Ease.Linear;
        public bool AutoKillOnComplete = true;
        public System.Action OnComplete;

        private Tween lookAtTween;

        public override void Play()
        {
            if (lookAtTween == null)
            {
                lookAtTween = TransformToTween.DOLookAt(Target.position, TweenDuration)
                                             .SetEase(RotationEase)
                                             .SetAutoKill(AutoKillOnComplete)
                                             .OnComplete(() => OnComplete?.Invoke())
                                             .Pause();
            }

            lookAtTween.Restart();
        }
    }
}