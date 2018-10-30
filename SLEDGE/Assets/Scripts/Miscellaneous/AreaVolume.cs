using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaVolume : MonoBehaviour {

    [SerializeField] private GameObject AreaTitleUI;

    private bool hasShown = false;

	private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Guardian")) {
            if(!hasShown) {
                StartCoroutine(PlayAreaTitleUI());
                hasShown = true;
            }
        }
    }

    private IEnumerator PlayAreaTitleUI()
    {
        AreaTitleUI.SetActive(true);

        yield return new WaitForSeconds(6f);

        AreaTitleUI.SetActive(false);
    }
}
