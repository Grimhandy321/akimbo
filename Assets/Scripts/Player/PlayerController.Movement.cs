using UnityEngine;

public partial class PlayerController
{
    [Header("Movement")]
    public float moveSpeed = 15;
    public float jumpSpeed = 20;
    public float gravity = 25;
    public float doubleJumpSpeed = 25;
    public float movementMultiplier = 2;
    public float airMultiplier = 1;
    public LayerMask groundMask;
    private float horizontalMovement;
    private float verticalMovement;
    private bool isGrounded;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool canDoubleJump;
    private bool canJump = true;
    private float groundDistance = 3f;


    private void InitializeMovement()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void MovementUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 0.5F, 0), groundDistance, groundMask);
        HandleMovementInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) Jump();
            else if (canDoubleJump) DoubleJump();
        }
    }

    private void MovementFixedUpdate()
    {
        MovePlayer();
        ApplyGravity();
    }

    private void HandleMovementInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    private void Jump()
    {
        canDoubleJump = true;
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
    }

    private void DoubleJump()
    {
        canDoubleJump = false;
        rb.velocity = new Vector3(rb.velocity.x, doubleJumpSpeed, rb.velocity.z);
    }

    private void ApplyGravity()
    {
        rb.AddForce(new Vector3(0, -gravity, 0) * rb.mass);
    }

    private void MovePlayer()
    {
        float speed = isGrounded ? 1 : airMultiplier;
        rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * speed, ForceMode.Acceleration);
    }
}
