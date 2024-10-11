using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragPool : ObjectPool<ITEMDRAG>
{
    public ITEMDRAG GetItemDragPool()
    {
        return GetPooledObject();
    }
}
