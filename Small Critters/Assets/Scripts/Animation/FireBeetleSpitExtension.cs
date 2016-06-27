using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class FireBeetleSpitExtension : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<FireBeetleController>().AttackComplete();
    }
}
