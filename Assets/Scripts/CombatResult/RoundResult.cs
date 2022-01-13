using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Network.Data;

public class RoundResult
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

    public string winText;

    public Profile_RoundResult NetData { get; private set; }

    public User_Profile meProfile;

    public void GetResultDataFromServer(int _UserNo, Profile_RoundResult _Result, User_Profile _Profile)
    {
        NetData          = _Result;

        score            = _Profile.Score;
        nowRound         = NetData.Current_Round;
        timeup           = NetData.Time_Up;
        IsWinner         = NetData.Result_flag;
        shootCount       = NetData.Shoot_Count;
        rootingCount     = NetData.Getting_Count;
        averageRoundTime = NetData.averageRoundTime;
        clearTime        = NetData.minTime;
        retryCount       = NetData.Result_Count;

        if (IsWinner)      winText = "WIN";
        else               winText = "LOSE";
        
        if(Manager_Ingame.Instance.m_Profiles[_UserNo].ID.Equals(_Profile.ID))
        {
            meProfile = _Profile;
        }
    }
}