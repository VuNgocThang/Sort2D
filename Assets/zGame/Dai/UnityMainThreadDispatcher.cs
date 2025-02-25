using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// Author: Pim de Witte (pimdewitte.com) and contributors
/// <summary>
/// A thread-safe class which holds a queue with actions to execute on the next Update() method. It can be used to make calls to the main thread for
/// things such as UI Manipulation in Unity. It was developed for use in combination with the Firebase Unity plugin, which uses separate threads for event handling
/// </summary>
public class UnityMainThreadDispatcher : MonoBehaviour
{

    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    /// <summary>
    /// Locks the queue and adds the IEnumerator to the queue
    /// </summary>
    /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
    public void Enqueue(IEnumerator action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
            {
                StartCoroutine(action);
            });
        }
    }

    /// <summary>
    /// Locks the queue and adds the Action to the queue
    /// </summary>
    /// <param name="action">function that will be executed from the main thread.</param>
    public void Enqueue(Action action)
    {
        Enqueue(ActionWrapper(action));
    }
    IEnumerator ActionWrapper(Action a)
    {
        //yield return null;
        yield return null;
        yield return null;
        a();
        //yield return null;
    }
    public void EnqueueTime1(Action action)
    {
        Enqueue(ActionWrapperTime1(action));
    }

    private WaitForSeconds timeWait1s = new WaitForSeconds(1f);
    IEnumerator ActionWrapperTime1(Action a)
    {
        //yield return null;
        yield return timeWait1s;
        a();
        //yield return null;
    }
    public void EnqueueTime2(Action action)
    {
        Enqueue(ActionWrapperTime2(action));
    }

    private WaitForSeconds timeWait2s = new WaitForSeconds(2f);
    IEnumerator ActionWrapperTime2(Action a)
    {
        //yield return null;
        yield return timeWait2s;
        a();
        //yield return null;
    }
    
    public void Enqueue(Action<bool> action, bool value)
    {
        Enqueue(ActionWrapper(action,value));
    }
    IEnumerator ActionWrapper(Action<bool> a,bool value)
    {
        //yield return null;
        yield return null;
        yield return null;
        a(value);
        //yield return null;
    }
    public void Enqueue(Action<string> action, string value)
    {
        Enqueue(ActionWrapper(action, value));
    }
    IEnumerator ActionWrapper(Action<string> a, string value)
    {
        //yield return null;
        yield return null;
        yield return null;
        a(value);
        //yield return null;
    }

    private static UnityMainThreadDispatcher _instance = null;

    public static bool Exists()
    {
        return _instance != null;
    }

    public static UnityMainThreadDispatcher Instance()
    {
        if (!Exists())
        {
            throw new Exception("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        }
        return _instance;
    }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

}