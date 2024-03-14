using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 유니티 Scene

public class NextButton : MonoBehaviour
{
    public void Next()
    {
        SceneManager.LoadScene("LoadingScene3");
        // 여기다가 점수 초기화 및 text다시 끄기

    }
}
