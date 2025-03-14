using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the weight of an object based on its scale and material type.
/// Updates Rigidbody mass dynamically.
/// </summary>
public class WeightManager : MonoBehaviour
{
    /// <summary>
    /// Defines available material types and their reference densities in kg per cubic meter. 
    /// </summary>
    public enum MaterialType
    {
        Copper,
    }

    /// <summary>
    /// Dictionary mapping materials to their densities (kg/m³).
    /// </summary>
    private readonly Dictionary<MaterialType, float> materialMasses = new Dictionary<MaterialType, float>
    {
        { MaterialType.Copper, 8960f },
    };

    [Header("Material Settings")]
    [SerializeField] private MaterialType selectedMaterial;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateMass();
    }

    private void Update()
    {
        UpdateMass();
    }

    /// <summary>
    /// Updates the Rigidbody mass based on the object's scale and selected material.
    /// </summary>
    private void UpdateMass()
    {
        if (!materialMasses.TryGetValue(selectedMaterial, out float referenceMass))
        {
            Debug.LogWarning($"Material {selectedMaterial} not found in materialMasses dictionary!");
            return;
        }

        Vector3 currentScale = transform.localScale;
        float mass = referenceMass * currentScale.x * currentScale.y * currentScale.z;

        rb.mass = mass;
    }

    /// <summary>
    /// Returns the current mass of the object.
    /// </summary>
    public float GetMass()
    {
        return rb.mass;
    }
}
