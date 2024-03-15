using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonType currentType;
    public Transform buttonScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup typeGroup;


    Vector3 defaultScale; // ��ư �ʱ� ũ�� ���� ����
    bool isSound;

    private void Start()
    {
        defaultScale = buttonScale.localScale; // �ʱ�ȭ
    }

    public void OnBtnClick()
    {
        switch(currentType)
        {
            case ButtonType.GameStart:
                CanvasGroupOn(typeGroup);
                CanvasGroupOff(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case ButtonType.TypeOne:
                PlayerManager.Instance.playerType = States.PlayerOne;
                SceneManager.LoadScene("LoadingScene3");
                LoadManager.Instance.sceneNum = 0;
                break;
            case ButtonType.TypeTwo:
                PlayerManager.Instance.playerType = States.PlayerTwo;
                SceneManager.LoadScene("LoadingScene");
                break;
            case ButtonType.TypeThree:
                PlayerManager.Instance.playerType = States.PlayerThree;
                SceneManager.LoadScene("LoadingScene");
                break;
            case ButtonType.TypeBack:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                CanvasGroupOff(typeGroup);
                break;
            case ButtonType.Score:
                Debug.Log("���ھ����� �̵�");
                break;
            case ButtonType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                CanvasGroupOff(typeGroup);
                break;
            case ButtonType.Sound:
                if(isSound)
                {
                    isSound = !isSound;
                    Debug.Log("����off");
                }
                else
                {
                    Debug.Log("����on");
                }
                isSound = !isSound;
                break;
            case ButtonType.Back:
                CanvasGroupOn(mainGroup); 
                CanvasGroupOff(optionGroup);
                CanvasGroupOff(typeGroup);
                break;
            case ButtonType.Quit:
                Application.Quit();
                Debug.Log("����");
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData) // ���콺 ����
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
