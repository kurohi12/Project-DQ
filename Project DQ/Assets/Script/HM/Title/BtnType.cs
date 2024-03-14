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


    Vector3 defaultScale; // 버튼 초기 크기 담을 변수
    bool isSound;

    private void Start()
    {
        defaultScale = buttonScale.localScale; // 초기화
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
                Debug.Log("스코어점수 이동");
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
                    Debug.Log("사운드off");
                }
                else
                {
                    Debug.Log("사운드on");
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
                Debug.Log("종료");
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

    public void OnPointerEnter(PointerEventData eventData) // 마우스 감지
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
