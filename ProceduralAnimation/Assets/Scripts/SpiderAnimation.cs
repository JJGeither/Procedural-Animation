using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    public Transform[] legs;
    public GameObject[] spheres;
    public float stepLength = 20;
    public float sphereHeightOffset; // adjust as needed
    public float heightOffset;
    public float dragLength = 30;
    public float stepDuration = 1.0f;
    public float raycastDistance;
    public LayerMask groundLayer;

    private Vector3[] legRestPositions;
    private Vector3[] legStepRaycastTargets;

    private bool isMoving = false;

    private void Start()
    {
        legRestPositions = new Vector3[legs.Length];
        legStepRaycastTargets = new Vector3[legs.Length];

        // Initialize the leg rest positions based on their current position
        for (int i = 0; i < legs.Length; i++)
        {
            legRestPositions[i] = legs[i].position;
        }

        CalculateSpherePositions();
    }

    public void CalculateSpherePositions()
    {
        for (int i = 0; i < legs.Length; i++)
        {

            // Calculate the step length based on whether the index is even or odd
            float currentStepLength = stepLength;
            Vector3 spherePosition = legs[i].position + transform.TransformDirection(transform.forward) * currentStepLength;
            spherePosition.y += sphereHeightOffset; // add offset to y-coordinate
            spheres[i].transform.position = spherePosition;
        }
    }

    public void CalculateRaycastPositions()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(spheres[i].transform.position, Vector3.down, out hit, raycastDistance, groundLayer))
            {
                legStepRaycastTargets[i] = hit.point;
            }
            else
            {
                legStepRaycastTargets[i] = spheres[i].transform.position; // fallback to sphere position
            }

            // Calculate the step length based on whether the index is even or odd
        }
    }

    public float GetAverageY()
    {

        float average = 0f;
        if (legs.Length > 0)
        {
            foreach (var point in legs)
            {
                average += point.position.y;
            }
            average /= legs.Length;
        }
        return average;
    }

    private void FixedUpdate()
    {

        CalculateRaycastPositions();
        // Update the phase of each leg based on the current time
        for (int i = 0; i < legs.Length; i++)
        {
            if (Vector3.Distance(legs[i].position, legStepRaycastTargets[i]) > dragLength && !isMoving)
            {
                legRestPositions[i] = legStepRaycastTargets[i];
            }

            legs[i].position = Vector3.Lerp(legs[i].position, legRestPositions[i], stepDuration * Time.deltaTime);

            // Check if the position of any leg has changed from its initial position
            if (Vector3.Distance(legs[i].position, legRestPositions[i]) > 5)
            {
                isMoving = true;
            }
        }
        isMoving = false;
        this.transform.position = new Vector3(this.transform.position.x,GetAverageY() + heightOffset,this.transform.position.z);
    }
}