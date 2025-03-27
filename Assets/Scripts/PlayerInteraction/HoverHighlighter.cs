using UnityEngine;

/// <summary>
/// Performs a camera raycast each frame to check if an highlightable object is under the cursor.
/// </summary>
public class HoverHighlighter : MonoBehaviour
{
    [Header("Highlight Settings")]
    [SerializeField] private LayerMask targetMask;

    private Highlight currentHighlighted;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this.targetMask))
        {
            Highlight highlight = hit.collider.GetComponentInParent<Highlight>();

            if (highlight != null)
            {
                if (highlight != this.currentHighlighted)
                {
                    this.ClearHighlight();
                    this.currentHighlighted = highlight;
                    this.currentHighlighted.EnableHighlight(true);
                }

                return;
            }
        }

        this.ClearHighlight();
    }

    /// <summary>
    /// Disables the highlight on the currently highlighted object (if any).
    /// </summary>
    private void ClearHighlight()
    {
        if (this.currentHighlighted != null)
        {
            this.currentHighlighted.EnableHighlight(false);
            this.currentHighlighted = null;
        }
    }
}
