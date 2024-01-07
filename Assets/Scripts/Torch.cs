using UnityEngine;

public class Torch : MonoBehaviour
{
    public Light torchLight; // Assign this in the inspector
    public ParticleSystem fireParticleSystem; // Assign this in the inspector
    public float burnOutTime = 60f; // Total time in seconds for the torch to burn out

    private float initialLightIntensity;
    private float initialEmissionRate;
    private float burnOutTimer;

    void Start()
    {
        if (torchLight == null)
            torchLight = GetComponentInChildren<Light>();

        initialLightIntensity = torchLight.intensity;

        if (fireParticleSystem == null)
            fireParticleSystem = GetComponentInChildren<ParticleSystem>();

        var emissionModule = fireParticleSystem.emission;
        initialEmissionRate = emissionModule.rateOverTime.constant;

        burnOutTimer = burnOutTime;
    }

    void Update()
    {
        if (burnOutTimer > 0)
        {
            // Calculate the new intensity and emission rate based on the elapsed time
            burnOutTimer -= Time.deltaTime;
            float ratio = burnOutTimer / burnOutTime;

            // Set the new intensity and emission rate
            torchLight.intensity = initialLightIntensity * ratio;
            var emissionModule = fireParticleSystem.emission;
            emissionModule.rateOverTime = initialEmissionRate * ratio;

            // If the torch has burned out, ensure the light and particle system are turned off
            if (burnOutTimer <= 0)
            {
                torchLight.enabled = false;
                emissionModule.rateOverTime = 0;
                fireParticleSystem.Stop(); // Stop the particle system
            }
        }
    }
}
