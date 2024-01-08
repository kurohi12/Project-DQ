using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;

[System.Serializable]
public class PoolData : MonoBehaviour
{
    public const int COUNT = 10;
    public const int MAX_COUNT = 50;

    public KeyType key;
    public GameObject prefab;
    public int objectCount = COUNT; // 초기 생성 개수
    public int maxObjectCount = MAX_COUNT;     // 큐 내에 보관할 수 있는 오브젝트 최대 개수
}
