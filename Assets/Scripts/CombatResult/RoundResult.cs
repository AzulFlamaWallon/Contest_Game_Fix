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

    public UInt16 session_ID;

    public Profile_RoundResult meNetData;

    public User_Profile meProfile;

    public void GetResultDataFromServer(Profile_RoundResult _Result, User_Profile _Profile)
    {
        int length = Manager_Ingame.Instance.m_Profiles.Count;
        for (int i = 0; i < length; i++)
        {
            if (_Profile.Session_ID == Manager_Ingame.Instance.m_Profiles[i].Session_ID)
            {//먼기아상
                meProfile = _Profile;
                
            }
            else
            {
                meProfile = Manager_Ingame.Instance.m_Client_Profile;
            }
        }
        
        if(_Result.session_id == Manager_Ingame.Instance.m_Client_Profile.Session_ID)
        {
            meNetData = _Result;
            session_ID = meNetData.session_id;
            nowRound = meNetData.Current_Round;
            timeup = meNetData.Time_Up;
            IsWinner = meNetData.Result_flag;
            shootCount = meNetData.Shoot_Count;
            rootingCount = meNetData.Getting_Count;
            averageRoundTime = meNetData.averageRoundTime;
            clearTime = meNetData.minTime;
            retryCount = meNetData.Result_Count;
        }
        score = _Profile.Score;
   

        if (IsWinner) winText = "WIN";
        else winText = "LOSE";
    }
}