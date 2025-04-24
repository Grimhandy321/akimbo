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
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        GameObject thrownObject = Instantiate(throwableObject, Camera.main.transform.position + ray.direction.normalized * 1f, Quaternion.identity);

        Rigidbody thrownRb = thrownObject.GetComponent<Rigidbody>();
        if (thrownRb != null)
        {
            thrownRb.AddForce(ray.direction.normalized * throwForce, ForceMode.Impulse);        }
    }
}
