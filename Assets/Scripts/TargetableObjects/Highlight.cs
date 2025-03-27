using UnityEngine;

/// <summary>
/// Handles enabling/disabling the highlight effect on this object.
/// </summary>
public class Highlight : MonoBehaviour
{
    [SerializeField] private RenderingLayerMask highlightLayer;
    
    private Renderer[] renderers;
    private uint originalLayer;

    private void Start()
    {
        this.renderers = this.TryGetComponent<Renderer>(out var meshRenderer)
            ? new[] { meshRenderer }
            : this.GetComponentsInChildren<Renderer>();

        this.originalLayer = this.renderers[0].renderingLayerMask;
    }

    public void EnableHighlight(bool enable)
    {
        foreach (Renderer renderer in this.renderers)
        {
            renderer.renderingLayerMask = enable
                ? this.originalLayer | this.highlightLayer
                : this.originalLayer;
        }
    }
}
