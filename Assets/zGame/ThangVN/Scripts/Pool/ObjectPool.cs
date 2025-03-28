using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    public static ObjectPool<T> Instance;
    public List<T> pooledObjects;
    public T prefab;
    public int amountPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<T>();
        T tmp;
        for (int i = 0; i < amountPrefab; i++)
        {
            tmp = Instantiate(prefab);
            tmp.gameObject.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public T GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        T tmp = Instantiate(prefab);
        tmp.gameObject.SetActive(false);
        pooledObjects.Add(tmp);
        return tmp;
    }
}