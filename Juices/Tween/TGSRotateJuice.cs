using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Rotate", "Transform Icon")]
    [JuiceDescription("Rotate gameobject smoothly")]
    public class TGSRotateJuice : TGSJuiceBase
    {
        public Transform TransformToRotate;
        public bool IsLocal = false;
        public Vector3 RotateAngles = Vector3.zero;
        public float RotateDuration = .2f;
        public Ease RotateEase = Ease.Linear;
        public RotateMode RotateMode = RotateMode.FastBeyond360;
        public bool Loop = false;
        [HideIfFalse("Loop")]
        public int LoopCount = 0;
        [HideIfFalse("Loop")]
        public LoopType LoopType = LoopType.Restart;
        public bool AutoKillOnComplete = false;
        public UnityEvent OnComplete;

        private Tween rotateTween;

        public override void Play()
        {
            if (rotateTween == null)
            {
                rotateTween = (IsLocal ? TransformToRotate.DOLocalRotate(RotateAngles, RotateDuration, RotateMode)
                                       : TransformToRotate.DORotate(RotateAngles, RotateDuration, RotateMode))
                                        .SetEase(RotateEase)
                                        .SetAutoKill(AutoKillOnComplete)
                                        .OnComplete(() => OnComplete?.Invoke())
                                        .Pause();

                if (Loop) rotateTween.SetLoops(LoopCount, LoopType);
            }

            rotateTween.Restart();
        }
    }
}