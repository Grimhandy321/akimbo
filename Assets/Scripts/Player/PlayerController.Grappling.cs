using UnityEngine;

public partial class PlayerController
{
    public float maxGrappleDistance = 40f;
    public float grappleDelayTime = 0.2f;
    public float overshootYAxis = 5f;
    public float grapplingCd = 2f;
    public LineRenderer lr;
    public KeyCode grappleKey = KeyCode.R;

    private Vector3 grapplePoint;
    private float grapplingCdTimer;
    private bool grappling;

    private void InitializeGrapple()
    {
        lr.positionCount = 2;
        lr.enabled = false;
    }

    private void GrappleUpdate()
    {
        if (grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;

        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        if (grappling) lr.SetPosition(0, transform.position);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        lr.enabled = true;
        rb.velocity = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;
            lr.SetPosition(1, grapplePoint);
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = Camera.main.transform.position + Camera.main.transform.forward * maxGrappleDistance;
            lr.SetPosition(1, grapplePoint);
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float relativeY = grapplePoint.y - lowestPoint.y;
        float height = (relativeY < 0) ? overshootYAxis : relativeY + overshootYAxis;
        Vector3 velocity = CalculateJumpVelocity(transform.position, grapplePoint, height);

        rb.velocity = Vector3.zero;
        rb.AddForce(velocity, ForceMode.VelocityChange);
        Invoke(nameof(StopGrapple), 1.2f);
    }

    private void StopGrapple()
    {
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
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
}
