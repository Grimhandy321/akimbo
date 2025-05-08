using UnityEngine;

public partial class PlayerController
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public GameObject mesh;
    private float xRotation = 0f;

    private void LateUpdate()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        mesh.transform.Rotate(Vector3.up * mouseX);
    }
}
