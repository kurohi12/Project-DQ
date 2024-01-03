using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

}
