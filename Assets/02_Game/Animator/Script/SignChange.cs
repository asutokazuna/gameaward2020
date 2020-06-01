using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignChange : StateMachineBehaviour
{
    [SerializeField] bool _change;
    [SerializeField] int _count = 0;
    private E_INPUT_MODE[] _inputMode = { E_INPUT_MODE.TRIGGER, E_INPUT_MODE.BUTTON, E_INPUT_MODE.TRIGGER, E_INPUT_MODE.BUTTON };
    private E_INPUT[] _inputKey = { E_INPUT.L_STICK_RIGHT, E_INPUT.A, E_INPUT.L_STICK_LEFT, E_INPUT.A };
    private bool _init = true;
    private Controller _input;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_init)
        {
            _input = GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>();
        }

        if (!_change)
        {
            animator.SetBool("Change", false);
        }
        else
        {
            _count = animator.GetInteger("Count");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_change)
        {
            if (_count != 2)
            {
                if (_input.isInput(_inputMode[_count], _inputKey[_count]) && !_input.isInput(E_INPUT_MODE.BUTTON, E_INPUT.B))
                {
                    _count++;

                    if (_inputMode.Length - 1 < _count)
                    {
                        _count = _inputMode.Length - 1;
                    }
                    else
                    {
                        animator.SetBool("Change", true);
                    }
                }
            }
            else
            {
                if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_LEFT) && _input.isInput(E_INPUT_MODE.BUTTON, E_INPUT.B))
                {
                    _count++;

                    if (_inputMode.Length - 1 < _count)
                    {
                        _count = _inputMode.Length - 1;
                    }
                    else
                    {
                        animator.SetBool("Change", true);
                    }
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_change)
        {
            animator.SetInteger("Count", _count);
        }
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
