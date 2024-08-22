using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class EasyAnimation : MonoBehaviour
    {
        [SerializeField] AnimationClip clip;
        [SerializeField] float mixTime;

        void Start()
        {
            GetComponent<Animator>().CrossFadeInFixedTime(clip.name, mixTime, 0, Ez.Random(0, 1f));
        }

        void OnValidate()
        {
            GetComponent<Animator>().CrossFadeInFixedTime(clip.name, mixTime, 0, Ez.Random(0, 1f));
        }
    }
}
