using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour {

    [SerializeField] private GameObject endOfLevelUI;
    
    public static bool goalReached;

    private void Awake()
    {
        goalReached = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Guardian")) {
            endOfLevelUI.SetActive(true);
            goalReached = true;
        }
    }
}
