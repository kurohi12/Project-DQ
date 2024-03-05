using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("UIScene");
        operation.allowSceneActivation = false;
        
        while(!operation.isDone)
        {
            yield return null;


            if(progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }

            else if(operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if(progressbar.value >= 1f)
            {
                loadtext.text = "Press SpaceBar";
            }

            if(Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >= 0.9f) // 스페이스바를 누르면 씬 전환
            {
                operation.allowSceneActivation = true;
                GameManager.Instance.GameStart();
                LevelManager.Instance.GameStart();
            }
        }
        
        /*  UI공부 메모
        operation.isDone; // isDone = 작업 완료의 유무를 boolean 형식으로 반환함
        operation.progress; // progress = 진행정도를 float형 0,1을 반환한다 (0-진행중/1-진행완료)
        operation.allowSceneActivation; // allowSceneActivation = true면 로딩이 완료되면 바로 신을 넘기고 false면 progress가 0.9f에서 멈춤(이때 다시 true를 해야 불러온 Scene으로 넘어감)
        */
    }
}
