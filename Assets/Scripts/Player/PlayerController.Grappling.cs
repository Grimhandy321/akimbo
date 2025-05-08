using UnityEngine;

public partial class PlayerController
{
    public LineRenderer lineRenderer;
    public float maxDistance = 50f;
    public float grappleSpeed = 100f;
    public float grappleDelay = 0.1f; 
    public Transform player;

    public KeyCode grappleKey = KeyCode.R;  

    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private SpringJoint springJoint;
    private bool isLineDrawn = false;

    void GrappleUpdate()
    {
        if (Input.GetKeyDown(grappleKey))  
        {
            StartGrappling();
        }
        else if (Input.GetKeyUp(grappleKey)) 
        {
            StopGrappling();
        }

        if (isGrappling)
        {
            DrawRope();
        }
    }

    void StartGrappling()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, grapplePoint);


            isLineDrawn = true;

            Invoke("LaunchPlayer", grappleDelay);
        }
    }

    void LaunchPlayer()
    {
        if (isLineDrawn)
        {
            springJoint = player.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = grapplePoint;
            springJoint.maxDistance = Vector3.Distance(player.position, grapplePoint);
            springJoint.spring = 50f;

            Invoke("StopGrappling", 2f);  
        }
    }

    void StopGrappling()
    {
        if (springJoint != null)
        {
            Destroy(springJoint);
        }
        lineRenderer.positionCount = 0;
        isGrappling = false;
        isLineDrawn = false;
    }

    void DrawRope()
    {
        if (lineRenderer != null && isGrappling)
        {
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }
}
