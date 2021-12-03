using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    [Header("결과화면 타이틀")]
    public Text  resultTitle;
    public Image resultBackGround;

    [Header("결과화면 항목들")]
    public Text[] resultCompo;

    [Header("결과화면 버튼")]
    public Button btn_Retry;
    public Button btn_Quit;

    CharacterController m_PlayerCtrler;
    private void Init()
    {
        resultTitle.text = "게임 결과";
    }

    void SetModelCombatResult(bool _IsWin)
    {
        if(_IsWin)
        {
            if(m_PlayerCtrler.TryGetComponent(out Animator _anim))
            {
                // 승리 캐릭터 모델 애니메이션
            }
        }
    }

    void GetCombatResultServer()
    {

    }



}
