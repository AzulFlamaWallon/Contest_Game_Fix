using Network.Data;
using System;

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
        meProfile        = _Profile;
        score            = _Profile.Score;
        meNetData        = _Result;
        session_ID       = meNetData.session_id;
        nowRound         = meNetData.Current_Round;
        timeup           = (ulong)TimeSpan.FromMilliseconds(meNetData.Time_Up).TotalSeconds;
        IsWinner         = meNetData.Result_flag;
        shootCount       = meNetData.Shoot_Count;
        rootingCount     = meNetData.Getting_Count;
        averageRoundTime = (ulong)TimeSpan.FromMilliseconds(meNetData.averageRoundTime).TotalSeconds;
        clearTime        = (ulong)TimeSpan.FromMilliseconds(meNetData.minTime).TotalSeconds;
        retryCount       = meNetData.Result_Count;

        if (IsWinner) winText = "WIN";
        else          winText = "LOSE";
    }

    
}