using UnityEngine;
// by nt.Dev93
namespace ntDev
{
    public class RandomAnimSpine : MonoBehaviour
    {
        void Start()
        {
            ManagerSpine anim = GetComponent<ManagerSpine>();
            // float time = anim.GetTimeOfAnim(anim.AnimName(0));
            // anim.SetTime(0, Ez.Random(0, time));
        }
    }
}
