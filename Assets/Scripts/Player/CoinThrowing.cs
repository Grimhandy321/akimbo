using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinThrowing : MonoBehaviour
{
    public GameObject throwableObject; // coin
    public float throwForce = 30f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowObject();
        }
    }
    void ThrowObject()
    {
        if (throwableObject == null) return;

        GameObject thrownObject = Instantiate(throwableObject, transform.position + transform.forward, new Quaternion(90,90,90,0));

        Rigidbody thrownRb = thrownObject.GetComponent<Rigidbody>();
        if (thrownRb != null)
        {
            thrownRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        }
    }
}
