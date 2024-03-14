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

//복사된 오브젝트 정보
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

    private Dictionary<KeyType, GameObject> _prefabDict;   // 오브젝트 원본
    private Dictionary<KeyType, PoolData> _dataDict; // 풀 정보
    private Dictionary<KeyType, Stack<GameObject>> _poolDict;         // 오브젝트 풀
    private Dictionary<GameObject, CloneScheduleInfo> _cloneDict;  // 복제된 오브젝트

    private Dictionary<KeyType, GameObject> _t_ContainerDict; //하이어라키에서 보여질 컨테이너
    private Dictionary<Stack<GameObject>, KeyType> _t_poolKeyDict; //하이어라키에서 보여질 풀

    private bool testModeOn = true; 

    //유니티 에디터에서만 보이게 설정, 실제 빌드할 때는 적용 X
    [System.Diagnostics.Conditional("TEST_ON")]
    private void TestModeOnly(System.Action action)
    {
        if (!testModeOn) return;
        action();
    }

    // 컨테이너 이름 변경
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

        // Dictionary 생성
        _prefabDict = new Dictionary<KeyType, GameObject>(len);
        _dataDict = new Dictionary<KeyType, PoolData>(len);
        _poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
        _cloneDict = new Dictionary<GameObject, CloneScheduleInfo>(len * PoolData.COUNT);

        // Data로부터 새로운 Pool 오브젝트 정보 생성
        foreach (var data in _poolDataList)
        {
            RegisterInternal(data);
        }
    }

    //풀 데이터 정보 등록
    private void RegisterInternal(PoolData data)
    {
        // 중복 키는 등록 불가능
        if (_poolDict.ContainsKey(data.key))
        {
            return;
        }

        // 프리팹 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
        GameObject sample = Instantiate(data.prefab);
        sample.name = data.prefab.name;
        sample.SetActive(false);

        // Pool Dictionary에 풀 생성 + 풀에 미리 오브젝트들 만들어 담아놓기
        Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
        for (int i = 0; i < data.objectCount; i++)
        {
            GameObject clone = Instantiate(data.prefab);
            clone.SetActive(false);
            pool.Push(clone);

            _cloneDict.Add(clone, new CloneScheduleInfo(clone, pool)); // 복제 데이터 캐싱
        }

        // 딕셔너리에 추가
        _prefabDict.Add(data.key, sample);
        _dataDict.Add(data.key, data);
        _poolDict.Add(data.key, pool);

        TestModeOnly(() =>
        {
            // 프리팹을 공통 게임오브젝트의 자식으로 묶기
            string posName = "ObjectPool Prefab";
            GameObject parentOfSamples = GameObject.Find(posName);
            if (parentOfSamples == null)
                parentOfSamples = new GameObject(posName);

            sample.transform.SetParent(parentOfSamples.transform);

            // 풀 - 키 딕셔너리에 추가
            _t_poolKeyDict.Add(pool, data.key);

            // 컨테이너 게임오브젝트 생성
            _t_ContainerDict.Add(data.key, new GameObject(""));

            // 컨테이너 자식으로 설정
            foreach (var item in pool)
            {
                item.transform.SetParent(_t_ContainerDict[data.key].transform);
            }

            // 컨테이너 이름 변경
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
        // 키가 존재하지 않는 경우 null 리턴
        if (!_poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject go;

        // 풀에 재고가 있는 경우 꺼내오기
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 재고가 없는 경우 프리팹 복제
        else
        {
            go = CloneFromPrefab(key);
            _cloneDict.Add(go, new CloneScheduleInfo(go, pool)); // 복제 데이터 캐싱
        }

        go.SetActive(true);
        go.transform.SetParent(null);

        TestModeOnly(() =>
        {
            // 컨테이너 이름 변경
            Test_ChangeContainerName(key);
        });

        return go;
    }

    //풀에 오브젝트 넣는 처리
    private void DespawnInternal(CloneScheduleInfo data)
    {
        // 풀에 집어넣기
        data.clone.SetActive(false);

        //data.pool.Push(data.clone);
        StartCoroutine(PushCount(data));

        TestModeOnly(() =>
        {
            KeyType key = _t_poolKeyDict[data.pool];

            // 컨테이너 자식으로 넣기
            data.clone.transform.SetParent(_t_ContainerDict[key].transform);

            // 컨테이너 이름 변경
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

        // 복제된 게임오브젝트가 아닌 경우 삭제
        if (!_cloneDict.TryGetValue(go, out var cloneData))
        {
            Destroy(go);
            return;
        }

        DespawnInternal(cloneData);
    }
}