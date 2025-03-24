using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private Vector3 rotateY;

    public float walkSpeed = 2.5f;
    public float rotateSpeed = 150.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        rb.MovePosition(rb.position + transform.forward * Input.GetAxis("Vertical") * walkSpeed * Time.deltaTime); if (Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                rotateY = new Vector3(0, rotateSpeed * Time.deltaTime, 0);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                rotateY = new Vector3(0, -rotateSpeed * Time.deltaTime, 0);
            }
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateY));

        }
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        if (Input.GetKey("r")){
            animator.SetTrigger("Mele");
        }
    }

}
