using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Represents a scale that detects objects and displays their weight.
/// </summary>
public class Scale : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI weightDisplay;

    private readonly List<Weight> objectsOnScale = new List<Weight>();

    private void OnTriggerEnter(Collider other)
    {
        Weight weight = other.GetComponent<Weight>();
        if (weight != null && !this.objectsOnScale.Contains(weight))
        {
            this.objectsOnScale.Add(weight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Weight weight = other.GetComponent<Weight>();
        if (weight != null && this.objectsOnScale.Contains(weight))
        {
            this.objectsOnScale.Remove(weight);
        }
    }

    private void Update()
    {
        float totalMass = 0f;

        this.objectsOnScale.RemoveAll(item => item == null);

        foreach (Weight weight in this.objectsOnScale)
        {
            totalMass += weight.GetMass();
        }

        this.UpdateDisplay(totalMass);
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
