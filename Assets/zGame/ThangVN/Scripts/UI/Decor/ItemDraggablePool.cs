using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDraggablePool : ObjectPool<ItemDraggable>
{
   public ItemDraggable GetItemDraggable()
    {
        return GetPooledObject();
    }
}
