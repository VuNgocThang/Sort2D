using UnityEngine;

public class ScriptableObjectData
{
    public static T Load<T>(string path) where T : ScriptableObject
    {
        return Resources.Load<T>(path);
    }
    private const string FOLDER = "ScriptableObjectData/";
    private const string OBJECT = FOLDER + "ObjectConfig";

    private static ObjectConfig _objectConfig;

    public static ObjectConfig ObjectConfig
    {
        get
        {
            if (_objectConfig == null)
            {
                _objectConfig = Load<ObjectConfig>(OBJECT);
                if (_objectConfig == null)
                {
                    Debug.Log("ObjectConfig null");
                }
            }
            return _objectConfig;
        }
    }
}
