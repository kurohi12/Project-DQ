using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;

    private float curHp = 100;
    private float maxHp = 100;

    float imsi;

    public Text bossHpCount;

    // Start is called before the first frame update
    void Start()
    {
        hpBar.value = (float)curHp / (float)maxHp;
        imsi = (float)curHp / (float)maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(curHp > 0)
            {
                curHp -= 10; // 임시체크
            }
            else
            {
                curHp = 0;
            }
        }
        imsi = (float)curHp / (float)maxHp;
        
        HandleHp();
    }

    public void BossHpCount(int bossHp)
    {
        bossHpCount.text = string.Format("{0}", bossHp);
    }

    private void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, imsi, Time.deltaTime * 10);
    }

    public float MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public float CurHp
    {
        get { return curHp; }
        set { curHp = value; }
    }
}

    
