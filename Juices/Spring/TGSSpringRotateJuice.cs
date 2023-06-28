using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Spring/Spring rotate")]
    [JuiceDescription("Apply a spring damping rotate effect to any GameObject.")]
    public class TGSSpringRotateJuice : TGSJuiceBase
    {
        public enum RotationMode
        {
            SetTarget,
            AddToCurrent
        }

        public Vector3 Rotation;
        public RotationMode Mode = RotationMode.SetTarget;
        public float Duration = 1f;
        public AnimationCurve SpringCurve;

        private Quaternion _startRotation;
        private Quaternion _targetRotation;
        private float _timeElapsed = 0f;
        private bool _isPlaying = false;

        private void Start()
        {
            UpdateTargetRotation();
        }

        public override void Play()
        {
            UpdateTargetRotation();
            _timeElapsed = 0f;
            _isPlaying = true;
        }

        private void Update()
        {
            if (!_isPlaying)
            {
                return;
            }

            _timeElapsed += Time.deltaTime;

            float t = _timeElapsed / Duration;
            float springT = SpringCurve.Evaluate(t);

            transform.rotation = Quaternion.SlerpUnclamped(_startRotation, _targetRotation, springT);

            if (t >= 1f)
            {
                _isPlaying = false;
            }
        }

        private void UpdateTargetRotation()
        {
            _startRotation = transform.rotation;
            switch (Mode)
            {
                case RotationMode.SetTarget:
                    _targetRotation = Quaternion.Euler(Rotation);
                    break;
                case RotationMode.AddToCurrent:
                    _targetRotation = Quaternion.Euler(transform.eulerAngles + Rotation);
                    break;
            }
        }
    }
}