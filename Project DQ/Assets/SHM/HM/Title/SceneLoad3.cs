using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad3 : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;

    public int num;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        num = LoadManager.Instance.sceneNum;
        //AsyncOperation operation = SceneManager.LoadSceneAsync("TitleScene");
        AsyncOperation operation = SceneManager.LoadSceneAsync(num);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;


            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }

            else if (operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if (progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

    }
}
