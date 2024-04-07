using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    public float minGreen = 30f;    // Minimum value for the green channel
    public float maxGreen = 140f;   // Maximum value for the green channel
    public float period = 5f;       // Period of the sine wave in seconds

    private Light lightComponent;   // Reference to the Light component
    private Color initialColor;     // Initial color of the light

    void Start()
    {
        // Get the Light component attached to this GameObject
        lightComponent = GetComponent<Light>();

        // Store the initial color of the light
        initialColor = lightComponent.color;
    }

    void Update()
    {
        // Calculate the green channel value based on sine wave motion
        float greenValue = Mathf.Lerp(minGreen, maxGreen, Mathf.Sin(Time.time * Mathf.PI * 2 / period) * 0.5f + 0.5f);

        // Set the new color with the modified green channel
        Color newColor = new Color(initialColor.r, greenValue / 255f, initialColor.b, initialColor.a);
        lightComponent.color = newColor;
    }
}
