using UnityEngine;

public class DamageFallOff : MonoBehaviour
{
    public float BulletTravelDistance { get; private set; }

    private Vector3 bulletOrigin;

    private void OnEnable()
    {
        // Save origin of the bullet when it gets activated
        bulletOrigin = transform.position;
    }

    void Update()
    {
        // Calculates distance between origin of activation and exit point
        BulletTravelDistance = Vector3.Distance(bulletOrigin, transform.position);
    }
}
