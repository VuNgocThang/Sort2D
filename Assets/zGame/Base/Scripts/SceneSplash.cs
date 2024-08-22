using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class SceneSplash : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                ManagerLoading.ShowLoadScene(() => ManagerLoading.HideLoadScene());
            // ManagerToast.Show("DM NEKO");
        }
    }
}