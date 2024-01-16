using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;

public class LevelDisplay : MonoBehaviour
{
    public TextMeshProUGUI storyText; // Reference to your TextMeshPro component
    public TextMeshProUGUI levelText; // Reference to your TextMeshPro component
    public TextMeshProUGUI timeText; // Reference to your TextMeshPro component
    public string message;
    public string time;

    public void DisplayMessage()
    {
        //string story = message;
        levelText.text = "Level";
        //storyText.text = story;
        storyText.text = message;
        timeText.text = "Time: " + time + " seconds";
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

    public void SetMessage(string mess){
        this.message = mess;
    }

    public void SetTime(string tim){
        this.time = tim;
    }


}

