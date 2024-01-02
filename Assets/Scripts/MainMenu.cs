using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameManager gameManager;
    public PlayableDirector timelineDirector; // Reference to the PlayableDirector component of the Timeline


    void Update()
    {
        
    }
    

    public void LoadNewGame()
    {
        gameManager.DeleteSave();
        StartCoroutine(PlayTimelineAndLoadScene("Dock Thing", "MazeScene"));
    }


    IEnumerator PlayTimelineAndLoadScene(string timelineSceneName, string nextSceneName)
    {
        // Load the timeline scene
        SceneManager.LoadScene(timelineSceneName, LoadSceneMode.Additive);
        yield return null; // Wait one frame for the scene to load

        // Play the timeline
        timelineDirector.Play();

        // Wait for the timeline to finish
        while (timelineDirector.state == PlayState.Playing)
        {
            yield return null;
        }

        // Unload the timeline scene
        SceneManager.UnloadSceneAsync(timelineSceneName);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("MazeScene");
    }

    
    public void SaveGame()
    {
        Debug.Log("Save function called.");

        if (gameManager != null)
        {
            gameManager.SaveGame();
            //ResumeGame();

        }
        else
        {
            Debug.LogError("GameManager is not set in the PauseMenu");
        }
    }
    public void LoadGame()
    {
        Debug.Log("Load function called.");

        if (gameManager != null)
        {
            gameManager.LoadGame();
            //ResumeGame();
        }
        else
        {
            Debug.LogError("GameManager is not set in the PauseMenu");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit function called.");

        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);

    }
}
