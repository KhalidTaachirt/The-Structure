using UnityEngine;

public class RockslideState : StateMachineBehaviour
{
    private GameObject rockSlideObject;

    private ParticleSystem rockSlide;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rockSlideObject = GameObject.FindWithTag("RockSlide");
        rockSlide = rockSlideObject.GetComponent<ParticleSystem>();
    }

    // Rockslide particle effect being played with added screenshake
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        FindObjectOfType<AudioManager>().Play("Rockslide");
        CameraShake.instance.CinemachineShake(25, 2f);     
    }

    // Exit out of the rockslide animation after activating it
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!rockSlide.isPlaying)
        {
            rockSlide.Play();
        }

        animator.SetBool("isRockslide", false);
    }    
}
