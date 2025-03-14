using UnityEngine;
using TMPro;

/// <summary>
/// Represents a scale that detects an object and displays its weight.
/// </summary>
public class Scale : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI weightDisplay;

    private void OnTriggerStay(Collider other)
    {
        WeightManager weightManager = other.GetComponent<WeightManager>();

        if (weightManager != null)
        {
            float objectMass = weightManager.GetMass();
            UpdateDisplay(objectMass);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UpdateDisplay(0);
    }

    /// <summary>
    /// Updates the scale's display with the given weight.
    /// </summary>
    private void UpdateDisplay(float weight)
    {
        if (weightDisplay != null)
        {
            weightDisplay.text = $"{weight:F2} kg";
        }
        else
        {
            Debug.LogWarning("No weight display assigned to the scale!");
        }
    }
}
