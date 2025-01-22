using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHeart : MonoBehaviour
{
    public void RecyclePool()
    {
        PoolManager.Recycle(this.gameObject.GetInstanceID());
    }
}
