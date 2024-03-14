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
    public int objectCount = COUNT; // �ʱ� ���� ����
    public int maxObjectCount = MAX_COUNT;     // ť ���� ������ �� �ִ� ������Ʈ �ִ� ����
}
