#if UNITY_EDITOR
#define TEST_ON
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using KeyType = System.String;

//����� ������Ʈ ����
class CloneScheduleInfo
{
    public readonly GameObject clone;
    public readonly Stack<GameObject> pool;
    public CloneScheduleInfo(GameObject clone, Stack<GameObject> pool)
    {
        this.clone = clone;
        this.pool = pool;
    }
}

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    private List<PoolData> _poolDataList = new List<PoolData>();

    private Dictionary<KeyType, GameObject> _prefabDict;   // ������Ʈ ����
    private Dictionary<KeyType, PoolData> _dataDict; // Ǯ ����
    private Dictionary<KeyType, Stack<GameObject>> _poolDict;         // ������Ʈ Ǯ
    private Dictionary<GameObject, CloneScheduleInfo> _cloneDict;  // ������ ������Ʈ

    private Dictionary<KeyType, GameObject> _t_ContainerDict; //���̾��Ű���� ������ �����̳�
    private Dictionary<Stack<GameObject>, KeyType> _t_poolKeyDict; //���̾��Ű���� ������ Ǯ

    private bool testModeOn = true; 

    //����Ƽ �����Ϳ����� ���̰� ����, ���� ������ ���� ���� X
    [System.Diagnostics.Conditional("TEST_ON")]
    private void TestModeOnly(System.Action action)
    {
        if (!testModeOn) return;
        action();
    }

    // �����̳� �̸� ����
    [System.Diagnostics.Conditional("TEST_ON")]
    private void Test_ChangeContainerName(KeyType key)
    {
        if (!testModeOn) return;
        Stack<GameObject> pool = _poolDict[key];

        int cloneCount = _cloneDict.Values.Where(v => v.pool == pool).Count();
        int PoolCount = pool.Count;
        int maxCount = _dataDict[key].maxObjectCount;
        _t_ContainerDict[key].name
               = $"Pool <{key}> - [{cloneCount - PoolCount}] Used, [{PoolCount}] InPool, [{maxCount}] Max";
    }

    protected override void Init()
    {
        TestModeOnly(() =>
        {
            _t_ContainerDict = new Dictionary<KeyType, GameObject>();
            _t_poolKeyDict = new Dictionary<Stack<GameObject>, KeyType>();
        });

        int len = _poolDataList.Count;
        if (len == 0) return;

        // Dictionary ����
        _prefabDict = new Dictionary<KeyType, GameObject>(len);
        _dataDict = new Dictionary<KeyType, PoolData>(len);
        _poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
        _cloneDict = new Dictionary<GameObject, CloneScheduleInfo>(len * PoolData.COUNT);

        // Data�κ��� ���ο� Pool ������Ʈ ���� ����
        foreach (var data in _poolDataList)
        {
            RegisterInternal(data);
        }
    }

    //Ǯ ������ ���� ���
    private void RegisterInternal(PoolData data)
    {
        // �ߺ� Ű�� ��� �Ұ���
        if (_poolDict.ContainsKey(data.key))
        {
            return;
        }

        // ������ ���ӿ�����Ʈ ����, PoolObject ������Ʈ ���� Ȯ��
        GameObject sample = Instantiate(data.prefab);
        sample.name = data.prefab.name;
        sample.SetActive(false);

        // Pool Dictionary�� Ǯ ���� + Ǯ�� �̸� ������Ʈ�� ����� ��Ƴ���
        Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
        for (int i = 0; i < data.objectCount; i++)
        {
            GameObject clone = Instantiate(data.prefab);
            clone.SetActive(false);
            pool.Push(clone);

            _cloneDict.Add(clone, new CloneScheduleInfo(clone, pool)); // ���� ������ ĳ��
        }

        // ��ųʸ��� �߰�
        _prefabDict.Add(data.key, sample);
        _dataDict.Add(data.key, data);
        _poolDict.Add(data.key, pool);

        TestModeOnly(() =>
        {
            // �������� ���� ���ӿ�����Ʈ�� �ڽ����� ����
            string posName = "ObjectPool Prefab";
            GameObject parentOfSamples = GameObject.Find(posName);
            if (parentOfSamples == null)
                parentOfSamples = new GameObject(posName);

            sample.transform.SetParent(parentOfSamples.transform);

            // Ǯ - Ű ��ųʸ��� �߰�
            _t_poolKeyDict.Add(pool, data.key);

            // �����̳� ���ӿ�����Ʈ ����
            _t_ContainerDict.Add(data.key, new GameObject(""));

            // �����̳� �ڽ����� ����
            foreach (var item in pool)
            {
                item.transform.SetParent(_t_ContainerDict[data.key].transform);
            }

            // �����̳� �̸� ����
            Test_ChangeContainerName(data.key);
        });
    }

    private GameObject CloneFromPrefab(KeyType key)
    {
        if (!_prefabDict.TryGetValue(key, out GameObject sample)) return null;

        return Instantiate(sample);
    }

    public GameObject Spawn(KeyType key)
    {
        // Ű�� �������� �ʴ� ��� null ����
        if (!_poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject go;

        // Ǯ�� ��� �ִ� ��� ��������
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // ��� ���� ��� ������ ����
        else
        {
            go = CloneFromPrefab(key);
            _cloneDict.Add(go, new CloneScheduleInfo(go, pool)); // ���� ������ ĳ��
        }

        go.SetActive(true);
        go.transform.SetParent(null);

        TestModeOnly(() =>
        {
            // �����̳� �̸� ����
            Test_ChangeContainerName(key);
        });

        return go;
    }

    //Ǯ�� ������Ʈ �ִ� ó��
    private void DespawnInternal(CloneScheduleInfo data)
    {
        // Ǯ�� ����ֱ�
        data.clone.SetActive(false);

        //data.pool.Push(data.clone);
        StartCoroutine(PushCount(data));

        TestModeOnly(() =>
        {
            KeyType key = _t_poolKeyDict[data.pool];

            // �����̳� �ڽ����� �ֱ�
            data.clone.transform.SetParent(_t_ContainerDict[key].transform);

            // �����̳� �̸� ����
            Test_ChangeContainerName(key);
        });
    }

    private IEnumerator PushCount(CloneScheduleInfo data)
    {
        yield return new WaitForSeconds(4f);
        data.pool.Push(data.clone);
        StopCoroutine(PushCount(data));
    }

    public void Despawn(GameObject go)
    {
        if (go == null) return;
        if (go.activeSelf == false) return;

        // ������ ���ӿ�����Ʈ�� �ƴ� ��� ����
        if (!_cloneDict.TryGetValue(go, out var cloneData))
        {
            Destroy(go);
            return;
        }

        DespawnInternal(cloneData);
    }
}