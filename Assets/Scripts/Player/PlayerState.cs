using System;
using UnityEngine;
using UnityEngine.Animations;
using MonsterLove.StateMachine;

public class PlayerState : MonoBehaviour
{
    /// <summary>
    /// 플레이어블 캐릭터의 상태 열거형
    /// 언더바는 몬스터러브 스테이트머신의 구조상 인식에 방해가 되니(이벤트 식별자로 오인함) 언더바를 쓰지 않습니다.
    /// </summary>
    public enum PlayableCharaState
    {
        PlayerSpawn,
        PlayerIdle,
        PlayerMove,
        PlayerOnHit,
        PlayerOnStun,
        PlayerOnDeath
    }

    public Animator charaAnimator;
    public StateMachine<PlayableCharaState> state;

    private void Awake()
    {
        state = new StateMachine<PlayableCharaState>(this);
        state.ChangeState(PlayableCharaState.PlayerSpawn);        
    }

    /*
     * PLAYER_Spawn
     */
    public void PlayerSpawn_Enter()
    {
        
    }

    public void PlayerSpawn_Update()
    {

    }

    public void PlayerSpawn_Exit()
    {
        
    }
    /*
    * PLAYER_IDLE
    */
    public void PlayerIdle_Enter()
    {
        state.ChangeState(PlayableCharaState.PlayerIdle);
    }

    public void PlayerIdle_Update()
    {

    }

    public void PlayerIdle_Exit()
    {

    }
    /*
    * PLAYER_Move
    */
    public void PlayerMove_Enter()
    {
        state.ChangeState(PlayableCharaState.PlayerMove);
    }

    public void PlayerMove_Update()
    {

    }

    public void PlayerMove_Exit()
    {

    }

    /*
     * PLAYER_ONHIT
     */
    public void PlayerOnHit_Enter()
    {
        state.ChangeState(PlayableCharaState.PlayerOnHit);
    }

    public void PlayerOnHit_Update()
    {

    }

    public void PlayerOnHit_Exit()
    {

    }
    /*
    * PLAYER_ONSTUN
    */
    public void PlayerOnStun_Enter()
    {
        state.ChangeState(PlayableCharaState.PlayerOnStun);
    }

    public void PlayerOnStun_Update()
    {

    }

    public void PlayerOnStun_Exit()
    {

    }

    /*
    * PlayerOnDeath
    */
    public void PlayerOnDeath_Enter()
    {
        state.ChangeState(PlayableCharaState.PlayerOnDeath);
    }

    public void PlayerOnDeath_Update()
    {

    }

    public void PlayerOnDeath_Exit()
    {

    }
}