using UnityEngine;

public class SphereCaster : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float sphereRadius;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 offset;

    public GameObject CurrentHitObject { get; private set; }

    private float currentHitDistance;

    private Vector3 sphereOrigin;
    private Vector3 sphereDirection;

    void Update()
    {
        SphereCast();
    }

    // Casts a sphere in front of the attached gameobject
    // on impact with another gameobject of the assigned layermask
    // assigns the value of the current object that got hit and its distance
    private void SphereCast()
    {
        sphereOrigin = transform.position + offset;
        sphereDirection = transform.forward;

        if (Physics.SphereCast(sphereOrigin, sphereRadius, sphereDirection,
            out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            CurrentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }
        else
        {
            currentHitDistance = maxDistance;
            CurrentHitObject = null;
        }
    }
}
