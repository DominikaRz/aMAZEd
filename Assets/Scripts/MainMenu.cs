using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    private SaveLevel saveLevelInstance;

    private void Awake()
    {
        saveLevelInstance = gameObject.AddComponent<SaveLevel>();
    }


    public void LoadNewGame()
    {
        saveLevelInstance.Delete();
        SceneManager.LoadScene("DockThing");
    }



    public void ContinueGame()
    {
        SceneManager.LoadScene("MazeScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit function called.");

        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);

    }
}
