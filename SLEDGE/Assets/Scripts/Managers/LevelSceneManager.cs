using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour {

    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Escape)) {
            QuitGame();
        }
        */
        if(Input.GetKeyDown(KeyCode.R)) {
            ReloadScene();
        }
    }

	public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
