using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //  [Serializable]

public class BulletManager : Singleton<BulletManager>
{
    [Serializable]
    public struct BulletPool
    {
        public int poolSize; //오브젝트 풀 크기
        public GameObject[] bulletObject; //저장된 오브젝트
        public Transform spawnPoint; //플레이어 좌표
        public GameObject prefab; //프리펩
    }

    [SerializeField]
    private BulletPool[] pool;
    private GameObject bullet;

    private void Start()
    {
        //오브젝트 풀링
        for (int i = 0; i < pool.Length; i++)
        {
            //오브젝트 풀을 탄막들을 담을 수 있는 크기로 설정
            pool[i].bulletObject = new GameObject[pool[i].poolSize];
            //오브젝트 풀에 넣을 탄막 개수만큼 반복
            for (int j = 0; j < pool[i].poolSize; j++)
            {
                //탄막 생성
                bullet = Instantiate(pool[i].prefab);
                //탄막을 오브젝트 풀에 입력
                pool[i].bulletObject[j] = bullet;
                //비활성화
                bullet.SetActive(false);
            }
        }
    }

    public void BulletOn(int Power)
    {
        for (int i = 0; i < Power; i++)
        {
            for (int j = 0; j < pool[i].poolSize; j++)
            {
                bullet = pool[i].bulletObject[j];
                if(bullet.activeSelf == false)
                {
                    //탄막 위치
                    bullet.transform.position = pool[i].spawnPoint.position;
                    //탄막 활성화
                    bullet.SetActive(true);
                    //검색 중단
                    break;
                }
            }
        }
    }
}
