using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            GameObject gameObj;
            gameObj = GameObject.Find(typeof(T).Name);
            if (instance == null)
            {
                instance = gameObj.AddComponent<T>();
            }
            else
            {
                instance = gameObj.GetComponent<T>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
