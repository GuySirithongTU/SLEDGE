using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour {

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject contentPanel;

    [SerializeField] private Animator fadeAnimator;

    private static bool gameIsPaused = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(!gameIsPaused) {
                Pause();
            } else {
                Resume();
            }
        }
    }

    private void Pause()
    {
        gameIsPaused = true;

        pauseUI.SetActive(true);

        Time.timeScale = 0f;
        EnableButtons();
        pauseUI.GetComponent<Animator>().SetTrigger("pause");
    }

    public void Resume()
    {
        gameIsPaused = false;

        Time.timeScale = 1f;
        DisableButtons();
        pauseUI.GetComponent<Animator>().SetTrigger("resume");
    }

    public void Restart()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;

        StartCoroutine(FadeLoadScene(SceneManager.GetActiveScene().buildIndex));
    }
        


    public void Menu()
    {
        Resume();
        StartCoroutine(FadeLoadScene(0));
    }

    private IEnumerator FadeLoadScene(int sceneIndex)
    {
        fadeAnimator.SetTrigger("fadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(sceneIndex);
    }

    public void EnableButtons()
    {
        Button[] buttons = contentPanel.GetComponentsInChildren<Button>();
        Debug.Log(buttons.Length);
        foreach(Button button in buttons) {
            button.interactable = true;
        }
    }

    public void DisableButtons()
    {
        Button[] buttons = contentPanel.GetComponentsInChildren<Button>();
        foreach(Button button in buttons) {
            button.interactable = false;
        }
    }

    public static bool GetGameIsPaused()
    {
        return gameIsPaused;
    }

}
