using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private Animator mainMenuAnimator;
    [SerializeField] private Animator fadeAnimator;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float landingY = 1f;
    [SerializeField] private float levelSelectorY = -3f;

	public void LevelSelector()
    {
        cameraTarget.position = new Vector3(cameraTarget.position.x, levelSelectorY, cameraTarget.position.z);

        mainMenuAnimator.SetTrigger("levelSelector");
    }

    public void Landing()
    {
        cameraTarget.position = new Vector3(cameraTarget.position.x, landingY, cameraTarget.position.z);

        mainMenuAnimator.SetTrigger("landing");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(FadeLoadLevel(level));
    }

    private IEnumerator FadeLoadLevel(int level)
    {
        fadeAnimator.SetTrigger("fadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(level);
    }
}
