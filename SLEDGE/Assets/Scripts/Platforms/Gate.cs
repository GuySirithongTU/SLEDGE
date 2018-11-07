using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject keyInstructionUI;

    [SerializeField] private Rigidbody gateBlock;
    [SerializeField] private Transform gateDestination;

    [SerializeField] private GameObject[] keyholes;

    [SerializeField] private int keysRequired;
    private int keysRequiredLeft;

    [Range(0.05f, 0.2f)] [SerializeField] private float smoothTime = 0.1f;

    private Vector3 gateVelocity;

    private bool isOpened = false;
    private bool guardianIsNear = false;
    private bool noKeyHoles = false;
    
    private void Start()
    {
        if(keysRequired == 0) {
            noKeyHoles = true;
            keyholes[0].SetActive(false);
            keyholes[1].SetActive(false);
        } else if(keysRequired == 1) {
            noKeyHoles = false;
            keyholes[0].SetActive(true);
            keyholes[1].SetActive(false);
            keysRequiredLeft = 1;
        } else {
            noKeyHoles = false;
            keyholes[0].SetActive(true);
            keyholes[1].SetActive(true);
            keysRequiredLeft = 2;
        }
    }

    private void Update()
    {
        // Blinking animation.

        if(keysRequired == 1) {
            if (FindObjectOfType<GuardianController>().GetKeyCount() > 0) {
                if (guardianIsNear) {
                    keyholes[0].GetComponentInChildren<Animator>().SetBool("guardianIsNear", true);

                    keyInstructionUI.SetActive(true);
                    keyInstructionUI.GetComponent<Animator>().SetBool("display", true);
                }
                keyInstructionUI.transform.position = mainCamera.WorldToScreenPoint(keyholes[0].transform.position);
            }

        } else if(keysRequired == 2) {
            if(keysRequiredLeft == 2) {
                if (FindObjectOfType<GuardianController>().GetKeyCount() > 0) {
                    if (guardianIsNear) {
                        keyholes[0].GetComponentInChildren<Animator>().SetBool("guardianIsNear", true);

                        keyInstructionUI.SetActive(true);
                        keyInstructionUI.GetComponent<Animator>().SetBool("display", true);
                    }
                    keyInstructionUI.transform.position = mainCamera.WorldToScreenPoint(keyholes[0].transform.position);
                }

            } else if(keysRequiredLeft == 1) {
                if (FindObjectOfType<GuardianController>().GetKeyCount() > 0) {
                    if (guardianIsNear) {
                        keyholes[1].GetComponentInChildren<Animator>().SetBool("guardianIsNear", true);

                        keyInstructionUI.SetActive(true);
                        keyInstructionUI.GetComponent<Animator>().SetBool("display", true);
                    }
                    keyInstructionUI.transform.position = mainCamera.WorldToScreenPoint(keyholes[1].transform.position);
                }
            }
        }

        if(!guardianIsNear) {
            keyholes[0].GetComponentInChildren<Animator>().SetBool("guardianIsNear", false);
            keyholes[1].GetComponentInChildren<Animator>().SetBool("guardianIsNear", false);

        }

        if(!guardianIsNear || FindObjectOfType<GuardianController>().GetKeyCount() == 0) {
            keyInstructionUI.GetComponent<Animator>().SetBool("display", false);
        }

        // Use keys.
        if(Input.GetKeyDown(KeyCode.E) && FindObjectOfType<GuardianController>().GetKeyCount() > 0 && guardianIsNear && keysRequiredLeft > 0) {
            keysRequiredLeft--;
            FindObjectOfType<GuardianController>().DecrementKey();

            if(keysRequired == 1) {
                keyholes[0].GetComponentInChildren<Animator>().SetTrigger("insert");
            }

            if(keysRequired == 2) {
                if (keysRequiredLeft == 1) {
                    keyholes[0].GetComponentInChildren<Animator>().SetTrigger("insert");
                } else {
                    keyholes[1].GetComponentInChildren<Animator>().SetTrigger("insert");
                }
            }

            if(keysRequiredLeft == 0) {
                OnGateOpenStart();
            }
        }
    }

    private void FixedUpdate()
    {
        gateVelocity = gateBlock.velocity;
    }

    public void OnGateOpenStart()
    {
        StartCoroutine(OnGateOpen());
        isOpened = true;
    }

    private IEnumerator OnGateOpen()
    {
        GetComponent<AudioSource>().Play();

        while(Vector3.Distance(gateBlock.transform.position, gateDestination.position) >= 0.01f) {
            gateBlock.transform.position = Vector3.SmoothDamp(gateBlock.transform.position, gateDestination.position, ref gateVelocity, smoothTime);

            yield return null;
        }

        gateBlock.transform.position = gateDestination.position;
    }

    public void OnGuardianEnter()
    {
        guardianIsNear = true;
    }

    public void OnGuardianExit()
    {
        guardianIsNear = false;
    }

    public int getKeysRequired()
    {
        return keysRequired;
    }
}
