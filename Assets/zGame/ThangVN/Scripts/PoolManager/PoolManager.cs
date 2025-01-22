using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


    [System.Serializable]
    public class InfoObjectToPool
    {
        public int id;
        public GameObject prefab;
    }

    public class InfoDelayPool
    {
        public float time;
        public int id;
        public int instanceId;
        public float timer;
        public bool setActive;

        public bool isDone
        {
            get { return timer >= time; }
        }
    }
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;
        public static PoolManager instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "PoolManager";
                    _instance = temp.AddComponent<PoolManager>();
                }

                return _instance;
            }
        }



        public List<InfoObjectToPool> objectToPools = new List<InfoObjectToPool>();
        public int createDefault = 1;
        
        Dictionary<int, List<int>> pooledObjects = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> usingObjects = new Dictionary<int, List<int>>();
        Dictionary<int, GameObject> instanceIdObjects = new Dictionary<int, GameObject>();
        Dictionary<int, int > instanceToIds = new Dictionary<int, int>();
        Dictionary<int, GameObject> idToPrefab = new Dictionary<int, GameObject>();


        void ForceCreateObject(int id, GameObject prefab, int countCreate)
        {
            prefab.SetActive(false);
            if (!idToPrefab.ContainsKey(id))
            {
                idToPrefab.Add(id,prefab);
            }
            
            if (pooledObjects.ContainsKey(id))
            {
                for (int i = 0; i < countCreate; i++)
                {
                    GameObject temp = Instantiate(prefab);
                    int _instanceId = temp.GetInstanceID();
                    pooledObjects[id].Add(_instanceId);
                    instanceIdObjects.Add(_instanceId, temp);
                    instanceToIds.Add(_instanceId, id);
                }
            }
            else
            {
                List<int> tempInstanceIds = new List<int>();
                for (int i = 0; i < countCreate; i++)
                {
                    GameObject temp = Instantiate(prefab);
                    int _instanceId = temp.GetInstanceID();
                    tempInstanceIds.Add(_instanceId);
                    instanceIdObjects.Add(_instanceId, temp);
                    instanceToIds.Add(_instanceId,id);
                    
                }
                pooledObjects.Add(id, tempInstanceIds);
            }
        }
        void Setup()
        {
            int size = objectToPools.Count;
            for (int i = 0; i < size; i++)
            {
                CreateObject(objectToPools[i].id, objectToPools[i].prefab);
            }
            
        }
        void CreateObject(int id, GameObject prefab)
        {
            prefab.SetActive(false);
            if (!idToPrefab.ContainsKey(id))
            {
                idToPrefab.Add(id,prefab);
            }
            if (pooledObjects.ContainsKey(id))
            {
                
                for (int i = 0; i < createDefault; i++)
                {
                    GameObject temp = Instantiate(prefab);
                    int _instanceId = temp.GetInstanceID();
                    pooledObjects[id].Add(_instanceId);
                    instanceIdObjects.Add(_instanceId, temp);
                    instanceToIds.Add(_instanceId, id);
                }
            }
            else
            {
                List<int> tempInstanceIds = new List<int>();
                for (int i = 0; i < createDefault; i++)
                {
                    GameObject temp = Instantiate(prefab);
                    int _instanceId = temp.GetInstanceID();
                    tempInstanceIds.Add(_instanceId);
                    instanceIdObjects.Add(_instanceId, temp);
                    instanceToIds.Add(_instanceId,id);
                    
                }
                pooledObjects.Add(id, tempInstanceIds);
                
            }
        }
        GameObject SpawnObject(int id)
        {
            if (pooledObjects.ContainsKey(id))
            {
                if (pooledObjects[id].Count > 0)
                {
                    int temp = pooledObjects[id][0];
                    if (usingObjects.ContainsKey(id))
                    {
                        usingObjects[id].Add(temp);
                    }
                    else
                    {
                        usingObjects.Add(id, new List<int>(){temp});
                    }
                    pooledObjects[id].RemoveAt(0);
                    if (pooledObjects.Count == 0)
                    {
                        for (int i = 0; i < objectToPools.Count; i++)
                        {
                            if (objectToPools[i].id == id)
                            {
                                CreateObject(id, objectToPools[i].prefab);
                                break;
                            }
                        }
                    }
                    return instanceIdObjects[temp];
                }
//                else
//                {
//                    
//                    prefab.SetActive(false);
//                    GameObject temp = Instantiate(prefab);
//                    int _instanceId = temp.GetInstanceID();
//                    if(!usingObjects.ContainsKey(id))
//                        usingObjects.Add(id, new List<int>(){_instanceId});
//                    else
//                    {
//                        usingObjects[id].Add(_instanceId);
//                    }
//                    instanceIdObjects.Add(_instanceId, temp);
//                    return temp;
//                }
            }

            if (idToPrefab.ContainsKey(id))
            {
                GameObject prefab = idToPrefab[id];
                return SpawnObject(prefab);
            }
            
//            else
//            {
//                pooledObjects.Add(id, new List<int>());
//                prefab.SetActive(false);
//                GameObject temp = Instantiate(prefab);
//                int _instanceId = temp.GetInstanceID();
//                usingObjects.Add(id, new List<int>(){_instanceId});
//                instanceIdObjects.Add(_instanceId, temp);
//                return temp;
//            }
            return null;
        }
       
        GameObject SpawnObject(GameObject prefab)
        {
            int id = prefab.GetInstanceID();
            if (!idToPrefab.ContainsKey(id))
            {
                idToPrefab.Add( id, prefab);
            }
            foreach (var objectTemp in objectToPools)
            {
                if (objectTemp.prefab.GetInstanceID() == id)
                {
                    id = objectTemp.id;
                }
            }
            if (pooledObjects.ContainsKey(id))
            {
                if (pooledObjects[id].Count > 0)
                {
                    int temp = pooledObjects[id][0];
                    if (usingObjects.ContainsKey(id))
                    {
                        usingObjects[id].Add(temp);
                    }
                    else
                    {
                        usingObjects.Add(id, new List<int>(){temp});
                    }
                    pooledObjects[id].RemoveAt(0);
                    return instanceIdObjects[temp];
                }
                else
                {
                    prefab.SetActive(false);
                    GameObject temp = Instantiate(prefab);
                    int _instanceId = temp.GetInstanceID();
                    if(!usingObjects.ContainsKey(id))
                        usingObjects.Add(id, new List<int>(){_instanceId});
                    else
                    {
                        usingObjects[id].Add(_instanceId);
                    }
                    instanceIdObjects.Add(_instanceId, temp);
                    instanceToIds.Add(_instanceId,id);
                    return temp;
                }
            }
            else
            {
                pooledObjects.Add(id, new List<int>());
                prefab.SetActive(false);
                GameObject temp = Instantiate(prefab);
                int _instanceId = temp.GetInstanceID();
                usingObjects.Add(id, new List<int>(){_instanceId});
                instanceIdObjects.Add(_instanceId, temp);
                instanceToIds.Add(_instanceId,id);
                return temp;
            }
            return null;
        }
        
        GameObject SpawnObject(int id, GameObject prefab)
        {
            if (!idToPrefab.ContainsKey(id))
            {
                idToPrefab.Add( id, prefab);
            }
            if (pooledObjects.ContainsKey(id))
            {
                if (pooledObjects[id].Count > 0)
                {
                    int temp = pooledObjects[id][0];
                    if (usingObjects.ContainsKey(id))
                    {
                        usingObjects[id].Add(temp);
                    }
                    else
                    {
                        usingObjects.Add(id, new List<int>(){temp});
                    }
                    pooledObjects[id].RemoveAt(0);
                    return instanceIdObjects[temp];
                }
                else
                {
                    prefab.SetActive(false);
                    GameObject temp = Instantiate(prefab);
                    int _instanceId = temp.GetInstanceID();
                    if(!usingObjects.ContainsKey(id))
                        usingObjects.Add(id, new List<int>(){_instanceId});
                    else
                    {
                        usingObjects[id].Add(_instanceId);
                    }
                    instanceIdObjects.Add(_instanceId, temp);
                    instanceToIds.Add(_instanceId,id);
                    return temp;
                }
            }
            else
            {
                pooledObjects.Add(id, new List<int>());
                prefab.SetActive(false);
                GameObject temp = Instantiate(prefab);
                int _instanceId = temp.GetInstanceID();
                usingObjects.Add(id, new List<int>(){_instanceId});
                instanceIdObjects.Add(_instanceId, temp);
                instanceToIds.Add(_instanceId,id);
                return temp;
            }
            return null;
        }

        void RecycleObject(int id, int tempInstanceId, bool setActive=false)
        {
            List<int> tempUsingObject = usingObjects[id];
            if (tempUsingObject.Contains(tempInstanceId))
            {
                pooledObjects[id].Add(tempInstanceId);
                tempUsingObject.Remove(tempInstanceId);
                GameObject objectToRecycle = instanceIdObjects[tempInstanceId];
                if (objectToRecycle!=null)
                {
                    objectToRecycle.SetActive(setActive);
                    objectToRecycle.transform.SetParent(null);
                    //Debug.Log("not null Recycle");
                }
                else
                {
                    //Debug.Log("null Recycle");
                }
            }
            else
            {
                //Debug.Log("khong tim thay Key");
            }
        }
        void RecycleObject( int tempInstanceId, bool setActive=false)
        {
            if (!instanceToIds.ContainsKey(tempInstanceId)) return;
            
            int id = instanceToIds[tempInstanceId];
            RecycleObject(id, tempInstanceId, setActive);
        }

        void RecycleObject(int id, int tempInstanceId, float time, bool setActive = false)
        {
            int cache = tempInstanceId;
            List<int> tempUsingObject = usingObjects[id];
            if (tempUsingObject.Contains(tempInstanceId))
            {

                tempUsingObject.Remove(tempInstanceId);

                InfoDelayPool tempInfoDelay = CreateInfo();
                tempInfoDelay.time = time;
                tempInfoDelay.id = id;
                tempInfoDelay.instanceId = tempInstanceId;
                tempInfoDelay.setActive = setActive;
                _poolingTimer.Add(tempInfoDelay);
            }
        }

        void ClearAll()
        {
            foreach (var key in usingObjects.Keys)
            {
                for (int i = 0; i < usingObjects[key].Count; i++)
                {
                    Recycle(usingObjects[key][i]);
                }
            }

            foreach (var VARIABLE in instanceIdObjects.Values)
            {
                Destroy(VARIABLE);
            }
        }
        
        #region STATIC METHOD

        public static void Create(GameObject prefab)
        {
            instance.CreateObject(prefab.GetInstanceID(), prefab);
        }
        
        public static GameObject Spawn(int id)
        {
            return instance.SpawnObject(id);
        }
        
        public static GameObject Spawn(GameObject prefab)
        {
            return instance.SpawnObject(prefab);
        }
        
        public static GameObject Spawn(int id, GameObject prefab)
        {
            return instance.SpawnObject(id, prefab);
        }

        public static void Recycle(int id, int instanceId )
        {
            instance.RecycleObject(id, instanceId);
        }

        public static void Recycle(int instanceId, bool setActive = false)
        {
            instance.RecycleObject( instanceId,setActive);
        }
        
        public static void Recycle(int id, int instanceId, float time , bool setActive = false)
        {
            instance.RecycleObject(id, instanceId, time, setActive);
        }

        public static void Recycle(int instanceId, float time, bool setActive = false)
        {
            int id = instance.instanceToIds[instanceId];
            Recycle(id, instanceId, time, setActive);
        }

        public static int GetID(int instanceId)
        {
            return instance.instanceToIds[instanceId];
        }

        public static void ForceToCreatePool(GameObject prefab, int count)
        {
            instance.ForceCreateObject(prefab.GetInstanceID(), prefab, count);
        }

        public static void ClearPool()
        {
            instance.ClearAll();
        }
        #endregion

        #region UNITY METHOD
        List<InfoDelayPool> _infoDelayPools = new List<InfoDelayPool>();
        List<InfoDelayPool> _poolingTimer = new List<InfoDelayPool>();

        InfoDelayPool CreateInfo()
        {
            if (_infoDelayPools.Count != 0)
            {
                InfoDelayPool temp = _infoDelayPools[0];
                _infoDelayPools.RemoveAt(0);
                temp.timer = 0;
                return temp;
            }
            else
            {
                InfoDelayPool temp = new InfoDelayPool();
                temp.timer = 0;
                return temp;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            Setup();
        }

        
        private void Update()
        {
//            GameLogic.deltaTime = Time.deltaTime;
//            float deltaTime = GameLogic.deltaTime;
            float deltaTime = Time.deltaTime;
            for (int i = _poolingTimer.Count-1; i >=0 ; i--)
            {
                if (!_poolingTimer[i].isDone)
                {
                    _poolingTimer[i].timer += deltaTime;
                    if (_poolingTimer[i].isDone)
                    {
                        InfoDelayPool temp = _poolingTimer[i];
                        pooledObjects[temp.id].Add(temp.instanceId);
                        GameObject objectToRecycle = instanceIdObjects[temp.instanceId];
                        if (objectToRecycle != null)
                        {
                            objectToRecycle.SetActive(false);
                            objectToRecycle.transform.SetParent(null); 
                        }

                        _poolingTimer.RemoveAt(i);
                        _infoDelayPools.Add(temp);
                    }
                }
            }
        }

        #endregion
//        public int GetInstanceIdObject(GameObject obj)
//        {
//            obj.SetActive(false);
//            return obj.GetInstanceID();
//        }
    }


//    [CustomEditor(typeof(PoolManager))]
//    public class PoolManagerEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            DrawDefaultInspector();
//
//            PoolManager yourClass = (PoolManager)target;
//
////            if (GUILayout.Button("Button 1"))
////            {
////                yourClass.GetInstanceIdObject();
////            }
//
//        }
//    }


