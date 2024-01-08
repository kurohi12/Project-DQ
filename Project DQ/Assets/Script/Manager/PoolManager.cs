#if UNITY_EDITOR
#define TEST_ON
#endif

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using KeyType = System.String;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    private List<PoolData> _poolDataList = new List<PoolData>(4);

    private Dictionary<KeyType, GameObject> _prefabDict;   // 오브젝트 원본
    private Dictionary<KeyType, PoolData> _dataDict; // 풀 정보
    private Dictionary<KeyType, Stack<GameObject>> _poolDict;         // 오브젝트 풀
    private Dictionary<GameObject, Stack<GameObject>> _cloneDict; // 복제된 오브젝트

    private Dictionary<KeyType, GameObject> _t_ContainerDict;
    private Dictionary<Stack<GameObject>, KeyType> _t_poolKeyDict;

    private bool testModeOn = true;

    private void Start()
    {
        Init();
    }


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

        //int cloneCount = _cloneDict.Values.Count(stack => stack.Contains(_poolDict[key].Last()));
        int PoolCount = pool.Count;
        int maxCount = _dataDict[key].maxObjectCount;
        /*[{cloneCount - PoolCount}] Used,*/
        _t_ContainerDict[key].name
            = $"Pool <{key}> - [{PoolCount}] Available, [{maxCount}] Max";
    }

    private void Init()
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
        _cloneDict = new Dictionary<GameObject, Stack<GameObject>>(len * PoolData.COUNT);

        // Data로부터 새로운 Pool 오브젝트 정보 생성
        foreach (var data in _poolDataList)
        {
            RegisterInternal(data);
        }
    }

    /// <summary> Pool 데이터로부터 새로운 Pool 오브젝트 정보 등록 </summary>
    private void RegisterInternal(PoolData data)
    {
        // 중복 키는 등록 불가능
        if (_poolDict.ContainsKey(data.key))
        {
            return;
        }

        // 샘플 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
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

            _cloneDict.Add(clone, pool); // Clone-Stack 캐싱
        }

        // 딕셔너리에 추가
        _prefabDict.Add(data.key, sample);
        _dataDict.Add(data.key, data);
        _poolDict.Add(data.key, pool);

        TestModeOnly(() =>
        {
            // 샘플을 공통 게임오브젝트의 자식으로 묶기
            string posName = "ObjectPool Samples";
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

    private GameObject CloneFromSample(KeyType key)
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

        // 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 재고가 없는 경우 샘플로부터 복제
        else
        {
            go = CloneFromSample(key);
            _cloneDict.Add(go, pool); // Clone-Stack 캐싱
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

    public void Despawn(GameObject go)
    {
        // 캐싱된 게임오브젝트가 아닌 경우 파괴
        if (!_cloneDict.TryGetValue(go, out var pool))
        {
            Destroy(go);
            return;
        }

        // 집어넣기
        go.SetActive(false);
        pool.Push(go);

        TestModeOnly(() =>
        {
            KeyType key = _t_poolKeyDict[pool];

            // 컨테이너 자식으로 넣기
            go.transform.SetParent(_t_ContainerDict[key].transform);

            // 컨테이너 이름 변경
            Test_ChangeContainerName(key);
        });
    }
}