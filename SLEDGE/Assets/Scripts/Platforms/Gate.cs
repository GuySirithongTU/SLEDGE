using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    [SerializeField] private Rigidbody gateBlock;
    [SerializeField] private Transform gateDestination;

    [SerializeField] private int keysRequired;

    [Range(0.05f, 0.2f)] [SerializeField] private float smoothTime = 0.1f;

    private Vector3 gateVelocity;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            StartCoroutine(OnGateOpen());
        }
    }

    private void FixedUpdate()
    {
        gateVelocity = gateBlock.velocity;
    }

    private IEnumerator OnGateOpen()
    {
        Debug.Log("open");
        while(Vector3.Distance(gateBlock.transform.position, gateDestination.position) >= 0.01f) {
            gateBlock.transform.position = Vector3.SmoothDamp(gateBlock.transform.position, gateDestination.position, ref gateVelocity, smoothTime);

            yield return null;
        }

        gateBlock.transform.position = gateDestination.position;
    }

    public int getKeysRequired()
    {
        return keysRequired;
    }
}
