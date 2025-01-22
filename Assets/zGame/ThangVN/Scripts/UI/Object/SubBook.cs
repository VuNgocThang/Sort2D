using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubBook : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtSubBook;
    [SerializeField] int subBook;

    public void Init(int _sub)
    {
        subBook = _sub;
        txtSubBook.text = $"-{subBook}";
    }

    public void RecyclePool()
    {
        PoolManager.Recycle(this.gameObject.GetInstanceID());
    }
}
