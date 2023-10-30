using UnityEngine;

public class ShockwaveState : StateMachineBehaviour
{
    private BossProperties shockWave;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shockWave = animator.gameObject.GetComponent<BossProperties>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isShockwave", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shockWave.ShootShockwaves();
    }
}
