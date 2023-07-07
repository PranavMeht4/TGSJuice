using UnityEngine;

namespace TGSJuice.Demo
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private TGSJuices _juiceOnJump;
        [SerializeField] private TGSJuices _juiceOnGroundHit;
        [SerializeField] private float _jumpForce = 5;
        [SerializeField] private float _extraGravity = 2.0f;

        private Rigidbody _rb;
        private bool isGrounded = false;
        private bool shouldJump = false;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                shouldJump = true;
                isGrounded = false;
                _juiceOnJump.PlayAll();
            }
        }

        private void FixedUpdate()
        {
            if (shouldJump)
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                shouldJump = false;
            }

            if (!isGrounded && _rb.velocity.y < 0)
            {
                _rb.AddForce(Vector3.down * _extraGravity, ForceMode.Acceleration);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            isGrounded = true;
            _juiceOnGroundHit.PlayAll();
        }
    }
}