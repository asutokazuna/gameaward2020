using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineShrinkState : StateMachineBehaviour
{
    [SerializeField] int _maxShrinkCount = 1;   //!< 飛んだあとのトランポリンが動く最大回数

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("StartJumpTP", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool("Jump"))
        {
            if(animator.GetInteger("Count") >= _maxShrinkCount)
            {
                animator.SetBool("FinishTP", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetInteger("Count") >= _maxShrinkCount)
        {
            animator.SetBool("FinishTP", false);
            animator.SetBool("Jump", false);
        }
    }
}
