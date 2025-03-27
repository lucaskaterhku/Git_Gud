using UnityEngine;

/// <summary>
/// Manages the perspective scaling of objects when picked up by the player.
/// Allows resizing of objects based on distance and raycasting to avoid clipping.
/// </summary>
public class PerspectiveScaling : MonoBehaviour
{
    [Header("Components")]
    public Transform target;             

    [Header("Parameters")]
    public LayerMask targetMask;        
    public LayerMask ignoreTargetMask;   
    public float offsetFactor;           

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
    /// Resizes the target object based on the player's view and raycast distance.
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
            this.target.position = hit.point - this.transform.forward * this.offsetFactor * this.targetScale.x;

            float currentDistance = Vector3.Distance(this.transform.position, this.target.position);
            float scaleRatio = currentDistance / this.originalDistance;

            this.targetScale = Vector3.one * scaleRatio;
            this.target.localScale = this.targetScale * this.originalScale;
        }
    }
}
