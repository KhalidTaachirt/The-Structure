using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    private readonly float destroyTime = 3f;

    private Vector3 randomizeLocation = new(0.5f, 0, 0);

    void Start()
    {
        Destroy(gameObject, destroyTime);

        // Slight change of text location
        transform.localPosition += new Vector3(Random.Range(-randomizeLocation.x, randomizeLocation.x),
            Random.Range(-randomizeLocation.y, randomizeLocation.z),
            Random.Range(-randomizeLocation.y, randomizeLocation.z));
    }
}
