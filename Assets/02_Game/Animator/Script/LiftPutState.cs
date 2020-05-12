using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPutState : StateMachineBehaviour
{
    [SerializeField] PlayerAnim.PlayerState _state;   //!< プレイヤーの状態取得用

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("Height", 3);

        if (PlayerAnim.PlayerState.E_PUT == _state)
        {
            if (animator.GetBool("withBox"))
            {
                animator.SetBool("withBox", false);
            }
            else if (animator.GetBool("withChara"))
            {
                animator.SetBool("withChara", false);
            }
        }
    }
}
