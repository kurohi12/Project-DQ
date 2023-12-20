using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Serializable]
    public struct SumonPattern
    {
        public Transform spawnPosition;
        public float spawnTime;
        public float createTime;
        public int spawnCount;
        public int[] itemCode;
        public Vector3[] wayPoint;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
