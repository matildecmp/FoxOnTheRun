using UnityEngine;

public class SkyboxChange : MonoBehaviour
{
    [Header("Skybox Materials")]
    public Material dayMaterial;
    public Material sunsetMaterial;
    public Material nightMaterial;

    [Header("Lighting")]
    public Light dayLight;
    public Light sunsetLight;
    public Light nightLight;
    public float dayIntensity = 1.7f;
    public float sunsetIntensity = 2.0f;
    public float nightIntensity = 2.0f;

    [Header("Score Thresholds")]
    public float sunsetMin = 200f; 
    public float nightMin = 500f;  

    private int lastScore = -1;

    // Start with Day
    private void Start()
    {
        ApplyState(0); 
    }

    // Update skybox and lighting according to Player's score
    private void Update()
    {
        int score = ShowScore.score;
        if (score != lastScore)
        {
            lastScore = score;
            UpdateSkyboxAndLighting(score);
        }
    }

    // Update skybox and lighting according to the declared score thresholds
    private void UpdateSkyboxAndLighting(int score)
    {
        if (score < sunsetMin)
        {
            ApplyState(0); 
        }
        else if (score < nightMin)
        {
            ApplyState(1); 
        }
        else
        {
            ApplyState(2); 
        }
    }

    // Set skybox and lighting settings
    private void ApplyState(int state)
    {
        switch (state)
        {
            case 0: // Day
                RenderSettings.skybox = dayMaterial;
                dayLight.enabled = true;
                dayLight.intensity = dayIntensity;
                sunsetLight.enabled = false;
                nightLight.enabled = false;
                break;

            case 1: // Sunset
                RenderSettings.skybox = sunsetMaterial;
                dayLight.enabled = false;
                sunsetLight.enabled = true;
                sunsetLight.intensity = sunsetIntensity;
                nightLight.enabled = false;
                break;

            case 2: // Night
                RenderSettings.skybox = nightMaterial;
                dayLight.enabled = false;
                sunsetLight.enabled = false;
                nightLight.enabled = true;
                nightLight.intensity = nightIntensity;
                break;
        }
        DynamicGI.UpdateEnvironment();
    }
}