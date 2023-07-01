using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Move", "Transform Icon")]
    [JuiceDescription("Move one or list of gameobjects smoothly")]
    public class TGSMoveJuice : TGSJuiceBase
    {
        public Transform TransformToMove;
        public float MoveDuration = 1f;
        public Vector3 MovePositions = Vector3.zero;
        public AnimationCurve RotationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public int LoopCount = 1;

        public override void Play()
        {
            StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine()
        {
            int currentLoop = 0;
            while (currentLoop < LoopCount || LoopCount == -1)
            {
                Vector3 initalPosition = TransformToMove.position;
                Vector3 targetPosition = MovePositions + initalPosition;
                float elapsed = 0f;

                while (elapsed < MoveDuration)
                {
                    float t = elapsed / MoveDuration;
                    t = RotationCurve.Evaluate(t);
                    TransformToMove.position = Vector3.Lerp(initalPosition, targetPosition, t);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                currentLoop++;
            }
        }
    }
}