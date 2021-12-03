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

    public RoundResult GameResult { get; set; }
    private void Start()
    {
        SetModelCombatResult();
        SetResultScreen();
    }

    public void SetModelCombatResult()
    {
        if(GameResult.IsWinner)
        {
            if(GameResult.Player.TryGetComponent(out Animator _anim))
            {
                // 승리 캐릭터 모델 애니메이션
            }
        }
    }

    // 여기에 받아온 캐릭터들 정보 입력
    public void SetResultScreen()
    {
        
        switch (GameResult.Player.m_MyProfile.Role_Index)
        {
            case 1:
                resultCompo[0].text = "" + GameResult.timeup.ToString();
                resultCompo[1].text = "" + GameResult.shootCount.ToString();
                resultCompo[2].text = "" + GameResult.rootingCount.ToString();
                resultCompo[3].text = "" + GameResult.averageRoundTime.ToString();
                resultCompo[4].text = "" + GameResult.clearTime.ToString();
                resultCompo[5].text = "" + GameResult.retryCount.ToString();
                break;
            case 2:
                resultCompo[0].text = "" + GameResult.timeup.ToString();
                resultCompo[1].text = "" + GameResult.shootCount.ToString();
                resultCompo[2].text = "" + GameResult.rootingCount.ToString();
                resultCompo[3].text = "" + GameResult.averageRoundTime.ToString();
                resultCompo[4].text = "" + GameResult.clearTime.ToString();
                resultCompo[5].text = "" + GameResult.retryCount.ToString();
                break;
        }
        
    }



}
