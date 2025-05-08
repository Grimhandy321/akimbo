using UnityEngine;

public partial class PlayerController
{

    [Header("Player")]
    public Alteruna.Avatar Avatar;
    [Header("Movement")]
    public float moveSpeed = 15;
    public float jumpSpeed = 200;
    public float gravity = 25;
    public float doubleJumpSpeed = 25;
    public float movementMultiplier = 2;
    public float airMultiplier = 1;
    public LayerMask groundMask;
    public LayerMask wallMask;
    private float horizontalMovement;
    private float verticalMovement;
    private bool isGrounded;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool canDoubleJump;
    private bool canJump = true;
    private float groundDistance = 3f;
    [Header("Materials")]
    public Material blackMaterial;
    public Material greenMaterial;
    [Header("Wall Run")]
    public float wallMoveSpeed = 15;
    public float wallPullForce = 500;
    public float wallDistance = 5f;
    public float wallRunGravity = 0.2f;
    public bool isWallRunning;
    public float wallRunJumpForceX = 1200, wallRunJumpForceY = 800;
    private Transform currentWall;
    private bool wallLeft, wallRight;
    private RaycastHit leftWallHit, rightWallHit;
    private bool pulledToTheWall;
    private bool materialTurnedOriginal;
    [Header("Camera")]
    public Camera cam;
    public float camTilt;
    public float camTiltTime;
    public float wallRunTilt;

    private void InitializeMovement()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        isGrounded = true;
    }

    private void MovementUpdate()
    {
        if (!_isOwner) return; 
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 3F, 0), groundDistance, groundMask);
        HandleMovementInput();
        Debug.DrawRay(transform.position, transform.right * wallDistance, Color.green);
        Debug.DrawRay(transform.position, -transform.right * wallDistance, Color.red);
        var jumpPressed = Input.GetKeyDown(KeyCode.Space);
        if (jumpPressed && isGrounded)
        {
            Jump();
        }
        else if (jumpPressed && canDoubleJump)
        {
            DoubleJump();
        }

        WallRunning();
    }

    private void MovementFixedUpdate()
    {
        if (!_isOwner) return; 
        MovePlayer();
        ApplyGravity();
    }

    private void WallRunning()
    {
        CheckWall();
        if (CanWallRun())
        {
            if (wallLeft || wallRight)
            {
                WallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
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
    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallDistance, wallMask);
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallDistance, wallMask);
    }

    private void WallRun()
    {
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        isWallRunning = true;

        if (wallLeft)
        {
            if (!pulledToTheWall)
            {
                pulledToTheWall = true;
                currentWall = leftWallHit.transform;

                ChangeMaterials(currentWall.GetComponent<MeshRenderer>(), greenMaterial);
                materialTurnedOriginal = false;

                if (leftWallHit.normal.x > 0)
                {
                    rb.AddForce(new Vector3(-wallPullForce, 0, 0));
                }
                else
                {
                    rb.AddForce(new Vector3(wallPullForce, 0, 0));
                }
            }

            wallRunTilt = Mathf.Lerp(wallRunTilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            if (!pulledToTheWall)
            {
                pulledToTheWall = true;
                currentWall = rightWallHit.transform;

                ChangeMaterials(currentWall.GetComponent<MeshRenderer>(), greenMaterial);
                materialTurnedOriginal = false;

                if (rightWallHit.normal.x > 0)
                {
                    rb.AddForce(new Vector3(-wallPullForce, 0, 0));
                }
                else
                {
                    rb.AddForce(new Vector3(wallPullForce, 0, 0));
                }
            }

            wallRunTilt = Mathf.Lerp(wallRunTilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
            if (wallLeft)
            {
                wallRunJumpDirection = transform.up + leftWallHit.normal;
            }
            else if (wallRight)
            {
                wallRunJumpDirection = transform.up + rightWallHit.normal;
            }

            canDoubleJump = true;
            rb.AddForce(new Vector3(wallRunJumpDirection.x * wallRunJumpForceX, wallRunJumpDirection.y * wallRunJumpForceY), ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        if (currentWall != null && !materialTurnedOriginal)
        {
            materialTurnedOriginal = true;
            ChangeMaterials(currentWall.GetComponent<MeshRenderer>(), blackMaterial);
        }

        isWallRunning = false;
        pulledToTheWall = false;
        wallRunTilt = Mathf.Lerp(wallRunTilt, 0, camTiltTime * Time.deltaTime);
    }

    private bool CanWallRun()
    {
        return true;
    }
    private void ChangeMaterials(MeshRenderer mr, Material newMat)
    {
        Material[] materials = mr.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = newMat;
        }
        mr.materials = materials;
    }


}
