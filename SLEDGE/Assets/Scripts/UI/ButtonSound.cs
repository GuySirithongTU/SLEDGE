using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour {

	[SerializeField] private AudioSource highlightSound;
    [SerializeField] private AudioSource clickSound;

    public void OnHighlightSound()
    {
        highlightSound.Play();
    }

    public void OnClickSound()
    {
        clickSound.Play();
    }
}
