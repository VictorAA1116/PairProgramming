using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private float rotationSmoothSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    
    void LateUpdate()
    {
        if (!player) return;

        Transform playerTransform = player.transform;
        
        Vector3 rotatedOffset = playerTransform.rotation * offset;
        Vector3 desiredPosition = playerTransform.position + rotatedOffset;

        float positionLerp = rotationSmoothSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionLerp);

        Quaternion desiredRotation = Quaternion.LookRotation(playerTransform.forward, Vector3.up);
        float  rotationLerp = rotationSmoothSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationLerp);
    }
}
