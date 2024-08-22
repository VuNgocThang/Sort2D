using UnityEngine;
/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

// by nt.Dev93
namespace ntDev
{
    [RequireComponent(typeof(CharacterController))]
    public class UnityController : MonoBehaviour
    {
        public bool IsRunning;
        public float MoveSpeed = 3.0f;
        public float RunSpeed = 6.0f;
        public float Acceleration = 10.0f;
        float TurnRate = 0.12f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        float JumpTimeout = 0.00f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        float FallTimeout = 0.15f;

        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = 0.1f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.2f;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField] LayerMask GroundLayers;

        // player
        float _speed;
        float _animationBlend;
        float _targetRotation = 0.0f;
        float _rotationVelocity;
        float _verticalVelocity;
        float _terminalVelocity = 53.0f;

        // timeout deltatime
        float _jumpTimeoutDelta;
        float _fallTimeoutDelta;

        // animation IDs
        int _animIDSpeed;
        int _animIDGrounded;
        int _animIDJump;
        int _animIDFreeFall;
        int _animIDMotionSpeed;

        Animator anim;
        bool HadAnim;

        CharacterController characterController;
        CharacterController CharacterController
        {
            get
            {
                if (characterController == null) characterController = GetComponent<CharacterController>();
                return characterController;
            }
        }

        void Start()
        {
            InitAnimation();
            ResetTimer();
        }

        void InitAnimation()
        {
            anim = GetComponentInChildren<Animator>();
            HadAnim = anim != null;

            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        void ResetTimer()
        {
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        void Update()
        {
            GroundedCheck();
            JumpAndGravity();
            Move();
        }

        void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            // update animator if using character   
            if (HadAnim) anim.SetBool(_animIDGrounded, Grounded);
        }

        [HideInInspector] public bool IsJumping;
        void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (HadAnim) anim.SetBool(_animIDJump, false);
                if (HadAnim) anim.SetBool(_animIDFreeFall, false);

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;

                // Jump
                if (IsJumping && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (HadAnim) anim.SetBool(_animIDJump, true);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Ez.TimeMod;
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= Ez.TimeMod;
                else if (HadAnim) anim.SetBool(_animIDFreeFall, true);

                // if we are not grounded, do not jump
                IsJumping = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity) _verticalVelocity += Gravity * Ez.TimeMod;
        }

        [HideInInspector] public Vector2 vMove;
        Vector3 targetDirection;
        void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = IsRunning ? RunSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (vMove == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(CharacterController.velocity.x, 0.0f, CharacterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = 1;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * Acceleration);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * Acceleration);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(vMove.x, 0.0f, vMove.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (vMove != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, TurnRate);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            targetDirection.x *= Grounded ? 1f : 0.1f;

            // move the player
            a = targetDirection.normalized * _speed * Ez.TimeMod + new Vector3(0.0f, _verticalVelocity, 0.0f) * Ez.TimeMod;
            CharacterController.Move(targetDirection.normalized * _speed * Ez.TimeMod + new Vector3(0.0f, _verticalVelocity, 0.0f) * Ez.TimeMod);

            // update animator if using character
            if (HadAnim) anim.SetFloat(_animIDSpeed, _animationBlend);
            // if (HadAnim) anim.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        public Vector3 a, b, c;

        static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}