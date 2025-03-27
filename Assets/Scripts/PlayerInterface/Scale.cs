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
        Weight weight = other.GetComponent<Weight>();

        if (weight != null)
        {
            float objectMass = weight.GetMass();
            this.UpdateDisplay(objectMass);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.UpdateDisplay(0);
    }

    /// <summary>
    /// Updates the scale's display with the given weight with decimals of 2.
    /// </summary>
    private void UpdateDisplay(float weight)
    {
        if (this.weightDisplay != null)
        {
            this.weightDisplay.text = $"{weight:F2} kg";
        }
        else
        {
            Debug.LogWarning("No weight display assigned to the scale!");
        }
    }
}
