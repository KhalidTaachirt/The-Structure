using UnityEngine;

public class BossCondition : MonoBehaviour
{
    [SerializeField] private GameObject damageNumbersPrefab;
    [SerializeField] private HealthBar bossHealthbar;

    public float BossHealth { get; private set; }
    public float BossMaxHealth { get; private set; } = 1000f;

    private readonly float baseDamage = 150f;
    private readonly float rangeModifier = 0.2f;

    private float damageFallOff;

    void Start()
    {
        BossHealth = BossMaxHealth;
        bossHealthbar.SetMaxHealth(BossMaxHealth);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            collider.gameObject.SetActive(false);

            FindObjectOfType<AudioManager>().Play("BulletHit");

            // Calculate the damage based on the distance and set damage variable
            damageFallOff = Mathf.Round((baseDamage - collider.gameObject.GetComponent<DamageFallOff>().BulletTravelDistance) * rangeModifier);

            SetBossHealth();
            DamageNumbers();
        }
    }

    private void SetBossHealth()
    {
        BossHealth -= damageFallOff;

        bossHealthbar.SetHealth(BossHealth);
    }

    // Spawns in a new gameobject that showcases the damage number with text
    private void DamageNumbers()
    {
        if (damageNumbersPrefab != null)
        {
            // Flip the damage number around
            Quaternion textRotation = Quaternion.identity * Quaternion.Euler(180, 90, 180);

            // Create a new damage number gameobject and converts it to a string to display the text
            var numbersObject = Instantiate(damageNumbersPrefab, transform.position, textRotation, transform);

            numbersObject.GetComponent<TextMesh>().text = damageFallOff.ToString();

            ChangeTextColor(numbersObject);
        }  
    }

    // Changes the color of the text based on the damage
    private void ChangeTextColor(GameObject textObject)
    {   
        if (damageFallOff < 15)
        {
            textObject.GetComponent<TextMesh>().color = Color.red;
        }
        else if (damageFallOff >= 15 && damageFallOff < 20)
        {
            textObject.GetComponent<TextMesh>().color = Color.yellow;
        }
        else
        {
            textObject.GetComponent<TextMesh>().color = Color.white;
        }
    }
}