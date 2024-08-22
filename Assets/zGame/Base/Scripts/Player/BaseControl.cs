using UnityEngine;
using ntDev;

// by nt.Dev93
namespace ntDev
{
    public enum MoveType
    {
        GROUND,
        AIR,
    }

    public class BaseControl : MonoBehaviour
    {
        Rigidbody rBody;
        public Rigidbody Rigidbody
        {
            get
            {
                if (rBody == null) rBody = GetComponent<Rigidbody>();
                return rBody;
            }
        }
        UnityController unityController;
        public UnityController UnityController
        {
            get
            {
                if (unityController == null) unityController = GetComponent<UnityController>();
                return unityController;
            }
        }

        Animator animator;
        protected int animIDSpeed;
        protected int animIDGrounded;
        protected int animIDJump;
        protected int animIDFreeFall;
        public Animator Animator
        {
            get
            {
                if (animator == null) animator = GetComponentInChildren<Animator>();
                animIDSpeed = Animator.StringToHash("Speed");
                animIDGrounded = Animator.StringToHash("Grounded");
                animIDJump = Animator.StringToHash("Jump");
                animIDFreeFall = Animator.StringToHash("FreeFall");
                return animator;
            }
        }
    }
}
