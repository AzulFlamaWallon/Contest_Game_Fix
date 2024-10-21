using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ResultScreen : MonoBehaviour
{
    [Header("결과화면 타이틀")]
    public Text resultTitle;
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

        int length = GameResult.Length;
        var clientSessionID = Manager_Ingame.Instance.m_Client_Profile.Session_ID;

        for (int i = 0; i < length; i++)
        {
            // 내 프로필과 받아온 내 네트워크 세션이 일치하는지 확인
            if (GameResult[i].meProfile.Session_ID == clientSessionID && GameResult[i].meNetData.session_id == clientSessionID)
            {
                Debug.LogFormat($"NetProfile_Session_ID : {GameResult[i].meProfile.Session_ID}, InGameProfile_Session_ID : {clientSessionID}");

                resultCompo[0].text = $"소탕시간 : {GameResult[i].timeup} 초";
                resultCompo[1].text = $"발포횟수 : {GameResult[i].shootCount} 번";
                resultCompo[2].text = $"검거횟수 : {GameResult[i].rootingCount} 번";
                resultCompo[3].text = $"평균소탕시간 : {GameResult[i].averageRoundTime} 초";
                resultCompo[4].text = $"최소소탕시간 : {GameResult[i].clearTime} 초";

                if (GameResult[i].meProfile.Role_Index == 1)
                {
                    winText.text = $"Guard {GameResult[i].winText}";
                    winText.color = Color.blue; 
                }
                else if (GameResult[i].meProfile.Role_Index == 2)
                {
                    resultCompo[0].text = $"생존시간 : {GameResult[i].timeup} 초";
                    resultCompo[1].text = $"가드스턴수 : {GameResult[i].shootCount} 번";
                    resultCompo[2].text = $"물건획득수 : {GameResult[i].rootingCount} 번";
                    resultCompo[3].text = $"평균생존시간 : {GameResult[i].averageRoundTime} 초";
                    resultCompo[4].text = $"최장생존시간 : {GameResult[i].clearTime} 초";
                    winText.text = $"Rouge {GameResult[i].winText}";
                    winText.color = Color.red;
                }

                resultCompo[5].text = $"스코어 : {GameResult[i].score} 점";
            }
        }
    }


    void Retry()
    {
        Manager_Ingame.Instance.Quit_Game();
        Destroy(this);
    }



}
