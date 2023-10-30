using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject robotObject;
    [SerializeField] private PlayerHealth playerHealth;

    private Animator bossAnimator;
    private Animator robotAnimator;

    private readonly float invincibilityTime = 3f;
    private float timer = 3f;

    public bool playerGotHit = false;
  
    private void Start()
    {
        bossAnimator = bossObject.GetComponent<Animator>();
        robotAnimator = robotObject.GetComponent<Animator>();     
    }

    private void Update()
    {
        InvincibilityTimer();
    }

    // Deactivate invincibility after the invincibility time expires
    private void InvincibilityTimer()
    {    
        if (!playerGotHit) 
        {
            return;
        }

        if (timer < 0 && playerGotHit)
        {
            playerGotHit = false;
            timer = invincibilityTime;
        }

        timer -= Time.deltaTime;
    }

    // When the enemies attack collide with the player
    // the player will lose health and gain invincibility for a few seconds 
    private void SetPlayerHealth(float damage)
    {
        playerHealth.health -= damage;
        playerGotHit = true;
        FindObjectOfType<AudioManager>().Play("PlayerGetsHit");
        healthBar.SetHealth(playerHealth.health);
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        // Exit out of the method if the player is invincible
        if (playerGotHit)
        {
            return;
        }

        if (collision.CompareTag("Shockwave"))
        {
            collision.gameObject.SetActive(false);
            SetPlayerHealth(20);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (playerGotHit)
        {
            return;
        }
        // Boss attacks
        if (bossAnimator.GetBool("isAttacking") && bossAnimator.GetBool("isHitting"))
        {
            if (bossAnimator.GetBool("isRampaged") && collision.CompareTag("RightBossLeg") ||
                collision.CompareTag("LeftBossLeg"))
            {
                SetPlayerHealth(20);

            }
            else if (!bossAnimator.GetBool("isRampaged") && collision.CompareTag("RightBossLeg"))
            {
                SetPlayerHealth(10);
            } else
            {
                SetPlayerHealth(15);
            }
        }

        // Robot attacks
        if (robotAnimator.GetBool("punch") && collision.CompareTag("RobotArm"))
        {
            SetPlayerHealth(10);
        }
    }

    private void OnParticleCollision(GameObject collision)
    {    
        // Exit out of the method if the player is invincible
        if (playerGotHit)
        {
            return;
        }
        // Deal damage if a collision happens with one of the rocks
        if (collision.CompareTag("RockSlide"))
        {
            SetPlayerHealth(15);
        }
    }
}