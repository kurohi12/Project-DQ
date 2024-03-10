using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private GameObject obj = null;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            obj = PoolManager.Instance.Spawn("Fire");
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            obj = PoolManager.Instance.Spawn("Ice");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            obj = PoolManager.Instance.Spawn("Ex");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            obj.GetComponent<Object>().Attack();
        }
    }
}
