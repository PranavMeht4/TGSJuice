using System.Collections;
using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Tween/Rotation Effect")]
    [JuiceDescription("Rotate this gameobject smoothly")]
    public class TGSRotateJuice : TGSJuiceBase
    {
        public Transform TransformToRotate;
        public float RotationDuration = 1f;
        public Vector3 RotationAngles = Vector3.zero;
        public AnimationCurve RotationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public int LoopCount = 1;

        public override void Play()
        {
            StartCoroutine(RotateCoroutine());
        }

        private IEnumerator RotateCoroutine()
        {
            int currentLoop = 0;
            while (currentLoop < LoopCount || LoopCount == -1)
            {
                Quaternion initialRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(RotationAngles) * initialRotation;
                float elapsed = 0f;

                while (elapsed < RotationDuration)
                {
                    float t = elapsed / RotationDuration;
                    t = RotationCurve.Evaluate(t);
                    TransformToRotate.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                transform.rotation = targetRotation;

                currentLoop++;
            }
        }
    }
}