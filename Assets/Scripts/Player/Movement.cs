using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 15;
    public float jumpSpeed = 20;
    public float gravity = 25;
    public float doubleJumpSpeed = 25;
    public float movementMultiplier = 2;
    public bool canJump;
    public bool canDoubleJump;

    public float airMultiplier = 1;

    private float horizontalMovement;
    private float verticalMovement;

    private bool isGrounded;

    private Vector3 moveDirection;

    private Rigidbody rb;

    float groundDistance = 3f;

    [Header("Wall Run")]
    public float wallMoveSpeed = 15;
    public float wallPullForce = 500;
    public float wallDistance = 50;
    public float wallRunGravity = 0.2f;
    public bool isWallRunning;
    public float wallRunJumpForceX = 1200, wallRunJumpForceY = 800;
    private Vector3 wallRunDirection;
    private Transform currentWall;
    private bool wallLeft, wallRight;
    private RaycastHit leftWallHit, rightWallHit;
    private bool pulledToTheWall;
    private bool materialTurnedOriginal;

    [Header("Camera")] public Camera cam;
    public float camTilt;
    public float camTiltTime;

    public float wallRunTilt;

    [Header("LayerMasks")] public LayerMask wallMask;
    public LayerMask groundMask;

    [Header("Materials")] public Material blackMaterial;
    public Material greenMaterial;


    [Header("Grappling")]
    public float maxGrappleDistance = 40f;
    public float grappleDelayTime = 0.2f;
    public float overshootYAxis = 5f;
    private Vector3 grapplePoint;

    [Header("Grapple Cooldown")]
    public float grapplingCd = 2f;
    private float grapplingCdTimer;
    public LineRenderer lr;

    public KeyCode grappleKey = KeyCode.R;
    private bool grappling;



    private Alteruna.Avatar _avatar;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _avatar = GetComponent<Alteruna.Avatar>();

        canJump = true;
        rb.freezeRotation = true;

        lr.positionCount = 2;
        lr.enabled = false;
    }

    private void Update()
    {


        Debug.DrawRay(transform.position, transform.right * wallDistance, Color.green);
        Debug.DrawRay(transform.position, -transform.right * wallDistance, Color.red);


        if (Input.GetKeyDown(grappleKey)) StartGrapple();
        if (grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;

        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 0.5F, 0), groundDistance, groundMask);

        HandleInput();

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

        if (grappling)
        {
            lr.SetPosition(0, transform.position);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Gravity();
    }

    private void Gravity()
    {
        if (!isWallRunning)
        {
            rb.AddForce(new Vector3(0, -gravity, 0) * rb.mass);
        }
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

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallDistance, wallMask);
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallDistance, wallMask);
    }



    private void WallRun()
    {
        //rb.useGravity = false;
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

                Debug.Log("Left Normal X: " + leftWallHit.normal.x);
                if (leftWallHit.normal.x > 0)
                {
                    rb.AddForce(new Vector3(-wallPullForce, 0, 0));
                    wallRunDirection = leftWallHit.transform.forward;
                }
                else
                {
                    rb.AddForce(new Vector3(wallPullForce, 0, 0));
                    wallRunDirection = -leftWallHit.transform.forward;
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
                    wallRunDirection = -rightWallHit.transform.forward;
                }
                else
                {
                    rb.AddForce(new Vector3(wallPullForce, 0, 0));
                    wallRunDirection = rightWallHit.transform.forward;
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

    void Jump()
    {
        if (canJump && !isWallRunning)
        {
            canDoubleJump = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
    }

    void DoubleJump()
    {
        if (canDoubleJump && !isWallRunning)
        {
            canDoubleJump = false;
            rb.velocity = new Vector3(rb.velocity.x, doubleJumpSpeed, rb.velocity.z);
        }
    }

    private void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            // friction
            rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);
        }
        else if (isWallRunning)
        {
            rb.AddForce(wallRunDirection * wallMoveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }

    }

    private void HandleInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
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
    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        rb.velocity = Vector3.zero;
        lr.enabled = true;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, grapplePoint);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = Camera.main.transform.position + Camera.main.transform.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        Debug.Log("asd");
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeY = grapplePoint.y - lowestPoint.y;
        float highestPoint = grapplePointRelativeY + overshootYAxis;
        if (grapplePointRelativeY < 0) highestPoint = overshootYAxis;

        JumpToPosition(grapplePoint, highestPoint);
        Invoke(nameof(StopGrapple), 1.2f);
    }

    private void JumpToPosition(Vector3 target, float trajectoryHeight)
    {
        Vector3 velocity = CalculateJumpVelocity(transform.position, target, trajectoryHeight);
        rb.velocity = Vector3.zero;
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float height)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0, endPoint.z - startPoint.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY;
    }

    private void StopGrapple()
    {
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
    }

}