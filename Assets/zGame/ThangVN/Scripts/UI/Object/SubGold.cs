using TMPro;
using UnityEngine;

public class SubGold : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtSubGold;
    [SerializeField] int subGold;

    public void Init(int _sub)
    {
        subGold = _sub;
        txtSubGold.text = $"-{subGold}";
    }

    public void RecyclePool()
    {
        PoolManager.Recycle(this.gameObject.GetInstanceID());
    }
}
