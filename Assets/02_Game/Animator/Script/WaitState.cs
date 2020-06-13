using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : StateMachineBehaviour
{
    public bool _isWait;    //!< 待機フラグ

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _isWait = true;
        animator.SetInteger("PlayerState", (int)PlayerAnim.PlayerState.E_WAIT);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Finish", false);
    }    
}
