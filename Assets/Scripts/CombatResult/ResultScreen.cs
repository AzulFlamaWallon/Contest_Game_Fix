using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultScreen : MonoBehaviour
{
    [Header("결과화면 타이틀")]
    public Text  resultTitle;
    public Image resultBackGround;

    [Header("결과화면 항목들")]
    public TMP_Text winText;
    public Text[] resultCompo;

    [Header("결과화면 버튼")]
    public Button btn_Retry;
    public Button btn_Quit;

    public RoundResult GameResult { get; set; }


    private void Start()
    {
        //SetModelCombatResult();
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
                resultCompo[0].text = "생존시간 : " + GameResult.timeup.ToString();
                resultCompo[1].text = "가드스턴수 : " + GameResult.shootCount.ToString();
                resultCompo[2].text = "물건획득수 : " + GameResult.rootingCount.ToString();
                resultCompo[3].text = "평균생존시간 : " + GameResult.averageRoundTime.ToString();
                resultCompo[4].text = "최장 생존 시간 :" + GameResult.clearTime.ToString();
                winText.text = "Rogue " + GameResult.IsWinner.ToString();
                winText.color = Color.red;
                break;
            case 2:
                resultCompo[0].text = "소탕시간 : " + GameResult.timeup.ToString();
                resultCompo[1].text = "발포횟수 : " + GameResult.shootCount.ToString();
                resultCompo[2].text = "검거횟수" + GameResult.rootingCount.ToString();
                resultCompo[3].text = "평균소탕시간" + GameResult.averageRoundTime.ToString();
                resultCompo[4].text = "최소소탕시간" + GameResult.clearTime.ToString();
                winText.text = "Guard " + GameResult.IsWinner.ToString();
                break;
        }
        resultCompo[5].text = "스코어 " + GameResult.score.ToString();

        btn_Retry.gameObject.GetComponentInChildren<Text>().text = "Retry";
        btn_Retry.onClick.AddListener(() => Manager_Ingame.Instance.Quit_Game());
        btn_Quit.gameObject.GetComponentInChildren<Text>().text = "Quit";
        btn_Quit.onClick.AddListener(() => Application.Quit());
    }



}
