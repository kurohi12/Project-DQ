using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Transform score;
    public GameObject canvas;

    public bool gameOver = false;

    protected override void Init()
    {
        canvas = GameObject.Find("Canvas");
        score = canvas.transform.Find("Score");
        if(gameOver == true)
        {
            StartCoroutine(DisplayTexts());
            gameOver = false;
        }
    }

    public IEnumerator DisplayTexts()
    {
        for(int i = 0;i < score.transform.childCount;i++)
        {
            score.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
