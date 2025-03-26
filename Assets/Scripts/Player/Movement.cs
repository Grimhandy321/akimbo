using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float slideSpeed = 12f;
    public float wallRunSpeed = 10f;
    public float jumpForce = 7f;
    public float slideJumpForce = 10f;

    public float wallRunDuration = 1.5f;
    public float slideDuration = 0.8f;

    public float mouseSensitivity = 100f;
    public float wallRunPushForce = 5f;
    public float wallTiltAngle = 5f;

    public float slideCameraHeight = 0.5f;
    private float normalCameraHeight;

    private Rigidbody rb;
    private bool isSliding = false;
    private bool isWallRunning = false;
    private bool canDoubleJump = true;

    private float xRotation = 0f;

    public Text movementInfoText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store the initial camera height
        normalCameraHeight = Camera.main.transform.localPosition.y;
    }

    private void Update()
    {
        Move();
        LookAround();

        // Sliding
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isSliding)
        {
            StopSliding();
        }

        // Wall Running
        if (IsTouchingWall() && Input.GetKey(KeyCode.Space) && !isWallRunning)
        {
            StartCoroutine(WallRun());
        }

        if (Input.GetKeyUp(KeyCode.Space) && isWallRunning)
        {
            StopWallRun();
        }

        UpdateMovementInfo();
    }

    void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        if (isSliding) speed = slideSpeed;
        if (isWallRunning) speed = wallRunSpeed;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, isSliding ? slideJumpForce : jumpForce, rb.velocity.z);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                canDoubleJump = false;
            }
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    IEnumerator Slide()
    {
        isSliding = true;

        // Lower the camera
        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, slideCameraHeight, Camera.main.transform.localPosition.z);

        // Remove friction while sliding
        PhysicMaterial slideMaterial = new PhysicMaterial { frictionCombine = PhysicMaterialCombine.Minimum, dynamicFriction = 0f, staticFriction = 0f };
        GetComponent<Collider>().material = slideMaterial;

        yield return new WaitForSeconds(slideDuration);

        // Reset camera position if still sliding
        if (isSliding)
        {
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, normalCameraHeight, Camera.main.transform.localPosition.z);
            // Restore friction
            GetComponent<Collider>().material = null;
            isSliding = false;
        }
    }

    void StopSliding()
    {
        // Stop sliding and reset everything
        isSliding = false;
        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, normalCameraHeight, Camera.main.transform.localPosition.z);

        // Restore friction
        GetComponent<Collider>().material = null;
    }

    IEnumerator WallRun()
    {
        isWallRunning = true;
        canDoubleJump = true; // Reset double jump on wall run

        Vector3 wallDirection = GetWallDirection();
        bool isRightWall = wallDirection == transform.right;

        float timer = 0f;

        while (timer < wallRunDuration && IsTouchingWall())
        {
            rb.velocity = new Vector3(wallDirection.x * wallRunSpeed, 0, wallDirection.z * wallRunSpeed);

            // Push player slightly towards the wall
            rb.AddForce(transform.forward * wallRunPushForce, ForceMode.Force);

            // Tilt the camera
            float tilt = isRightWall ? wallTiltAngle : -wallTiltAngle;
            Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, tilt);

            if (Input.GetAxis("Vertical") <= 0)
            {
                rb.velocity += transform.right * (isRightWall ? -wallRunPushForce : wallRunPushForce);
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isWallRunning = false;

        // Reset camera tilt
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void StopWallRun()
    {
        // Stop wall running and reset everything
        isWallRunning = false;

        // Reset camera tilt
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void UpdateMovementInfo()
    {
        string movementType = "Walking";
        if (isSliding) movementType = "Sliding";
        else if (isWallRunning) movementType = "Wall Running";
        else if (Input.GetKey(KeyCode.LeftShift)) movementType = "Running";

        movementInfoText.text = $"Speed: {rb.velocity.magnitude:F1}\nMovement: {movementType}";
    }

    Vector3 GetWallDirection()
    {
        if (Physics.Raycast(transform.position, transform.right, 1f)) return transform.right;
        if (Physics.Raycast(transform.position, -transform.right, 1f)) return -transform.right;
        return Vector3.zero;
    }

    bool IsTouchingWall()
    {
        return GetWallDirection() != Vector3.zero;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
