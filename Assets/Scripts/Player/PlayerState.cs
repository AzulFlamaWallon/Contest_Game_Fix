using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations;
using MonsterLove.StateMachine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    public Animator charaAnimator;
    public StateMachine<PlayableCharaState> state;

    void Awake()
    {
        state = new StateMachine<PlayableCharaState>(this);
        state.ChangeState(PlayableCharaState.PlayerIdle);        
    }

    /*
    * PLAYER_IDLE
    */
    public void PlayerIdle_Enter()
    {
        charaAnimator.SetBool("Idle", true);
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
        state.ChangeState(PlayableCharaState.PlayerIdle);
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
        state.ChangeState(PlayableCharaState.PlayerIdle);
    }
    /*
    * PLAYER_ONSTUN
    */
    public void PlayerOnStun_Enter()
    {
        charaAnimator.SetTrigger("Stun");
    }

    public void PlayerOnStun_Update()
    {
        
    }

    public void PlayerOnStun_Exit()
    {
        state.ChangeState(PlayableCharaState.PlayerIdle);
    }

    /*
    * PlayerOnDeath
    */
    public void PlayerOnDeath_Enter()
    {
        
    }

    public void PlayerOnDeath_Update()
    {

    }

    public void PlayerOnDeath_Exit()
    {

    }

    /*
   * PlayerOnCloaking
   */
    public void PlayerOnCloaking_Enter()
    {

    }

    public void PlayerOnCloaking_Update()
    {

    }

    public void PlayerOnCloaking_Exit()
    {

    }
}