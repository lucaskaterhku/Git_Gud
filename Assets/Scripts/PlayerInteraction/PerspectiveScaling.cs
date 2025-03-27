using UnityEngine;

/// <summary>
/// Manages the perspective scaling of objects when picked up by the player.
/// Also handles prevention of clipping through other objects like walls.
/// </summary>
public class PerspectiveScaling : MonoBehaviour
{
    [Header("Components")]
    public Transform target;             

    [Header("Parameters")]
    public LayerMask targetMask;        
    public LayerMask ignoreTargetMask;   
            
    private float originalDistance;      
    private float originalScale;        
    private Vector3 targetScale;       

    private void Start()
    {   
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        this.HandleInput();
        this.ResizeTarget();
    }

    /// <summary>
    /// Handles player input for selecting and releasing target objects.
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (this.target == null)
            {
                this.AttemptToSelectTarget();
            }
            else
            {
                this.ReleaseTarget();
            }
        }
    }

    /// <summary>
    /// Attempts to select a target object using raycasting.
    /// </summary>
    private void AttemptToSelectTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity, this.targetMask))
        {
            this.target = hit.transform;

            Rigidbody rb = this.target.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            this.originalDistance = Vector3.Distance(this.transform.position, this.target.position);
            this.originalScale = this.target.localScale.x;
            this.targetScale = this.target.localScale;
        }
    }

    /// <summary>
    /// Releases the currently held target object, re-enabling its physics.
    /// </summary>
    private void ReleaseTarget()
    {
        Rigidbody rb = this.target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        this.target = null;
    }

    /// <summary>
    /// Resizes the target object based on the player's view and raycast distance, preventing clipping.
    /// </summary>
    private void ResizeTarget()
    {
        if (this.target == null)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity, this.ignoreTargetMask))
        {
            Collider targetCollider = this.target.GetComponent<Collider>();
            if (targetCollider == null)
            {
                Debug.LogWarning("Target has no collider!");
                return;
            }

            Quaternion rotation = this.target.rotation;
            Vector3 direction = this.transform.forward;

            const float step = 0.01f;
            const float minDistance = 0.2f; 
            float distance = Vector3.Distance(this.transform.position, hit.point);

            while (distance > minDistance)
            {
                float scaleRatio = distance / this.originalDistance;
                Vector3 scaledScale = Vector3.one * scaleRatio;
                Vector3 finalScale = scaledScale * this.originalScale;
                Vector3 halfExtents = (finalScale / 2f) * 0.98f;

                Vector3 position = this.transform.position + direction * distance;
                Vector3 offsetPosition = position - direction * scaledScale.x;

                if (Physics.OverlapBox(offsetPosition, halfExtents, rotation, this.ignoreTargetMask).Length == 0)
                {
                    this.targetScale = scaledScale;
                    this.target.localScale = finalScale;
                    this.target.position = offsetPosition;
                    return;
                }

                distance -= step;
            }

            float fallbackScaleRatio = minDistance / this.originalDistance;
            this.targetScale = Vector3.one * fallbackScaleRatio;
            this.target.localScale = this.targetScale * this.originalScale;
            this.target.position = this.transform.position + direction * minDistance;
        }
    }
}