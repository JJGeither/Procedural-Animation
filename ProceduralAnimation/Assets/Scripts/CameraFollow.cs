using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float rotationSpeed = 180.0f;

    private float currentRotation = 0.0f;

    void LateUpdate()
    {
        // Calculate the current rotation based on the mouse movement
        currentRotation += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // Calculate the position of the camera based on the target's position and rotation
        Vector3 offset = new Vector3(0.0f, height, -distance);
        Quaternion rotation = Quaternion.Euler(0.0f, currentRotation, 0.0f);
        Vector3 targetPosition = target.position + rotation * offset;

        // Set the position and rotation of the camera
        transform.position = targetPosition;
        transform.LookAt(target);
    }
}
