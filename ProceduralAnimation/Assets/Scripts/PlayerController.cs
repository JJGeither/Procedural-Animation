using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float maxSlopeAngle = 45.0f;

    private Transform mainCameraTransform;
    private Rigidbody rb;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cameraForward = mainCameraTransform.forward;
            Vector3 cameraRight = mainCameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            Vector3 moveDirection = (cameraForward.normalized * vertical) + (cameraRight.normalized * horizontal);

            if (moveDirection.magnitude > 0)
            {
                Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime));
            }

            Vector3 velocity = moveDirection * moveSpeed;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        
    }

}
