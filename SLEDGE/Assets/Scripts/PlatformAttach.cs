using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour {

    private bool guardianOn = false;
    private bool platformRotating = false;

    private bool wasAnchored = false;

    private Transform guardianAnchor;

    [SerializeField] private Transform guardianTransform;
    [SerializeField] private Transform transformPrefab;

    public event Action AnchorStartEvent;
    public event Action AnchorEndEvent;

    private void Start()
    {
        transform.parent.GetComponent<Rotatable>().RotateStartEvent += OnRotateStart;
        transform.parent.GetComponent<Rotatable>().RotateEndEvent += OnRotateEnd;
    }

    private void Update()
    {
        if(platformRotating && guardianOn && !wasAnchored) {
            guardianAnchor = Instantiate(transformPrefab);
            guardianAnchor.parent = transform.parent;
            guardianAnchor.position = guardianTransform.position;

            if(AnchorStartEvent != null) {
                AnchorStartEvent.Invoke();
            }
            wasAnchored = true;
        }

        if(platformRotating && guardianOn) {
            guardianTransform.position = guardianAnchor.position;
        }

        if(!platformRotating) {
            if (AnchorEndEvent != null) {
                AnchorEndEvent.Invoke();
            }
            wasAnchored = false;
        }
    }

	private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Guardian")) {
            guardianOn = true;

            //collision.gameObject.transform.SetParent(transform.parent);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Guardian")) {
            guardianOn = false;

            //collision.gameObject.transform.SetParent(null);
        }
    }

    private void OnRotateStart()
    {
        platformRotating = true;
    }

    private void OnRotateEnd()
    {
        platformRotating = false;
    }
}
