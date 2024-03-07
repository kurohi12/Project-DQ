using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour
{
    [SerializeField]
    public List<SumonPattern> patterns = new List<SumonPattern>();

    private void Start()
    {
        LevelManager.Instance.patterns = patterns.ToList();
    }
}
