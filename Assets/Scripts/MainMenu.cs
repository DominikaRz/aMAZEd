using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public AudioSource backgroundAudioSource; // Assign this in the inspector

    private SaveLevel saveLevelInstance;

    private void Awake()
    {
        saveLevelInstance = gameObject.AddComponent<SaveLevel>();
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundAudioSource != null && !backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }
    }

    public void LoadNewGame()
    {
        saveLevelInstance.Delete();
        SceneManager.LoadScene("DockThing");
        // Optional: Stop music if it shouldn't play in the next scene
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("MazeScene");
        // Optional: Stop music if it shouldn't play in the next scene
    }

    public void QuitGame()
    {
        Debug.Log("Quit function called.");
        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);
        // Optional: Stop music here if needed for any reason
    }
}
