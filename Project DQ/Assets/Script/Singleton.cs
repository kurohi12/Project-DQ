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
            // �ν��Ͻ��� ������ ���� ����
            if (instance == null)
            {
                // ������ ã�ƺ��� ������ ���� ����
                instance = FindObjectOfType<T>();

                // ���� ���� ��� ���� ����
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        Init();
    }

    protected virtual void Init()
    {

    }
}
