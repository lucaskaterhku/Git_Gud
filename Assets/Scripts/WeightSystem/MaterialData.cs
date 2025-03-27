using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds material data such as density.
/// </summary>
public static class MaterialData
{
    /// <summary>
    /// Densities in kg/m³.
    /// </summary>
    private static readonly Dictionary<MaterialType, float> densities = new Dictionary<MaterialType, float>
    {
        { MaterialType.Copper, 8960f },
    };

    public static float GetDensity(MaterialType type)
    {
        if (densities.TryGetValue(type, out float density))
        {
            return density;
        }

        Debug.LogWarning($"Density for material '{type}' not found. Returning default of 1000.");
        return 1000f;
    }
}
