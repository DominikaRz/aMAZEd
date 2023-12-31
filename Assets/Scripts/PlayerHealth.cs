using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    private HUDManager hudManager;
    public Player playerInstance;
    //public float CurrentHealth { get { return currentHealth; } }

    private void Start()
    {
        currentHealth = maxHealth;

        // Find the HUDManager in the scene
        hudManager = FindObjectOfType<HUDManager>();
        if (hudManager == null)
        {
            Debug.LogError("HUDManager not found in the scene!");
            return;
        }

        hudManager.SetHealth(currentHealth);


    }

    public void TakeDamage(float amount)
    {
        if(currentHealth > 0){
            currentHealth -= amount;
            Debug.Log($"PlayerHealth: {currentHealth}");
            hudManager.OnHealthChanged(currentHealth);
        }
        else
        {
            if (DeathScreen.Instance != null)
            {
                playerInstance.Death();
                DeathScreen.Instance.ShowDeathScreen();
            }
            else
            {
                Debug.LogError("DeathScreen component not assigned.");
            }
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
        hudManager.OnHealthChanged(currentHealth);
    }

    public void SetHealthTo100(){
        currentHealth = 100;
    }

    public float getHealthValue(){
        return this.currentHealth;
    }
}