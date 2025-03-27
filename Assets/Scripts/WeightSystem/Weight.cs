using UnityEngine;

/// <summary>
/// Calculates and applies mass to an object based on shape, scale (uniform), and material.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Weight : MonoBehaviour
{
    [Header("Material and Shape Settings")]
    [SerializeField] private MaterialType selectedMaterial = MaterialType.Copper;
    [SerializeField] private ShapeType shape = ShapeType.Sphere;

    private Rigidbody rb;

    private void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.UpdateMass();
    }

    private void UpdateMass()
    {
        float density = MaterialData.GetDensity(this.selectedMaterial);
        Vector3 scale = this.transform.localScale;

        float volume = 0f;

        switch (this.shape)
        {
            case ShapeType.Sphere:
                float radius = scale.x / 2f;
                volume = (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);
                break;

            case ShapeType.Cube:
                volume = scale.x * scale.y * scale.z;
                break;

            default:
                Debug.LogWarning("Unsupported shape type!");
                return;
        }

        float mass = density * volume;
        this.rb.mass = mass;
    }

    public float GetMass()
    {
        return this.rb != null ? this.rb.mass : 0f;
    }
}
