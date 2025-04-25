using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float speed = 10;
    public float sensitivityMultiplier = 5;

    public Vector3 baseEulerOffset = new Vector3(0, 180, 0); 

    private Quaternion refRotation;
    private float xRotation;
    private float yRotation;

    private void Start()
    {
        refRotation = Quaternion.Euler(baseEulerOffset);
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = refRotation * rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}