using UnityEngine;

public class MovementState : StateMachineBehaviour
{   
    private BossStateHandler bossStateHandler;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossStateHandler = animator.gameObject.GetComponent<BossStateHandler>(); 
    }
    // Move towards the player with a certain speed
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("isRampaged"))
        {
            bossStateHandler.MoveToPlayer(5f);
        }
        else
        {
            bossStateHandler.MoveToPlayer(10f);
        }
    }
}
