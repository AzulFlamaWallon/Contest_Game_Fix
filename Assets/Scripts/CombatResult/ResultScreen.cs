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

    public RoundResult[] GameResult { get; set; }

    void Init()
    {
        transform.SetAsLastSibling();
    }

    void Start()
    {
        btn_Retry.gameObject.GetComponentInChildren<Text>().text = "Retry"; // 이녀석만 남았다       
        btn_Quit.gameObject.GetComponentInChildren<Text>().text = "Quit";
        btn_Retry.onClick.AddListener(() => Retry());
        btn_Quit.onClick.AddListener(() => Application.Quit());
    }

    // 여기에 받아온 캐릭터들 정보 입력
    public void ShowResultScreen()
    {
        Init();

        int length = Manager_Ingame.Instance.m_Profiles.Count;
        for (int i = 0; i < length; i++)
        {
            if (GameResult[i].meProfile.Session_ID == Manager_Ingame.Instance.m_Client_Profile.Session_ID)
            {
                Debug.LogFormat("GameResult[i].meProfile.Session_ID : {0}, Manager_Ingame.Instance.m_Client_Profile.Session_ID : {1}", GameResult[i].meProfile.Session_ID, Manager_Ingame.Instance.m_Client_Profile.Session_ID);
                if(GameResult[i].meNetData.session_id == Manager_Ingame.Instance.m_Client_Profile.Session_ID)
                {
                    switch (GameResult[i].meProfile.Role_Index)
                    {
                        case 1:
                            resultCompo[0].text = "소탕시간 : " + GameResult[i].timeup.ToString();
                            resultCompo[1].text = "발포횟수 : " + GameResult[i].shootCount.ToString();
                            resultCompo[2].text = "검거횟수" + GameResult[i].rootingCount.ToString();
                            resultCompo[3].text = "평균소탕시간" + GameResult[i].averageRoundTime.ToString();
                            resultCompo[4].text = "최소소탕시간" + GameResult[i].clearTime.ToString();
                            winText.text = "Guard " + GameResult[i].winText;
                             
                            break;
                        case 2:
                            resultCompo[0].text = "생존시간 : " + GameResult[i].timeup.ToString();
                            resultCompo[1].text = "가드스턴수 : " + GameResult[i].shootCount.ToString();
                            resultCompo[2].text = "물건획득수 : " + GameResult[i].rootingCount.ToString();
                            resultCompo[3].text = "평균생존시간 : " + GameResult[i].averageRoundTime.ToString();
                            resultCompo[4].text = "최장 생존 시간 :" + GameResult[i].clearTime.ToString();
                            winText.text = "Rogue " + GameResult[i].winText;
                            winText.color = Color.red;
                            break;
                    }
                    resultCompo[5].text = "스코어 " + GameResult[i].score.ToString();
                }           
            }
        }
    }

    void Retry()
    {
        Manager_Ingame.Instance.Quit_Game();
        Destroy(this);
    }



}
