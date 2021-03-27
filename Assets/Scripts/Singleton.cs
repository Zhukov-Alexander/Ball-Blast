using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Object[] instances = FindObjectsOfType(typeof(T));
                if (instances.Length > 1)
                {
                    Debug.LogError("Multiple instancing of singleton " + typeof(T));
                }
                if (instances.Length == 0)
                {
                    Debug.LogError("No instances of singleton " + typeof(T));
                }
                instance = (T)instances[0];
            }
            return instance;
        }
    }
}
public interface ISingleton { };
