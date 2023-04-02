using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingletonClass<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<T>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("Singleton Class").AddComponent<T>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<T>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;

        }
        DontDestroyOnLoad(gameObject);
    }
}

