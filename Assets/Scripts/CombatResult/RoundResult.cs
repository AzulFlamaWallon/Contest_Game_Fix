using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Network.Data;

public class RoundResult : MonoBehaviour
{
    public int score; // 스코어

    public UInt16 nowRound; // 현재 라운드

    public UInt64 timeup; // 생존/소탕

    public bool IsWinner; // 이겼는지

    public UInt16 shootCount; // 사격

    public UInt16 rootingCount; // 검거/아이템

    public UInt64 averageRoundTime; // 평균 클리어 라운드 시간

    public UInt64 clearTime;  // 최소(소탕-가드), 최장(생존-로그)

    public UInt32 retryCount; // 리트 횟수

    public Profile_RoundResult NetData { get; private set; }
    public CharacterController Player { get; private set; } // 플레이어 세팅해야하는데 이게 안왔어..

    void Start()
    {
        
    }

    public void GetResultDataFromServer(Profile_RoundResult _Result)
    {
        NetData          = _Result;

        score            = Player.m_MyProfile.Score;
        nowRound         = NetData.Current_Round;
        timeup           = NetData.Time_Up;
        IsWinner         = NetData.Result_flag;
        shootCount       = NetData.Shoot_Count;
        rootingCount     = NetData.Getting_Count;
        averageRoundTime = NetData.averageRoundTime;
        clearTime        = NetData.minTime;
        retryCount       = NetData.Result_Count;
    }
}