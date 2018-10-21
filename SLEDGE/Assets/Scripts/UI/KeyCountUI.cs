using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCountUI : MonoBehaviour {

    [SerializeField] private Text keyCount;

    private void Start()
    {
        FindObjectOfType<GuardianController>().keyCountUpdateEvent += OnKeyCountUpdate;
    }

    private void OnKeyCountUpdate(int newKeyCount)
    {
        keyCount.text = newKeyCount.ToString();
    }
}
