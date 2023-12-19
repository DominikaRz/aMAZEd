using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;

public class LevelDisplay : MonoBehaviour
{
    public TextMeshProUGUI storyText; // Reference to your TextMeshPro component
    public TextMeshProUGUI level; // Reference to your TextMeshPro component
    public string message;

    public void DisplayMessage()
    {
        string story = message;
        level.text = "Level";
        storyText.text = story;
        storyText.gameObject.SetActive(true); // Activate TextMeshPro
        gameObject.SetActive(true); // Activate Canvas

        StartCoroutine(HideStoryAfterDelay(3f)); // Hide after 15 seconds
    }

    private IEnumerator HideStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        storyText.gameObject.SetActive(false); // Deactivate TextMeshPro
        gameObject.SetActive(false); // Deactivate Canvas
    }


}

