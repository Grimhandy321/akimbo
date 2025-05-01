using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinThrowing : MonoBehaviour
{
    public GameObject throwableObject; // coin prefab
    public float throwForce = 300f;

    public Vector3 baseEulerOffset = new Vector3(90, 45, 45
        );

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

        // Create rotation from offset
        Quaternion rotation = Quaternion.LookRotation(ray.direction) * Quaternion.Euler(baseEulerOffset);

        // Instantiate with rotation
        GameObject thrownObject = Instantiate(
            throwableObject,
            Camera.main.transform.position + ray.direction.normalized * 1f,
            rotation
        );

        Rigidbody thrownRb = thrownObject.GetComponent<Rigidbody>();
        if (thrownRb != null)
        {
            thrownRb.AddForce(ray.direction.normalized * throwForce, ForceMode.Impulse);
        }
    }
}
