using UnityEngine;
using DG.Tweening;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Move", "Transform Icon")]
    [JuiceDescription("Move gameobject smoothly")]
    public class TGSMoveJuice : TGSJuiceBase
    {
        public Transform TransformToMove;
        public bool IsLocal = false;
        public Vector3 MovePositions = Vector3.zero;
        public float MoveDuration = .2f;
        public Ease MoveEase = Ease.Linear;
        public bool Loop = false;
        [HideIfFalse("Loop")]
        public int LoopCount = 0;
        [HideIfFalse("Loop")]
        public LoopType LoopType = LoopType.Restart;
        public bool AutoKillOnComplete = true;
        public System.Action OnComplete;

        private Tween moveTween;

        public override void Play()
        {
            if (moveTween == null)
            {
                Vector3 targetPosition = MovePositions;
                if (!IsLocal)
                    targetPosition += TransformToMove.position;

                moveTween = (IsLocal ? TransformToMove.DOLocalMove(targetPosition, MoveDuration)
                                           : TransformToMove.DOMove(targetPosition, MoveDuration))
                                            .SetEase(MoveEase)
                                            .SetAutoKill(AutoKillOnComplete)
                                            .OnComplete(() => OnComplete?.Invoke())
                                            .Pause();

                if (Loop) moveTween.SetLoops(LoopCount, LoopType);
            }


            moveTween.Restart();
        }
    }
}