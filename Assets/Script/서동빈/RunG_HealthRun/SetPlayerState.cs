using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState // Player의 상태
{ 
    RUN, JUMP, SLIDE, DIE
}

public class SetPlayerState : StateMachineBehaviour {

    public PlayerState state; // 이 Class를 포함한 Animation의 상태

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) // 이 Animation에 들어왔을 때
    {
        animator.SetInteger("PlayerState", (int)state); // state의 값으로 PlayerState를 변환
    }
}
