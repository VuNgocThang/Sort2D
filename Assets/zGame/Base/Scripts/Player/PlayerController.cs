using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class PlayerController : BaseControl
    {
        LogicControl logicControl;
        LogicControl LogicControl
        {
            get
            {
                if (logicControl == null) logicControl = FindObjectOfType<LogicControl>();
                return logicControl;
            }
        }

        void Start()
        {
            if (LogicControl == null) return;
            if (LogicControl.ControllerMove != null) LogicControl.ControllerMove.OnEventDragV = OnMove;
            if (LogicControl.ControllerMove != null) LogicControl.ControllerMove.OnEventRelease = (f) => OnMove(Vector3.zero);
            if (LogicControl.ControllerCam != null) LogicControl.ControllerCam.OnEventDragD = OnMoveCam;
            if (LogicControl.Btn != null) LogicControl.Btn.OnEventTouch = () => goJumping = true;
        }

        Vector3 vMove;
        void OnMove(Vector3 v)
        {
            vMove = v;
            if (vMove == Vector3.zero)
            {
                Rigidbody.AddForce(Vector3.zero, ForceMode.VelocityChange);
                return;
            }
            vMove.z = vMove.y;
            vMove.y = 0;
            vMove = vMove.normalized;
        }

        protected float SpeedCamX = 15f;
        protected float SpeedCamY = 0.2f;
        void OnMoveCam(Vector3 d)
        {
            // logicControl.CinemachineFreeLook.m_XAxis.Value += d.x * SpeedCamX * Ez.TimeMod;
            // logicControl.CinemachineFreeLook.m_YAxis.Value -= d.y * SpeedCamY * Ez.TimeMod;
        }

        void FixedUpdate()
        {
            CheckGround();
            CheckJump();
            CheckMove();
        }

        bool isGrounded;
        [SerializeField] LayerMask layerGround;
        void CheckGround()
        {
            isGrounded = Physics.CheckSphere(transform.position, 0.02f, layerGround, QueryTriggerInteraction.Ignore);
            Animator.SetBool(animIDGrounded, isGrounded);
        }

        bool goJumping;
        bool isJumping;
        [SerializeField] float jumpPower;
        void CheckJump()
        {
            if (isJumping) isJumping = false;
            if (isGrounded && goJumping)
            {
                isJumping = true;
                Animator.Play("JumpStart");
                Rigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            }
            goJumping = false;
        }

        [SerializeField] float moveSpeed = 50f;
        float vMoveAnimation;
        void CheckMove()
        {
            if (vMove != Vector3.zero)
            {
                Rigidbody.AddForce(vMove * moveSpeed * Ez.FixedTimeMod, ForceMode.VelocityChange);
                float _targetRotation = Mathf.Atan2(vMove.x, vMove.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
                vMoveAnimation = Mathf.Lerp(vMoveAnimation, moveSpeed, 0.5f);
            }
            else vMoveAnimation = Mathf.Lerp(vMoveAnimation, 0, 0.5f);
            if (vMoveAnimation < 0.1f) vMoveAnimation = 0;
            Animator.SetFloat(animIDSpeed, vMoveAnimation);
        }
    }
}
