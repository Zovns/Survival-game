using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(1f, 24f)]
    public float TimeOfDay = 12f; // You control this (1 to 24)

    public Light directionalLight;
    public Gradient lightColorOverTime;
    public AnimationCurve lightIntensityOverTime;
    public AnimationCurve ambientIntensityOverTime;

    void Update()
    {
        // Normalize time from 0 to 1 (1h -> 1/24, 24h -> 1)
        float timePercent = TimeOfDay / 24f;

        // Rotate sun based on time
        float sunAngle = (TimeOfDay / 24f) * 360f - 90f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));

        // Change light color
        directionalLight.color = lightColorOverTime.Evaluate(timePercent);

        // Change light intensity
        directionalLight.intensity = lightIntensityOverTime.Evaluate(timePercent);

        // Change ambient light
        RenderSettings.ambientIntensity = ambientIntensityOverTime.Evaluate(timePercent);
    }
}
