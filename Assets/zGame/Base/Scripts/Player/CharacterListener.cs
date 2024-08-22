using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class CharacterListener : MonoBehaviour
    {
        CharacterController characterController;
        CharacterController CharacterController
        {
            get
            {
                if (characterController == null) characterController = GetComponentInParent<CharacterController>();
                return characterController;
            }
        }

        // [SerializeField] AudioClip[] FootstepSound;
        // [SerializeField] AudioClip LandingSound;
        // [Range(0, 100)]
        // [SerializeField] int SoundVolume = 50;

        public void OnFootstep(AnimationEvent animationEvent)
        {
            // if (animationEvent.animatorClipInfo.weight > 0.5f)
            // {
            //     if (FootstepSound.Length > 0)
            //     {
            //         var index = Random.Range(0, FootstepSound.Length);
            //         AudioSource.PlayClipAtPoint(FootstepSound[index], transform.TransformPoint(CharacterController.center), SoundVolume / 100f);
            //     }
            // }
        }

        public void OnLand(AnimationEvent animationEvent)
        {
            // if (animationEvent.animatorClipInfo.weight > 0.5f) AudioSource.PlayClipAtPoint(LandingSound, transform.TransformPoint(CharacterController.center), SoundVolume / 100f);
        }
    }
}
