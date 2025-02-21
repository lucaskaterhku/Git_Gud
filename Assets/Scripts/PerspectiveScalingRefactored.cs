using UnityEngine;

/// <summary>
/// Manages the perspective scaling of objects when picked up by the player.
/// Allows resizing of objects based on distance and raycasting to avoid clipping.
/// </summary>
public class PerspectiveScalingRefactored : MonoBehaviour
{
    [Header("Components")]
    public Transform target;             // The target object for scaling

    [Header("Parameters")]
    public LayerMask targetMask;         // Layer mask for raycasting target objects
    public LayerMask ignoreTargetMask;   // Layer mask to ignore player and target objects
    public float offsetFactor;           // Offset to avoid wall clipping

    private float originalDistance;      // Original distance from camera to target
    private float originalScale;         // Original scale of the target object
    private Vector3 targetScale;         // Desired scale of the target object

    private void Start()
    {   
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleInput();
        ResizeTarget();
    }

    /// <summary>
    /// Handles player input for selecting and releasing target objects.
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (target == null)
            {
                AttemptToSelectTarget();
            }
            else
            {
                ReleaseTarget();
            }
        }
    }

    /// <summary>
    /// Attempts to select a target object using raycasting.
    /// </summary>
    private void AttemptToSelectTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
        {
            target = hit.transform;

            Rigidbody rb = target.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            originalDistance = Vector3.Distance(transform.position, target.position);
            originalScale = target.localScale.x;
            targetScale = target.localScale;
        }
    }

    /// <summary>
    /// Releases the currently held target object, re-enabling its physics.
    /// </summary>
    private void ReleaseTarget()
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        target = null;
    }

    /// <summary>
    /// Resizes the target object based on the player's view and raycast distance.
    /// </summary>
    private void ResizeTarget()
    {
        if (target == null)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            target.position = hit.point - transform.forward * offsetFactor * targetScale.x;

            float currentDistance = Vector3.Distance(transform.position, target.position);
            float scaleRatio = currentDistance / originalDistance;

            targetScale = Vector3.one * scaleRatio;
            target.localScale = targetScale * originalScale;
        }
    }
}
