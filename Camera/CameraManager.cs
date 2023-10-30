using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    private readonly float cameraSpeed = 2f;
    private readonly float baseDistance = 10f;
    private readonly float rangeModifier = 2.5f;

    private BossStateHandler bossStateHandler;

    private void Start()
    {
        bossStateHandler = bossObject.GetComponent<BossStateHandler>();
    }

    private void LateUpdate()
    {
        // Exit out of the method if the player isn't found
        if (player == null)
        {
            Debug.LogWarning("Player Reference not set");
            return;
        }

        CameraLerp();
        ZoomCameraOut();
    }

    // By using lerp the camera now gradually follows the position of the player
    // instead of instantly snapping to the location of the player
    private void CameraLerp()
    {
        float interpolation = cameraSpeed * Time.deltaTime;

        Vector3 position = transform.position;
        position.z = Mathf.Lerp(transform.position.z, player.transform.position.z, interpolation);
        position.x = Mathf.Lerp(transform.position.x, player.transform.position.x, interpolation);

        transform.position = position;
    }

    // The camera now gradually zooms in and out based on the distance between the boss and player
    private void ZoomCameraOut()
    {
        vcam.m_Lens.OrthographicSize = bossStateHandler.DistanceBetweenEnemyAndPlayer / rangeModifier + baseDistance;
    }
}