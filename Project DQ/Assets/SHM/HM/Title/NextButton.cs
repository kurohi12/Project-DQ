using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ����Ƽ Scene

public class NextButton : MonoBehaviour
{
    public void Next()
    {
        SceneManager.LoadScene("LoadingScene3");
        // ����ٰ� ���� �ʱ�ȭ �� text�ٽ� ����

    }
}
