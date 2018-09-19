using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour {

	private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Guardian")) {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Guardian")) {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
