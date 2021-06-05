using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameConfigContainer;

public class Singleton<T> : MonoBehaviour, ISingleton where T : Singleton<T>
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
                else if (instances.Length == 0)
                {
                    foreach (var item in gameConfig.singletonPrefabs)
                    {
                        if(item.TryGetComponent(out T component))
                        {
                            instance = Instantiate(item).GetComponent<T>();
                        }
                    }
                    Debug.LogError("No instances of singleton " + typeof(T));
                }
                else
                    instance = (T)instances[0];
            }
            return instance;
        }
    }
}
public interface ISingleton { };
