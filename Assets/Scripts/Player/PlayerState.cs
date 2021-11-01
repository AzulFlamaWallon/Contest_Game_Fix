using System;
using UnityEngine;
using UnityEngine.Animations;
using MonsterLove.StateMachine;

public class PlayerState : MonoBehaviour
{
    public enum PlayableCharaState
    {
        PlayerSpawn,
        PlayerIdle,
        PlayerMove,
        PlayerOnHit,
        PlayerOnStun,
        PlayerOnDeath
    }

    StateMachine<PlayableCharaState> state;

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
        state.ChangeState(PlayableCharaState.PlayerSpawn);
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