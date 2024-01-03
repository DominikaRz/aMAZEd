using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FirstCutscene : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public void Trigger()
    {
        SceneManager.LoadScene(sceneName);
    }
}