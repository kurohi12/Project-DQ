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

            if(Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >= 0.9f) // �����̽��ٸ� ������ �� ��ȯ
            {
                operation.allowSceneActivation = true;
                GameManager.Instance.GameStart();
                LevelManager.Instance.GameStart();
            }
        }
        
        /*  UI���� �޸�
        operation.isDone; // isDone = �۾� �Ϸ��� ������ boolean �������� ��ȯ��
        operation.progress; // progress = ���������� float�� 0,1�� ��ȯ�Ѵ� (0-������/1-����Ϸ�)
        operation.allowSceneActivation; // allowSceneActivation = true�� �ε��� �Ϸ�Ǹ� �ٷ� ���� �ѱ�� false�� progress�� 0.9f���� ����(�̶� �ٽ� true�� �ؾ� �ҷ��� Scene���� �Ѿ)
        */
    }
}
